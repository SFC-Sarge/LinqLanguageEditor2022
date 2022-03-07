using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LinqLanguageEditor2022.Extensions
{
    public static class LinqEnumExtensions
    {
        public static int GetEnumIndexFromNameValue<T>(Enum value)
        {
            return Convert.ToInt32($"{(T)Enum.Parse(typeof(T), value.ToString()):D}");
        }
        public static string GetEnumNameFromNameValue<T>(Enum value)
        {
            return Enum.GetNames(typeof(T)).Where(n => n.Equals(value.ToString())).FirstOrDefault().ToString();
        }
        public static T EnumNameValueFromString<T>(string nameValue)
        {
            return (T)Enum.Parse(typeof(T), nameValue);
        }
        public static string GetDescriptionFromEnumValue(Enum value)
        {
            return value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() is not DescriptionAttribute attribute ? value.ToString() : attribute.Description;
        }
        public static T GetEnumValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new ArgumentException();
            FieldInfo[] fields = type.GetFields();
            var field = fields
                            .SelectMany(f =>
                            f.GetCustomAttributes(typeof(DescriptionAttribute), false),
                            (f, a) => new { Field = f, Att = a })
                            .Where(a =>
                            ((DescriptionAttribute)a.Att).Description == description).SingleOrDefault();
            return field == null ? default : (T)field.Field.GetRawConstantValue();
        }
        public static int EnumIndexFromString<T>(string nameValue)
        {
            return (int)Enum.Parse(typeof(T), nameValue);
        }

    }
}
