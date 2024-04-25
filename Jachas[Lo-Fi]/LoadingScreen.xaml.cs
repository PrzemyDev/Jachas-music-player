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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Jachas_Lo_Fi_
{
    /// <summary>
    /// Logika interakcji dla klasy LoadingScreen.xaml
    /// </summary>
    public partial class LoadingScreen : Window
    {
        int timerTest = 0;
        private void TimerEvent(object sender, EventArgs e)
        {
            RotateTransform rotateTransform2 = new RotateTransform(timerTest)
            {
                CenterX = 0,
                CenterY = 0
            };
            JachasSpin.RenderTransform = rotateTransform2;
            timerTest++;
        }

        public LoadingScreen()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += TimerEvent;
            timer.Start();
        }
    }
}
