namespace UniGame.TypeConverters.Editor
{
    using System;
    using Abstract;
    using UnityEngine;

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

            var convertResult = ConvertValue(source, target);
            
            result.IsComplete = convertResult.success;
            result.Result = convertResult.value;
            
            return result;
        }

        public ConvertResult ConvertValue(object source, Type target)
        {
#if UNITY_EDITOR
            try
            {
#endif
                var canConvert = CanConvert(source.GetType(), target);
                if (!canConvert) return new ConvertResult(){success = false, value = source};

                var result = Convert.ChangeType(source, target);

                return new ConvertResult()
                {
                    success = true,
                    value = result,
                };
#if UNITY_EDITOR
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to convert {source?.GetType().Namespace} to {target.Name}: {e.Message}");
                return new ConvertResult(){success = false, value = source};
            }
#endif
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
        
        [Serializable]
        public struct ConvertResult
        {
            public bool success;
            public object value;
        }
    }
}