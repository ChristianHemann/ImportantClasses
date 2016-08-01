using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ImportantClasses.Attributes;
using ImportantClasses.Exceptions;
using System.Configuration;

namespace ImportantClasses.Settings
{
    /// <summary>
    /// provides attribute based setting functions. Gets, sets and saves Settings according to the attributes used in the code.\n
    /// uses the Attributes Setting, SettingMenuItem and SettingEntryPoint.
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// contains a list of errors fround when calling Validate()
        /// </summary>
        public static string AttributeErrors => _attributeErros;

        /// <summary>
        /// the number of Settings which are just temporary changed
        /// </summary>
        public static int NumberOfTemporaryChanges => _temporaryChanges.Count;

        /// <summary>
        /// ture if there are temporary changes
        /// </summary>
        public static bool HasTemporaryChanges => _temporaryChanges.Any();

        private static bool _isValidated;
        private static string _attributeErros = "";

        private static readonly Dictionary<IEnumerable<string>, object> _temporaryChanges =
            new Dictionary<IEnumerable<string>, object>();
        

        /// <summary>
        /// Should be invoked before the Settings class is used. Checks if the Attributes are used correctly.\n
        /// Throws an exception if the a SettingsEntryPointAttribute is applied to a non-static object.
        /// </summary>
        /// <returns>true if all Attributes are set correctly</returns>
        public static bool Validate()
        {
            bool errorsFound = false;

            string entryPointIsNotStatic = ""; //set if an SettingEntryPointAttribute is set to a non-static object
            IEnumerable<AttributedObject<object>> attributedObjects =
                AttributeFinder.FindNonStaticAttributes(typeof(SettingEntryPointAttribute));
            foreach (AttributedObject<object> attributedObject in attributedObjects)
            {
                errorsFound = true;
                entryPointIsNotStatic += ((SettingEntryPointAttribute) attributedObject.Attribute).Name + " at " +
                                         attributedObject.ParentType.FullName;
                _attributeErros = "SettingEntryPoint " + ((SettingEntryPointAttribute) attributedObject.Attribute).Name +
                                  " at " + attributedObject.ParentType.FullName + " is not static.\n";
            }
            if (!String.IsNullOrEmpty(entryPointIsNotStatic))
                throw new StaticModifierException(
                    "there is at least one SettingEntryPointAttribte set to a non-static object: ");

            _isValidated = true;
            return !errorsFound;
        }

        /// <summary>
        /// searches for objects marked with the Attribute SettingMenuItem.
        /// </summary>
        /// <param name="path">the path of SettingMenuItems to go to the searched SettingMenuItems</param>
        /// <returns>The SettingMenuItems that were found</returns>
        public static IEnumerable<string> GetSettingMenuItems(IEnumerable<string> path)
        {
            if (!_isValidated)
                Validate();
            List<AttributedObject<object>> attributedObjects =
                AttributeFinder.FindStaticAttributes(typeof(SettingMenuItemAttribute), null, true).ToList();

            string searchedPath = "EntryPoint";
            foreach (string name in path)
            {
                if (!attributedObjects.Any())
                    return new string[0];
                bool nameFound = false;
                foreach (AttributedObject<object> attributedObject in attributedObjects)
                {
                    if (((SettingMenuItemAttribute) attributedObject.Attribute).Name == name)
                    {
                        attributedObjects = attributedObject.FindAttributes(typeof(SettingMenuItemAttribute)).ToList();
                        nameFound = true;
                        break;
                    }
                }
                if (!nameFound)
                    throw new Exceptions.KeyNotFoundException("The name " + name +
                                                                               " was not found in the settings at " +
                                                                               searchedPath);
                searchedPath += " + " + name;
            }

            List<string> menuItems = new List<string>();
            foreach (AttributedObject<object> attributedObject in attributedObjects)
            {
                menuItems.Add(((SettingMenuItemAttribute) attributedObject.Attribute).Name);
            }
            return menuItems;
        }

