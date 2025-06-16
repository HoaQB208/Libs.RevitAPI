using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Libs.RevitAPI._Common
{
    public class EnumUtils
    {
        public static string GetDescription(Enum e)
        {
            System.Reflection.MemberInfo[] members = e.GetType().GetMember(e.ToString());
            object[] attributes = members[0].GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);
            DescriptionAttribute descriptionAttribute = attributes[0] as DescriptionAttribute;
            return descriptionAttribute.Description;
        }

        public static List<string> GetDescriptions(Type enumType)
        {
            List<string> descs = new List<string>();
            string[] names = Enum.GetNames(enumType);
            foreach (string name in names)
            {
                object[] attributes = enumType.GetField(name).GetCustomAttributes(typeof(DescriptionAttribute), true);
                DescriptionAttribute descriptionAttribute = attributes[0] as DescriptionAttribute;
                descs.Add(descriptionAttribute.Description);
            }
            return descs;
        }
        public static ObservableCollection<T> GetEumns<T>()
        {
            return new ObservableCollection<T>(Enum.GetValues(typeof(T)).Cast<T>());
        }
    }
}