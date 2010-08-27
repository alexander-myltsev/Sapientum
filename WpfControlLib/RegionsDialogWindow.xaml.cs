using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfControlLib
{
    /// <summary>
    /// Interaction logic for RegionsDialogWindow.xaml
    /// </summary>
    public partial class RegionsDialogWindow
    {
        private readonly List<CheckBox> _checkBoxes = new List<CheckBox>();

        public RegionsDialogWindow()
        {
            InitializeComponent();

            const string regions = "Москва и область	Санкт-Петербургиобласть	СтраныСНГ	Украина	Беларусь	Казахстан";
            const string regionsOfRussia = "ДальнийВосток	Поволжье	Северо-Запад	Сибирь	Урал	Центр	Юг";

            var regionsOfRussiaCheckboxes = regionsOfRussia
                .Split('\t')
                .Select(str =>
                            {
                                var checkBox = new CheckBox
                                                   {
                                                       Content = str,
                                                       Margin = new Thickness(5, 0, 0, 0),
                                                   };
                                return checkBox;
                            })
                .ToList();
            var russiaCheckbox = new CheckBox
            {
                Content = "Россия",
                Tag = regionsOfRussiaCheckboxes
            };
            russiaCheckbox.Checked += RussiaCheckboxCheckedChanged;
            russiaCheckbox.Unchecked += RussiaCheckboxCheckedChanged;

            stackPanelLeft.Children.Add(russiaCheckbox);
            regionsOfRussiaCheckboxes.ForEach(chkBox => stackPanelLeft.Children.Add(chkBox));
            var regionsCheckBoxes = regions
                .Split('\t')
                .Select(str =>
                            {
                                var checkBox = new CheckBox { Content = str };
                                return checkBox;
                            })
                .ToList();
            regionsCheckBoxes.ForEach(chkBox => stackPanelRight.Children.Add(chkBox));

            _checkBoxes.Add(russiaCheckbox);
            _checkBoxes.AddRange(regionsOfRussiaCheckboxes);
            _checkBoxes.AddRange(regionsCheckBoxes);
        }

        static void RussiaCheckboxCheckedChanged(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            ((List<CheckBox>)checkBox.Tag).ForEach(box => box.IsChecked = checkBox.IsChecked);
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
            var selected = _checkBoxes
                .Where(box => (bool)box.IsChecked)
                .Select(box => (string)box.Content)
                .ToArray();
            return selected;
        }
    }
}
