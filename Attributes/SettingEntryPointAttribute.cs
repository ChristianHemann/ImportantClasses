using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportantClasses.Attributes
{
    /// <summary>
    /// defines an entry-point for the settings functions to search
    /// </summary>
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property)]
    public class SettingEntryPointAttribute : Attribute
    {
        /// <summary>
        /// the name to identify the atttribute. Will be shown in the menu if ShowAsMenuItem is true
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// will be Attribute be shown in the menu as a menuItem?
        /// </summary>
        public bool ShowAsMenuItem { get; }

        /// <summary>
        /// defines an entry-point for the settings functions to search
        /// </summary>
        /// <param name="name">the name to identify the atttribute. Will be shown in the menu if ShowAsMenuItem is true</param>
        /// <param name="showAsMenuItem">will be Attribute be shown in the menu as a menuItem?</param>
        public SettingEntryPointAttribute(string name, bool showAsMenuItem = true)
        {
            Name = name;
            ShowAsMenuItem = showAsMenuItem;
        }
    }
}
