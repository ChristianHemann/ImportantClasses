using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImportantClasses;
using ImportantClasses.Attributes;
using ImportantClasses.Settings;

namespace UnitTests
{
    [TestClass]
    public class TestAttributedObject
    {
        private AttributedObject obj;
        [TestInitialize]
        public void Init()
        {
            Attribute attr =
                typeof(TestAttributedObjectTestObjects).GetField("TestObject", BindingFlags.Public|BindingFlags.Static)
                    .GetCustomAttribute(typeof(ContainSettingsAttribute));
            obj = new AttributedObject(typeof(TestAttributedObjectTestObjects), "TestObject", attr);

        }

        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(typeof(Message), obj.Value.GetType());
            Assert.AreEqual(typeof(TestAttributedObjectTestObjects), obj.ParentType);
            Assert.IsTrue(obj.IsStatic);
            Assert.AreEqual(null, obj.Parent);
            Assert.AreEqual("just to test something",((Message)obj.Value).MessageText);
            obj.Value = new Message("some other text");
            Assert.AreEqual("some other text", ((Message)obj.Value).MessageText);

        }
    }

    public class TestAttributedObjectTestObjects
    {
        [ContainSettings]
        public static object TestObject = new Message("just to test something");
    }
}
