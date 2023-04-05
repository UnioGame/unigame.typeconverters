namespace UniModules.UniGame.TypeConverters.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Editor;
    using NUnit.Framework;
    using UnityEngine;

    public class TypeConverterTests
    {
        private ObjectTypeConverter converter;
        
        [OneTimeSetUp]
        public void TestSetupOnce()
        {
            converter = ScriptableObject.CreateInstance<ObjectTypeConverter>();
            converter.ResetToDefault();
        }
        
        [Test]
        public void SelectJsonConvertTest()
        {
            //init
            var sourceValue = new List<string>(){"3","22d"};
            var checkText      = "[\"3\",\"22d\"]";
            
            //action
            var result       = converter.TryConvert(sourceValue, typeof(string));
            var resultString = result.Result as string;
            Assert.That(result.IsComplete);
            Assert.That(result.Result is string);
            Assert.That(resultString == checkText);
        }
        
        [Test]
        public void StringToListJsonConvertTest()
        {
            //init
            var sourceValue  = new List<string>(){"3","22d"};
            var sourceString = "[\"3\",\"22d\"]";
            
            //action
            var result       = converter.TryConvert(sourceString, typeof(List<string>));
            var resultValue = result.Result as List<string>;
            Assert.That(result.IsComplete);
            Assert.That(result.Result is List<string>);
            Assert.That(resultValue.Count == 2);
            Assert.That(resultValue.SequenceEqual(sourceValue));
        }
        
        [Test]
        public void StringToListIntJsonConvertTest()
        {
            //init
            var sourceValue  = new List<int>(){3,22};
            var sourceString = "[\"3\",\"22\"]";
            
            //action
            var result      = converter.TryConvert(sourceString, typeof(List<int>));
            var resultValue = result.Result as List<int>;
            Assert.That(result.IsComplete);
            Assert.That(result.Result is List<int>);
            Assert.That(resultValue.Count == 2);
            Assert.That(resultValue.SequenceEqual(sourceValue));
        }
        
        [Test]
        public void IntToStringConvertTest()
        {
            //init
            var sourceValue = 32;
            var checkText   = "32";
            
            //action
            var result       = converter.TryConvert(sourceValue, typeof(string));
            var resultString = result.Result as string;
            Assert.That(result.IsComplete);
            Assert.That(result.Result is string);
            Assert.That(resultString == checkText);
        }
        
        [Test]
        public void StringToIntConvertTest()
        {
            //init
            var sourceValue = 32;
            var sourceString = "32";
            
            //action
            var result       = converter.TryConvert(sourceString, typeof(int));
            var resultValue = (int)result.Result;
            Assert.That(result.IsComplete);
            Assert.That(resultValue == sourceValue);
        }
        
        [Test]
        public void StringDotToFloatConvertTest()
        {
            //init
            var sourceValue  = 3.2f;
            var sourceString = "3.2";
            
            //action
            var result      = converter.TryConvert(sourceString, typeof(float));
            var resultValue = (float)result.Result;
            Assert.That(result.IsComplete);
            Assert.That(Mathf.Approximately(sourceValue,resultValue));
        }
        
        [Test]
        public void StringToFloatConvertTest()
        {
            //init
            var sourceValue  = 3.2f;
            var sourceString = "3,2";
            
            //action
            var result      = converter.TryConvert(sourceString, typeof(float));
            var resultValue = (float)result.Result;
            Assert.That(result.IsComplete);
            Assert.That(Mathf.Approximately(sourceValue,resultValue));
        }
        
        [Test]
        public void FloatToFloatConvertTest()
        {
            //init
            var sourceValue  = 3.2f;
            
            //action
            var result      = converter.TryConvert(sourceValue, typeof(float));
            var resultValue = (float)result.Result;
            Assert.That(result.IsComplete);
            Assert.That(Mathf.Approximately(sourceValue,resultValue));
        }
        
    }
}
