using Microsoft.AspNetCore.Http.Connections;
using System.ComponentModel.DataAnnotations;

namespace Labb1EntityFramework.Utility
{
    public static class EnumHelper
    {
        // Static method to fetch a enum's name.
        public static string GetEnumName(Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var member = enumType.GetMember(enumValue.ToString());            
            var attributes = member[0].GetCustomAttributes(typeof(DisplayAttribute), false);
            var name = ((DisplayAttribute)attributes[0]).Name;

            if (name != null)
            {
                return name;
            }
            else
            {
                return enumValue.ToString();
            }            
        }
    }
}
