namespace UniGame.TypeConverters.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abstract;
    using TypeConverters;
    using UnityEngine;
    
#if ALCHEMY_INSPECTOR
    using Alchemy.Inspector;
#endif
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    [Serializable]
    public class TypeConverter : ITypeConverter
    {
        
#if ODIN_INSPECTOR
        [ListDrawerSettings(ListElementLabelName = "@ConverterName")]
#endif
        [SerializeReference]
        public List<BaseTypeConverter> converters = new();

        [ContextMenu(nameof(ResetToDefault))]
#if ODIN_INSPECTOR
        [Button]
#endif
#if ALCHEMY_INSPECTOR
        [Button]
#endif
        public void ResetToDefault()
        {
            converters.Clear();

            var convertable = new ConvertableTypeConverter();
            convertable.FillConverters();
            
            converters.Add(new AssetReferenceToStringConverter());
            converters.Add(convertable);
            converters.Add(new AssetToStringConverter());
            converters.Add(new StringToAssetConverter());
            converters.Add(new StringToAssetReferenceConverter());
            converters.Add(new VectorTypeConverter());
            converters.Add(new StringToPercentableTypeConverter());
            converters.Add(new EnumStringToStringConverter());
            converters.Add(new JsonSerializableClassConverter());
            converters.Add(new StringToPrimitiveTypeConverter());
            converters.Add(new PrimitiveTypeConverter());
        }
        
        public bool CanConvert(Type fromType, Type toType)
        {
            return converters.Any(x => x.CanConvert(fromType, toType));
        }

        public TypeConverterResult TryConvert(object source, Type target)
        {
            var result = new TypeConverterResult()
            {
                Result = source,
                IsComplete = false,
                Target = target,
            };
            
            if (source == null) return result;

            for (var i = 0; i < converters.Count; i++) {
                var converter     = converters[i];
                var convertResult = converter.TryConvert(source, target);
                if (convertResult.IsComplete)
                    return convertResult;
            }

            return result;
        }

        public TypeConverterResult ConvertValue(object source, Type toType)
        {
            var result = new TypeConverterResult()
            {
                IsComplete = false,
                Result = null,
            };
            
            if (source == null) return result;
            
            var sourceType = source.GetType();
            if (sourceType == toType || toType.IsAssignableFrom(sourceType))
            {
                result.IsComplete = true;
                result.Result = source;
                return result;
            }

            var convertResult = TryConvert(source, toType);

            if (!convertResult.IsComplete) {
                Debug.LogWarning($"Convert Failed for {source} to Type = {toType.Name}");
            }

            return convertResult;
        }
    }
}