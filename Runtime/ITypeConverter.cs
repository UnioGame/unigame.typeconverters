namespace UniGame.TypeConverters.Editor.Abstract
{
    using System;

    public interface ITypeConverter
    {
        bool CanConvert(Type fromType, Type toType);
        TypeConverterResult TryConvert(object source, Type target);
    }
}