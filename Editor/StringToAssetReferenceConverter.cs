namespace UniModules.UniGame.TypeConverters.Editor
{
    using System;
    using Abstract;
    using AddressableExtensions.Editor;
    using UniModules.Editor;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using Object = UnityEngine.Object;

    [Serializable]
    public class StringToAssetReferenceConverter : BaseTypeConverter
    {
        private static Type assetReferenceType = typeof(AssetReference);
        private static Type stringType         = typeof(string);
        
        public sealed override bool CanConvert(Type fromType, Type toType)
        {
            if (!assetReferenceType.IsAssignableFrom(toType))
                return false;
            if (!assetReferenceType.IsAssignableFrom(fromType) && fromType != stringType)
                return false;

            return true;
        }

        public sealed override TypeConverterResult TryConvert(object source, Type target)
        {
            var result = new TypeConverterResult()
            {
                Result = source,
                IsComplete = false,
                Target = target,
            };
            var filter = source as string;
            if (source == null || string.IsNullOrEmpty(filter)) return result;

            var sourceType = source.GetType();
            var canConvert = CanConvert(sourceType, target);
            if(!canConvert) return result;

            if (assetReferenceType.IsAssignableFrom(sourceType))
            {
                result.Result = source;
                result.IsComplete = true;
                return result;
            }
            
            var addressableAssetEntry = filter.FindAddressableAssetEntryByAddress();
            var guid = addressableAssetEntry?.guid;
            
            if(addressableAssetEntry == null)
            {
                Object asset = null;
                if (target.IsGenericType && !filter.Contains("t:",StringComparison.OrdinalIgnoreCase))
                {
                    var genericType = target.GetGenericArguments()[0];
                    asset = AssetEditorTools.GetAsset(genericType,filter);
                }
                else
                {
                    asset = AssetEditorTools.GetAsset(filter);
                }
                guid = asset == null ? string.Empty : asset.GetGUID();
            }

            if (string.IsNullOrEmpty(guid)) return result;

            var args = new object[]{guid};
            var reference = Activator.CreateInstance(target, args);

            result.IsComplete = true;
            result.Result = reference;
            
            return result;
        }
        
        
    }
}
