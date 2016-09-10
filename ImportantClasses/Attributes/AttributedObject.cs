using System;
using System.Reflection;
using ImportantClasses.Enums;
using ImportantClasses.Exceptions;

namespace ImportantClasses.Attributes
{
    /// <summary>
    /// provides a simple access to an object with its attribute
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AttributedObject<T>
    {
        private enum InfoType
        {
            FieldInfo,
            PropertyInfo
        }
        /// <summary>
        /// the attribute of the object
        /// </summary>
        public Attribute Attribute { get; }

        /// <summary>
        /// the value of the object
        /// </summary>
        public T Value
        {
            get
            {
                if (_infoType == InfoType.FieldInfo)
                {
                    return (T)_parent.GetType().GetField(ObjectName).GetValue(_parent);
                }
                else if (_infoType == InfoType.PropertyInfo)
                {
                    return (T)_parent.GetType().GetProperty(ObjectName).GetValue(_parent, null);
                }
                return default(T);
            }
            set
            {
                if (_infoType == InfoType.FieldInfo)
                {
                    _parent.GetType().GetField(ObjectName).SetValue(_parent, value);
                }
                else if (_infoType == InfoType.PropertyInfo)
                {
                    _parent.GetType().GetProperty(ObjectName).SetValue(_parent, value, null);
                }
            }
        }

        /// <summary>
        /// if the object is static or not
        /// </summary>
        public bool IsStatic { get; }

        /// <summary>
        /// the parent object of the object. null if the object is static
        /// </summary>
        public object Parent
        {
            get { return _parent; }
            set
            {
                if (IsStatic)
                {
                    Message.SendMessage(this, 
                        "The Parent of the AttributedObject cannot be set for a static object at " +
                        Environment.StackTrace, MessageCode.Error);
                    return;
                }
                if (value.GetType() != ParentType)
                {
                    Message.SendMessage(this, "The Type of the set object does not match the Type of the origin object",MessageCode.FatalError);
                    throw new TypeException("The Type of the set object does not match the Type of the origin object");
                }

                _parent = value;
            }
        }

        /// <summary>
        /// the parent Type of the object
        /// </summary>
        public Type ParentType { get; }

        /// <summary>
        /// the name of the object
        /// </summary>
        private string ObjectName { get; }

        private readonly InfoType _infoType;
        private object _parent;

        /// <summary>
        /// provides a simple access to an object with its attribute
        /// </summary>
        /// <param name="parent">the parent object of the object. null if the object is static</param>
        /// <param name="objectName">the name of the object</param>
        /// <param name="attribute">the attribute of the object</param>
        public AttributedObject(object parent, string objectName, Attribute attribute)
            : this(parent.GetType(), objectName, attribute)
        {
            _parent = parent;
            if (IsStatic)
                _parent = null;
        }

        /// <summary>
        /// provides a simple access to an object with its attribute
        /// </summary>
        /// <param name="parentType">the parent type of the object. If the object is not static please use the AttributedObject(object, string, Attribute) constructor.</param>
        /// <param name="objectName">the name of the object</param>
        /// <param name="attribute">the attribute of the object</param>
        public AttributedObject(Type parentType, string objectName, Attribute attribute)
        {
            Attribute = attribute;
            ParentType = parentType;
            ObjectName = objectName;
            _parent = null;

            FieldInfo fieldInfo = parentType.GetField(ObjectName);
            if (fieldInfo != null)
            {
                _infoType = InfoType.FieldInfo;
                IsStatic = fieldInfo.IsStatic;
            }
            else
            {
                PropertyInfo propertyInfo = parentType.GetProperty(ObjectName);
                if (propertyInfo != null)
                {
                    _infoType = InfoType.PropertyInfo;
                    IsStatic = propertyInfo.GetGetMethod().IsStatic;
                }
            }
        }
    }
}
