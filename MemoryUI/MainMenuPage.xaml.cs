using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MemoryLogic;
namespace MemoryUI
{
    public partial class MainMenuPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public static MainMenuPage Instance { get; set; }
        public int MinimumOfPairs { get; } = 5;
        public int MaximumOfPairs { get; } = 20;
        public int MinimumPairSize { get; } = 2;
        public int MaximumPairSize { get; } = 5;
        public Theme CurrentTheme;
        private int mNOPValue = 5;
        public int NumberOfPairsValue
        {
            get => mNOPValue;
            set
            {
                if (mNOPValue != value)
                {
                    mNOPValue = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(NumberOfPairsValue)));
                }
            }
        }
        private int mSOPValue = 2;
        public int SizeOfPairsValue
        {
            get => mSOPValue;
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
            Instance = this;
            ListItemIs = new();
            SliderNOP.Minimum = MinimumOfPairs;
            SliderNOP.Maximum = MaximumOfPairs;
            SliderSOP.Minimum = MinimumPairSize;
            SliderSOP.Maximum = MaximumPairSize;
            ButtonEASY.Background = new SolidColorBrush(Colors.DarkGray);
            ButtonNORMAL.Background = new SolidColorBrush(Colors.LightGray);
            ButtonHARD.Background = new SolidColorBrush(Colors.DarkGray);
            mDifficultyLevel = Difficulty.Normal;
            CreateThemeListView();
        }
        private void CreateThemeListView() {
            bool select = true;
            foreach (var pair in MainWindow.Instance.Themes)
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
                _ = panel.Children.Add(label);
                _ = panel.Children.Add(image);
                ListViewItem item= new();
                item.Content = panel;
                item.IsSelected = select;
                ListItemIs.Add(item, pair.Value);
                _ = ThemeListView.Items.Add(item);
                select = false;
            }
        }
        private void SetDifficulty(Difficulty level){
            mDifficultyLevel = level switch
            {
                Difficulty.Easy => Difficulty.Easy,
                Difficulty.Normal => Difficulty.Normal,
                Difficulty.Hard => Difficulty.Hard,
                _ => Difficulty.Normal,
            };
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
            SliderNOP.Maximum = (MaximumOfPairs > CurrentTheme.CardList.Count) ? CurrentTheme.CardList.Count : MaximumOfPairs;
        }
        private void CommandClose_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void CommandStartGame_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            GameScreen page = new();
            _ = MainWindow.Instance.MainFrame.Navigate(page);
        }
        private void CommandInformation_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Information InformationWindow = new();
            InformationWindow.Show();
        }
    }
}
