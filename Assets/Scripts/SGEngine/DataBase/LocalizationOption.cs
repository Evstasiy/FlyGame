using System.Runtime.Serialization;
using static Assets.Scripts.SGEngine.DataBase.LocalizationOption;

namespace Assets.Scripts.SGEngine.DataBase 
{
    public class LocalizationOption
    {
        public enum LocalizationRegion
        {
            [EnumMember(Value = "Русский")]
            Rus,
            [EnumMember(Value = "English")]
            Eng
        }
    }
    public static class EnumLocalizationRegionExtention
    {
        public static LocalizationRegion ToEnumLanguage(string languageCode)
        {
            switch (languageCode)
            {
                case "kz":
                case "by":
                case "kg":
                case "ru":
                    return LocalizationRegion.Rus;
                default:
                    return LocalizationRegion.Eng;
            }
        }
    }
}
