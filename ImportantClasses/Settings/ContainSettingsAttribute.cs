using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportantClasses.Settings
{
    /// <summary>
    /// Use this Attribute to show the settings search function that this public static object contains settings. 
    /// When changing the value of this object it is strongly recommended to call Settings.Initialize() to make sure the settings work on the same object. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property)]
    public class ContainSettingsAttribute : Attribute
    {
        /// <summary>
        /// Use this Attribute to show the settings search function that this public static object contains settings. 
        /// When changing the value of this object it is strongly recommended to call Settings.Initialize() to make sure the settings work on the same object. 
        /// </summary>
        public ContainSettingsAttribute() { }
    }
}
