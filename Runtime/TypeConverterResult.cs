namespace UniGame.TypeConverters.Editor.Abstract
{
    using System;

    public class TypeConverterResult
    {
        public object Result;
        public Type Target;
        public string Message;
        public Exception Exception;
        public bool IsComplete;
    }
}