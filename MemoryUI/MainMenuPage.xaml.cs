using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using MemoryLogic;
namespace MemoryUI
{
    /// <summary>
    /// Interaction logic for MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuPage : Page, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;


        public static MainMenuPage mInstance;
        public static MainMenuPage Instance
        {
            get
            {
                return mInstance;
            }
        }
        private int mMinimumOfPairs = 5;
        public int MinimumOfPairs { get { return mMinimumOfPairs; } }
        private int mMaximumOfPairs = 20;
        public int MaximumOfPairs { get { return mMaximumOfPairs; } }
        private int mMinimumPairSize = 2;
        public int MinimumPairSize { get { return mMinimumPairSize; } }
        private int mMaximumPairSize = 5;
        public int MaximumPairSize { get { return mMaximumPairSize; } }

        public Theme CurrentTheme;



        private int mNOPValue = 5;
        public int NumberOfPairsValue
        {
            get => mNOPValue; 
            set 
            {
                if (mNOPValue != value){
                    mNOPValue = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(NumberOfPairsValue)));
                }
            }
        }

        private int mSOPValue = 2;
        public int SizeOfPairsValue 
        {    
        get { return mSOPValue; }
            set
            {
               if (mSOPValue != value)
                {
                    mSOPValue = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(SizeOfPairsValue)));
                }
            }
        }

        private Difficulty mDifficultyLevel;
        public Difficulty DifficultyLevel { get { return mDifficultyLevel; } }


        private Dictionary<ListViewItem, Theme> ListItemIs;
        public MainMenuPage()
        {
            
            InitializeComponent();
            
            DataContext = this;
            mInstance = this;
            ListItemIs = new();
            

            SliderNOP.Minimum = mMinimumOfPairs;
            SliderNOP.Maximum = mMaximumOfPairs;
            //SliderNOP.Value = mMinimumOfPairs;
            //TextBoxNOP.Text = mMinimumOfPairs.ToString();

            SliderSOP.Minimum = mMinimumPairSize;
            SliderSOP.Maximum = mMaximumPairSize;
            //SliderSOP.Value = mMinimumPairSize;
            //TextBoxSOP.Text = mMinimumPairSize.ToString();
            ButtonEASY.Background = new SolidColorBrush(Colors.DarkGray);
            ButtonNORMAL.Background = new SolidColorBrush(Colors.LightGray);
            ButtonHARD.Background = new SolidColorBrush(Colors.DarkGray);
            mDifficultyLevel = Difficulty.Normal;

            CreateThemeListView();
        }

   

        private void CreateThemeListView() {
            bool select = true;
            foreach (KeyValuePair<string,Theme> pair in MainWindow.Instance.Themes)
            {
                StackPanel panel = new();
                panel.Height = 120;
                panel.Orientation = Orientation.Horizontal;
                Label label = new();
                label.Content = pair.Key;
                label.Width = 200;
                Image image;
                image = new();
                image = MainWindow.Instance.LoadImage(pair.Value.Thumbnail);
                panel.Children.Add(label);
                panel.Children.Add(image);
                
                
                ListViewItem item= new();
                item.Content = panel;
                item.IsSelected = select;
                ListItemIs.Add(item, pair.Value);
                ThemeListView.Items.Add(item);
                select = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //GameScreen page = new();
            //MainWindow.Instance.MainFrame.Navigate(page);
        }
        //TODO hier weiter
        private void SetDifficulty(Difficulty level){
            switch (level) {
                case Difficulty.Easy: mDifficultyLevel = Difficulty.Easy; break;
                case Difficulty.Normal: mDifficultyLevel = Difficulty.Normal; break;
                case Difficulty.Hard: mDifficultyLevel = Difficulty.Hard; break;
                default: mDifficultyLevel = Difficulty.Normal; break;
            }
        }

        private void SliderSOP_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //mSOPValue = (int)e.NewValue;
            //LabelSOP.Content = e.NewValue.ToString();
        }

        private void SliderNOP_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //mNOPValue = (int)e.NewValue;
            //LabelNOP.Content = e.NewValue.ToString();
        }

        private void ButtonEASY_Click(object sender, RoutedEventArgs e)
        {
            SetDifficulty(Difficulty.Easy);
            ButtonEASY.Background = new SolidColorBrush(Colors.LightGray);
            ButtonNORMAL.Background = new SolidColorBrush(Colors.DarkGray);
            ButtonHARD.Background = new SolidColorBrush(Colors.DarkGray);
        }

        private void ButtonNORMAL_Click(object sender, RoutedEventArgs e)
        {
            SetDifficulty(Difficulty.Normal);
            ButtonEASY.Background = new SolidColorBrush(Colors.DarkGray);
            ButtonNORMAL.Background = new SolidColorBrush(Colors.LightGray);
            ButtonHARD.Background = new SolidColorBrush(Colors.DarkGray);
        }

        private void ButtonHARD_Click(object sender, RoutedEventArgs e)
        {
            SetDifficulty(Difficulty.Hard);
            ButtonEASY.Background = new SolidColorBrush(Colors.DarkGray);
            ButtonNORMAL.Background = new SolidColorBrush(Colors.DarkGray);
            ButtonHARD.Background = new SolidColorBrush(Colors.LightGray);
        }
        private void ThemeListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView lisView = (ListView)sender;
            ListViewItem item = (ListViewItem)lisView.SelectedItem;
            CurrentTheme = ListItemIs[item];
            MenuBackground.Children.Clear();
            MenuBackground.Children.Add(MainWindow.Instance.LoadImage(CurrentTheme.MenuBackground));
            SliderNOP.Maximum = (mMaximumOfPairs > CurrentTheme.CardList.Count) ? CurrentTheme.CardList.Count : mMaximumOfPairs;
        }
        private void CommandClose_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void CommandStartGame_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            GameScreen page = new();
            MainWindow.Instance.MainFrame.Navigate(page);
        }

        private void CommandInformation_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Information InformationWindow = new();
            InformationWindow.Show();
        }
    }
}
