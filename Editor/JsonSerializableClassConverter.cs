using System;
using System.Linq;
using UniGame.Core.Runtime.Extension;
using UniModules.Editor;
using UnityEngine;

namespace UniModules.UniGame.TypeConverters.Editor
{
    using Newtonsoft.Json;
    using UniModules.UniCore.Runtime.ReflectionUtils;

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

        public sealed override (bool isValid, object result) TryConvert(object source, Type target)
        {
            if (source == null || !CanConvert(source.GetType(), target)) {
                return (false, source);
            }
            
            if (target == stringType) {
                var textValue = JsonConvert.SerializeObject(source);
                return (true,textValue);
            }

            if (source is string value) {
                try
                {
                    var serializedData = JsonConvert.DeserializeObject(value, target);
                    return (true,serializedData);
                }
                catch (JsonReaderException exception)
                {
                    Debug.LogError($"{nameof(JsonSerializableClassConverter)}: There was exception while deserialization\nTarget: {target};\nValue: {value};\nMessage: {exception.Message}");
                    
                    return (false, source);
                }
                catch (JsonSerializationException exception)
                {
                    Debug.LogError($"{nameof(JsonSerializableClassConverter)}: There was exception while deserialization\nTarget: {target};\nValue: {value};\nMessage: {exception.Message}");

                    return (false, source);
                }
            }
            
            return (false, source);
        }
    }
}