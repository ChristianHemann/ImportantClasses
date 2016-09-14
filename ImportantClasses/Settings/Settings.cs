using System;
using System.Collections.Generic;
using System.Linq;
using ImportantClasses.Attributes;
using ImportantClasses.Enums;
using ImportantClasses;

namespace ImportantClasses.Settings
{
    /// <summary>
    /// provides attribute based setting functions. Gets and sets Settings according to the attributes used in the code. 
    /// uses the Attributes [Setting] and [ContainSettings]. 
    /// To mark a property or field as a Setting the field or property must contain the [Setting]-attribute and must be static or an instance of the class must be static and marked with the [ContainSettings]-attribute. 
    /// To save and load settings you need to write your own functions wich get and set the property SettingItems. 
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// set to true if the the Initialize-function has run successfully
        /// </summary>
        public static bool IsInitialized { get; private set; }

        /// <summary>
        /// retruns a list of all SettingItems in the application
        /// </summary>
        public static List<SettingItem> SettingItems
        {
            get
            {
                return _settingItems.Clone();
                //clone the list to make sure the origin one is not changed
            }
            set
            {
                List<SettingItem> newSettings = value.Clone();
                while (newSettings.Any())
                {
                    SettingItem newSetting = value.First();
                    bool newSettingFound = false;
                    foreach (SettingItem settingItem in _settingItems)
                    {
                        if (newSetting.Equals(settingItem))
                        {
                            settingItem.Value = newSetting.Value;
                            newSettingFound = true;
                            break;
                        }
                    }
                    if (!newSettingFound)
                    {
                        Message.SendMessage(null,"The setting called "+newSetting.Name+" at "+newSetting.Path+" could not be found",MessageCode.Warning);
                    }
                    value.Remove(newSetting);
                }
            }
        }

        private static List<SettingItem> _settingItems;

        /// <summary>
        /// This function must be calles before the settings class can be used.
        /// </summary>
        /// <param name="parentTypeForUnitTest">This parameter should only be used to test this function on a specific class, to be able to have repreducable boundary conditions</param>
        public static void Initialize(Type parentTypeForUnitTest = null)
        {
            _settingItems = new List<SettingItem>();
            //find static attributes
            IEnumerable<AttributedObject> objects =
                AttributeFinder.FindStaticAttributes(typeof(SettingAttribute),parentTypeForUnitTest);
            foreach (AttributedObject attributedObject in objects)
            {
                SettingAttribute attr = (SettingAttribute) attributedObject.Attribute;
                _settingItems.Add(new SettingItem(attr.Path, attributedObject));
            }

            //find non-static attributes which parent is static
            IEnumerable<AttributedObject> containSettings =
                AttributeFinder.FindStaticAttributes(typeof(ContainSettingsAttribute), parentTypeForUnitTest);
            foreach (AttributedObject parent in containSettings)
            {
                objects = parent.Value.FindAttributedChildren(typeof(SettingAttribute));
                foreach (AttributedObject attributedObject in objects)
                {
                    SettingAttribute attr = (SettingAttribute)attributedObject.Attribute;
                    _settingItems.Add(new SettingItem(attr.Path, attributedObject));
                }
            }

            IsInitialized = true;
        }

        /// <summary>
        /// search for all menuitems in the given path
        /// </summary>
        /// <param name="path">the path to the menuitem. Use a / (Slash) for subfolders</param>
        /// <returns>The menuitems that were found</returns>
        public static List<string> GetMenuItems(string path)
        {
            int length = path.Split('/').Length;
            List<string> menuItems = new List<string>();

            foreach (SettingItem settingItem in _settingItems)
            {
                if (settingItem.Path.StartsWith(path))
                {
                    string[] itemFolder = settingItem.Path.Split('/');
                    if (itemFolder.Length > length)
                    {
                        string folder = itemFolder.Skip(length).First();
                        if(!menuItems.Contains(folder))
                            menuItems.Add(folder);
                    }
                }
            }

            return menuItems;
        }

        /// <summary>
        /// search for all settings in the given Path
        /// </summary>
        /// <param name="path">the path to the settings</param>
        /// <returns>the list of all found settings</returns>
        public static List<SettingItem> GetSettings(string path)
        {
            List<SettingItem> settings = new List<SettingItem>();
            foreach (SettingItem settingItem in _settingItems)
                if (settingItem.Path.Equals(path))
                    settings.Add(settingItem);
            return settings;
        }

        /// <summary>
        /// search for one setting
        /// </summary>
        /// <param name="path">the path to the setting</param>
        /// <param name="name">the name of the setting</param>
        /// <returns>the first setting item found or null if no setting was found</returns>
        public static SettingItem GetSetting(string path, string name)
        {
            foreach (SettingItem settingItem in _settingItems)
                if(settingItem.Path.Equals(path) && settingItem.Name.Equals(name))
                    return settingItem;
            Message.SendMessage(null,"No Setting found for "+name+" in "+path,MessageCode.Warning);
            return null;
        }

        /// <summary>
        /// Adopt the temporary changes of each SettingItem
        /// </summary>
        public static void AdoptAllTemporaryChanges()
        {
            foreach (SettingItem settingItem in _settingItems)
                settingItem.AdoptTemporaryChange();
        }

        /// <summary>
        /// Discard the temporary changes of each SettingItem
        /// </summary>
        public static void DiscardAllTemporaryChanges()
        {
            foreach (SettingItem settingItem in _settingItems)
                if(settingItem.HasChanges)
                    settingItem.DiscardTemporaryChange();
        }

        /// <summary>
        /// change the values of all settings temporary to its default values. To adopt the new values to the origin variables call AdoptAllTemporaryChanges().
        /// </summary>
        public static void RestoreAllDefaultValues()
        {
            foreach (SettingItem settingItem in _settingItems)
            {
                settingItem.RestoreDefaultValue();
            }
        }
    }
}
