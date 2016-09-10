using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ImportantClasses.Attributes
{
    /// <summary>
    /// searches in the code for attributes
    /// </summary>
    public static class AttributeFinder
    {
        /// <summary>
        /// searches for all static Attributes of the given Type
        /// </summary>
        /// <param name="attributeType">the attribute type to search for</param>
        /// <param name="parentType">the type to search in for attributed objects. 
        /// \nIf null all assemblies are searched through.
        /// \nIf not null the search function will only search in the given Type, but not in its children</param>
        /// <param name="includeInheritedAttributes">shall the types which inherits from the attributeType be included?</param>
        /// <returns>All static objects found which has the specified attribute</returns>
        public static IEnumerable<AttributedObject<object>> FindStaticAttributes(Type attributeType, Type parentType = null, bool includeInheritedAttributes = false)
        {
            List<AttributedObject<object>> objList = new List<AttributedObject<object>>();
            if (parentType == null)
            {
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (Assembly assembly in assemblies)
                {
                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        objList.AddRange(FindStaticAttributes(attributeType, type));
                    }
                }
            }
            else
            {
                IEnumerable<FieldInfo> fieldInfos =
                    parentType.GetFields(BindingFlags.Public | BindingFlags.Static).Where(field => field.IsDefined(attributeType, includeInheritedAttributes));
                foreach (FieldInfo fieldInfo in fieldInfos)
                {
                    IEnumerable<Attribute> attrs =
                        (IEnumerable<Attribute>) fieldInfo.GetCustomAttributes(attributeType, includeInheritedAttributes);
                    foreach (Attribute attribute in attrs) //If an object has more than one attribute of the given type it is added multiple
                    {
                        objList.Add(new AttributedObject<object>(parentType, fieldInfo.Name, attribute));
                    }
                }
                IEnumerable<PropertyInfo> propertyInfos =
                    parentType.GetProperties(BindingFlags.Public | BindingFlags.Static).Where(prop => prop.IsDefined(attributeType, includeInheritedAttributes));
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    IEnumerable<Attribute> attrs =
                        (IEnumerable<Attribute>) propertyInfo.GetCustomAttributes(attributeType, includeInheritedAttributes);
                    foreach (Attribute attribute in attrs) //If an object has more than one attribute of the given type it is added multiple
                    {
                        objList.Add(new AttributedObject<object>(parentType, propertyInfo.Name, attribute));
                    }
                }
            }
            return objList;
        }

        /// <summary>
        /// searches for all non-static attributes on the given type
        /// </summary>
        /// <param name="attributeType">the attribute type to search for</param>
        /// <param name="parentType">the type to search in for attributed objects. 
        /// \nIf null all assemblies are searched through.
        /// \nIf not null the search function will only search in the given Type, but not in its children</param>
        /// <param name="includeInheritedAttributes">shall the types which inherits from the attributeType be included?</param>
        /// <returns>All non-static objects found which has the specified attribute</returns>
        public static IEnumerable<AttributedObject<object>> FindNonStaticAttributes(Type attributeType, Type parentType = null, bool includeInheritedAttributes = false)
        {
            List<AttributedObject<object>> objList = new List<AttributedObject<object>>();
            if (parentType == null)
            {
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (Assembly assembly in assemblies)
                {
                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        objList.AddRange(FindStaticAttributes(attributeType, type));
                    }
                }
            }
            else
            {
                IEnumerable<FieldInfo> fieldInfos =
                    parentType.GetFields(BindingFlags.Public).Where(field => field.IsDefined(attributeType, includeInheritedAttributes));
                foreach (FieldInfo fieldInfo in fieldInfos)
                {
                    IEnumerable<Attribute> attrs =
                        (IEnumerable<Attribute>)fieldInfo.GetCustomAttributes(attributeType, includeInheritedAttributes);
                    foreach (Attribute attribute in attrs) //If an object has more than one attribute of the given type it is added multiple
                    {
                        objList.Add(new AttributedObject<object>(parentType, fieldInfo.Name, attribute));
                    }
                }
                IEnumerable<PropertyInfo> propertyInfos =
                    parentType.GetProperties(BindingFlags.Public).Where(prop => prop.IsDefined(attributeType, includeInheritedAttributes));
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    IEnumerable<Attribute> attrs =
                        (IEnumerable<Attribute>)propertyInfo.GetCustomAttributes(attributeType, includeInheritedAttributes);
                    foreach (Attribute attribute in attrs) //If an object has more than one attribute of the given type it is added multiple
                    {
                        objList.Add(new AttributedObject<object>(parentType, propertyInfo.Name, attribute));
                    }
                }
            }
            return objList;
        }

        /// <summary>
        /// searches for non-static children which has the given attribute
        /// </summary>
        /// <param name="attributeType">the attribute type to search for</param>
        /// <param name="parent">the parent object </param>
        /// <param name="includeInheritedAttributes">shall the types which inherits from the attributeType be included?</param>
        /// <returns>All child objects of the parent object which has the specified attribute</returns>
        public static IEnumerable<AttributedObject<object>> FindAttributedChildren(this object parent, Type attributeType, bool includeInheritedAttributes = false)
        {
            if(parent == null)
                return new AttributedObject<object>[0];

            List<AttributedObject<object>> objList = new List<AttributedObject<object>>();
            IEnumerable<FieldInfo> fieldInfos =
                parent.GetType().GetFields().Where(field => field.IsDefined(attributeType, includeInheritedAttributes));
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                if(fieldInfo.IsStatic)
                    continue;
                IEnumerable<Attribute> attrs =
                    (IEnumerable<Attribute>) fieldInfo.GetCustomAttributes(attributeType, includeInheritedAttributes);
                foreach (Attribute attribute in attrs)
                {
                    objList.Add(new AttributedObject<object>(parent, fieldInfo.Name, attribute));
                }
            }
            IEnumerable<PropertyInfo> propertyInfos =
                parent.GetType().GetProperties().Where(prop => prop.IsDefined(attributeType, includeInheritedAttributes));
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if(propertyInfo.GetGetMethod().IsStatic)
                    continue;
                IEnumerable<Attribute> attrs =
                    (IEnumerable<Attribute>) propertyInfo.GetCustomAttributes(attributeType, includeInheritedAttributes);
                foreach (Attribute attribute in attrs)
                {
                    objList.Add(new AttributedObject<object>(parent, propertyInfo.Name, attribute));
                }
            }
            return objList;
        }
    }
}
