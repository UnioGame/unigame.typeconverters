namespace UniModules.UniGame.TypeConverters.Editor
{
    using System;
    using System.Linq;
    using Abstract;
    using UniModules.Editor;
    using global::UniGame.Core.Runtime.Extension;
    using UnityEngine;
    using Object = UnityEngine.Object;

    [Serializable]
    public class AssetToStringConverter : BaseTypeConverter
    {
        #region inspector

        public bool addTypeFilter = true;
        
        #endregion
        
        private static Type stringType = typeof(string);
        
        public sealed override bool CanConvert(Type fromType, Type toType)
        {
            return toType == stringType && fromType.IsAsset();
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

            if (source is not Object asset) return result;
            
            var value = addTypeFilter ? 
                $"t:{asset.GetType().Name} {asset.name}" : 
                asset.name;
            result.Result = value;
            result.IsComplete = true;
            return result;

        }
    }
}
