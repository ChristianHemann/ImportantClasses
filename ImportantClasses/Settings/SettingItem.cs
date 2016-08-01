using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImportantClasses.Attributes;

namespace ImportantClasses.Settings
{
    public class SettingItem
    {
        public string Name
        {
            get { return ((SettingAttribute) _attributedObject.Attribute).Name; }
        }

        public object Value
        {
            get
            {
                if (_changedValue != null)
                    return _changedValue;
                else
                    return
                        _attributedObject.Value;
            }
            set
            {
                _changedValue = value;
                Settings.ChangeSettingTemporary(PathName, value);
            }
        }

        internal List<string> PathName { get;}

        private readonly AttributedObject<object> _attributedObject;
        private object _changedValue = null;

        internal SettingItem(IEnumerable<string> path, AttributedObject<object> attributedObject)
        {
            _attributedObject = attributedObject;
            PathName = path.ToList();
            PathName.Add(((SettingAttribute) attributedObject.Attribute).Name);
        }

        internal static IEnumerable<SettingItem> FromAttributedObjects(IEnumerable<string> path,
            IEnumerable<AttributedObject<object>> attributedObjects)
        {
            List<SettingItem> settingList = new List<SettingItem>();
            foreach (AttributedObject<object> attributedObject in attributedObjects)
            {
                settingList.Add(new SettingItem(path, attributedObject));
            }
            return settingList;
        }
    }
}
