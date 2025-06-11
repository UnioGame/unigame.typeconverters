using UniGame.Core.Runtime.Extension;

namespace UniGame.TypeConverters.Editor
{
    using System;
    using System.Linq;
    using Abstract;
    using UniModules.Editor;
    
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
            var asset = AssetEditorTools.GetAsset(target,assetFilter);
            if (asset == null)
                asset = AssetEditorTools.GetAsset(assetFilter);
            
            result.Result = asset;
            result.IsComplete = asset!=null;
            
            return result;
        }
    }
}
