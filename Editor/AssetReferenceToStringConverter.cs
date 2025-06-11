namespace UniGame.TypeConverters.Editor
{
    using System;
    using Abstract;
    using UniModules.Editor;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    [Serializable]
    public class AssetReferenceToStringConverter : BaseTypeConverter
    {
        #region inspector

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.LabelWidth(120)]
#endif
        public bool addTypeFilter = true;
        
        #endregion
        
        private static Type assetReferenceType = typeof(AssetReference);
        private static Type stringType = typeof(string);
        
        public sealed override bool CanConvert(Type fromType, Type toType)
        {
            return toType == stringType && assetReferenceType.IsAssignableFrom(fromType);
        }

        public sealed override TypeConverterResult TryConvert(object source, Type target)
        {
            var result = new TypeConverterResult()
            {
                Result = source,
                IsComplete = false,
                Target = target,
            };
            
            if (source == null) return result;
            
            var sourceType = source.GetType();

            if(!CanConvert(sourceType, target)) return result;
 
            var reference = source as AssetReference;
            var asset     = reference?.editorAsset;
            
            var assetName = asset == null ? 
                string.Empty : 
                addTypeFilter ? 
                    $"t:{asset.GetType().Name} {asset.name}" :
                    asset.name;
            
            result.Result = assetName;
            result.IsComplete = true;
            
            return result;
        }
        
    }
}
