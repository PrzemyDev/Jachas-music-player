using Jachas_Player.ViewModel;
using System;
using System.Windows;
using System.Windows.Input;

namespace Jachas_Player
{

    public partial class MainWindow : Window
    {
        private void MainCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            var mainViewModel = DataContext as MainViewModel;
            mainViewModel.CanvasWidth = MainCanvas.ActualWidth;
            mainViewModel.CanvasHeight = MainCanvas.ActualHeight;
            mainViewModel.InitializeViewModel();
            mainViewModel.InitiateAnimationTimers();
        }
        private void MainViewModel_OnLaserGenerated(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Visible;
        }
        private void slider_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is MainViewModel mainViewModel)
            {
                mainViewModel.OnSliderPreviewMouseUp();
            }
        }
        MainViewModel mainViewModel = new MainViewModel();
        public MainWindow()
        {
            //InitializeComponent();
            mainViewModel.OnLaserGenerated += MainViewModel_OnLaserGenerated;
            mainViewModel.GenerateLasersAndShowLoadingScreen();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimizeWindow_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
