using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
    /// Interaction logic for GameScreen.xaml
    /// </summary>
    public partial class GameScreen : Page
    {
        private MemoryLogic.Difficulty mDifficulty = MainMenuPage.Instance.DifficultyLevel;
        private int mNumberOfPairs = MainMenuPage.Instance.NumberOfPairsValue;
        private int mSizeOfPairs = MainMenuPage.Instance.SizeOfPairsValue;

        private SoundPlayer mCardFlipSound = new(MainMenuPage.Instance.CurrentTheme.CardFlipSound);
        private SoundPlayer mPointSound = new(MainMenuPage.Instance.CurrentTheme.PointSound);
        private SoundPlayer mBigPointSound = new(MainMenuPage.Instance.CurrentTheme.BigPointSound);
        private MediaPlayer mBackgroundSound = new();
        

        private List<BitmapImage> mPictureIndex;
        private List<BitmapImage> mCardImageList;
        private List<Image> mCardCoverList;
        private MemoryLogic.Logic mLogic;
        private Dictionary<Button, int> mButtonIsPosition = new();
        private int mCardsPerRow= 10;
        private List<StackPanel> mRawList = new();
        private int mCardSize;
        private int mCardImageCounter;
        private Thickness mDistance = new();
        private DateTime mTimeGameStart;
        private readonly DispatcherTimer mTimer;
        private double mGivenTime;
        private int mCurrentTimerTime;
        private double mTimerTimebarSteps;
        private Image timebackground;
        private int mLastPosition;
        
        public GameScreen()
        {
            InitializeComponent();
            DataContext = this;
            mGivenTime = 10000;
            mBackgroundSound.Open(new Uri(MainMenuPage.Instance.CurrentTheme.GameBackgroundSound , UriKind.RelativeOrAbsolute));
            mBackgroundSound.MediaEnded += LoopBackgroundSound;
            mBackgroundSound.Play();
            mCardImageList = new();
            mCardCoverList = new();
            mCardImageCounter = 0;
            if (mSizeOfPairs * mNumberOfPairs <= 20) mCardsPerRow = 5;

            mCardSize = 100;
            mLogic = new(mNumberOfPairs, mSizeOfPairs);
            
            mLastPosition = -1;
            mDistance.Left = 0;
            mDistance.Right = 0;
            mDistance.Top = 0;
            mDistance.Bottom = 0;
            Image background = MainWindow.Instance.LoadImage(MainMenuPage.Instance.CurrentTheme.GameBackground);
            background.Margin = mDistance;
            
            BackgroundField.Children.Add(background);
            mDistance .Left = 5;
            mDistance .Right = 5;
            mDistance .Top = 5;
            mDistance .Bottom = 5;
            ResetTimeCountDown();
            SetDifficulty();
            CreateImageList();
            CreateCoverList();
            CreateCardField();
            
            LiveUpdate();
            mTimerTimebarSteps =  mGivenTime / 500.0;

            mTimer = new DispatcherTimer(
              new TimeSpan(0, 0, 0, 0, 1), 
              DispatcherPriority.Render, 
              (_, _) => TimeCountDown(), 
              Dispatcher.CurrentDispatcher);
            mTimer.Stop(); 
        }

        private void SetDifficulty() {
            switch (mDifficulty)
            {
                case MemoryLogic.Difficulty.Easy:
                    mGivenTime = 10000;
                    break;
                case MemoryLogic.Difficulty.Normal:
                    mGivenTime = 8000;
                    break;
                case MemoryLogic.Difficulty.Hard:
                    mGivenTime = 5000;
                    break; 
            }
        }
        private void LoopBackgroundSound(object sender, EventArgs e)
        {
            mBackgroundSound.Position = TimeSpan.Zero;
            mBackgroundSound.Play();
        }
        private void ResetTimeCountDown() {
            mCurrentTimerTime = 0;
            TimeBar.Height = 500;
            timebackground = MainWindow.Instance.LoadImage(MainMenuPage.Instance.CurrentTheme.TimeBarBigPoint);
            timebackground.Opacity = 0.7;
            if (TimeBar.Children.Count > 0) TimeBar.Children.Clear();
            TimeBar.Children.Add(timebackground);
        }
        private void TimeCountDown()
        {
            mCurrentTimerTime++;
            if (mCurrentTimerTime >= mTimerTimebarSteps)
            {
                TimeBar.Height--;
            }
            if(TimeBar.Height > 400) mLogic.SetScoreTyp(MemoryLogic.ScorePoint.BigPoint);
            if (TimeBar.Height == 400)
            {
                mLogic.SetScoreTyp(MemoryLogic.ScorePoint.Point);
                timebackground = MainWindow.Instance.LoadImage(MainMenuPage.Instance.CurrentTheme.TimeBarPoint);
                timebackground.Opacity = 0.7;
                if (TimeBar.Children.Count > 0) TimeBar.Children.Clear();
                TimeBar.Children.Add(timebackground);
            }
            else if(TimeBar.Height == 100)
            {
                timebackground = MainWindow.Instance.LoadImage(MainMenuPage.Instance.CurrentTheme.TimeBarCritical);
                timebackground.Opacity = 0.7;
                if (TimeBar.Children.Count > 0) TimeBar.Children.Clear();
                TimeBar.Children.Add(timebackground);
            }
            
            if (TimeBar.Height <= 0)
            {
                mCurrentTimerTime= 0;
                TimeBar.Height = 500;
                mLogic.AddDeath(1);
                LiveUpdate();
                if (mLogic.Life <= 0) GameOver(MemoryLogic.TurnResult.GameLose);
            }
        }
        private void CreateCardField()
        {
            int numberofCards = mSizeOfPairs * mNumberOfPairs;
            int rows = numberofCards / mCardsPerRow;
            int cardRowOverflow = numberofCards % mCardsPerRow;
            if (cardRowOverflow != 0)
            {
                rows++;
            }

            for (int counter = 0; counter < rows; counter++)
            {
                StackPanel panel = new();
                panel.Name = "GameRow_" + counter;
                panel.Orientation = Orientation.Horizontal;
                panel.HorizontalAlignment = HorizontalAlignment.Center;
                panel.VerticalAlignment = VerticalAlignment.Center;

                GridCardField.Children.Add(panel);
                mRawList.Add(panel);
            }
            LayCardsToField(rows, cardRowOverflow);
        }
        private void LayCardsToRow(int pos, int numberOfCards)
        {
            for (int counter = 0; counter < numberOfCards; counter++)
            {
                mRawList[pos].Children.Add(CreateCardButton());
            }
        }
        private void LayCardsToField(int rows, int overflow)
        {
            for (int rowCounter = 0; rowCounter < rows; rowCounter++)
            {
                if (overflow != 0)
                {
                    if (rowCounter == rows / 2) LayCardsToRow(rowCounter, overflow);
                    else LayCardsToRow(rowCounter, mCardsPerRow);

                }
                else LayCardsToRow(rowCounter, mCardsPerRow);
            }

        }
        private void CreateCoverList() {
            for (int counter = 0; counter < mLogic.Gamefield.Length; counter++)
            {
                Image cover = MainWindow.Instance.LoadImage(MainMenuPage.Instance.CurrentTheme.Cover);
                mCardCoverList.Add(cover);
            }         
        }
        private Button CreateCardButton()
        {          
            Grid ImagePanel = new();

            Image image = MainWindow.Instance.LoadImage(mCardImageList[mLogic.Gamefield[mCardImageCounter].CardId - 1], Stretch.Uniform);
            Image cover = mCardCoverList[mCardImageCounter];
            ImagePanel.Children.Add(image);
            ImagePanel.Children.Add(cover);
            ImagePanel.HorizontalAlignment = HorizontalAlignment.Center;
            ImagePanel.VerticalAlignment = VerticalAlignment.Center;
            ImagePanel.Margin = mDistance;

            Button button = new();
            button.Click += CardButtonClick;
            button.MouseLeave += CardFocusLost;
            button.Content = ImagePanel;
            button.Height = mCardSize;
            button.Width = mCardSize;
            button.Margin = mDistance ;
            button.Background = new SolidColorBrush(Colors.DarkGray);
            button.Foreground = new SolidColorBrush(Colors.Black);
            mButtonIsPosition.Add(button, mCardImageCounter++);

            return button;
        }

        private void CardFocusLost(object sender, RoutedEventArgs e)
        {
            for (int counter = 0; counter < mLogic.Gamefield.Length; counter++)
            {
                mCardCoverList[counter].Visibility = (mLogic.Gamefield[counter].Visibility) ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        private void LiveUpdate() { 
            while(LiveField.Children.Count != mLogic.Life)
            {
                if (LiveField.Children.Count < mLogic.Life) 
                {
                    Image life = MainWindow.Instance.LoadImage(MainMenuPage.Instance.CurrentTheme.Life);
                    life.Height = 30;
                    life.Width = 30;
                    LiveField.Children.Add(life);
                } 
                if (LiveField.Children.Count > mLogic.Life) LiveField.Children.RemoveAt(LiveField.Children.Count-1);
            }
        }
        private void ScoreUpdate()
        {      
            if (ScoreField.Children.Count < mLogic.Score.Count)
            {
                if (mLogic.Score.Last() == MemoryLogic.ScorePoint.Point) mPointSound.Play();
                else mBigPointSound.Play();
                Image point = MainWindow.Instance.LoadImage(
                    mLogic.Score.Last()== MemoryLogic.ScorePoint.Point ? MainMenuPage.Instance.CurrentTheme.Point : MainMenuPage.Instance.CurrentTheme.BigPoint, 
                    Stretch.Fill
                    );
                point.Height = 30;
                point.Width = 30;
                ScoreField.Children.Add(point);
            }
            if (ScoreField.Children.Count > mLogic.Score.Count) ScoreField.Children.RemoveAt(ScoreField.Children.Count - 1);
            
        }

        private void CreateImageList()
        {
            mPictureIndex = new();
            mCardImageList = new();
            Random rand = new();

            foreach (BitmapImage cardBitmap in MainMenuPage.Instance.CurrentTheme.CardList){
                mPictureIndex.Add(cardBitmap);
            }
            
            for (int counter = 0; counter < mNumberOfPairs; counter++)
            {
                int typNumber = rand.Next(0, mPictureIndex.Count);
                BitmapImage bitmap = mPictureIndex[typNumber];
                mPictureIndex.RemoveAt(typNumber);
                mCardImageList.Add(bitmap);
            }
        }
        private void GameOver(MemoryLogic.TurnResult turnresult) {
            if (turnresult == MemoryLogic.TurnResult.GameLose)
            {
                mTimer.Stop();
                MainWindow.Instance.MainFrame.Navigate(new GameOver(mLogic.Score, turnresult, mDifficulty, mLogic.Life));
            }
            if (turnresult == MemoryLogic.TurnResult.GameWin || mLogic.Score.Count == mNumberOfPairs)
            {
                mTimer.Stop();
                MainWindow.Instance.MainFrame.Navigate(new GameOver(mLogic.Score, MemoryLogic.TurnResult.GameWin, mDifficulty, mLogic.Life));
            }

        }
        private void CardButtonClick(object sender,RoutedEventArgs e)
        {

            if (!mTimer.IsEnabled) 
            {
                mTimeGameStart = DateTime.Now; 
                mTimer.Start(); 
            }
            Button button = (Button)sender;
            int position = mButtonIsPosition[button];
            if (position != mLastPosition)
            {
                mCardFlipSound.Play();
                ResetTimeCountDown();
            }
            mLastPosition = position;

            MemoryLogic.TurnResult turnresult = mLogic.TurnStep(position); 
            mCardCoverList[position].Visibility = Visibility.Collapsed;
            for (int counter = 0; counter < mLogic.Gamefield.Length; counter++)
            {
                if (counter != position)
                mCardCoverList[counter].Visibility = mLogic.Gamefield[counter].Visibility ? Visibility.Collapsed : Visibility.Visible;
            }

            GameOver(turnresult);
            LiveUpdate();
            ScoreUpdate();
            if(mLogic.Gamefield[position].Try > 2 && mDifficulty == MemoryLogic.Difficulty.Easy) button.Background = new SolidColorBrush(Colors.DarkRed);
        }
    }
}
