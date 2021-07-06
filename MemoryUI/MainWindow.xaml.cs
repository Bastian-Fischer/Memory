using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace MemoryUI
{
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
            Themes = new();         
            CreateSystemThemes();
            GetAddons();
            _ = MainFrame.Navigate(new MainMenuPage());
        }
        private void GetAddons()
        {
            if (!Directory.Exists("addons"))
                _ = Directory.CreateDirectory("addons");
            string[] filePaths = Directory.GetFiles(@"addons", "*.mtp", SearchOption.AllDirectories);
            if(filePaths != null) //Hack: filePath existiert immer da GetFiles ein Array liefern muss. Dieses könnte aber 0 einträge haben.
            { 
                foreach (string name in filePaths)
                {
                    string _name = String.Empty;
                    List<BitmapImage> _Cards = new();
                    BitmapImage _gameBackground = new();
                    BitmapImage _menuBackground = new();
                    BitmapImage _thumbnail = new();
                    BitmapImage _point = new();
                    BitmapImage _bigPoint = new();
                    BitmapImage _life = new();
                    BitmapImage _timeBarBigPoint = new();
                    BitmapImage _timeBarPoint = new();
                    BitmapImage _timeBarCritical = new();
                    BitmapImage _cover = new();
                    string _cardFlipSound = String.Empty;
                    string _pointSound = String.Empty;
                    string _bigPointSound = String.Empty;
                    string _gameBackgroundSound = String.Empty;
                    using (ZipArchive archive = ZipFile.Open(name, ZipArchiveMode.Update)) //Hack: OpenRead da du keine änderungen machst
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            //Name
                            _name = name[7..^4];
                            //Pictures
                            if (entry.Name.EndsWith("jpg") || entry.Name.EndsWith("png")) //Hack: Regex rockt!  "[jpg,png]$"
                            {
                                BitmapImage bimg = new();
                                bimg.BeginInit();
                                bimg.StreamSource = entry.Open();
                                bimg.CacheOption = BitmapCacheOption.OnLoad;
                                bimg.EndInit();
                                if (entry.Name[..^4] == "game_background")
                                    _gameBackground = bimg;
                                if (entry.Name[..^4] == "menu_background")
                                    _menuBackground = bimg;
                                if (entry.Name[..^4] == "thumbnail")
                                    _thumbnail = bimg;
                                if (entry.Name[..^4] == "point")
                                    _point = bimg;
                                if (entry.Name[..^4] == "bigpoint")
                                    _bigPoint = bimg;
                                if (entry.Name[..^4] == "life")
                                    _life = bimg;
                                if (entry.Name[..^4] == "bonus_timebackground")
                                    _timeBarBigPoint = bimg;
                                if (entry.Name[..^4] == "timebackground")
                                    _timeBarPoint = bimg;
                                if (entry.Name[..^4] == "critical_timebackground")
                                    _timeBarCritical = bimg;
                                if (entry.Name[..^4] == "cover")
                                    _cover = bimg;
                                if (entry.FullName.Contains("cards/")) 
                                    _Cards.Add(bimg);  
                            }
                            //Sounds
                            if (entry.Name.EndsWith("wav") || entry.Name.EndsWith("mp3"))//Hack: Regex
                            {
                                if (entry.Name[..^4] == "cardflip")
                                    _cardFlipSound = entry.Name;
                                if (entry.Name[..^4] == "point")
                                    _pointSound = entry.Name;
                                if (entry.Name[..^4] == "bigpoint")
                                    _bigPointSound = entry.Name;
                                if (entry.Name[..^4] == "background")
                                    _gameBackgroundSound = entry.Name;
                            }
                        }
                    }
                    if (   _name == String.Empty
                        || _Cards.Count < 3
                        || _gameBackground.PixelHeight == 0
                        || _menuBackground.PixelHeight == 0
                        || _thumbnail.PixelHeight == 0
                        || _point.PixelHeight == 0
                        || _bigPoint.PixelHeight == 0
                        || _life.PixelHeight == 0
                        || _timeBarBigPoint.PixelHeight == 0
                        || _timeBarPoint.PixelHeight == 0
                        || _timeBarCritical.PixelHeight == 0
                        || _cover.PixelHeight == 0
                        || _cardFlipSound == String.Empty
                        || _pointSound == String.Empty
                        || _bigPointSound == String.Empty
                        || _gameBackgroundSound == String.Empty)
                        MessageBox.Show("The theme: " + name + " is flawed");
                    else 
                    {
                        Theme theme =new(
                            false,
                            _name,
                            _Cards,
                            _gameBackground,
                            _menuBackground,
                            _thumbnail,
                            _point,
                            _bigPoint,
                            _life,
                            _timeBarBigPoint,
                            _timeBarPoint,
                            _timeBarCritical,
                            _cover,
                            _cardFlipSound,
                            _pointSound,
                            _bigPointSound,
                            _gameBackgroundSound
                            );
                        Themes.Add(theme.Name, theme);
                    }
                }
            }
        }
        private void CreateSystemThemes()
        {
            for (int categoryCounter = 1; categoryCounter < 6; categoryCounter++)
            {
                List<BitmapImage> cardList = new();
                for (int typCounter = 1; typCounter < 31; typCounter++)
                {
                    cardList.Add(LoadBitmapImage(mSystemThemesDirectory +categoryCounter + " (" + typCounter + ").png"));  
                }
                string name = categoryCounter switch { 1 => "Green Forrest", 2 => "Orange Forrest", 3 => "Purple Forrest", 4 => "Mint Green Forrest", 5 => "Cyan Forrest", _ => "" };
                Theme theme = new(
                    true,
                    name,
                    cardList,
                    LoadBitmapImage(mSystemThemesDirectory + categoryCounter + " game_background.png"),
                    LoadBitmapImage(mSystemThemesDirectory + categoryCounter + " menu_background.png"),
                    LoadBitmapImage(mSystemThemesDirectory + categoryCounter + " thumb.png"),
                    LoadBitmapImage(mSystemThemesDirectory + "point.png"),
                    LoadBitmapImage(mSystemThemesDirectory + categoryCounter + " bigpoint.png"),
                    LoadBitmapImage(mSystemThemesDirectory + "life.png"),
                    LoadBitmapImage(mSystemThemesDirectory + "bonus_timebackground.png"),
                    LoadBitmapImage(mSystemThemesDirectory + "timebackground.png"),
                    LoadBitmapImage(mSystemThemesDirectory + "crit_timebackground.png"),
                    LoadBitmapImage(mSystemThemesDirectory + categoryCounter + " cover.png"),
                    "System/cardflip.wav",
                    "System/point.wav",
                    "System/bigpoint.wav", //Hack einheitliche ordner
                    "System/gamebackground.mp3"
                    );
                Themes.Add(name, theme);
            }
        }
        public Image LoadImage(BitmapImage resource, Stretch stretch = Stretch.UniformToFill)
        {
            Image image = new();
            image.Source = resource;
            image.Stretch = stretch;
            return image;
        }
        public BitmapImage LoadBitmapImage(string fileName, UriKind uriKind = UriKind.RelativeOrAbsolute) {
            var position = new Uri(fileName, uriKind);
            return new(position);
        }
        private void CommandStartGame_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Geht");
        }
    }
}
