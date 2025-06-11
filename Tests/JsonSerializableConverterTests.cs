namespace UniModules.UniGame.TypeConverters.Tests
{
    using System.Collections.Generic;
    using global::UniGame.TypeConverters.Editor;
    using NUnit.Framework;
    using UnityEngine;

    public class JsonSerializableConverterTests
    {
        
        // A Test behaves as an ordinary method
        [Test]
        public void ListAndArraySerializeCheckTest()
        {
            
            //info
            var serializedValue = "[\"1\",\"4\"]";
            var converter       = new JsonSerializableClassConverter();

            //action

            var listResult =converter.TryConvert(serializedValue, typeof(List<int>));
            var listStringResult = converter.TryConvert(serializedValue, typeof(List<string>));
            var arrayInt = converter.TryConvert(serializedValue, typeof(int[]));
            var arrayString = converter.TryConvert(serializedValue, typeof(string[]));
            
            Debug.Log(listResult.Result);
            Debug.Log(listStringResult.Result);
            Debug.Log(arrayInt.Result);
            Debug.Log(arrayString.Result);
            
            //check
            Assert.That(listResult.IsComplete);
            Assert.That(listStringResult.IsComplete);
            Assert.That(arrayInt.IsComplete);
            Assert.That(arrayString.IsComplete);

        }

        // A Test behaves as an ordinary method
        [Test]
        public void ConvertToListAndArraySerializeCheckTest()
        {
            //info
            var converter   = new JsonSerializableClassConverter();
            var targetList  = new List<string>(){"1","4"};
            var targetArray = targetList.ToArray();
            
            //action

            var listResult  = converter.TryConvert(targetList, typeof(string));
            var arrayResult = converter.TryConvert(targetArray, typeof(string));

            //check
            Assert.That(listResult.IsComplete);
            Assert.That(arrayResult.IsComplete);

        }
        
    }
}
