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
    /// Interaction logic for YacaCategoriesDialogWindow.xaml
    /// </summary>
    public partial class YacaCategoriesDialogWindow
    {
        private const string CategoriesLongString = @" Авто
Автолюбители
Автомобиль и закон
Запчасти, аксессуары
Мотоциклы
Подготовка водителей
Продажа автомобилей
Техническое обслуживание
 Бизнес
Все для офиса
Деловые услуги
Недвижимость
Производство и поставки
Реклама
Строительство
Универсальное
Финансы
 Дом
Все для праздника
Домашние животные
Здоровье
Квартира и дача
Кулинария
Мода и красота
Покупки
Семья
Универсальное
 Компьютеры
Hardware
Безопасность
Интернет
Интерфейс
Компьютеры
Мобильная связь
Программы
Сети и связь
Универсальное
 Культура
Изобразительные искусства
Кино
Литература
Музеи
Музыка
Танец
Театры
Универсальное
Фотография
 Общество
Власть
Законы
НКО
Политика
Прочее
Религия
 Отдых
Где развлечься
Туризм
Хобби
 Работа
 Развлечения
Знакомства
Игры
Непознанное
Прочее
Универсальное
Эротика
Юмор
 СМИ
Информационные агентства
Периодика
Прочее
Радио
Телевидение
Универсальное
 Спорт
Автоспорт
Баскетбол
Водный спорт
Единоборства
Зимние виды спорта
Конкурсы, тотализатор
Летние виды спорта
Прочее
Силовые виды спорта
Спортивные товары
Теннис
Универсальное
Футбол
Хоккей
Шахматы, шашки
Экстремальный спорт
 Справки
Афиша
Карты
Погода
Поиск людей
Словари
Транспорт
Энциклопедии
 Универсальное
 Учеба
Высшее образование
Курсы
Науки
Среднее образование
Универсальное
Учебные материалы";

        private readonly List<CheckBox> _checkBoxes = new List<CheckBox>();

        public YacaCategoriesDialogWindow()
        {
            InitializeComponent();

            var itemsPerColumn = new[] { 4, 4 + 6, 4 + 6 + 4 + 1 };
            var yacaCategories = CategoriesLongString.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            CheckBox parentCheckbox = null;
            int parentCheckboxIndex = -1;
            StackPanel stackPanel = stackPanelLeft;
            for (int yacaCategoryIndex = 0; yacaCategoryIndex < yacaCategories.Length; yacaCategoryIndex++)
            {
                CheckBox checkBox;
                var yacaCategory = yacaCategories[yacaCategoryIndex];
                if (yacaCategory.StartsWith(" "))
                {
                    parentCheckboxIndex++;
                    if (parentCheckboxIndex < itemsPerColumn[0])
                    {
                        //stackPanel = stackPanelLeft;
                    }
                    else if (parentCheckboxIndex >= itemsPerColumn[0] && parentCheckboxIndex < itemsPerColumn[1])
                        stackPanel = stackPanelCenter;
                    else if (parentCheckboxIndex >= itemsPerColumn[1] && parentCheckboxIndex <= itemsPerColumn[2])
                        stackPanel = stackPanelRight;
                    else throw new Exception("Unexpected yacaCategories index");

                    checkBox = new CheckBox { Content = yacaCategory.Trim() };
                    parentCheckbox = checkBox;
                    parentCheckbox.Tag = new List<CheckBox>(); // child CheckBoxes
                    parentCheckbox.Checked += ParentCheckboxCheckChanged;
                    parentCheckbox.Unchecked += ParentCheckboxCheckChanged;
                }
                else
                {
                    checkBox = new CheckBox
                                   {
                                       Content = yacaCategory,
                                       Margin = new Thickness(10, 0, 0, 0)
                                   };
                    if (parentCheckbox != null)
                        ((List<CheckBox>)parentCheckbox.Tag).Add(checkBox);
                }
                _checkBoxes.Add(checkBox);
                stackPanel.Children.Add(checkBox);
            }
        }

        private static void ParentCheckboxCheckChanged(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            var childCheckBoxes = (List<CheckBox>)checkBox.Tag;
            childCheckBoxes.ForEach(chkBox => chkBox.IsChecked = checkBox.IsChecked);
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void ForAllCheckBoxChanged(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            _checkBoxes.ForEach(chkBox => chkBox.IsChecked = checkBox.IsChecked);
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
