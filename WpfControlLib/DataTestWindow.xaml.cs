using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using Microsoft.FSharp.Collections;
using Sapientum;
using Sapientum.Types.Wpf;

namespace WpfControlLib
{
    /// <summary>
    /// Interaction logic for DataTestWindow.xaml
    /// </summary>
    public partial class DataTestWindow
    {
        public static string CalculateMd5Hash(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }


        public DataTestWindow()
        {
            InitializeComponent();

            buttonLogin.Click += (sender, args) => ButtonLoginClick(sender, args);

            //var sapeApi = new XmlRpc.SapeApi();
            //sapeApi.Login("sanyok_m", CalculateMd5Hash("abc123"), true);
            //var sapeProjects = sapeApi.GetProjects();
            //var projects = new Projects();
            //for (int i = 0; i < sapeProjects.Length; i++)
            //{
            //    var projectUrls = sapeApi.GetProjectUrls(sapeProjects[i].Id);
            //    FSharpList<ProjectUrl> wpfProjectUrls =
            //        projectUrls.Length > 0
            //            ? new FSharpList<ProjectUrl>(new ProjectUrl(projectUrls[0].Name), FSharpList<ProjectUrl>.Empty)
            //            : FSharpList<ProjectUrl>.Empty;
            //    var prj = new Project(sapeProjects[i].Id, sapeProjects[i].Name, wpfProjectUrls);
            //    projects.AddProject(prj);
            //    //((Projects)projectsTreeView.DataContext).AddProject(prj);
            //}
            //mainGrid.Resources["projects"] = projects;
            //projectsTreeView.DataContext = ((Projects)mainGrid.Resources["projects"]).Projects;
            //projectsTreeView.DataContext = projects;
        }

        public event RoutedEventHandler ButtonLoginClick;

        //private void HyperlinkRequestNavigate(object sender, RequestNavigateEventArgs e)
        //{
        //    Process.Start(e.Uri.ToString());
        //}

        public LoginInfo GetLoginInfo()
        {
            var loginInfo = (LoginInfo)mainGrid.Resources["loginInfo"];
            return loginInfo;
        }

        public Projects GetProjects()
        {
            var projects = (Projects)mainGrid.Resources["projects"];
            return projects;
        }

        public string GetPassword()
        {
            return passwordBox.Password;
        }

        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            foreach (var project in ((Projects)mainGrid.Resources["projects"]).Projects)
            {
                project.Name += "!";

                foreach (var projectUrl in project.Urls)
                {
                    projectUrl.UrlName += "!";
                }
            }
        }
    }
}