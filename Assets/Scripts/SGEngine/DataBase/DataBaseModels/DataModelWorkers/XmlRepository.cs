using Assets.Scripts.SGEngine.DataBase.DataBaseModels.Interfaces;
using Assets.Scripts.SGEngine.DataBase.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers
{
    public class XmlRepository<T> : IRepository<T> where T : class
    {
        private readonly string _readFilePath;
        private readonly string _writeFilePath;

        public XmlRepository(string readFilePath, string writeFilePath)
        {
            _readFilePath = readFilePath;
            _writeFilePath = writeFilePath;
        }

        public IEnumerable<T> GetAll()
        {
            var data = GetGlobalInfoFile(_readFilePath);
            if (typeof(T) == typeof(AchievementModel))
            {
                return data.WorldObjects.AchivmentsItems.AchivmentItems.Cast<T>().ToList();
            } /*else if (typeof(T) == typeof(GameItemModel))
            {
                return data.GameItems.Cast<T>().ToList();
            }*/
            return new List<T>();
        }

        public T GetById(int id)
        {
            var allItems = GetAll();
            return allItems.FirstOrDefault(item => GetEntityId(item) == id);
        }

        public void Add(T entity)
        {
            var data = LoadData(_writeFilePath);
            if (typeof(T) == typeof(AchievementModel))
            {
                data.Save_WorldObjects.Save_Achivments.SaveAchivmentItem.Add(entity as AchievementModel);
            } /*else if (typeof(T) == typeof(GameItemModel))
            {
                data.GameItems.Add(entity as GameItemModel);
            }*/
            SaveData(data, _writeFilePath);
        }

        public void Update(T entity)
        {
            var data = LoadData(_writeFilePath);
            if (typeof(T) == typeof(AchievementModel))
            {
                var index = data.Save_WorldObjects.Save_Achivments.SaveAchivmentItem.FindIndex(item => item.Id == GetEntityId(entity));
                if (index != -1)
                {
                    data.Save_WorldObjects.Save_Achivments.SaveAchivmentItem[index] = entity as AchievementModel;
                }
            } 
            /*else if (typeof(T) == typeof(GameItemModel))
            {
                var index = data.GameItems.FindIndex(item => item.Id == GetEntityId(entity));
                if (index != -1)
                {
                    data.GameItems[index] = entity as GameItemModel;
                }
            }*/
            SaveData(data, _writeFilePath);
        }

        public void Delete(int id)
        {
            var data = LoadData(_writeFilePath);
            if (typeof(T) == typeof(AchievementModel))
            {
                data.Save_WorldObjects.Save_Achivments.SaveAchivmentItem.RemoveAll(item => item.Id == id);
            } else if (typeof(T) == typeof(GameItemModel))
            {
                data.Save_WorldObjects.Save_Items.SaveItemList.RemoveAll(item => item.Id == id);
            }
            SaveData(data, _writeFilePath);
        }

        private void SaveData(SaveGameInformation data, string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(SaveGameInformation));
                serializer.Serialize(stream, data);
            }
        }

        private SaveGameInformation LoadData(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new SaveGameInformation();
            }

            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof(SaveGameInformation));
                return (SaveGameInformation)serializer.Deserialize(stream);
            }
        }

        private int GetEntityId(T entity)
        {
            var property = typeof(T).GetProperty("Id");
            if (property != null)
            {
                return (int)property.GetValue(entity);
            }
            throw new Exception("Entity does not have an Id property");
        }
        
        private GameParameters GetGlobalInfoFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new GameParameters();
            }

            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof(GameParameters));
                return (GameParameters)serializer.Deserialize(stream);
            }
        }
    }
}
