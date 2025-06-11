namespace UniGame.TypeConverters.Editor
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using Abstract;
    using UnityEngine;

    [Serializable]
    public class StringToPrimitiveTypeConverter : BaseTypeConverter
    {
        private static Type stringType = typeof(string);
        private static Type floatType = typeof(float);
        private const string globalizationSeparator = ",";

        public sealed override bool CanConvert(Type fromType, Type toType)
        {
            var sourceValidation = fromType == stringType || fromType == null;
            var destValidation = toType == stringType || toType.IsPrimitive || toType.IsEnum;
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
            var resultValue = ConvertValue(sourceValue,target);
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

            if (target.IsEnum)
            {
                return Enum.TryParse(target, source, true, out var enumValue) 
                    ? enumValue 
                    : Activator.CreateInstance(target);
            }
            
            if (target == typeof(float)) {
                var floatSource = source.Replace(globalizationSeparator, ".");
                var style   = NumberStyles.Any;
                var culture = CultureInfo.InvariantCulture;
                float.TryParse(floatSource,style,culture,out var resultFloat);
                return resultFloat;
            }
            
            var typeConverter = TypeDescriptor.GetConverter(target);
            var propValue = typeConverter.ConvertFromString(source);
            return propValue;
        }
    }
}