        /// <summary>
        /// searches for objects marked with the Attribute Setting
        /// </summary>
        /// <param name="path">the path of SettingMenuItems to go to the searched Setting</param>
        /// <returns>Returns all settings which were found under the given path.</returns>
        public static IEnumerable<SettingItem> GetSettings(string[] path)
        {
            if (!_isValidated)
                Validate();
            if (!path.Any())
                return SettingItem.FromAttributedObjects(path,
                    AttributeFinder.FindStaticAttributes(typeof(SettingAttribute)));

            List<AttributedObject<object>> attributedObjects =
                AttributeFinder.FindStaticAttributes(typeof(SettingMenuItemAttribute), null, true).ToList();
            IEnumerable<AttributedObject<object>> settingsBuffer = new AttributedObject<object>[0];

            string searchedPath = "EntryPoint";
            for (int i = 0; i < path.Length; i++)
            {
                if (!attributedObjects.Any())
                    return new List<SettingItem>();
                string name = path.ElementAt(i);
                bool nameFound = false;
                foreach (AttributedObject<object> attributedObject in attributedObjects)
                {
                    if (((SettingMenuItemAttribute) attributedObject.Attribute).Name == name)
                    {
                        nameFound = true;
                        if (i == path.Length - 1)
                            settingsBuffer = attributedObject.FindAttributes(typeof(SettingAttribute));
                        else
                            attributedObjects = attributedObject.FindAttributes(typeof(SettingMenuItemAttribute)).ToList();
                        break;
                    }
                }
                if (!nameFound)
                    throw new Exceptions.KeyNotFoundException("The name " + name +
                                                                               " was not found in the settings at " +
                                                                               searchedPath);
                searchedPath += " + " + name;
            }

            List<SettingItem> settings = new List<SettingItem>();
            foreach (AttributedObject<object> attributedObject in settingsBuffer)
            {
                settings.Add(new SettingItem(path, attributedObject));
            }
            return settings;
        }

        /// <summary>
        /// Saves the change of a setting to a buffer. Affects only the functon GetSetting(path).\n
        /// The original object will be changed when the function AdoptTemporaryChanged() is called.\n
        /// Aims at navigation through the menu without losing changes.
        /// </summary>
        /// <param name="pathName">the path of SettingMenuItems to the setting plus the name of the setting</param>
        /// <param name="value">the value to change temporary</param>
        public static void ChangeSettingTemporary(string[] pathName, object value)
        {
            if (!pathName.Any())
                throw new ArgumentException("there was no path to the setting defined. Value: " + value);

            if (_temporaryChanges.ContainsKey(pathName))
                _temporaryChanges[pathName] = value;
            else
                _temporaryChanges.Add(pathName, value);
        }

        /// <summary>
        /// Adopt the changed setting for usage in the application.\n
        /// It is recommended to use ChangeSettingsTemporary to be able to discard changes.
        /// </summary>
        /// <param name="pathName">the path of SettingMenuItems to the setting plus the name of the setting</param>
        /// <param name="value">the value to change temporary</param>
        public static void ChangeSetting(string[] pathName, object value)
        {
            if (!pathName.Any())
                throw new ArgumentException("there was no path to the setting defined. Value: " + value);

            object parent = null;
            //find the correct parent
            for (int i = 0; i < pathName.Length - 1; i++)
            {
                string path = pathName[i];
                List<AttributedObject<object>> attributedObjects;
                if (parent == null)
                    attributedObjects = AttributeFinder.FindStaticAttributes(typeof(SettingMenuItemAttribute), null,
                        true).ToList();
                else
                    attributedObjects = parent.FindAttributes(typeof(SettingMenuItemAttribute)).ToList();

                bool newParentFound = false;
                foreach (AttributedObject<object> attributedObject in attributedObjects)
                {
                    if (((SettingMenuItemAttribute) attributedObject.Attribute).Name == path)
                    {
                        parent = attributedObject.Value;
                        newParentFound = true;
                        break;
                    }
                }
                if (!newParentFound)
                    throw new Exceptions.KeyNotFoundException("there was no parent found for " + path + " in " + pathName);
            }

            List<AttributedObject<object>> settings;
            if (parent == null)
                settings =
                    AttributeFinder.FindStaticAttributes(typeof(SettingAttribute))
                        .Where(attr => ((SettingAttribute) attr.Attribute).Name == pathName.Last())
                        .ToList();
            else
                settings =
                    parent.FindAttributes(typeof(SettingAttribute))
                        .Where(attr => ((SettingAttribute) attr.Attribute).Name == pathName.Last())
                        .ToList();
            if (settings.Count != 1)
                throw new AmbiguousMatchException("The number of found settings is different than 1 for: " + pathName);
            settings.First().Value = value;
        }

        /// <summary>
        /// Adopt the temporary changes for usage in the application.
        /// </summary>
        public static void AdoptTemporaryChanges()
        {
            foreach (KeyValuePair<IEnumerable<string>, object> temporaryChange in _temporaryChanges)
            {
                ChangeSetting(temporaryChange.Key.ToArray(), temporaryChange.Value);
            }
        }

        /// <summary>
        /// Saves settings to the fileSystem
        /// </summary>
        /// <param name="name">the name defined in the SettingEntryPointAttribute</param>
        /// <param name="filePath">the path on the fileSystem</param>
        public static void SaveSettings(string name, string filePath)
        {
            AdoptTemporaryChanges();
            //TODO: Add save and load functions
        }
    }
}
