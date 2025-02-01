using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;

namespace Assets.Scripts.SGEngine.DataBase.Models
{
    public class MarketModel : IItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public EnumMarkets MarketType { get; set; }

        public int GetId()
        {
            return Id;
        }
    }
}
