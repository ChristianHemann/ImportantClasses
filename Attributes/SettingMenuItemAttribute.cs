using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportantClasses.Attributes
{
    /// <summary>
    /// Attribute to make a field or property shown in the menu as a Menu Item which can contain Settings or other menuItems. The object must be public to be found.
    /// The fields or properties parent object must contain a SettingEntryPointAttribute or SettingMenuItemAttribute too.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SettingMenuItemAttribute : Attribute
    {
        public readonly string Name;

        /// <summary>
        /// Attribute to make a field or property shown in the menu as a Menu Item which can contain Settings or other menuItems. The object must be public to be found.
        /// The fields or properties parent object must contain a SettingEntryPointAttribute or SettingMenuItemAttribute too.
        /// </summary>
        /// <param name="name">The Name which will be shown in the Menu</param>
        public SettingMenuItemAttribute(string name)
        {
            Name = name;
        }
    }
}
