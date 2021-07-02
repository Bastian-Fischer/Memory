using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
using System.Drawing;
namespace MemoryUI
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        static public MainWindow Instance;
        public Dictionary<string, Theme> Themes { get; private set; }
        public string mSystemThemesDirectory{ get; private set; }
        public MainWindow()
        {
            Instance = this;
            mSystemThemesDirectory = "Assets/Themes/SystemThemes/";
            InitializeComponent();
            //GetAddons();
            Themes = new();


            CreateSystemThemes();

            MainFrame.Navigate(new MainMenuPage());
        }

        private void GetAddons()
        {

            List<BitmapImage> cards = new();
            if (!Directory.Exists("addons"))
                Directory.CreateDirectory("addons");


            string[] filePaths = Directory.GetFiles(@"addons", "*.mtp", SearchOption.AllDirectories);
            if(filePaths != null){ 
                foreach (string name in filePaths)
                {
                    using (ZipArchive archive = ZipFile.Open(name, ZipArchiveMode.Update))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            //Pictures
                            if (entry.Name == "*.jpg" || entry.Name == "*.png"){
                                //if (entry.Name == "cover.???") ;
                                //if (entry.Name == "menu_background.???") ;
                                //if (entry.Name == "game_background.???") ;
                                //if (entry.Name == "bigpoint_image.???") ;
                                if (entry.FullName == "*cards/*") 
                                {
                                    BitmapImage bimg = new();                                   
                                    bimg.StreamSource = entry.Open();                                
                                }                             
                            }
                            //Sounds
                        }             
                    }
                }
            }


            //TODO zip auslesen
            //bmpinit extrs sourth für stream
            //
            //chach option on load
            //
        }



        private void CreateSystemThemes(){

            for (int categoryCounter = 1; categoryCounter < 6; categoryCounter++)
            {
                List<string> cardList = new();

                for (int typCounter = 1; typCounter < 31; typCounter++)
                {
                    cardList.Add(mSystemThemesDirectory+categoryCounter + " (" + typCounter + ").png");
                    
                }
                string name = categoryCounter switch { 1 => "Green Forrest", 2 => "Orange Forrest", 3 => "Purple Forrest", 4 => "Mint Green Forrest", 5 => "Cyan Forrest", _ => "" };
                Theme theme = new(
                    name,
                    cardList,
                    mSystemThemesDirectory + categoryCounter + " game_background.png",
                    mSystemThemesDirectory + categoryCounter + " menu_background.png",
                    mSystemThemesDirectory + categoryCounter + " thumb.png",
                    mSystemThemesDirectory + "point.png",
                    mSystemThemesDirectory + categoryCounter + " bigpoint.png",
                    mSystemThemesDirectory + "life.png",
                    mSystemThemesDirectory + "bonus_timebackground.png",
                    mSystemThemesDirectory + "timebackground.png",
                    mSystemThemesDirectory + "crit_timebackground.png",
                    mSystemThemesDirectory + categoryCounter + " cover.png",
                    "./System/cardflip.wav",
                    "./System/point.wav",
                    "./System/bigpoint.wav",
                    "System/gamebackground.mp3"
                    );
                Themes.Add(name, theme);
            }
        }
        public Image LoadImage(string fileName, Stretch stretch = Stretch.UniformToFill, UriKind uriKind = UriKind.Relative)
        {
            var position = new Uri(fileName, uriKind);
            BitmapImage resource = new(position);
            Image image = new();
            image.Source = resource;
            image.Stretch = stretch;
            return image;
        }

        private void CommandStartGame_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Geht");
        }
    }
}
