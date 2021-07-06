using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Windows.Media.Imaging;

namespace MemoryUI
{
    public class Theme
    {
        public bool SystemTheme;
        public List<BitmapImage> CardList { get; private set; }
        public BitmapImage GameBackground { get; private set; }
        public BitmapImage MenuBackground { get; private set; }
        public BitmapImage Thumbnail { get; private set; }
        public BitmapImage Point { get; private set; }
        public BitmapImage BigPoint { get; private set; }
        public BitmapImage Life { get; private set; }
        public BitmapImage TimeBarBigPoint { get; private set; }
        public BitmapImage TimeBarPoint { get; private set; }
        public BitmapImage TimeBarCritical { get; private set; }
        public BitmapImage Cover { get; private set; }
        public string Name { get; private set; }
        public string CardFlipSound { get; private set; }
        public string PointSound    { get; private set; }
        public string BigPointSound { get; private set; }
        public string GameBackgroundSound { get; private set; }

        public Theme(
            bool systemTheme,
            string name,
            List<BitmapImage> cardList,
            BitmapImage gameBackground,
            BitmapImage menuBackground,
            BitmapImage thumbnail,
            BitmapImage point,
            BitmapImage bigPoint,
            BitmapImage life,
            BitmapImage timeBarBigPoint,
            BitmapImage timeBarPoint,
            BitmapImage timeBarCritical,
            BitmapImage cover,
            string cardFlipSound,
            string pointSound,
            string bigPointSound,
            string gameBackgroundSound
            )
        {
            SystemTheme = systemTheme;
            Name  = name;
            CardList = cardList;
            GameBackground = gameBackground;
            MenuBackground = menuBackground;
            Thumbnail = thumbnail;
            Point = point;
            BigPoint = bigPoint;
            Life = life;
            TimeBarBigPoint = timeBarBigPoint;
            TimeBarPoint = timeBarPoint;
            TimeBarCritical = timeBarCritical;
            Cover = cover;
            CardFlipSound = cardFlipSound;
            PointSound = pointSound;
            BigPointSound = bigPointSound;
            GameBackgroundSound = gameBackgroundSound;
        }
        public BitmapImage GetPoint(MemoryLogic.ScorePoint pointTyp) {
            return (pointTyp == MemoryLogic.ScorePoint.Point) ? Point : BigPoint ;
        }

        public void LoadThemeSounds() 
        {
            if (!SystemTheme) 
            {
                if (!Directory.Exists("System/Temp/")) _ = Directory.CreateDirectory("System/Temp/");
                if (File.Exists("System/Temp/" + CardFlipSound)) File.Delete("System/Temp/" + CardFlipSound);
                if (File.Exists("System/Temp/" + PointSound)) File.Delete("System/Temp/" + PointSound);
                if (File.Exists("System/Temp/" + BigPointSound)) File.Delete("System/Temp/" + BigPointSound);
                if (File.Exists("System/Temp/" + GameBackgroundSound)) File.Delete("System/Temp/" + GameBackgroundSound);
                using (ZipArchive archive = ZipFile.Open("addons/"+Name+".mtp", ZipArchiveMode.Update)) {
                    archive.GetEntry(CardFlipSound).ExtractToFile("System/Temp/" + CardFlipSound);
                    archive.GetEntry(PointSound).ExtractToFile("System/Temp/" + PointSound);
                    archive.GetEntry(BigPointSound).ExtractToFile("System/Temp/" + BigPointSound);
                    archive.GetEntry(GameBackgroundSound).ExtractToFile("System/Temp/" + GameBackgroundSound);
                }
            }
        }
    }
}
