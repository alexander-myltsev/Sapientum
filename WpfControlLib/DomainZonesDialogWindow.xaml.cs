using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfControlLib
{
    /// <summary>
    /// Interaction logic for DomainZonesDialogWindow.xaml
    /// </summary>
    public partial class DomainZonesDialogWindow
    {
        private readonly List<CheckBox> _checkBoxes = new List<CheckBox>();

        public DomainZonesDialogWindow()
        {
            InitializeComponent();

            const string allDomainZones = @".ru .com .ua .info .net .org .cn .su .biz .uz .kz .by
.name .ws .in .us .tj .uk .lv .md .cc .am .kg .eu .de .tv .pl .mobi .gg .ee .cz .nl
.it .il .me .ca .at .au .be .ch .ge .bz .az .cx .lt .dk .asia .fm .gr .za .es .nu
.no .ro .ms .edu .fi .im .se .tk .an .br .ec .hu .ie .mn .sk .st .th .to .travel .bg
.cd .dj .gs .hr .id .io .jp .kr .li .nz .pro .pt .sh .tr .tw .vc .vg .vu ";

            var strings = allDomainZones.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            var stackPanels = new[] { stackPanel0, stackPanel1, stackPanel2, stackPanel3, stackPanel4 };
            for (int i = 0; i < strings.Length; i++)
            {
                var stackPanel = stackPanels[i];
                strings[i]
                    .Split(' ')
                    .ToList()
                    .ForEach(domainZone =>
                                 {
                                     var checkBox = new CheckBox { Content = domainZone };
                                     stackPanel.Children.Add(checkBox);
                                     _checkBoxes.Add(checkBox);
                                 });
            }
        }

        public void ChoosePopulart()
        {
            _checkBoxes.ForEach(box => box.IsChecked = false);
            ".ru .com .ua .info .net .org .cn .su .biz .uz .kz .by"
                .Split(' ')
                .ToList()
                .ForEach(str => _checkBoxes.Find(box => (string)box.Content == str).IsChecked = true);
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void ForAllCheckBoxChanged(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            _checkBoxes.ForEach(box => box.IsChecked = checkBox.IsChecked);
        }

        public string[] GetSelected()
        {
            return _checkBoxes
                .Where(box => (bool)box.IsChecked)
                .Select(box => (string)box.Content)
                .ToArray();
        }
    }
}
