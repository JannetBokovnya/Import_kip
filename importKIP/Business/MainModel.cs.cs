using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ImportKIP.Business
{
    public class MainModel : INotifyPropertyChanged
    {

        public MainModel()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isShowBusy;
        private string _isBusyContent;

        public void FirePropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }

        public bool IsShowBusy
        {
            get { return _isShowBusy; }
            protected set
            {
                _isShowBusy = value;
                FirePropertyChanged("IsShowBusy");

            }
        }

        public string IsBusyContent
        {
            get { return _isBusyContent; }
            protected set
            {
                _isBusyContent = value;
                FirePropertyChanged("IsBusyContent");

            }
        }


    }
}
