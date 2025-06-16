namespace UniGame.TypeConverters
{
    using System;
    using Runtime.Utils;
    using Editor;
    using Editor.Abstract;
    using UnityEngine;

    [Serializable]
    public class VectorTypeConverter : BaseTypeConverter
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
            return CanConvertFromVector(fromType, toType) ||
                   CanConvertToVector(fromType, toType);
        }
        
        public bool CanConvertToVector(Type fromType, Type toType)
        {
            if (fromType != stringType) return false;

            return IsVectorType(toType);
        }
        
        public bool CanConvertFromVector(Type fromType, Type toType)
        {
            if (toType != stringType) return false;

            return IsVectorType(fromType);
        }

        public bool IsVectorType(Type type)
        {
            return type == vector3Type || 
                   type == vector2Type || 
                   type == vector4Type || 
                   type == vector3IntType || 
                   type == vector2IntType;
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

            var isToVector = sourceType == stringType;
            object resultValue = null;
            
            if (isToVector)
            {
                var sourceString = source.ToString();
                if (string.IsNullOrEmpty(sourceString))
                    return result;

                resultValue = ConvertToVector(sourceString, target);
            }
            else
            {
                resultValue = ConvertToVector(source);
            }
            
            result.IsComplete = true;
            result.Result = resultValue;
            
            return result;
        }

        public string ConvertToVector(object vector)
        {
            var target = vector.GetType();
            var resultValue = string.Empty;
            
            if (target == vector3Type)
            {
                var vector3 = (Vector3)vector;
                resultValue = $"{vector3.x}{separator}{vector3.y}{separator}{vector3.z}";
            }
            if (target == vector2Type)
            {
                var vector2 = (Vector2)vector;
                resultValue = $"{vector2.x}{separator}{vector2.y}";
            }

            if (target == vector4Type)
            {
                var vector4 = (Vector4)vector;
                resultValue = $"{vector4.x}{separator}{vector4.y}{separator}{vector4.z}{separator}{vector4.w}";
            }

            if (target == vector2IntType)
            {
                var vector2 = (Vector2Int)vector;
                resultValue = $"{vector2.x}{separator}{vector2.y}";
            }

            if (target == vector3IntType)
            {
                var vector3 = (Vector3Int)vector;
                resultValue = $"{vector3.x}{separator}{vector3.y}{separator}{vector3.z}";
            }

            return resultValue;
        }
        
        public object ConvertToVector(string sourceString, Type target)
        {
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

            return resultValue;
        }


    }
}