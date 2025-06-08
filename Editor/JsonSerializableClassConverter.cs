using System;
using System.Linq;
using UniGame.Core.Runtime.Extension;
using UniModules.Editor;
using UnityEngine;

namespace UniModules.UniGame.TypeConverters.Editor
{
    using Abstract;
    using Newtonsoft.Json;
    using global::UniGame.Runtime.ReflectionUtils;

    [Serializable]
    public class JsonSerializableClassConverter : BaseTypeConverter
    {
        private static Type stringType = typeof(string);

        public sealed override bool CanConvert(Type fromType, Type toType)
        {
            if (fromType != stringType && toType != stringType)
                return false;
            if (fromType.IsRegularType() && toType.IsRegularType())
                return false;

            if (fromType == stringType && toType.HasAttribute<SerializableAttribute>())
                return true;
            
            if (toType == stringType && fromType.HasAttribute<SerializableAttribute>())
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
            
            if (target == stringType) {
                var textValue = JsonConvert.SerializeObject(source);
                result.Result = textValue;
                result.IsComplete = true;
                return result;
            }

            if (source is not string value) return result;
            if (string.IsNullOrEmpty(value)) return result;
            
            try
            {
                var serializedData = JsonConvert.DeserializeObject(value, target);
                result.Result = serializedData;
                result.IsComplete = true;
            }
            catch (JsonReaderException exception)
            {
                var message =
                    $"{nameof(JsonSerializableClassConverter)}: There was exception while deserialization\nTarget: {target};\nValue: {value};\nMessage: {exception.Message}";
                Debug.LogError(message);
                result.Exception = exception;
                result.Message = message;
                return result;
            }
            catch (JsonSerializationException exception)
            {
                var message =
                    $"{nameof(JsonSerializableClassConverter)}: There was exception while deserialization\nTarget: {target};\nValue: {value};\nMessage: {exception.Message}";
                Debug.LogError(message);
                result.Exception = exception;
                result.Message = message;
                return result;
            }

            return result;
        }
    }
}