namespace UniModules.UniGame.TypeConverters.Editor
{
    using System;
    using Abstract;
    using Core.EditorTools.Editor;
    using UniModules.Editor;
    using UnityEngine;

    [CreateAssetMenu(menuName = "UniGame/ObjectTypeConverter/Create Converter", fileName = nameof(ObjectTypeConverter))]
    public class ObjectTypeConverter : ScriptableObject, ITypeConverter
    {
        #region static data

        private static string _defaultConverterPath;
        
        public static string DefaultConverterPath => _defaultConverterPath = string.IsNullOrEmpty(_defaultConverterPath) ?
                EditorFileUtils.Combine(EditorPathConstants.GeneratedContentPath,"TypeConverters/Editor/") : 
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
        }
        
        public bool CanConvert(Type fromType, Type toType)
        {
            return typeConverter.CanConvert(fromType,toType);
        }

        public (bool isValid, object result) TryConvert(object source, Type target)
        {
            return typeConverter.TryConvert(source, target);
        }

        public object ConvertValue(object source, Type toType)
        {
            return typeConverter.ConvertValue(source, toType);
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