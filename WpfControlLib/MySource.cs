using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfControlLib
{
    public class MySource : INotifyPropertyChanged
    {
        //public MySource()
        //{
        //    ThreadPool.QueueUserWorkItem(
        //        state =>
        //        {
        //            while (true)
        //            {
        //                Name += ".";
        //                PropertyChanged(this, new PropertyChangedEventArgs("Name"));
        //                Thread.Sleep(1000);
        //            }
        //        });
        //}

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Name"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    [ValueConversion(typeof(Color), typeof(SolidColorBrush))]
    public class ColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (Color)value;
            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

}