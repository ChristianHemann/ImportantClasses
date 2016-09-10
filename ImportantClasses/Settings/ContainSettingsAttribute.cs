using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportantClasses.Settings
{
    /// <summary>
    /// Use this Attribute to show the settings search function that this static object contains settings
    /// </summary>
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property)]
    public class ContainSettingsAttribute : Attribute
    {
        /// <summary>
        /// Use this Attribute to show the settings search function that this static object contains settings
        /// </summary>
        public ContainSettingsAttribute() { }
    }
}
