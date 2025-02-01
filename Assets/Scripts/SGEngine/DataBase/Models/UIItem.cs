using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;

namespace Assets.Scripts.SGEngine.DataBase.Models
{
    public class UIItem : IItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string InternalName { get; set; }

        public static explicit operator UIItem(SkinXML Item)
        {
            return new UIItem { Id = Item.Id };
        }

        public int GetId()
        {
            return Id;
        }
    }
}
