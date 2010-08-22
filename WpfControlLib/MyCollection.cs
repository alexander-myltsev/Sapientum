using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace WpfControlLib
{
    public class MyData
    {
        public MyData(int i)
        {
            I = i;
        }

        public int I { get; set; }
    }

    public class MyCollection : ObservableCollection<MyData>
    {
        public MyCollection()
        {
            for (int i = 0; i < 7; i++)
            {
                Add(new MyData(i));
            }
        }

        public List<int> Is
        {
            get
            {
                //return from data in this
                //       where data.I % 3 == 0
                //       select data.I * 3;
                return (from data in this
                        select data.I).ToList();
            }
        }
    }
}
