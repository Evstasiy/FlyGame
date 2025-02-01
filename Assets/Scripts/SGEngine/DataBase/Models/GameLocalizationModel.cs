using System.Collections.Generic;
using System.Xml.Serialization;

namespace Assets.Scripts.SGEngine.DataBase.Models
{
    public class GameLocalizationModel
    {
        public WorldObjectsLocalizationModel WorldObjectsLocalization {  get; set; }
        public UIItemsLocalizationModel UIItemsLocalizationModel {  get; set; }
        public UpdateLocalizationModel UpdateLocalization {  get; set; }
    }
    
    public class UpdateLocalizationModel
    {
        public List<UpdateItemsLocalizationModel> UpdateGameItemsLocalization { get; set; }   
        public List<BoostItemsLocalizationModel> BoostItemsLocalization { get; set; }   
        public List<UpdateBoostItemsLocalizationModel> UpdateBoostItemsLocalization { get; set; }   
    }
    
    public class WorldObjectsLocalizationModel
    {
        public ItemsLocalizationModel ItemsLocalization { get; set; }   
        public AchivmentsLocalizationModel AchivmentsLocalization { get; set; }   
        public SkinsLocalizationModel SkinsLocalization { get; set; }   
    }
    public class ItemsLocalizationModel
    { 
        public List<DescriptionItemsModel> DescriptionItems {  get; set; }
    }
    public class UIItemsLocalizationModel
    { 
        public List<DescriptionItemsModel> DescriptionItems {  get; set; }
    }
    
    public class AchivmentsLocalizationModel
    { 
        public List<DescriptionItemsModel> DescriptionItems {  get; set; }
    }
    
    public class SkinsLocalizationModel
    { 
        public List<DescriptionItemsModel> DescriptionItems {  get; set; }
    }

    public class DescriptionItemsModel
    {
        public int Id { get; set; }

        public string MainDescription { get; set; }

        public string SecondaryDescription { get; set; }

    }
    
    public class UpdateItemsLocalizationModel
    {
        public int Id { get; set; }

        public string MainDescription { get; set; }

        public string SecondaryDescription { get; set; }

    }
    
    public class BoostItemsLocalizationModel
    {
        public int Id { get; set; }

        public string MainDescription { get; set; }

        public string SecondaryDescription { get; set; }

    }
    
    public class UpdateBoostItemsLocalizationModel
    {
        public int Id { get; set; }

        public string MainDescription { get; set; }

        public string SecondaryDescription { get; set; }

    }
}
