using System.Collections.Generic;
using System.Linq;
using ImportantClasses;
using ImportantClasses.Attributes;
using ImportantClasses.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class TestAttributeFinder
    {
        [TestMethod]
        public void TestStaticObjects()
        {
            List<AttributedObject> objs = AttributeFinder.FindStaticAttributes(typeof(ContainSettingsAttribute),
                typeof(TestAttributeFinderTestObjects)).ToList();
            Assert.AreEqual(1, objs.Count);
            AttributedObject obj = objs.First();
            Assert.AreEqual("text1", ((Message)objs.First().Value).MessageText);
            Assert.IsTrue(obj.IsStatic);
            Assert.AreEqual(null, obj.Parent);
            Assert.AreEqual(typeof(TestAttributeFinderTestObjects), obj.ParentType);
        }

        [TestMethod]
        public void TestNonStaticObjects()
        {
            List<AttributedObject> objs = AttributeFinder.FindNonStaticAttributes(typeof(ContainSettingsAttribute),
                typeof(TestAttributeFinderTestObjects)).ToList();
            Assert.AreEqual(2, objs.Count);
            AttributedObject obj1 = objs.First();
            AttributedObject obj2 = objs.Last();
            
            Assert.IsFalse(obj1.IsStatic);
            Assert.AreEqual(null, obj1.Parent);
            Assert.AreEqual(typeof(TestAttributeFinderTestObjects), obj1.ParentType);
            
            Assert.IsFalse(obj2.IsStatic);
            Assert.AreEqual(null, obj2.Parent);
            Assert.AreEqual(typeof(TestAttributeFinderTestObjects), obj2.ParentType);

        }

        [TestMethod]
        public void TestChildren()
        {
            TestAttributeFinderTestObjects testObjects = new TestAttributeFinderTestObjects();
            List<AttributedObject> objs = testObjects.FindAttributedChildren(typeof(ContainSettingsAttribute)).ToList();
            Assert.AreEqual(2, objs.Count);
            AttributedObject obj1 = objs.First();
            AttributedObject obj2 = objs.Last();
            
            Assert.AreEqual("text2", ((Message)obj1.Value).MessageText);
            Assert.IsFalse(obj1.IsStatic);
            Assert.AreEqual(testObjects, obj1.Parent);
            Assert.AreEqual(typeof(TestAttributeFinderTestObjects), obj1.ParentType);

            Assert.AreEqual("text3", ((Message)obj2.Value).MessageText);
            Assert.IsFalse(obj2.IsStatic);
            Assert.AreEqual(testObjects, obj2.Parent);
            Assert.AreEqual(typeof(TestAttributeFinderTestObjects), obj2.ParentType);

        }
    }

    public class TestAttributeFinderTestObjects
    {
        [ContainSettings]
        public static object StaticObject = new Message("text1");

        [ContainSettings]
        public object NonStaticObject = new Message("text2");

        //test properties
        [ContainSettings]
        public object NonStaticObject2 { get; set; } = new Message("text3");

        //private objects should not be found
        private object NonStaticObject3 = new Message("");
    }
}
