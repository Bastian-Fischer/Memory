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

namespace MemoryUI
{
    /// <summary>
    /// Interaktionslogik für GameOver.xaml
    /// </summary>
    public partial class GameOver : Page
    {
        private int mLive;
        private int mLiveCounter;
        private int mScoreSum = 0;
        private Frame mMainFrame;
        private int mSOP;
        private int mNOP;
        private List<MemoryLogic.ScorePoint> mScore;
        private MemoryLogic.TurnResult mResult;
        private MemoryLogic.Difficulty mDifficulty;
        private int mScoreCounter;
        Thickness mDistance = new();
        private string mThemeName;
        private string mThemesDirectory = "Assets/Themes/";
        private string mImageDirectory;
        private bool mGetScoreFree = true;
        private DateTime mTimeGameStart; // Wird befüllt wenn das erste Feld umgedreht wird um die Startzeit des Spiels festzuhalten
        private int mScoreElementCounter = 0;
        private readonly DispatcherTimer mTimer; // Startet zeitbasiert Events. Hier wird er benutzt um die Uhrzeit im UI anzuzeigen.
        public GameOver(Frame MainFrame, int sop, int nop, List<MemoryLogic.ScorePoint> score, MemoryLogic.TurnResult result, MemoryLogic.Difficulty difficulty, string theme, int live)
        {
            InitializeComponent();
            mScoreCounter = 0;
            mMainFrame = MainFrame;
            mSOP = sop;
            mNOP = nop;
            mScore = score;
            mResult = result;
            mDifficulty = difficulty;
            mThemeName = theme;
            mLive = live;
            mImageDirectory = mThemesDirectory + mThemeName + "/";
            mDistance.Left = 0;
            mDistance.Right = 0;
            mDistance.Top = 0;
            mDistance.Bottom = 0;
            Image background = CreateImage("background.png");
            background.Margin = mDistance;
            BackgroundField.Children.Add(background);

            GameOverTitle.Content = result switch {
                MemoryLogic.TurnResult.GameWin => "VICTORY",
                MemoryLogic.TurnResult.GameLose => "GAME OVER",
                _ => "" };
            GameOverTitle.Foreground = result switch
            {
                MemoryLogic.TurnResult.GameWin => System.Windows.Media.Brushes.Green,
                MemoryLogic.TurnResult.GameLose => System.Windows.Media.Brushes.Red,
                _ => System.Windows.Media.Brushes.White
            };

            mTimer = new DispatcherTimer(
               new TimeSpan(0, 0, 0, 0, 1), // alle 100 millisekunden (manchmal später, aber nie früher)
               DispatcherPriority.Render, // legt die priorität fest mit welcher der DispatcherTimer überprüft ob er starten muss
               (_, _) => CreateScore(), // der auszuführende befehl
               Dispatcher.CurrentDispatcher);
            mTimer.Stop(); // hält den Timer an damit das UI nicht aktualisiert wird bevor das Spiel startet
        }

        private void CreateScore() {
            Image image = new();
            if (mScoreElementCounter < mScore.Count || mScoreSum != mScoreCounter)
            {
                if (mScoreSum == mScoreCounter && mLive != mLiveCounter)
                {
                    image = CreateImage("live.png");
                    mScoreSum += 300;
                    mLiveCounter++;
                    image.Width = 40;
                    image.Height = 40;
                    ScoreList.Children.Add(image);
                }
                if (mScoreSum == mScoreCounter && mScoreElementCounter != mScore.Count)
                { 
                    switch (mScore[mScoreElementCounter++])
                    {
                        case MemoryLogic.ScorePoint.Point:
                            image = CreateImage("point.png");
                            mScoreSum += 500;
                            break;
                        case MemoryLogic.ScorePoint.BigPoint:
                            image = CreateImage("bigpoint.png");
                            mScoreSum += 700;
                            break;
                        default: break;
                    }
                    image.Width = 40;
                    image.Height = 40;
                    ScoreList.Children.Add(image);
                }

                mScoreCounter++;
                ScoreLabel.Content = "SCORE: " + mScoreCounter;

            }
            else mTimer.Stop();
            
        }
        private Image CreateImage(string fileName, Stretch stretch = Stretch.UniformToFill)
        {
            var position = new Uri(mImageDirectory + fileName, UriKind.Relative);
            BitmapImage resource = new(position);
            Image image = new();
            image.Source = resource;
            image.Stretch = stretch;
            return image;
        }


        private void reset_Click(object sender, RoutedEventArgs e)
        {
            mMainFrame.Navigate(new GameScreen(mMainFrame, mSOP, mNOP));
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            mMainFrame.Navigate(new MainMenuPage(mMainFrame));
        }

        private void getScore_Click(object sender, RoutedEventArgs e)
        {
            if (!mTimer.IsEnabled) // testet ob das Spiel gerade gestartet wurde
            {
                mTimeGameStart = DateTime.Now; // startzeit speichern
                mTimer.Start(); // DispatcherTimer starten damit das UI die aktuelle Zeitdifferenz anzeigt
            }
        }
    }
}
