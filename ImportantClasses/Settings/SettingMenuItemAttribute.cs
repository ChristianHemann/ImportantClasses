using System;

namespace ImportantClasses.Settings
{
    /// <summary>
    /// Attribute to make a field or property shown in the menu as a Button which can redirect to Settings or other Buttons.
    /// The object must be public to be found.
    /// The fields or properties parent object must contain a SettingEntryPointAttribute or SettingMenuItemAttribute or must be static to be found. When static it is interpreted as a SettingEntryPoint.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SettingMenuItemAttribute : Attribute
    {
        /// <summary>
        /// The name to identify this attribute. The name will be shown in the menu.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Attribute to make a field or property shown in the menu as a Button which can redirect to Settings or other Buttons.
        /// The object must be public to be found.
        /// The fields or properties parent object must contain a SettingEntryPointAttribute or SettingMenuItemAttribute or must be static to be found.
        /// </summary>
        /// <param name="name">The name to identify this attribute. The name will be shown in the menu.</param>
        public SettingMenuItemAttribute(string name)
        {
            Name = name;
        }
    }
}
