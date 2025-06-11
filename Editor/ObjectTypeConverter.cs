namespace UniGame.TypeConverters.Editor
{
    using System;
    using Abstract;
    using UniModules;
    using UnityEngine;
    
#if UNITY_EDITOR
    using UnityEditor;
    using UniModules.Editor;
    using UnityEngine.Profiling;
#endif
    
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    [CreateAssetMenu(menuName = "UniGame/ObjectTypeConverter/Create Converter", fileName = nameof(ObjectTypeConverter))]
    public class ObjectTypeConverter : ScriptableObject, ITypeConverter
    {
        #region static data

        private static string _defaultConverterPath;
        
        public static string DefaultConverterPath => _defaultConverterPath = string.IsNullOrEmpty(_defaultConverterPath) ?
                FileUtils.Combine(EditorPathConstants.GeneratedContentPath,"TypeConverters/Editor/") : 
                _defaultConverterPath;

        private static ObjectTypeConverter _typeConverters;
        public static ObjectTypeConverter TypeConverters {
            get {
                if (_typeConverters)
                    return _typeConverters;
                
                _typeConverters = AssetEditorTools.GetAsset<ObjectTypeConverter>();
                if (!_typeConverters) {
                    _typeConverters = ScriptableObject.CreateInstance<ObjectTypeConverter>();
                    _typeConverters.ResetToDefault();
                    _typeConverters.SaveAsset(nameof(ObjectTypeConverter), DefaultConverterPath);
                }

                return _typeConverters;
            }
        }

        #endregion
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ListDrawerSettings(Expanded = true)]
        [Sirenix.OdinInspector.InlineProperty]
        [Sirenix.OdinInspector.HideLabel]
#endif
        public TypeConverter typeConverter = new TypeConverter();

        [ContextMenu(nameof(ResetToDefault))]
        public void ResetToDefault()
        {
            typeConverter.ResetToDefault();
            UpdateConverters();
        }
        
        public bool CanConvert(Type fromType, Type toType)
        {
            return typeConverter.CanConvert(fromType,toType);
        }

        public TypeConverterResult TryConvert(object source, Type target)
        {
            return typeConverter.TryConvert(source, target);
        }

        public TypeConverterResult ConvertValue(object source, Type toType)
        {
            return typeConverter.ConvertValue(source, toType);
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void UpdateConverters()
        {
#if UNITY_EDITOR
            var assets = AssetEditorTools.GetAssets<ObjectTypeConverter>();
            foreach (var asset in assets)
            {
                asset.Rebuild();
            }
#endif
        }

#if ODIN_INSPECTOR
        [Button]
#endif
        public void Rebuild()
        {
#if UNITY_EDITOR
            var converters = typeConverter.converters;
            
            foreach (var converter in converters)
            {
                if(converter is IUpdatableOnDomainReload updatable)
                    updatable.UpdateConverter();
            }
            
            this.MarkDirty();
#endif
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.OnInspectorInit]
#endif
        private void OnInspectorInitialize()
        {
            if(typeConverter.converters.Count == 0)
                typeConverter.ResetToDefault();
        }
    }
}