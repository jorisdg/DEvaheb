using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEvahebLib.Enums
{
    public abstract class EnumTableBase<T>
    {
        public string EnumName { get; protected set; }

        protected Dictionary<string, T> enumValues;

        public IEnumerable<KeyValuePair<string, T>> EnumTable { get { return enumValues; } }

        public EnumTableBase(string name, Dictionary<string, T> values)
        {
            EnumName = name;
            enumValues = values;
        }

        public string GetEnum(T enumValue)
        {
            return enumValues.FirstOrDefault(e => e.Value.Equals(enumValue)).Key;
        }

        public T GetValue(string enumKey)
        {
            return enumValues.FirstOrDefault(e => e.Key.Equals(enumKey)).Value;
        }

        public bool HasEnum(string enumKey)
        {
            return enumValues.ContainsKey(enumKey);
        }

        public bool HasValue(T value)
        {
            return enumValues.ContainsValue(value);
        }
    }

    public class EnumTableFloat : EnumTableBase<float>
    {
        public EnumTableFloat(string name, Dictionary<string, float> values)
            : base(name, values)
        {
        }

        public static EnumTableFloat FromEnum(Type enumType)
        {
            if (!enumType.IsEnum)
                throw new Exception("EnumValue node has to be instantiated with an type that is an enumeration");

            var enumValues = new Dictionary<string, float>();
            var values = enumType.GetEnumValues();
            for (int i = 0; i < values.Length; i++)
            {
                var value = (Int32)values.GetValue(i);

                enumValues.Add(enumType.GetEnumName(value), value);
            }

            return new EnumTableFloat(name: enumType.Name, enumValues);
        }
    }

    public class EnumTableInt : EnumTableBase<Int32>
    {
        public EnumTableInt(string name, Dictionary<string, Int32> values)
            : base(name, values)
        {
        }

        public static EnumTableInt FromEnum(Type enumType)
        {
            if (!enumType.IsEnum)
                throw new Exception("EnumValue node has to be instantiated with an type that is an enumeration");

            var enumValues = new Dictionary<string, Int32>();
            var values = enumType.GetEnumValues();
            for (int i = 0; i < values.Length; i++)
            {
                var value = (Int32)values.GetValue(i);

                enumValues.Add(enumType.GetEnumName(value), value);
            }

            return new EnumTableInt(name: enumType.Name, enumValues);
        }
    }

    public class EnumTableString : EnumTableBase<string>
    {
        public EnumTableString(string name, Dictionary<string, string> values)
            : base(name, values)
        {
        }

        public static EnumTableString FromEnum(Type enumType)
        {
            if (!enumType.IsEnum)
                throw new Exception("EnumValue node has to be instantiated with an type that is an enumeration");

            var enumValues = new Dictionary<string, string>();
            var values = enumType.GetEnumNames();

            for (int i = 0; i < values.Length; i++)
            {
                enumValues.Add(key: values[i], value: values[i]);
            }

            return new EnumTableString(name: enumType.Name, enumValues);
        }
    }
}
