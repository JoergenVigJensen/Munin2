using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using Munin.DAL;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Munin.Web
{
    public static class Utils
    {
        public static JsonSerializerSettings JsonSettings()
        {
            var settings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize
            };

            return settings;
        }

        public static List<UISelectItem> SelectListOf<T>()
        {

            List<UISelectItem> keyList = new List<UISelectItem>();
            Type t = typeof(T);
            if (t.IsEnum)
            {
                var v = Enum.GetValues(t);
                foreach (var e in v)
                {

                    //keyList.Add(new UISelectItem() { Value = (int)e, Text = GetEnumDescription<T>((T)e)});
                    keyList.Add(new UISelectItem() { Value = (int)e, Text = e.ToString() });
                }

                return keyList;
            }
            return null;
        }


        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr =
                           Attribute.GetCustomAttribute(field,
                           typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }

        public static string GetEnumDescription<T>(T source)
        {
            var enumType = source.GetType().GetField(source.ToString());
            if (enumType != null)
            {
                var display = ((DisplayAttribute[])enumType.GetCustomAttributes(typeof(DisplayAttribute), false)).FirstOrDefault();
                if (display != null)
                {
                    return display.Name;
                }
            }
            return "";
        }
    }

    public class UISelectItem
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }
}