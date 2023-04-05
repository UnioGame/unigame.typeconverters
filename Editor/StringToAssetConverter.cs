namespace UniModules.UniGame.TypeConverters.Editor
{
    using System;
    using System.Linq;
    using Abstract;
    using UniModules.Editor;
    using global::UniGame.Core.Runtime.Extension;

    [Serializable]
    public class StringToAssetConverter : BaseTypeConverter
    {
        private static Type stringType = typeof(string);
        
        public sealed override bool CanConvert(Type fromType, Type toType)
        {
            return fromType == stringType && toType.IsAsset();
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

            var assetFilter = source as string;
            var assets = AssetEditorTools.GetAssets(target,assetFilter);
            
            result.Result = assets.FirstOrDefault();
            result.IsComplete = assets.Count > 0;
            
            return result;
        }
    }
}
