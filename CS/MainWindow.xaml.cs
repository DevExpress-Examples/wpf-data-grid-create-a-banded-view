using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;

namespace BandedViewSample {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            List<TestData> list = new List<TestData>();
            for(int i = 0; i < 100; i++)
                list.Add(new TestData() { Id = i, Text = "Row" + i, Number = i, MultiLineText = "Row" + i + "Line0" + Environment.NewLine + "Row" + i + "Line1" + Environment.NewLine + "Row" + i + "Line2" });
            DataContext = list;
        }
    }

    public class TestData {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Number { get; set; }
        public string MultiLineText { get; set; }
    }
}
