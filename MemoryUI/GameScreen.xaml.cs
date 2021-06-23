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
    /// Interaction logic for GameScreen.xaml
    /// </summary>
    public partial class GameScreen : Page
    {
        private Frame mMainFrame;
        private List<List<string>> mPictureIndex;
        private List<string> mCardImageList;
        private List<Image> mCardCoverList;
        private int mLiveStart;
        private MemoryLogic.Logic mLogic;
        private string mThemeName = "MysticForest";
        private string mThemesDirectory = "Assets/Themes/";
        private string mImageDirectory;
        private Dictionary<Button, int> mButtonIsPosition = new();
        private int mCardsPerRow= 10;
        private List<StackPanel> mRawList = new();
        private readonly int mNOP;
        private readonly int mSOP;
        private int mCardSize;
        private int mCardImageCounter;
        Thickness mDistance = new();

        private DispatcherTimer mTimer;


        public GameScreen(Frame MainFrame,int sOP , int nOP)
        {
            mMainFrame = MainFrame;
            InitializeComponent();
            
            mCardImageList = new();
            mCardCoverList = new();
            mCardImageCounter = 0;
            if (sOP * nOP <= 20) mCardsPerRow = 5;
            mImageDirectory = mThemesDirectory + mThemeName + "/";
            mCardSize = 100;
            mLogic = new(nOP, sOP);
            mNOP = nOP;
            mSOP = sOP;

            mDistance.Left = 0;
            mDistance.Right = 0;
            mDistance.Top = 0;
            mDistance.Bottom = 0;
            Image background = CreateImage("background.png");
            background.Margin = mDistance;      
            BackgroundField.Children.Add(background);

            mDistance .Left = 5;
            mDistance .Right = 5;
            mDistance .Top = 5;
            mDistance .Bottom = 5;
            CreateImageList();
            CreateCoverList();
            CreateCardField();

            LiveUpdate();
            mLiveStart = mLogic.Live;
        }

        private void CreateCardField()
        {
            int numberofCards = mSOP * mNOP;
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
                Image cover = CreateImage("cover.png");
                mCardCoverList.Add(cover);
            }         
        }
        private Button CreateCardButton()
        {          
            Grid ImagePanel = new();

            Image image = CreateImage(mCardImageList[mLogic.Gamefield[mCardImageCounter].CardId - 1], Stretch.Uniform);
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
            while(LiveField.Children.Count != mLogic.Live)
            {
                if (LiveField.Children.Count < mLogic.Live) 
                {
                    Image life = CreateImage("live.png");
                    life.Height = 30;
                    life.Width = 30;
                    LiveField.Children.Add(life);
                } 
                if (LiveField.Children.Count > mLogic.Live) LiveField.Children.RemoveAt(LiveField.Children.Count-1);
            }
        }
        private void ScoreUpdate()
        {      
            if (ScoreField.Children.Count < mLogic.Score.Count)
            {
                string pointName;
                
                switch (mLogic.Score.Last()) {
                    case MemoryLogic.ScorePoint.Point: pointName = "point.png"; break;
                    case MemoryLogic.ScorePoint.BigPoint: pointName = "bigpoint.png"; break;
                    default: pointName = "point.png"; break;
                }
                
                Image point = CreateImage(pointName);
                point.Height = 30;
                point.Width = 30;
                ScoreField.Children.Add(point);
            }
            if (ScoreField.Children.Count > mLogic.Score.Count) ScoreField.Children.RemoveAt(ScoreField.Children.Count - 1);
            
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
        private void CreateImageList()
        {
            mPictureIndex = new();
            for (int categoryCounter = 1; categoryCounter < 6; categoryCounter++)
            {
                mPictureIndex.Add(new());
                for (int typCounter = 1; typCounter < 31; typCounter++)
                {
                    mPictureIndex[categoryCounter - 1].Add(categoryCounter + " (" + typCounter + ").png");
                }
            }
            mCardImageList = new();
            Random rand = new();
            int categoryIndexCounter = 0;
            for (int counter = 0; counter < mNOP; counter++)
            {
                if (categoryIndexCounter >= mPictureIndex.Count) 
                categoryIndexCounter = 0;

                int typNumber = rand.Next(0, mPictureIndex[categoryIndexCounter].Count);
                string name = mPictureIndex[categoryIndexCounter][typNumber];
                mPictureIndex[categoryIndexCounter].RemoveAt(typNumber);
                mCardImageList.Add(name);

                categoryIndexCounter++;
            }
        }
        private void CardButtonClick(object sender,RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int position = mButtonIsPosition[button];
            MemoryLogic.TurnResult turnresult = mLogic.TurnStep(position); 
            mCardCoverList[position].Visibility = Visibility.Collapsed;
            for (int counter = 0; counter < mLogic.Gamefield.Length; counter++)
            {
                if (counter != position)
                mCardCoverList[counter].Visibility = (mLogic.Gamefield[counter].Visibility) ? Visibility.Collapsed : Visibility.Visible;
            }

            if(turnresult ==  MemoryLogic.TurnResult.GameLose) 
            {
                mMainFrame.Navigate(new GameOver(mMainFrame,mSOP,mNOP,mLogic.Score, turnresult, MemoryLogic.Difficulty.Normal, mThemeName, mLogic.Live));
            }
            if(turnresult == MemoryLogic.TurnResult.GameWin || mLogic.Score.Count == mNOP)
            {
                mMainFrame.Navigate(new GameOver(mMainFrame, mSOP, mNOP, mLogic.Score, MemoryLogic.TurnResult.GameWin, MemoryLogic.Difficulty.Normal, mThemeName, mLogic.Live));
            }
            LiveUpdate();
            ScoreUpdate();
        }
        
        




























        /*
        private void CreateGrid() {
            int numberofCards = mSOP * mNOP;
            int rows = numberofCards / mCardsPerRow;
            int cardRowOverflow = numberofCards % mCardsPerRow;
            if (cardRowOverflow != 0)
            {
                rows++;
            }

            for (int counter = 0; counter < rows; counter++)
            {
                RowDefinition Row = new();
                Row.Height = GridLength.Auto;
                GridCardField.RowDefinitions.Add(Row);
                StackPanel panel = new();
                panel.Name = "GameRow_" + counter;
                panel.Orientation = Orientation.Horizontal;
                panel.HorizontalAlignment = HorizontalAlignment.Center;
                mRawList.Add(panel);
                GridCardField.Children.Add(panel);
            }
            LayCardsToField(rows, cardRowOverflow);
        }
        private Image CreateImage(string url)
        {
            var position = new Uri(url, UriKind.Relative);
            BitmapImage resource = new(position);
            //resource.BeginInit();
            //resource.Source 
            //resource.CacheOption = BitmapCacheOption.OnLoad;
            //resource.EndInit();
            Image image = new();
            image.Source = resource;
            return image;
        }
        private void CreateImageList(){
            
            mPictureIndex = new();
            for (int categoryCounter = 1; categoryCounter < 6; categoryCounter++)
            {
                mPictureIndex.Add(new());
                for (int typCounter = 1; typCounter < 31; typCounter++)
                {
                    mPictureIndex[categoryCounter - 1].Add(categoryCounter + "_("+ typCounter + ").svg");
                }
            }
            mCardImageList = new();
            Random rand = new();
            int categoryIndexCounter = 0;
            for (int counter = 0; counter < mNOP; counter++)
            {               
                int typNumber = rand.Next(0, mPictureIndex[categoryIndexCounter].Count);
                string url = mPictureIndex[categoryIndexCounter][typNumber];
                mPictureIndex[categoryIndexCounter].RemoveAt(typNumber);
                mCardImageList.Add(CreateImage(url));

                if (categoryIndexCounter < mPictureIndex.Count) categoryIndexCounter++ ; 
                else categoryIndexCounter = 0;
            }
        }

        private void CardButtonClick(Button button)
        {
           

        }
        private Button CreateCardButton(int pos) {
            Button card = new();
            card.Click += (s, e) => { CardButtonClick((Button)s); };
            
            
            Image cover = CreateImage("cover.svg");
            mCardCoverList.Add(cover);

            Image image = mCardImageList[mLogic.Gamefield[pos].CardId - 1];
            Grid imgField = new();
            //TODO
            imgField.Children.Add(image);
            imgField.Children.Add(cover);
            card.Content = imgField;
            mButtonIsPosition.Add(card, pos);
            return card;
        }
        private void LayCardsToRow(int pos,int numerOfCards) {
            for (int counter = 0; counter < numerOfCards; counter++)
            {
                mRawList[pos].Children.Add(CreateCardButton(counter));
            }
        }
        private void LayCardsToField(int rows, int overflow) {
            for (int rowCounter = 0; rowCounter < rows; rowCounter++)
            {
                if(overflow != 0)
                {
                    if (rowCounter == rows / 2) LayCardsToRow(rowCounter, overflow);
                    else LayCardsToRow(rowCounter, mCardsPerRow);

                }
                else LayCardsToRow(rowCounter, mCardsPerRow);
            }
        }
        */
    }
}
