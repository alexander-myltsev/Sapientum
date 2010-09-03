using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.FSharp.Collections;
using Microsoft.FSharp.Core;
using Sapientum;
using Sapientum.Types.Sape;
using Sapientum.Types.Wpf;

namespace WpfControlLib
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
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

        private readonly SiteCategoriesDialogWindow _siteCategoriesDialogWindow = new SiteCategoriesDialogWindow();
        private readonly YacaCategoriesDialogWindow _yacaCategoriesDialogWindow = new YacaCategoriesDialogWindow();
        private readonly RegionsDialogWindow _regionsDialogWindow = new RegionsDialogWindow();
        private readonly DomainZonesDialogWindow _domainZonesDialogWindow = new DomainZonesDialogWindow();

        public MainWindow()
        {
            InitializeComponent();

            //buttonLogin.Click += (sender, args) => ButtonLoginClick(sender, new LoginInfo { CurrentLogin = textBoxLogin.Text, PasswordHash = CalculateMd5Hash(passwordBox.Password) });
            buttonLogin.Click +=
                (sender, args) =>
                UIEventOccurred(this,
                                EventType.NewLogin(new LoginInfo
                                                       {
                                                           CurrentLogin = textBoxLogin.Text,
                                                           PasswordHash = CalculateMd5Hash(passwordBox.Password)
                                                       }));

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

#if DEBUG
            passwordBox.Password = "abc123";
            textBoxPriceTo.Text = "3";
            checkBoxNestedLevelMain.IsChecked = true;
            checkBoxNestedLevel2nd.IsChecked = true;
            //checkBoxNestedLevel3rd.IsChecked = true;
            textBoxWords.Text = "квартиры, челябинск";
            //textBoxPrFrom.Text = "1";
            //textBoxCitationIndexFrom.Text = "4";
#endif
        }

        //public delegate void ProjectUrlSelectedHandler(object sender, int projectUrlId);

        //public delegate void SearchSitesRequestHandler(object sender, CustomFilter filter);

        //public delegate void LoginAttemptHandler(object sender, LoginInfo loginInfo);

        //public delegate void WaitingSitesRefreshHandler(object sender, int projectUrlId);

        //public event LoginAttemptHandler ButtonLoginClick;

        //public event ProjectUrlSelectedHandler ProjectUrlSelected;

        //public event SearchSitesRequestHandler SearchSitesRequested;

        //public event WaitingSitesRefreshHandler WaitingSitesRefreshClick;

        public event UIEvent UIEventOccurred;

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

        public UIState GetUiState()
        {
            var uiState = (UIState)mainGrid.Resources["uiState"];
            return uiState;
        }

        public DataProviderForSearchedSites GetDataProviderForSearchedSites()
        {
            var provider = (DataProviderForSearchedSites)mainGrid.Resources["dataProviderForSearchedSites"];
            return provider;
        }

        public DataProviderForWaitingSites GetDataProviderForWaitingSites()
        {
            var provider = (DataProviderForWaitingSites)mainGrid.Resources["dataProviderForWaitingSites"];
            return provider;
        }

        public string GetPassword()
        {
            return passwordBox.Password;
        }

        //private void ButtonLoginClick__(object sender, RoutedEventArgs e)
        //{
        //    foreach (var project in ((Projects)mainGrid.Resources["projects"]).Projects)
        //    {
        //        project.Name += "!";

        //        foreach (var projectUrl in project.Urls)
        //        {
        //            projectUrl.UrlName += "!";
        //        }
        //    }
        //}

        private int _projectUrlIdSelected;
        private void RadioButtonChecked(object sender, RoutedEventArgs e)
        {
            var radioButton = (RadioButton)sender;
            Debug.WriteLine(string.Format("radioButton selected. radioButton.Tag: {0}", radioButton.Tag));
            _projectUrlIdSelected = (int)radioButton.Tag;
            //if (ProjectUrlSelected != null)
            //    ProjectUrlSelected(this, _projectUrlIdSelected);
        }

        private void SiteCategoriesChooseRadioButtonClick(object sender, RoutedEventArgs e)
        {
            var showDialog = _siteCategoriesDialogWindow.ShowDialog();
        }

        private void YacaCategoriesChooseRadioButtonClick(object sender, RoutedEventArgs e)
        {
            var showDialog = _yacaCategoriesDialogWindow.ShowDialog();
        }

        private void DomainZonesChooseRadioButtonClick(object sender, RoutedEventArgs e)
        {
            var showDialog = _domainZonesDialogWindow.ShowDialog();
        }

        private void RegionsChooseRadioButtonClick(object sender, RoutedEventArgs e)
        {
            var showDialog = _regionsDialogWindow.ShowDialog();
        }

        private void PopularDomainZonesChooseRadioButtonClick(object sender, RoutedEventArgs e)
        {
            _domainZonesDialogWindow.ChoosePopulart();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            _siteCategoriesDialogWindow.Owner = _domainZonesDialogWindow.Owner =
                _yacaCategoriesDialogWindow.Owner = _regionsDialogWindow.Owner = this;
        }

        private CustomFilter _customFilter;
        private void SearchSitesButtonClick(object sender, RoutedEventArgs e)
        {
            #region Garbage
            //// ----- SearchArea ----- 
            //SearchArea searchArea;
            //if (radioButtonAllSites.IsChecked == null ||
            //    radioButtonPrimaryBase.IsChecked == null ||
            //    radioButtonDubiousContent.IsChecked == null)
            //    throw new Exception();
            //if ((bool)radioButtonAllSites.IsChecked) searchArea = SearchArea.AllSites;
            //else if ((bool)radioButtonPrimaryBase.IsChecked) searchArea = SearchArea.PrimaryBase;
            //else if ((bool)radioButtonDubiousContent.IsChecked) searchArea = SearchArea.DubiousContent;
            //else throw new Exception();

            //// ----- PR ----- 
            //int prFrom, prTo;
            //var prFromOption = int.TryParse(textBoxPrFrom.Text, out prFrom)
            //                       ? FSharpOption<int>.Some(prFrom)
            //                       : FSharpOption<int>.None;
            //var prToOption = int.TryParse(textBoxPrTo.Text, out prTo)
            //                     ? FSharpOption<int>.Some(prTo)
            //                     : FSharpOption<int>.None;
            //var prTuple = new Tuple<FSharpOption<int>, FSharpOption<int>>(prFromOption, prToOption);

            //// ----- Current Citation Index ----- 
            //int cciFrom, cciTo;
            //var cciFromOption = int.TryParse(textBoxCitationIndexFrom.Text, out cciFrom)
            //                        ? FSharpOption<int>.Some(cciFrom)
            //                        : FSharpOption<int>.None;
            //var cciToOption = int.TryParse(textBoxCitationIndexTo.Text, out cciTo)
            //                      ? FSharpOption<int>.Some(cciTo)
            //                      : FSharpOption<int>.None;
            //var cciTuple = new Tuple<FSharpOption<int>, FSharpOption<int>>(cciFromOption, cciToOption);

            //// ----- External Links Count ----- 
            //int externalLinksCount;
            //var externalLinskCountOption = int.TryParse(textBoxExternalLinksCount.Text, out externalLinksCount)
            //                                   ? FSharpOption<int>.Some(externalLinksCount)
            //                                   : FSharpOption<int>.None;

            //// ----- Price Range -----
            //double priceFrom, priceTo;
            //var priceFromOption = double.TryParse(textBoxPriceFrom.Text, out priceFrom)
            //                          ? FSharpOption<double>.Some(priceFrom)
            //                          : FSharpOption<double>.None;
            //var priceToOption = double.TryParse(textBoxPriceTo.Text, out priceTo)
            //                          ? FSharpOption<double>.Some(priceTo)
            //                          : FSharpOption<double>.None;
            //var priceTuple = new Tuple<FSharpOption<double>, FSharpOption<double>>(priceFromOption, priceToOption);
            #endregion // Garbage

            _customFilter = CustomFilter.Create(_projectUrlIdSelected,
                                                               (bool)radioButtonPrimaryBase.IsChecked,
                                                               (bool)radioButtonDubiousContent.IsChecked,
                                                               (bool)radioButtonAllSites.IsChecked,
                                                               textBoxPrFrom.Text, textBoxPrTo.Text, textBoxCitationIndexFrom.Text,
                                                               textBoxCitationIndexTo.Text,
                                                               textBoxExternalLinksCount.Text,
                                                               textBoxExternalLinksForecastCount.Text,
                                                               textBoxPriceFrom.Text,
                                                               textBoxPriceTo.Text,
                                                               textBoxDomainDaysAge.Text, comboBoxIsDmoz.SelectedIndex,
                                                               comboBoxIsYaca.SelectedIndex,
                                                               (bool)radionButtonAllLevels.IsChecked,
                                                               (bool)radionButtonSecondLevel.IsChecked,
                                                               (bool)radionButtonThirdLevel.IsChecked,
                                                               (bool)checkBoxNestedLevelMain.IsChecked,
                                                               (bool)checkBoxNestedLevel2nd.IsChecked,
                                                               (bool)checkBoxNestedLevel3rd.IsChecked,
                                                               (bool)radioButtonSiteCategoriesIsAll.IsChecked,
                                                               _siteCategoriesDialogWindow.GetSelected(),
                                                               (bool)radioButtonYacaCategoriesIsAll.IsChecked,
                                                               _yacaCategoriesDialogWindow.GetSelected(),
                                                               (bool)radioButtonRegionsIsAll.IsChecked,
                                                               _regionsDialogWindow.GetSelected(),
                                                               (bool)radioButtonDomainZonesIsAll.IsChecked,
                                                               _domainZonesDialogWindow.GetSelected(),
                                                               textBoxWords.Text, comboBoxDateAdded.SelectedIndex,
                                                               (bool)checkBoxDontShowWithPlacedLinks.IsChecked,
                                                               comboBoxIsInYandex.SelectedIndex, comboBoxIsInGoogle.SelectedIndex,
                                                               (bool)radioButtonPagesFromSiteSingle.IsChecked,
                                                               (bool)radioButtonPagesFromSiteOptimal.IsChecked,
                                                               (bool)radioButtonPagesFromSiteAll.IsChecked);
            //SearchSitesRequested(this, customFilter);
            UIEventOccurred(this, EventType.NewSearchSites(_customFilter));
        }

        private void WaitingSitesButtonOnClick(object sender, RoutedEventArgs e)
        {
            //WaitingSitesRefreshClick(this, _projectUrlIdSelected);
            UIEventOccurred(this, EventType.NewWaitingSitesRefresh(_projectUrlIdSelected));
        }

        private void DownloadOpenedSites(object sender, RoutedEventArgs e)
        {
            UIEventOccurred(this, EventType.NewDownloadFoundOpenedSites(new Tuple<int, CustomFilter, FSharpList<Site>>(_projectUrlIdSelected, _customFilter, GetUiState().OpenedSites)));
            GetUiState().OpenedSites = FSharpList<Site>.Empty;
        }

        private void PlaceClosedSites(object sender, RoutedEventArgs e)
        {
            UIEventOccurred(this, EventType.NewPlaceFoundClosedSites(new Tuple<int, CustomFilter, FSharpList<Site>>(_projectUrlIdSelected, _customFilter, GetUiState().ClosedSites)));
            GetUiState().ClosedSites = FSharpList<Site>.Empty;
        }

        private void BuyWaitingSitesButtonOnClick(object sender, RoutedEventArgs e)
        {
            UIEventOccurred(this, EventType.NewPlaceWaitingPages(_projectUrlIdSelected));
        }

        private void BuySearchedSitesButtonOnClick(object sender, RoutedEventArgs e)
        {
            UIEventOccurred(this, EventType.NewPlaceOpenedPages(_projectUrlIdSelected));
        }

        private void HighlightWaitingSitesTitlesClick(object sender, RoutedEventArgs e)
        {
            var words = textBoxHighlightWaitingSitesTitles.Text.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; i++)
                words[i] = words[i].ToLower();
            UIEventOccurred(this, EventType.NewHighlightWaitingPages(words));
        }

        private void HighlightSearchedSitesTitlesClick(object sender, RoutedEventArgs e)
        {
            var words = textBoxHighlightSearchedSitesTitles.Text.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; i++)
                words[i] = words[i].ToLower();
            UIEventOccurred(this, EventType.NewHighlightSearchedPages(words));
        }
    }
}