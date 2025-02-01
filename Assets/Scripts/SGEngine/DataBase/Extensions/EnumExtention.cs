using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Assets.Scripts.SGEngine.DataBase.Extensions
{
    public static class EnumExtention
    {
        public static string? ToEnumMember<T>(this T value) where T : Enum
        {
            return typeof(T)
                .GetTypeInfo()
                .DeclaredMembers
                .SingleOrDefault(x => x.Name == value.ToString())?
                .GetCustomAttribute<EnumMemberAttribute>(false)?
                .Value;
        }
    }
}
