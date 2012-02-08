using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.ComponentModel;
using System.Reflection;

namespace Btl.Models
{
    public class EnumExtension : MarkupExtension
    {
        private Type enumType;

        public EnumExtension()
        {
        }

        public EnumExtension(Type enumType)
        {
            EnumType = enumType;
        }

        public Type EnumType
        {
            get { return enumType; }
            set
            {
                if (enumType == value) return;
                Type type = Nullable.GetUnderlyingType(value) ?? value;
                if (!type.IsEnum)
                    throw new ArgumentNullException("Type must be for an enumeration");
                enumType = value;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (enumType == null)
                throw new ArgumentNullException("The EnumType must be specified");
            Type actualType = Nullable.GetUnderlyingType(enumType) ?? enumType;

            List<string> items = new List<string>();
            if (actualType != enumType)
                items.Add(null);

            foreach (object item in Enum.GetValues(enumType))
            {
                string itemString = item.ToString();
                FieldInfo fInfo = enumType.GetField(itemString);
                object[] attribs = fInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attribs != null && attribs.Length > 0)
                    itemString = ((DescriptionAttribute)attribs[0]).Description;
                items.Add(itemString);
            }

            return items.ToArray();
        }
    }
}
