using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using JonglaInterview.ViewModels;
using JonglaInterview.Models;
using JonglaInterview.Helpers;

namespace JonglaInterview.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MapView : Window
    {
        public MapView()
        {
            InitializeComponent();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if (DataContext != null && ((MapViewModel)DataContext).Refresher == null)
            {
                //for testing purpose, just change here refresher and/or service, easy to mockup
                ((MapViewModel)DataContext).InitializeLoader(new TimerDataRefresher(), new JsonService());
                ((IDataRefresher)((MapViewModel)DataContext).Refresher).Start();
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            if (DataContext != null && ((MapViewModel)DataContext).Refresher != null)
                ((IDataRefresher) ((MapViewModel)DataContext).Refresher).Stop();
        }
    }
}
