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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GridBandedView.Tutorial {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            DataContext = new DataList();
            InitializeComponent();
        }
    }
    public class DataList : List<Data> {
        public DataList() {
            for(int i = 0; i < 20; i++) {
                Data d = new Data() {
                    First = "First #" + i.ToString(),
                    Second = "Second #" + i.ToString(),
                };
                Add(d);
            }
        }
    }

    public class Data {
        public string First { get; set; }
        public string Second { get; set; }
    }
}
