using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportantClasses.Attributes
{
    /// <summary>
    /// Attribute to make a field or property shown in the menu as a setting. The object must be public to be found.
    /// The fields or properties parent object must have a SettingEntryPointAttribute or SettingMenuItemAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SettingAttribute : Attribute
    {
        public object DefaultValue { get; }
        public string Name { get; }
        public double MinValue { get; }
        public double MaxValue {get;}
        public sbyte DecimalPlaces { get; }

        /// <summary>
        /// Attribute to make a field or property shown in the menu as a setting. The object must be public to be found.
        /// The fields or properties parent object must have a SettingEntryPointAttribute or SettingMenuItemAttribute.
        /// </summary>
        /// <param name="name">The Name which will be shown in the Menu</param>
        /// <param name="defaultValue">The defaultValue for this property. If it is not given the DefaultValue will be null</param>
        /// <param name="minValue">If the Value is numeric, the minimum Value can be given. When the maximum Value is lower than the minimum value or equal, there is no limitation. The DefaultValue is 0</param>
        /// <param name="maxValue">If the Value is numeric, the maximum Value can be given. When the maximum Value is lower than the minimum value or equal, there is no limitation. the DefaultValue is 0</param>
        /// <param name="decimalPlaces">If the Value is Numeric: Defines the accuracy, which can be set for this property. If the Value is negative, the predecimal Places are meant. The DefaultValue is 0</param>
        public SettingAttribute(string name, object defaultValue = null, double minValue = 0, double maxValue = 0, sbyte decimalPlaces = 0)
        {
            DefaultValue = defaultValue;
            Name = name;
            DecimalPlaces = decimalPlaces;
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}
