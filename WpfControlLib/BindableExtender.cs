using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace WpfControlLib
{
    public static class BindableExtender
    {

        public static Table GetBindableTable(DependencyObject obj)
        {
            return (Table)obj.GetValue(BindableTableProperty);
        }

        public static void SetBindableTable(DependencyObject obj, Table tbl)
        {
            obj.SetValue(BindableTableProperty, tbl);
        }

        public static string GetBindableText(DependencyObject obj)
        {
            return (string)obj.GetValue(BindableTextProperty);
        }

        public static void SetBindableText(DependencyObject obj, string value)
        {
            obj.SetValue(BindableTextProperty, value);
        }

        public static readonly DependencyProperty BindableTextProperty =
            DependencyProperty.RegisterAttached("BindableText",
                                                typeof (string),
                                                typeof (BindableExtender),
                                                new UIPropertyMetadata(null, BindableProperty_PropertyChanged));

        public static readonly DependencyProperty BindableTableProperty =
            DependencyProperty.RegisterAttached("BindableTable",
                typeof(Table), typeof(BindableExtender), new UIPropertyMetadata(null, BindableProperty_PropertyChanged));

        private static void BindableProperty_PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is Run)
            {
                ((Run)dependencyObject).Text = (string)e.NewValue;
            }

            if (dependencyObject is FlowDocument)
            {
                ((FlowDocument)dependencyObject).Blocks.Clear();
                if (e.NewValue != null)
                {
                    ((FlowDocument)dependencyObject).Blocks.Add((Block)e.NewValue);
                }
            }
        }
    }
}
