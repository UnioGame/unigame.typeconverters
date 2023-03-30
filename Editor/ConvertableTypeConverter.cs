namespace UniModules.UniGame.TypeConverters.Editor
{
    using System;
    using System.Collections.Generic;
    using Abstract;
    using UniCore.Runtime.ReflectionUtils;
    using UniCore.Runtime.Utils;
    using UnityEngine;
    using Object = UnityEngine.Object;

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

        public sealed override (bool isValid, object result) TryConvert(object source, Type target)
        {
            if(source == null) return (false, source);
            
            foreach (var converter in converters)
            {
                if(!converter.CanConvert(source.GetType(),target)) continue;
                var result = converter.TryConvert(source, target);
                if(result.isValid) return result;
            }

            return (false, source);
        }
        
        public void UpdateConverter()
        {
            FillConverters();
        }
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public ConvertableTypeConverter FillConverters()
        {
#if UNITY_EDITOR

            converters.Clear();
            var typeInstances = typeof(IGameTypeConverter).GetAssignableTypes();

            foreach (var instanceType in typeInstances)
            {
                if(!instanceType.HasDefaultConstructor()) continue;
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