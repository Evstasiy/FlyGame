using System.Xml.Serialization;

namespace Assets.Scripts.SGEngine.DataBase.Models
{
    [System.Serializable]
    public class PlayerFeatureModel
    {
        public int MainMoney;

        public int SpecialMoney;

        public int Experience;

        public int SelectedSkinId;

        public int PlayerDistanceRecord;

        public int PlayerTutorialPoint;
    }
}
