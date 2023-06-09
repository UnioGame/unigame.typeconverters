namespace UniModules.UniGame.TypeConverters.Editor
{
    using System;
    using Abstract;

    [Serializable]
    public class PrimitiveTypeConverter : BaseTypeConverter
    {
        private static Type stringType = typeof(string);

        public override bool CanConvert(Type fromType, Type toType)
        {
            if (fromType == toType || toType == stringType)
                return true;
            var result =
                (toType.IsPrimitive && toType.IsPrimitive) ||
                (fromType == stringType && toType.IsPrimitive);

            return result;
        }

        public override TypeConverterResult TryConvert(object source, Type target)
        {
            var result = new TypeConverterResult()
            {
                Result = source,
                IsComplete = false,
                Target = target,
            };
            
            if (source == null) {
                return ConvertToDefault(target,result);
            }

            var sourceType = source.GetType();
            var canConvert = CanConvert(sourceType, target);
            if (!canConvert) return result;

            result.IsComplete = true;
            result.Result = ConvertValue(source, target);
            
            return result;
        }

        public object ConvertValue(object source, Type target)
        {
            return CanConvert(source.GetType(), target) ? 
                Convert.ChangeType(source, target) : 
                source;
        }

        private TypeConverterResult ConvertToDefault(Type target,TypeConverterResult result)
        {
            switch (target) {
                case Type {IsPrimitive: true}:
                {
                    result.IsComplete = true;
                    result.Result = Activator.CreateInstance(target);
                    return result;
                }
                case Type to when to == typeof(string):
                {
                    result.IsComplete = true;
                    result.Result = string.Empty;
                    return  result;
                }   
                default:
                    return result;
            }

            return result;
        }
    }
}