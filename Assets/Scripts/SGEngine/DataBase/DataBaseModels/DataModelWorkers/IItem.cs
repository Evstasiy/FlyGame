using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers
{
    public interface IItem {

        /// <summary>
        /// Возвращает id объекта
        /// </summary>
        /// <returns></returns>
        int GetId();
    }
}
