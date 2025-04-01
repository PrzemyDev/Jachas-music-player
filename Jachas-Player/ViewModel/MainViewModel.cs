using Jachas_Lo_Fi_.Core;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Jachas_Player.ViewModel
{
    internal class EllipseData : ObservableObject
    {
        private double _rotationAngle;
        public double RotationAngle
        {
            get { return _rotationAngle; }
            set
            {
                _rotationAngle = value;
                OnPropertyChanged(nameof(RotationAngle));
            }
        }

        private double _x;
        public double X
        {
            get { return _x; }
            set
            {
                _x = value;
                OnPropertyChanged(nameof(X));
            }
        }

        private double _y;
        public double Y
        {
            get { return _y; }
            set
            {
                _y = value;
                OnPropertyChanged(nameof(Y));
            }
        }
        //Działa z obrotem wokół osi - lambda
        public double CenterX => X + 10; // Dodaj 10 do X 
        public double CenterY => Y + 10; // Dodaj 10 do Y 


        private Brush _fill;

        public Brush Fill
        {
            get { return _fill; }
            set { _fill = value; } 
        }
        private double _opacity;

        public double Opacity
        {
            get { return _opacity; }
            set { _opacity = value; }
        }
    }
    class MainViewModel : ObservableObject
    {
        //Muzyka
        private double _sliderValue;
        public double SliderValue
        {
            get
            {
                return _sliderValue;
            }
            set
            {
                _sliderValue = value;
                OnPropertyChanged();
            }
        }
        private string _currentMusicName;

        public string CurrentMusicName
        {
            get { return _currentMusicName; }
            set { _currentMusicName = value;
                OnPropertyChanged();
            }
        }
        private string _musicStatus;

        public string MusicStatus
        {
            get { return _musicStatus; }
            set { _musicStatus = value;
                OnPropertyChanged();
            }
        }


        //Elipsy
        private ObservableCollection<EllipseData> _ellipses = new ObservableCollection<EllipseData>();
        public ObservableCollection<EllipseData> Ellipses
        {
            get { return _ellipses; }
            set
            {
                _ellipses = value;
                OnPropertyChanged(nameof(Ellipses));
            }
        }

        private double _canvasWidth;
        public double CanvasWidth
        {
            get { return _canvasWidth; }
            set
            {
                _canvasWidth = value;
                OnPropertyChanged(nameof(CanvasWidth));
            }
        }

        private double _canvasHeight;
        public double CanvasHeight
        {
            get { return _canvasHeight; }
            set
            {
                _canvasHeight = value;
                OnPropertyChanged(nameof(CanvasHeight));
            }
        }
        private double _rotationAngle;
        public double RotationAngle
        {
            get { return _rotationAngle; }
            set
            {
                _rotationAngle = value;
                OnPropertyChanged(nameof(RotationAngle));
            }
        }
        //Dym:
        private ImageSource _displayedImage;
        public ImageSource DisplayedImage
        {
            get { return _displayedImage; }
            set
            {
                _displayedImage = value;
                OnPropertyChanged();
            }
        }
        byte smokeSwitcher = 2; 
        string filepath;
        internal readonly string[] Smokes = { "/images/D1.png", "/images/D2.png", "/images/D3.png" };
        public string currentSmoke;
        double CurrentSlidersPosition, SlidersProcentage;

        public event EventHandler OnLaserGenerated; //bt - event przekazanie informacji po zakończeniu
      
        public RelayCommand PlayPausecommand { get; set; }
        public RelayCommand StopButtonCommand { get; set; }
        public RelayCommand LoadFileCommand { get; set; }
        public RelayCommand InfoCommand { get; set; }
        public RelayCommand HelpCommand { get; set; }
        public RelayCommand AnimationControlCommand { get; set; }
        public RelayCommand SliderMouseUpCommand { get; set; }

        private MediaPlayer mediaPlayer = new MediaPlayer();
        DispatcherTimer musicTimer = new DispatcherTimer();
        DispatcherTimer smokeTimer = new DispatcherTimer();
        DispatcherTimer laserTimer = new DispatcherTimer();

        bool isMediaPlayerPlaying = false;

        public MainViewModel()
        
        {
            LoadFileCommand = new RelayCommand(o => OpenAudioFile());
            PlayPausecommand = new RelayCommand(o => PlayPause());
            StopButtonCommand = new RelayCommand(o => Stop());
            InfoCommand = new RelayCommand(o => ShowInfoMessageBox());
            HelpCommand = new RelayCommand(o => ShowHelpMessageBox());
            AnimationControlCommand = new RelayCommand(o => AnimationControl());
            Ellipses = new ObservableCollection<EllipseData>();
        }

        internal void InitializeViewModel()
        {   
            int rows = 5; // liczba wierszy
            int cols = 15; // liczba kolumn
            double cellWidth = CanvasWidth / cols;
            double cellHeight = CanvasHeight / rows;

            Random random = new Random();
            List<EllipseData> ellipses = new List<EllipseData>();
            int j = 0;
            for (int i = 1; i <= 13; i++) //30 //15 debug only
            {
                if (j == 4) { break; } //Nowy wiersz

                int col = i;
                int row = j;
                if (i % 13 == 0) 
                { j++; i = 0; }

                //Oblicz współrzędne elipsy na podstawie wybranej komórki - równa siatka
                //double x = col * cellWidth + cellWidth  /2;
                //double y = row * cellHeight + cellHeight /2;
                double x = col * cellWidth + cellWidth  / random.Next(1,6);
                double y = row * cellHeight + cellHeight / random.Next(1,3);

                byte R, G, B; // Losowe kolory
                R = (byte)random.Next(0, 255);
                G = (byte)random.Next(0, 255);
                B = (byte)random.Next(0, 255);
                EllipseData ellipse = new EllipseData
                {
                    X = x,
                    Y = y,
                    RotationAngle = 0,
                    Fill = new SolidColorBrush(Color.FromRgb(R, G, B)),
                    Opacity = 0.3
                };
                ellipses.Add(ellipse);

            }
            Ellipses = new ObservableCollection<EllipseData>(ellipses);

        }
        internal void InitiateAnimationTimers()
        {
            laserTimer.Interval = TimeSpan.FromMilliseconds(15);
            musicTimer.Interval = TimeSpan.FromSeconds(1);
            musicTimer.Tick += MusicTimer_Tick;
            smokeTimer.Interval = TimeSpan.FromMilliseconds(200);
            smokeTimer.Tick += SmokeAnims_tick;
            laserTimer.Start();
            smokeTimer.Start();
            MoveLasers(); //zapis laserTimer.Tick += (add) LaserTimer_Tick, druga możliwość zapisu (informacja)
        }
        internal void MoveLasers()
        {
            laserTimer.Tick += (sender, e) =>
            {
                foreach (var ellipse in Ellipses)
                {
                    ellipse.RotationAngle += 8; // kąt obrotu +8 (wpływa na "szybkość")
                }
            };
        }
        
        private void ShowHelpMessageBox()
        {
            MessageBox.Show("Aby odtwarzać muzykę, wybierz plik mp3, następnie wciśnij Play. Jeżeli używasz programu w tle i obciąża on mocno komputer, " +
                "wcisnij przycisk 'Animacje', który zatrzyma działanie animacji, zwalniając pamięć procesora.", "Pomoc");
        }
        private void ShowInfoMessageBox()
        {
            MessageBox.Show("Program utworzony w celach humorystycznych. Postać w tle jest własnością Bartosza Walaszka. Dziękuję za korzystanie z aplikacji! ~PrzemyDev", "Informacje");
        }
        private void AnimationControl()
        {
            if (laserTimer.IsEnabled == true) { 
                laserTimer.Stop();
                smokeTimer.Stop();
            }
            else { 
                laserTimer.Start();
                smokeTimer.Start();
            }

        }
        internal async void GenerateLasersAndShowLoadingScreen()
        {
            try
            {
                LoadingScreen loadingScreen = new LoadingScreen();
                loadingScreen.Show();
                await Task.Run(() =>
                {
                    InitializeViewModel();
                    Task.Delay(3500).Wait(); //Celowe opóźnienie - pokazanie ekranu ładowania
                });

                OnLaserGenerated?.Invoke(this, EventArgs.Empty); // Powiadom backcode, że elipsy się wygenerowały
                loadingScreen.Close();
            }
            catch (Exception exGenerateLasersAndShowLoadingScreen)
            {
                MessageBox.Show(exGenerateLasersAndShowLoadingScreen.Message);
            }
        }
        private void SmokeAnims_tick(object sender, EventArgs e)
        {
            try
            {
                if (smokeSwitcher > 2)
                    smokeSwitcher = 0;
                filepath = Smokes[smokeSwitcher];
                DisplayedImage = new BitmapImage(new Uri(Smokes[smokeSwitcher], UriKind.Relative));
                smokeSwitcher++;
            }
            catch (Exception exsmokeAnimsTick)
            {
                MessageBox.Show(exsmokeAnimsTick.Message);
            }
        }
        private void MusicTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                UpdateMusicDuration();
                //Current position / Final position -> w %
                SlidersProcentage = Math.Round((mediaPlayer.Position.TotalSeconds / mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds) * 100, 0);
                SliderValue = SlidersProcentage;
            }
            catch (Exception exmusicTimerTick)
            {
                MessageBox.Show(exmusicTimerTick.Message);
            }
        }
        internal void OnSliderPreviewMouseUp()
        {
            try
            {
                if (MusicStatus != null)
                {
                CurrentSlidersPosition = ((mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds * SliderValue) / 100);
                //currPos = 100 * 3.00000 / 100 = 3
                TimeSpan timeSpan = new TimeSpan(0, 0, Convert.ToInt32(CurrentSlidersPosition));
                mediaPlayer.Position = timeSpan;
                }
            }
            catch (Exception exOnSliderPreviewMouseUp)
            {
                MessageBox.Show(exOnSliderPreviewMouseUp.Message);
                //Przed if - brak utworu w wiadomości != null -> "\n No track"
            }

        }
 
        private async void OpenAudioFile()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == true)
                {
                    mediaPlayer.MediaOpened += MediaPlayer_MediaOpened; // Rejestracja obsługi zdarzenia MediaOpened
                    mediaPlayer.Open(new Uri(openFileDialog.FileName));
                    CurrentMusicName = $"{openFileDialog.SafeFileName}";
                }
            }
            catch (Exception exOpenAudioFile)
            {
                MessageBox.Show(exOpenAudioFile.Message);
            }
            
        }
        private void MediaPlayer_MediaOpened(object sender, EventArgs e)
        {
            UpdateMusicDuration();
        }
        private void UpdateMusicDuration()
        {
                MusicStatus = String.Format("{0} / {1}", mediaPlayer.Position.ToString(@"hh\:mm\:ss"),
                                              mediaPlayer.NaturalDuration.TimeSpan.ToString(@"hh\:mm\:ss"));
        }
        private void MediaPlayerStart()
        {
            if (MusicStatus != null)
            {
                musicTimer.Start();
                mediaPlayer.Play();
                isMediaPlayerPlaying = true;
            }           
        } 
        private void MediaPlayerPause()
        {
            mediaPlayer.Pause();
            musicTimer.Stop();
            isMediaPlayerPlaying = false;
        }
       
        private void PlayPause()
        {
            if (isMediaPlayerPlaying)
            {
                MediaPlayerPause();
            }
            else
            {
                MediaPlayerStart();
            }
        }

        private void Stop()
        {
            if (MusicStatus != null)
            {
                mediaPlayer.Stop();
                musicTimer.Stop();
                isMediaPlayerPlaying = false;
                SliderValue = 0.0;
                UpdateMusicDuration();
            }
               
        }     
    }
}
