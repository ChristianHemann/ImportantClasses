using System;
using System.Collections.Generic;
using ImportantClasses.Attributes;
using ImportantClasses.Enums;

namespace ImportantClasses.Settings
{
    /// <summary>
    /// provides easy access to a Setting
    /// </summary>
    public class SettingItem : ICloneable
    {
        /// <summary>
        /// The Name of the Setting to be shown in the menu
        /// </summary>
        public string Name => ((SettingAttribute)_attributedObject.Attribute).Name;

        /// <summary>
        /// the value of the setting. To Change the Value temporary use ChangeValueTemporary(...)
        /// </summary>
        public object Value
        {
            get
            {
                if (HasChanges)
                    return _changedValue;
                else
                    return
                        _attributedObject.Value;
            }
            set
            {
                //if(Value.GetType()!=value.GetType())
                //    throw new TypeException("The Type of the old value ("+Value.GetType()+") does not match the type of the new value ("+value.GetType()+").");
                //If the value is numeric and a range was set: check if the new value is out of range
                if (IsNumeric && MinValue < MaxValue)
                {
                    if (value == null)
                    {
                        _attributedObject.Value = null;
                        return;
                    }
                    bool valueTooBig = false, valueTooSmall = false;

                    IComparable newVal = (IComparable)Convert.ChangeType(value, _attributedObject.Value.GetType());
                    IComparable maxVal =
                        (IComparable)Convert.ChangeType(MaxValue, _attributedObject.Value.GetType());
                    IComparable minVal =
                        (IComparable)Convert.ChangeType(MinValue, _attributedObject.Value.GetType());
                    if(newVal == null)
                        throw new NullReferenceException("the new value was converted into null."); //I don't know how this should happen, but it seems that it could be pissible

                    if (newVal.CompareTo(maxVal) > 0) //if the new value is bigger than the maximum value
                    {
                        value = Convert.ChangeType(MaxValue, _attributedObject.Value.GetType());
                        valueTooBig = true;
                    }
                    else if (newVal.CompareTo(minVal) < 0) //if the new value is smaller than the minimum value
                    {
                        value = Convert.ChangeType(MinValue, _attributedObject.Value.GetType());
                        valueTooSmall = true;
                    }

                    if (valueTooSmall)
                    {
                        Message.SendMessage(this, "The Value of the SettingItem named " + Name + " at " + Path + " was too small. It was set to its minumum value of "+MaxValue, MessageCode.Warning);
                    }
                    else if (valueTooBig)
                    {
                        Message.SendMessage(this, "The Value of the SettingItem named " + Name + " at " + Path + " was too big. It was set to its maximum value of "+MinValue, MessageCode.Warning);
                    }
                }

                _attributedObject.Value = value;
            }
        }

        /// <summary>
        /// the path to the setting. folders are seperated by a / (Slash)
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// true of the Setting has temporary changes
        /// </summary>
        public bool HasChanges { get; private set; }

        /// <summary>
        /// the default value which was set in the attribute of the setting
        /// </summary>
        public object DefaultValue => ((SettingAttribute)_attributedObject.Attribute).DefaultValue;

        /// <summary>
        /// used for numeric values to specifiy the decimalplaces to show in the menu. If the value is negative it is interpreted as predecimal places
        /// </summary>
        public int DecimalPlaces
        {
            get
            {
                if (IsPointNumber || ((SettingAttribute)_attributedObject.Attribute).DecimalPlaces < 0)
                    return ((SettingAttribute)_attributedObject.Attribute).DecimalPlaces;
                return 0;
            }
        }

        /// <summary>
        /// used for numeric values to specify the maximum allowed value. 
        /// If the minimum value is bigger or equal to the maximum value the values will not be used. 
        /// </summary>
        public double MaxValue => ((SettingAttribute)_attributedObject.Attribute).MaxValue;

        /// <summary>
        /// used for numeric values to specify the minimum allowed value. 
        /// If the minimum value is bigger or equal to the maximum value the values will not be used. 
        /// </summary>
        public double MinValue => ((SettingAttribute)_attributedObject.Attribute).MinValue;

        /// <summary>
        /// true if the Value is a numeric value
        /// </summary>
        public bool IsNumeric { get; }

        /// <summary>
        /// true if the value is decimal, float or double
        /// </summary>
        public bool IsPointNumber { get; }

        private readonly AttributedObject _attributedObject;
        private object _changedValue;

        /// <summary>
        /// provides easy access to a Setting
        /// </summary>
        /// <param name="path">the path to the setting. folders are seperated by a / (Slash)</param>
        /// <param name="attributedObject">the object with the SettingAttribute</param>
        internal SettingItem(string path, AttributedObject attributedObject)
        {
            _attributedObject = attributedObject;
            Path = path;
            Type type = attributedObject.Value.GetType();
            if ((type == typeof(short)) ||
                (type == typeof(ushort)) ||
                (type == typeof(byte)) ||
                (type == typeof(sbyte)) ||
                (type == typeof(int)) ||
                (type == typeof(uint)) ||
                (type == typeof(long)) ||
                (type == typeof(ulong)))
            {
                IsNumeric = true;
            }
            else if ((type == typeof(double)) ||
                      (type == typeof(float)) ||
                      (type == typeof(decimal)))
            {
                IsNumeric = true;
                IsPointNumber = true;
            }
        }

        /// <summary>
        /// Change the Value in a temporary Buffer. To adopt the new Value to the origin variable call AdoptTemporaryChange().
        /// </summary>
        /// <param name="value">the new value to set</param>
        public void ChangeValueTemporary(object value)
        {
            _changedValue = value;
            HasChanges = true;
        }

        /// <summary>
        /// if the value has changed temporary it will be written to the origin variable
        /// </summary>
        public void AdoptTemporaryChange()
        {
            if (HasChanges)
            {
                Value = _changedValue;
                HasChanges = false;
            }
        }

        /// <summary>
        /// if the value was changed temporary it will be reset to the origin value
        /// </summary>
        public void DiscardTemporaryChange()
        {
            _changedValue = null;
            HasChanges = false;
        }

        /// <summary>
        /// Changes the value temporary to its default value. To adopt the new Value to the origin variable call AdoptTemporaryChange().
        /// </summary>
        public void RestoreDefaultValue()
        {
            ChangeValueTemporary(((SettingAttribute)_attributedObject.Attribute).DefaultValue);
        }

        /// <summary>
        /// creates new SettingItems from a list of AttributedObjects
        /// </summary>
        /// <param name="path">the path to all the objects</param>
        /// <param name="attributedObjects">the list of objects with the SettingAttribute</param>
        /// <returns></returns>
        internal static IEnumerable<SettingItem> FromAttributedObjects(string path,
            IEnumerable<AttributedObject> attributedObjects)
        {
            List<SettingItem> settingList = new List<SettingItem>();
            foreach (AttributedObject attributedObject in attributedObjects)
            {
                settingList.Add(new SettingItem(path, attributedObject));
            }
            return settingList;
        }

        /// <summary>
        /// checks if two SettingItems have the same path and value
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is SettingItem))
                return false;
            SettingItem itm = (SettingItem)obj;
            if (!Name.Equals(itm.Name) || !Path.Equals(itm.Path))
                return false;
            return true;
        }

        public object Clone()
        {
            SettingItem clone = new SettingItem(Path, _attributedObject);
            clone.HasChanges = HasChanges;
            clone._changedValue = _changedValue;
            return clone;
        }
    }
}
