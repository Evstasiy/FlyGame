using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.SGEngine.DataBase.Models
{
    public class GameItemModel : IItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsLock { get; set; }
        public int Price { get; set; }

        #region NotMapped
        public bool IsUserHas { get; set; } = false;

        #endregion NotMapped

        public int GetId()
        {
            return Id;
        }

        public static implicit operator SaveGameItemModel(GameItemModel gameItem)
        {
            return new SaveGameItemModel { Id = gameItem.Id, IsLock = gameItem.IsLock };
        }
    }
}
