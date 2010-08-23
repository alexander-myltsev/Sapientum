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
    /// Interaction logic for SiteCategoriesDialogWindow.xaml
    /// </summary>
    public partial class SiteCategoriesDialogWindow
    {
        private const string CategoriesLongString = @"English sites
Hi-End
MP3
Авто
Банки
Безопасность
Бесплатное
Блоги
Бухгалтерия
Города и регионы
Государство
Дом и семья
Знакомства и общение
Игры
Интернет
Кино
Компьютеры
Консалтинг
Культура и искусство
Литература
Мебель
Медицина
Музыка
Наука и техника
Недвижимость
Непознанное
Новости и СМИ
Обучение
Общество
Персональные страницы
Погода
Политика
Политические партии
Предприятия
Промышленность
Путешествия
Работа
Развлечения
Реклама
Сайты для взрослых
Связь
Софт
Спорт
Справки
Страхование
Строительство
Телевидение
Товары и услуги
Финансы
Флора и фауна
Фото
Хостинг
Юмор ";

        private readonly List<CheckBox> _checkBoxes;

        public SiteCategoriesDialogWindow()
        {
            InitializeComponent();

            _checkBoxes = CategoriesLongString.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
               .Select(str => new CheckBox { Content = str })
               .ToList();
            for (int i = 0; i < _checkBoxes.Count; i++)
            {
                StackPanel stackPanel = i < _checkBoxes.Count / 2 ? stackPanelLeft : stackPanelRight;
                stackPanel.Children.Add(_checkBoxes[i]);
            }
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ForAllCheckBoxChanged(object sender, RoutedEventArgs e)
        {
            var forAllCheckbox = (CheckBox)sender;
            _checkBoxes.ForEach(chkBox => chkBox.IsChecked = forAllCheckbox.IsChecked);
        }
    }
}
