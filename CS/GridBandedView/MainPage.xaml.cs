using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using DevExpress.Xpf.Core;
using System.IO;
using DevExpress.Xpf.Utils;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using DevExpress.Xpf.Core.Native;
#if SILVERLIGHT
using DevExpress.Xpf.Theming;
#else
using System.Data;
using System.IO.Compression;
#endif

namespace GridBandedView {
    public partial class MainPage : UserControl {
        public MainPage() {
            ThemeManager.ApplicationThemeName = "Seven";
            InitializeComponent();
            advBandedView.DataContext = CarsData.DataSource;
            simpleBandedView.DataContext = CarsData.DataSource;
        }
        private void Button_Click(object sender, RoutedEventArgs e) {
            ThemeManager.ApplicationThemeName = ((Button)sender).Content.ToString();
        }
    }
}
