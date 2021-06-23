using System;
using System.Collections.Generic;
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
    public partial class MainMenuPage : Page
    {
        private Frame mMainFrame;
        private int mMinimumOfPairs = 5;
        public int MinimumOfPairs { get { return mMinimumOfPairs; } }
        private int mMaximumOfPairs = 20;
        public int MaximumOfPairs { get { return mMaximumOfPairs; } }
        private int mMinimumPairSize = 2;
        public int MinimumPairSize { get { return mMinimumPairSize; } }
        private int mMaximumPairSize = 5;
        public int MaximumPairSize { get { return mMaximumPairSize; } }
        private int mCurrentSOPValue;
        private int mCurrentNOPValue;
        private Difficulty mDifficultyLevel;
        public MainMenuPage(Frame MainFrame)
        {
            mMainFrame = MainFrame;
            InitializeComponent();

            SliderNOP.Minimum = mMinimumOfPairs;
            SliderNOP.Maximum = mMaximumOfPairs;
            SliderNOP.Value = mMinimumOfPairs;
            LabelNOP.Content = mMinimumOfPairs.ToString();

            SliderSOP.Minimum = mMinimumPairSize;
            SliderSOP.Maximum = mMaximumPairSize;
            SliderSOP.Value = mMinimumPairSize;
            LabelSOP.Content = mMinimumPairSize.ToString();

            mDifficultyLevel = Difficulty.Normal;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GameScreen page = new(mMainFrame, mCurrentSOPValue, mCurrentNOPValue);
            mMainFrame.Navigate(page);
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
            mCurrentSOPValue = (int)e.NewValue;
            LabelSOP.Content = e.NewValue.ToString();
        }

        private void SliderNOP_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mCurrentNOPValue = (int)e.NewValue;
            LabelNOP.Content = e.NewValue.ToString();
        }

        private void ButtonEASY_Click(object sender, RoutedEventArgs e)
        {
            SetDifficulty(Difficulty.Easy);
        }

        private void ButtonNORMAL_Click(object sender, RoutedEventArgs e)
        {
            SetDifficulty(Difficulty.Normal);
        }

        private void ButtonHARD_Click(object sender, RoutedEventArgs e)
        {
            SetDifficulty(Difficulty.Hard);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
