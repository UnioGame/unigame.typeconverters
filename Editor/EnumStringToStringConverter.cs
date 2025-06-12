using System;

namespace UniGame.TypeConverters.Editor
{
    using Abstract;

    [Serializable]
    public class EnumStringToStringConverter : BaseTypeConverter
    {
        private static Type stringType = typeof(string);

        public sealed override bool CanConvert(Type fromType, Type toType)
        {
            if (toType != stringType) return false;
            if (fromType.IsEnum || fromType == stringType)
                return true;
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

            return result;
        }
    }
}