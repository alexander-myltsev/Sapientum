using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow
    {
        public TestWindow()
        {
            InitializeComponent();
        }

        private int _counter = 100;
        private void Button1Click(object sender, RoutedEventArgs e)
        {
            var observableCollection = (ObservableCollection<MyData>)Resources["listingDataView"];
            observableCollection.Add(new MyData(_counter++));
        }
    }
}
