using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;

namespace WpfControlLib
{
    public class SomeData
    {
        public SomeData(Uri uri, int outerLinks, int pr, float price, string title)
        {
            Uri = uri;
            OuterLinks = outerLinks;
            Pr = pr;
            Price = price;
            Title = title;
        }

        public Uri Uri { get; set; }
        public int OuterLinks { get; set; }
        public int Pr { get; set; }
        public float Price { get; set; }
        public string Title { get; set; }
    }

    public static class DataProviderCs
    {
        public static List<SomeData> GetData()
        {
            var someDatas = new List<SomeData>(
                Enumerable.Range(3, 9)
                    .Select(num =>
                                {
                                    string uri = "http://uri.ru", title = "Title";
                                    for (int i = 0; i < num; i++)
                                    {
                                        uri += "/&*#@$";
                                        title += title + " ";
                                    }
                                    var someData = new SomeData(new Uri(uri + num), num * 20, num * 300, (float)num / 10, title);
                                    return someData;
                                }));
            someDatas.Reverse();
            return someDatas;
        }

        private static Table BuildTable()
        {
            Table tbl = new Table();
            TableRowGroup rowGrp = new TableRowGroup();

            tbl.RowGroups.Add(rowGrp);

            TableRow row = new TableRow();
            rowGrp.Rows.Add(row);
            // you can style a row ...
            // row.Style = Application.Current.TryFindResource("HeaderRowStyle") as Style;
            row.Cells.Add(new TableCell(new Paragraph(new Run("DataLabel:"))));


            Paragraph pg = new Paragraph();
            // or you can style a Paragraph
            // I couldn't get pg.SetResourceReference(Paragraph.StyleProperty, "DataStyle") to work
            // it seems to never locate the Style resource properly ... the other approach below works
            // however
            // pg.Style = Application.Current.TryFindResource("DataStyle") as Style;
            pg.Inlines.Add(new Run("Some data"));
            row.Cells.Add(new TableCell(pg));

            return tbl;
        }

        public static Table Metadata
        {
            get
            {
                return BuildTable();
            }
        }
    }

    public class DataProvider1
    {
        Table tbl;

        private Table BuildTable()
        {
            if (tbl != null) return tbl;

            tbl = new Table();

            for (int i = 0; i < 200; i++)
            {
                var rowGrp = new TableRowGroup();
                tbl.RowGroups.Add(rowGrp);
                var row = new TableRow();
                rowGrp.Rows.Add(row);
                // you can style a row ...
                // row.Style = Application.Current.TryFindResource("HeaderRowStyle") as Style;
                //row.Cells.Add(new TableCell(new Paragraph(new Run("DataLabel:"))));
                row.Cells.Add(new TableCell(new Paragraph(new Run("datarow" + i))));

                var pg = new Paragraph();
                // or you can style a Paragraph
                // I couldn't get pg.SetResourceReference(Paragraph.StyleProperty, "DataStyle") to work
                // it seems to never locate the Style resource properly ... the other approach below works
                // however
                // pg.Style = Application.Current.TryFindResource("DataStyle") as Style;
                //pg.Inlines.Add(new Run("Some data"));

                var stackPanel = new StackPanel();
                var label1 = new Label { Content = "1-Contents-" + i };
                var label2 = new Label { Content = "2-Contents-" + i };
                var label3 = new Label { Content = "3-Contents-" + i };
                var label4 = new Label { Content = "4-Contents-" + i };
                stackPanel.Children.Add(label1);
                stackPanel.Children.Add(label2);
                stackPanel.Children.Add(label3);
                stackPanel.Children.Add(label4);

                pg.Inlines.Add(stackPanel);

                row.Cells.Add(new TableCell(pg));
            }

            return tbl;
        }

        public Table Metadata
        {
            get
            {
                return BuildTable();
            }
        }
    }
}
