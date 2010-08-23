using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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

        public delegate void ProjectUrlSelectedHandler(object sender, int projectUrlId);

        public event ProjectUrlSelectedHandler ProjectUrlSelected;

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

        private void RadioButtonChecked(object sender, RoutedEventArgs e)
        {
            var radioButton = (RadioButton)sender;
            Debug.WriteLine(string.Format("radioButton selected. radioButton.Tag: {0}", radioButton.Tag));
            ProjectUrlSelected(sender, (int)radioButton.Tag);
        }

        private void SiteCategoriesChooseRadioButtonClick(object sender, RoutedEventArgs e)
        {
            var siteCategoriesDialogWindow = new SiteCategoriesDialogWindow { Owner = this };
            var showDialog = siteCategoriesDialogWindow.ShowDialog();
        }

        private void YacaCategoriesChooseRadioButtonClick(object sender, RoutedEventArgs e)
        {
            var yacaCategoriesDialogWindow = new YacaCategoriesDialogWindow { Owner = this };
            var showDialog = yacaCategoriesDialogWindow.ShowDialog();
        }

        private void DomainZonesChooseRadioButtonClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RegionsChooseRadioButtonClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void PopularDomainZonesChooseRadioButtonClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}