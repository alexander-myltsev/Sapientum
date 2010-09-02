using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using Microsoft.FSharp.Collections;
using Sapientum.Types.Sape;
using Sapientum.Types.Wpf;
using Project = Sapientum.Types.Wpf.Project;

namespace WpfControlLib
{
    namespace Converters
    {
        [ValueConversion(typeof(LoginStatus), typeof(bool))]
        public class LoginStatusToTabsIsEnabledConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                //#if DEBUG
                //                return true;
                //#else
                var loginStatus = (LoginStatus)value;
                bool result;
                switch (loginStatus)
                {
                    case LoginStatus.LoggedIn:
                        result = true;
                        break;
                    case LoginStatus.LoginFailed:
                    case LoginStatus.LoggingIn:
                    case LoginStatus.LoggedOff:
                        result = false;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                return result;
                //#endif
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotSupportedException("Cannot convert back");
            }
        }

        [ValueConversion(typeof(LoginStatus), typeof(bool))]
        public class LoginStatusToButtonIsEnabledConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var loginStatus = (LoginStatus)value;
                bool result;
                switch (loginStatus)
                {
                    case LoginStatus.LoggedOff:
                    case LoginStatus.LoginFailed:
                    case LoginStatus.LoggedIn:
                        result = true;
                        break;
                    case LoginStatus.LoggingIn:
                        result = false;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                return result;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotSupportedException("Cannot convert back");
            }
        }

        [ValueConversion(typeof(LoginInfo), typeof(string))]
        public class LoginStatusToStatusBarMsgConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var loginInfo = (LoginInfo)value;
                string result;
                switch (loginInfo.Status)
                {
                    case LoginStatus.LoggedOff:
                        result = "Не зарегистрирован";
                        break;
                    case LoginStatus.LoggingIn:
                        result = "Идет регистрация...";
                        break;
                    case LoginStatus.LoggedIn:
                        result = string.Format("Регистрация пройдена для {0}. Баланс: {1}", loginInfo.LoggedLogin, loginInfo.Balance);
                        break;
                    case LoginStatus.LoginFailed:
                        result = "Регистрация не пройдена";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                return result;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotSupportedException("Cannot convert back");
            }
        }

        [ValueConversion(typeof(List<Project>), typeof(List<Project>))]
        public class IdConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return value;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotSupportedException("Cannot convert back");
            }
        }

        [ValueConversion(typeof(List<Site>), typeof(List<string>))]
        public class DownloadSearchedSitesButtonContentConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var val = (FSharpList<Site>)value;
                return "Скачать " + val.Length + " открытых площадок";
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotSupportedException("Cannot convert back");
            }
        }

        [ValueConversion(typeof(List<Site>), typeof(List<string>))]
        public class BuyWaitingSitesButtonContentConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var val = (FSharpList<Site>)value;
                return "Разместить " + val.Length + " закрытых площадок";
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotSupportedException("Cannot convert back");
            }
        }
    }
}