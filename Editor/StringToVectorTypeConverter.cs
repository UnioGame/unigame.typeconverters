namespace Game.Modules.UnioModules.UniGame.CoreModules.UniGame.TypeConverters.Editor
{
    using System;
    using UniModules.UniCore.Runtime.Utils;
    using UniModules.UniGame.TypeConverters.Editor;
    using UniModules.UniGame.TypeConverters.Editor.Abstract;
    using UnityEngine;

    [Serializable]
    public class StringToVectorTypeConverter : BaseTypeConverter
    {
        private static Type stringType = typeof(string);
        private static Type vector3Type = typeof(Vector3);
        private static Type vector4Type = typeof(Vector4);
        private static Type vector2Type = typeof(Vector2);
        private static Type vector2IntType = typeof(Vector2Int);
        private static Type vector3IntType = typeof(Vector3Int);
        private static string zero = "0";
        
        public string separator = ";";

        public override bool CanConvert(Type fromType, Type toType)
        {
            if (fromType != stringType) return false;
            
            return toType == vector3Type || 
                   toType == vector2Type || 
                   toType == vector4Type || 
                   toType == vector3IntType || 
                   toType == vector2IntType;
        }

        public override TypeConverterResult TryConvert(object source, Type target)
        {

            var result = new TypeConverterResult()
            {
                Result = source,
                IsComplete = false,
                Target = target,
            };

            var sourceType = source.GetType();
            var canConvert = CanConvert(sourceType, target);
            if (!canConvert) return result;

            var sourceString = source.ToString();
            if (string.IsNullOrEmpty(sourceString))
                return result;

            var split = sourceString.Trim().Split(separator);
            object resultValue = null;
            
            if (target == vector3Type)
            {
                var xString = split.Length > 0 ? split[0] : zero;
                var yString = split.Length > 1 ? split[1] : zero;
                var zString = split.Length > 2 ? split[2] : zero;
                
                float.TryParse(xString, out var x);
                float.TryParse(yString, out var y);
                float.TryParse(zString, out var z);
                
                resultValue = new Vector3(x, y, z);
            }

            if (target == vector2Type)
            {
                var xString = split.Length > 0 ? split[0] : zero;
                var yString = split.Length > 1 ? split[1] : zero;
                
                float.TryParse(xString, out var x);
                float.TryParse(yString, out var y);
                
                resultValue = new Vector2(x, y);
            }

            if (target == vector4Type)
            {
                var xString = split.Length > 0 ? split[0] : zero;
                var yString = split.Length > 1 ? split[1] : zero;
                var zString = split.Length > 2 ? split[2] : zero;
                var wString = split.Length > 3 ? split[3] : zero;
                
                float.TryParse(xString, out var x);
                float.TryParse(yString, out var y);
                float.TryParse(zString, out var z);
                float.TryParse(wString, out var w);
                
                resultValue = new Vector4(x, y, z,w);
            }

            if (target == vector2IntType)
            {
                var xString = split.Length > 0 ? split[0] : zero;
                var yString = split.Length > 1 ? split[1] : zero;
                
                float.TryParse(xString, out var x);
                float.TryParse(yString, out var y);
                
                resultValue = new Vector2Int(Mathf.RoundToInt(x),Mathf.RoundToInt(y));
            }

            if (target == vector3IntType)
            {
                var xString = split.Length > 0 ? split[0] : zero;
                var yString = split.Length > 1 ? split[1] : zero;
                var zString = split.Length > 2 ? split[2] : zero;
                
                float.TryParse(xString, out var x);
                float.TryParse(yString, out var y);
                float.TryParse(zString, out var z);
                
                resultValue = new Vector3Int(Mathf.RoundToInt(x),Mathf.RoundToInt(y),Mathf.RoundToInt(z));
            }

            result.IsComplete = true;
            result.Result = resultValue;
            
            return result;
        }


    }
}