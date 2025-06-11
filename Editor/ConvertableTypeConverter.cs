namespace UniGame.TypeConverters.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abstract;
    using global::UniGame.Runtime.ReflectionUtils;
    using global::UniGame.Runtime.Utils;
    using UnityEngine;
    using Object = UnityEngine.Object;

#if ODIN_INSPECTOR
     using Sirenix.OdinInspector;
#endif
    
#if UNITY_EDITOR
    using UnityEditor;
#endif
    
    [Serializable]
    public class ConvertableTypeConverter : BaseTypeConverter,IUpdatableOnDomainReload
    {
        [SerializeReference] 
        public List<IGameTypeConverter> converters = new List<IGameTypeConverter>();

        public sealed override bool CanConvert(Type fromType, Type toType)
        {
            foreach (var converter in converters)
            {
                if (converter.CanConvert(fromType, toType)) return true;
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
            
            if(source == null) return result;
            
            foreach (var converter in converters)
            {
                if(!converter.CanConvert(source.GetType(),target)) continue;
                var convertResult = converter.TryConvert(source, target);
                if(!convertResult.IsComplete) continue;
                result.Result = convertResult.Result;
                result.IsComplete = true;
            }

            return result;
        }
        
        
#if ODIN_INSPECTOR
        [Button]
#endif
        public void UpdateConverter()
        {
            FillConverters();
        }
        
#if ODIN_INSPECTOR
        [Button]
#endif
        public ConvertableTypeConverter FillConverters()
        {
#if UNITY_EDITOR

            converters.RemoveAll(x => x == null);
            
            var types = TypeCache.GetTypesDerivedFrom<IGameTypeConverter>();

            foreach (var instanceType in types)
            {
                if(instanceType.IsAbstract || instanceType.IsInterface) continue;
                if(!instanceType.HasDefaultConstructor()) continue;
                
                if(converters.FirstOrDefault(x => x.GetType() == instanceType)!=null) 
                    continue;
                
                var value = instanceType.CreateWithDefaultConstructor();
                var instance = value as IGameTypeConverter;
                if(instance==null || value is Object) continue;
                
                converters.Add(instance);
            }
            
#endif
            return this;
        }

    }
}