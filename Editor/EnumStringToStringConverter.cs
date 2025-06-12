using System;

namespace UniGame.TypeConverters.Editor
{
    using System.Linq;
    using Abstract;

    [Serializable]
    public class EnumStringToStringConverter : BaseTypeConverter
    {
        private static Type stringType = typeof(string);

        public sealed override bool CanConvert(Type fromType, Type toType)
        {
            if(fromType == toType) return true;
            
            if (toType == stringType)
            {
                if(fromType.IsEnum || fromType == stringType)
                    return true;
                return false;
            }

            if (toType.IsEnum)
            {
                if (fromType == stringType ||
                    fromType == typeof(int) ||
                    fromType == typeof(byte) ||
                    fromType == typeof(short))
                    return true;
            }

            return false;
        }

        public sealed override TypeConverterResult TryConvert(object source, Type target)
        {
            var result = new TypeConverterResult()
            {
                Result = source,
                IsComplete = false,
                Target = target,
            };
            
            if (source == null || !CanConvert(source.GetType(), target)) {
                return result;
            }
            
            var sourceType = source.GetType();

            if (target == sourceType)
            {
                result.IsComplete = true;
                result.Result = source;
                return result;
            }
            
            if (target == stringType)
            {
                if (sourceType == stringType)
                {
                    result.Result = source;
                    result.IsComplete = true;
                    return result;
                }

                if (sourceType.IsEnum)
                {
                    result.Result = Enum.GetName(sourceType, source);
                    result.IsComplete = true;
                    return result;
                }
            }
            
            if(target.IsEnum)
            {
                if (sourceType == stringType)
                {
                    var sourceString = source as string;
                    //if we have empty string, we can use first enum name as default
                    if (string.IsNullOrEmpty(sourceString))
                        sourceString = Enum.GetNames(target).FirstOrDefault();

                    if (string.IsNullOrEmpty(sourceString))
                        return result;
                    
                    // remove all spaces from string
                    sourceString = sourceString.Replace(" ", string.Empty);
                    
                    result.IsComplete = Enum.TryParse(target,sourceString, true, out var enumValue);
                    result.Result = enumValue;
                    return result;
                }
                if (sourceType == typeof(int) || sourceType == typeof(byte) 
                                              || sourceType == typeof(short))
                {
                    var enumValue = Enum.ToObject(target, source);
                    result.Result = enumValue;
                    result.IsComplete = true;
                    return result;
                }
            }
            
            return result;
        }
    }
}