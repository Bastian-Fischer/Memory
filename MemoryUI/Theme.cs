using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MemoryUI
{
    public class Theme
    {
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
    }
}