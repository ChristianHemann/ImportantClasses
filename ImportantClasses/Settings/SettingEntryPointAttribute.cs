using System;

namespace ImportantClasses.Settings
{
    /// <summary>
    /// Defines an entry-point for the settings functions to search.
    /// The object must be public and static to be found.
    /// The field or property will be shown as a Button exactly as a SettingMenuItem.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property)]
    public class SettingEntryPointAttribute : SettingMenuItemAttribute
    {
        /// <summary>
        /// defines an entry-point for the settings functions to search
        /// </summary>
        /// <param name="name">The name to identify this attribute. The name will be shown in the menu.</param>
        public SettingEntryPointAttribute(string name) : base(name) { }
    }
}
