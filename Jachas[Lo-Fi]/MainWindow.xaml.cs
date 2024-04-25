using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace Jachas_Lo_Fi_
{
   
    public partial class MainWindow : Window
    {
        //Music player:
        private MediaPlayer mediaPlayer = new MediaPlayer();

        private void btnOpenAudioFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                mediaPlayer.Open(new Uri(openFileDialog.FileName));
                lblMusicName.Content = $"Now Playing: {openFileDialog.SafeFileName}";
            }
            DispatcherTimer musicTimer = new DispatcherTimer();
            musicTimer.Interval = TimeSpan.FromSeconds(1);
            musicTimer.Tick += musicTimer_Tick;
            musicTimer.Start();
        }
        double CurrentPosition, FinalPosition, Procentage;
        void musicTimer_Tick(object sender, EventArgs e)
        {
            if (mediaPlayer.Source != null)
            {
                lblStatus.Content = String.Format("{0} / {1}", mediaPlayer.Position.ToString(@"mm\:ss"),
                    mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));

                CurrentPosition = mediaPlayer.Position.TotalSeconds;
                FinalPosition = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                //z = (x * 100) / y;  
                Procentage = Math.Round((CurrentPosition / FinalPosition) * 100, 0);
                slider.Value = Procentage;
            }
            else
                lblStatus.Content = "No file selected...";
        }
        private void slider_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            int SliderValue = (int)slider.Value;
            CurrentPosition = ((FinalPosition * SliderValue) / 100);
            int pośrednik = Convert.ToInt32(CurrentPosition);
            TimeSpan timeSpan = new TimeSpan(0, 0, pośrednik);
            mediaPlayer.Position = timeSpan;
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Play();
        }
        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Pause();
        }
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
        }
        //End of Music player part

        //Spawn new LaserBall
        private void GenerateNewRGBBallz(double test1, double test2, double test3, double test4, byte R, byte G, byte B)
        {
            Ellipse NewEllipse = new Ellipse
            {
                Name = "newellipse",
                Height = 25,
                Width = 25,
                Stroke = Brushes.Transparent,
                Opacity = 0.2,
                Fill = new SolidColorBrush(Color.FromRgb(R, G, B)),
                Margin = new Thickness(test1, test2, test3, test4)
            };
            Canvas.Children.Add(NewEllipse);
            Console.WriteLine(NewEllipse.Margin.ToString());
        }

        private void MoveOnTime()
        {
            RotateTransform rotateTransform2 = new RotateTransform(RotateAngleIGuess);
            rotateTransform2.CenterX = 50;
            rotateTransform2.CenterY = 1;
            RotateAngleIGuess++; RotateAngleIGuess++; RotateAngleIGuess++; RotateAngleIGuess++;
            foreach (Ellipse kulki in Canvas.Children.OfType<Ellipse>())
            {
                //Canvas.SetTop(kulki, z += 1);
                //Canvas.SetLeft(kulki, z);
                //z = -z;

                kulki.RenderTransform = rotateTransform2;
            }
        }

        int RotateAngleIGuess = 0;
        private void TimerEvent(object sender, EventArgs e)
        {
            MoveOnTime();
        }

        readonly string[] dymek = { "/images/D1.png", "/images/D2.png", "/images/D3.png" };
        int dswitch = 0;
        private void TimerEvent2(object sender, EventArgs e)
        {
            if (dswitch > 2) dswitch = 0;
            Dymek.Source = new BitmapImage(new Uri(dymek[dswitch], UriKind.Relative));
            dswitch++;
        }

        //Initialize loading screen
        LoadingScreen tempWindow = new LoadingScreen(); 
        private void ThreadStartingPoint()
        {
            LoadingScreen tempWindow = new LoadingScreen();
            tempWindow.Show();
            System.Windows.Threading.Dispatcher.Run();
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Aby odtwarzać muzykę, wybierz plik mp3, następnie wciśnij Play. Jeżeli używasz programu w tle i obciąża on mocno komputer, " +
                "wcisnij przycisk 'Animacje', który zatrzyma działanie animacji, zwalniając pamięć procesora.", "Pomoc");
        }

        DispatcherTimer Dtimer1 = new DispatcherTimer(); //Laserballs
        DispatcherTimer Dtimer2 = new DispatcherTimer(); //Smoke
        bool Efects = true;

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Program utworzony w celach humorystycznych. Postać w tle jest własnością Bartosza Walaszka. Utwory dołączone do aplikacji" +
                " są wolne od praw autorskich. Dziękuję za korzystanie z aplikacji! ~Przemy", "Informacje");
        }
        //Animation Button On/Off
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Efects)
            {
                Dtimer1.Stop();
                Dtimer2.Stop();
                Efects = false;
            }
            else
            {
                Dtimer1.Start();
                Dtimer2.Start();
                Efects = true;
            }
        }


            public MainWindow()
            {
            InitializeComponent();

            //LoadingScreen thread 
            Thread newWindowThread = new Thread(new ThreadStart(ThreadStartingPoint));
            newWindowThread.SetApartmentState(ApartmentState.STA);
            newWindowThread.IsBackground = true;
            newWindowThread.Start();

            //Generating laser balls
            for (int i = 0; i < 50; i++)
            {

                Random random = new Random();

                byte R, G, B;
                R = (byte)random.Next(0, 255);
                G = (byte)random.Next(0, 255);
                B = (byte)random.Next(0, 255);

                int l, t, r, b, timediff;
                l = random.Next(0, 750);
                t = random.Next(0, 400);
                r = random.Next(0, 2);
                b = random.Next(0, 2);

                r = -r; b = -b;
                timediff = random.Next(1, 100);
                GenerateNewRGBBallz(l, t, r, b, R, G, B);
                Thread.Sleep(timediff);
            }

            newWindowThread.Abort();
            tempWindow.Close();
            Dymek.Margin = new Thickness(520, -41, 0, 0);
            this.WindowState = WindowState.Minimized;
            this.Show();
            this.WindowState = WindowState.Normal;

            Dtimer1.Interval = TimeSpan.FromMilliseconds(15);
            Dtimer1.Tick += TimerEvent;
            Dtimer1.Start();

            //One generated laser ball, doesn't work without it:
            GenerateNewRGBBallz(800, 450, 450, 850, 25, 100, 125);

            //Dtimer2 - Smoke
            Dtimer2.Interval = TimeSpan.FromMilliseconds(200);
            Dtimer2.Tick += TimerEvent2;
            Dtimer2.Start();
        }
    }
}
