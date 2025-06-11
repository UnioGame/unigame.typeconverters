namespace UniGame.TypeConverters.Editor
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using Abstract;
    using UnityEngine;

    public struct Percentable
    {
        public float Value;
        public bool IsPercent;

        public Percentable(float value, bool isPercent)
        {
            Value = value;
            IsPercent = isPercent;
        }
    }

    [Serializable]
    public class StringToPercentableTypeConverter : BaseTypeConverter
    {
        private static Type stringType = typeof(string);
        private static Type percentableType = typeof(Percentable);
        private const string globalizationSeparator = ",";

        public sealed override bool CanConvert(Type fromType, Type toType)
        {
            var sourceValidation = fromType == stringType || fromType == null;
            var destValidation = toType == percentableType;
            return sourceValidation && destValidation;
        }

        public sealed override TypeConverterResult TryConvert(object source, Type target)
        {
            var result = new TypeConverterResult()
            {
                Result = source,
                IsComplete = false,
                Target = target,
            };

            var sourceType = source?.GetType();
            if (!CanConvert(sourceType, target))
                return result;

            var sourceValue = source == null ? string.Empty : source as string;
            var resultValue = ConvertValue(sourceValue, target);
            result.IsComplete = true;
            result.Result = resultValue;

            return result;
        }

        public object ConvertValue(string source, Type target)
        {
            //create default value of type
            if (string.IsNullOrEmpty(source))
            {
                if (target == typeof(string))
                    return string.Empty;

                return Activator.CreateInstance(target);
            }

            if (target == typeof(Percentable))
            {
                var style = NumberStyles.Any;
                var culture = CultureInfo.InvariantCulture;
                var isPercent = false;
                if (source.EndsWith('%'))
                {
                    source = source.Substring(0, source.Length - 1);
                    isPercent = true;
                }
                var floatSource = source.Replace(globalizationSeparator, ".");
                float.TryParse(floatSource, style, culture, out var resultFloat);
                
                return new Percentable(resultFloat, isPercent);
            }

            var typeConverter = TypeDescriptor.GetConverter(target);
            var propValue = typeConverter.ConvertFromString(source);
            return propValue;
        }
    }
}