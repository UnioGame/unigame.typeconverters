namespace UniModules.UniGame.TypeConverters.Editor
{
    using System;

    public static class ObjectTypeConverterExtension
    {
        public static object TryConvert(this object source, Type target)
        {
            return ObjectTypeConverter.TypeConverters.TryConvert(source, target).Result;
        }
        
        public static bool TryConvert<TType>(this object source, out TType result)
        {
            var convertResult = ObjectTypeConverter.TypeConverters
                .TryConvert(source, typeof(TType));
            
            result = default;
            if(convertResult.IsComplete) result = (TType) convertResult.Result;
            return convertResult.IsComplete;
        }
        
        public static T TryConvert<T>(this object source)
            where T : class
        {
            return TryConvert(source, typeof(T)) as T;
        }
    }
}