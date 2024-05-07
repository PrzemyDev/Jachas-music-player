using System.Windows;
using System.Windows.Input;

namespace Jachas_Player
{
    public partial class LoadingScreen : Window
    {
        public LoadingScreen()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
