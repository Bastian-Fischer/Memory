using System.Collections.Generic;
using System.Windows.Controls;
namespace MemoryUI
{
    public class Theme
    {
        public List<string> CardList { get; private set; }
        public string GameBackground { get; private set; }
        public string MenuBackground { get; private set; }
        public string Thumbnail { get; private set; }
        public string Point { get; private set; }
        public string BigPoint { get; private set; }
        public string Life { get; private set; }
        public string TimeBarBigPoint { get; private set; }
        public string TimeBarPoint { get; private set; }
        public string TimeBarCritical { get; private set; }
        public string Cover { get; private set; }
        public string Name { get; private set; }
        public string CardFlipSound { get; private set; }
        public string PointSound    { get; private set; }
        public string BigPointSound { get; private set; }
        public string GameBackgroundSound { get; private set; }
        public Theme(
            string name,
            List<string> cardList,
            string gameBackground,
            string menuBackground,
            string thumbnail,
            string point,
            string bigPoint,
            string life,
            string timeBarBigPoint,
            string timeBarPoint,
            string timeBarCritical,
            string cover,
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
        public string GetPoint(MemoryLogic.ScorePoint pointTyp) {
            return (pointTyp == MemoryLogic.ScorePoint.Point) ? Point : BigPoint ;
        }
    }
}