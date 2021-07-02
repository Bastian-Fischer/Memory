using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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


        private List<MemoryLogic.ScorePoint> mScore;
        private MemoryLogic.TurnResult mResult;
        private MemoryLogic.Difficulty mDifficulty;
        private int mScoreCounter;
        Thickness mDistance = new();

        private int mScoreElementCounter;

        //private bool mGetScoreFree = true;
        //private DateTime mTimeGameStart;
        private readonly DispatcherTimer mTimer;
        public GameOver(List<MemoryLogic.ScorePoint> score, MemoryLogic.TurnResult result, MemoryLogic.Difficulty difficulty, int live)
        {
            InitializeComponent();
            DifficultyLabel.Content = difficulty switch { MemoryLogic.Difficulty.Easy => "EASY", MemoryLogic.Difficulty.Normal => "NORMAL", MemoryLogic.Difficulty.Hard => "HARD", _=> "Error"};
            mScoreCounter = 0;

            mScore = score;
            mResult = result;
            mDifficulty = difficulty;

            mLive = live;

            mDistance.Left = 0;
            mDistance.Right = 0;
            mDistance.Top = 0;
            mDistance.Bottom = 0;
            Image background = MainWindow.Instance.LoadImage(MainMenuPage.Instance.CurrentTheme.GameBackground);
            background.Margin = mDistance;
            BackgroundField.Children.Add(background);
            mDifficulty = difficulty;
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
            //mTimer.Stop(); // hält den Timer an damit das UI nicht aktualisiert wird bevor das Spiel startet
        }

        private void CreateScore() {
            Image image = new();
            if (mScoreElementCounter < mScore.Count || mScoreSum != mScoreCounter)
            {
                if (mScoreSum == mScoreCounter && mLive != mLiveCounter)
                {
                    image = MainWindow.Instance.LoadImage(MainMenuPage.Instance.CurrentTheme.Life);
                    mScoreSum += mDifficulty switch { MemoryLogic.Difficulty.Easy => 30, MemoryLogic.Difficulty.Normal => 50, MemoryLogic.Difficulty.Hard => 70, _ => 0 };
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
                            image = MainWindow.Instance.LoadImage(MainMenuPage.Instance.CurrentTheme.Point);
                            mScoreSum += mDifficulty switch { MemoryLogic.Difficulty.Easy => 50, MemoryLogic.Difficulty.Normal => 70, MemoryLogic.Difficulty.Hard => 90, _ => 0};
                            break;
                        case MemoryLogic.ScorePoint.BigPoint:
                            image = MainWindow.Instance.LoadImage(MainMenuPage.Instance.CurrentTheme.BigPoint, Stretch.Fill); 
                            mScoreSum += mDifficulty switch { MemoryLogic.Difficulty.Easy => 70, MemoryLogic.Difficulty.Normal => 90, MemoryLogic.Difficulty.Hard => 110, _ => 0 };
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
        private void reset_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.Navigate(new GameScreen());
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.Navigate(new MainMenuPage());
        }
    }
}
