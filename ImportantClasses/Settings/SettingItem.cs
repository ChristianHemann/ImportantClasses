using System.Collections.Generic;
using ImportantClasses.Attributes;

namespace ImportantClasses.Settings
{
    /// <summary>
    /// provides easy access to a Setting
    /// </summary>
    public class SettingItem
    {
        /// <summary>
        /// The Name of the Setting to be shonw in the menu
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
                _attributedObject.Value = value;
            }
        }

        /// <summary>
        /// the path to the setting. folders are seperated by a / (Slash)
        /// </summary>
        public string Path { get;}

        /// <summary>
        /// true of the Setting has temporary changes
        /// </summary>
        public bool HasChanges { get; private set; }

        /// <summary>
        /// the default value which was set in the attribute of the setting
        /// </summary>
        public object DefaultValue => ((SettingAttribute) _attributedObject.Attribute).DefaultValue;

        /// <summary>
        /// used for numeric values to specifiy the decimalplaces to show in the menu. If the value is negative it is interpreted as predecimal places
        /// </summary>
        public int DecimalPlaces => ((SettingAttribute) _attributedObject.Attribute).DecimalPlaces;

        /// <summary>
        /// used for numeric values to specify the maximum allowed value. This value has to be utilized by the GUI
        /// </summary>
        public double MaxValue => ((SettingAttribute) _attributedObject.Attribute).MaxValue;

        /// <summary>
        /// used for numeric values to specify the minimum allowed value. This value has to be utilized by the GUI
        /// </summary>
        public double MinValue => ((SettingAttribute) _attributedObject.Attribute).MinValue;

        private readonly AttributedObject<object> _attributedObject;
        private object _changedValue;

        /// <summary>
        /// provides easy access to a Setting
        /// </summary>
        /// <param name="path">the path to the setting. folders are seperated by a / (Slash)</param>
        /// <param name="attributedObject">the object with the SettingAttribute</param>
        internal SettingItem(string path, AttributedObject<object> attributedObject)
        {
            _attributedObject = attributedObject;
            Path = path;
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
            IEnumerable<AttributedObject<object>> attributedObjects)
        {
            List<SettingItem> settingList = new List<SettingItem>();
            foreach (AttributedObject<object> attributedObject in attributedObjects)
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
            SettingItem itm = (SettingItem) obj;
            if (!Name.Equals(itm.Name) || !Path.Equals(itm.Path))
                return false;
            return true;
        }
    }
}
