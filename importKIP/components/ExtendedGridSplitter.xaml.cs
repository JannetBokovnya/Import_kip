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

namespace ImportKIP.components
{
    public partial class ExtendedGridSplitter : UserControl
    {
        public ExtendedGridSplitter()
        {
            InitializeComponent();

        }

        public event EventHandler CollapseButtonClicked;

        private void CollapseButton_OnClick(object sender, RoutedEventArgs routedEventArgs)
        {
           
            Button button = (Button)sender;
            button.Content = ((string)button.Content == "5") ? "6" : "5";

            if (CollapseButtonClicked != null)
            {
                CollapseButtonClicked(this, new EventArgs());
            }
        }
    }
}
