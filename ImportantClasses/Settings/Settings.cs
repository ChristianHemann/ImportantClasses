using System.Collections.Generic;
using System.Linq;
using ImportantClasses.Attributes;

namespace ImportantClasses.Settings
{
    /// <summary>
    /// provides attribute based setting functions. Gets, sets and saves Settings according to the attributes used in the code.\n
    /// uses the Attributes Setting, SettingMenuItem and SettingEntryPoint.
    /// </summary>
    public static class Settings
    {
        public static bool IsInitialized { get; private set; }

        /// <summary>
        /// retruns a list of all SettingItems in the application
        /// </summary>
        public static List<SettingItem> SettingItems
        {
            get
            {
                //clone the list to make sure the origin one is not changed
                List<SettingItem> itms = new List<SettingItem>();
                itms.AddRange(_settingItems);
                return itms;
            }
        }

        private static List<SettingItem> _settingItems;

        /// <summary>
        /// This function must be calles before the settings class can be used.
        /// </summary>
        public static void Initialize()
        {
            _settingItems = new List<SettingItem>();
            //find static attributes
            IEnumerable<AttributedObject<object>> objects =
                AttributeFinder.FindStaticAttributes(typeof(SettingAttribute));
            foreach (AttributedObject<object> attributedObject in objects)
            {
                SettingAttribute attr = (SettingAttribute) attributedObject.Attribute;
                _settingItems.Add(new SettingItem(attr.Path + "/" + attr.Name, attributedObject));
            }

            //find non-static attributes which parent is static
            IEnumerable<AttributedObject<object>> containSettings =
                AttributeFinder.FindStaticAttributes(typeof(ContainSettingsAttribute));
            foreach (AttributedObject<object> parent in containSettings)
            {
                objects = parent.FindAttributedChildren(typeof(SettingAttribute));
                foreach (AttributedObject<object> attributedObject in objects)
                {
                    SettingAttribute attr = (SettingAttribute)attributedObject.Attribute;
                    _settingItems.Add(new SettingItem(attr.Path + "/" + attr.Name, attributedObject));
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
                    if(itemFolder.Length>length)
                        menuItems.Add(itemFolder.Skip(length).First());
                }
            }

            return menuItems;
        }

        /// <summary>
        /// searches for all settings in the given Path
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
