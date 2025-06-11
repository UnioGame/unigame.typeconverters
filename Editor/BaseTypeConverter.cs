namespace UniGame.TypeConverters.Editor
{
    using System;
    using Abstract;
    
#if ALCHEMY_INSPECTOR
    using Alchemy.Inspector;
#endif
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif
#if UNITY_EDITOR
    using UniModules.Editor;
#endif
    
    [Serializable]
    public abstract class BaseTypeConverter : ITypeConverter
    {
        public abstract bool CanConvert(Type fromType, Type toType);

        public abstract TypeConverterResult TryConvert(object source, Type target);

#if UNITY_EDITOR  
        
#if ODIN_INSPECTOR
        [GUIColor(0.2f, 0.8f, 0.2f)]
        [Button(icon:SdfIconType.PlayBtn)]
        public void OpenScript()
        {
            this.GetType().OpenScript();
        }
#endif
#if ALCHEMY_INSPECTOR
        [Button]
        public void OpenScript()
        {
            this.GetType().OpenScript();
        }
#endif
#endif
    }
}