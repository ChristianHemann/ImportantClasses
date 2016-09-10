using System;

namespace ImportantClasses.Settings
{
    /// <summary>
    /// Attribute to mark a field or property as a setting. 
    /// The object must be public to be found.
    /// The field or property itself must be static or its parent object must have a ContainSettingsAttribute and must be static
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SettingAttribute : Attribute
    {
        /// <summary>
        /// The Path in the Menu to the setting. Use / (Slash) for a sub folder
        /// </summary>
        public string Path { get; }
        /// <summary>
        /// The defaultValue for this property. If it is not given the DefaultValue will be null
        /// </summary>
        public object DefaultValue { get; }
        /// <summary>
        /// The Name which will be shown in the Menu
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// If the Value is numeric, the minimum Value can be given. When the maximum Value is lower than the minimum value or equal, there is no limitation. The DefaultValue is 0
        /// </summary>
        public double MinValue { get; }
        /// <summary>
        /// If the Value is numeric, the maximum Value can be given. When the maximum Value is lower than the minimum value or equal, there is no limitation. the DefaultValue is 0
        /// </summary>
        public double MaxValue {get;}
        /// <summary>
        /// If the Value is Numeric: Defines the accuracy, which can be set for this property. If the Value is negative, the predecimal Places are meant. The DefaultValue is 0
        /// </summary>
        public sbyte DecimalPlaces { get; }

        /// <summary>
        /// Attribute to mark a field or property as a setting. 
        /// The object must be public to be found.
        /// The field or property itself must be static or its parent object must have a ContainSettingsAttribute and must be static
        /// </summary>
        /// <param name="path">The Path in the Menu to the setting. Use / (Slash) for a sub folder</param>
        /// <param name="name">The Name which will be shown in the Menu</param>
        /// <param name="defaultValue">The defaultValue for this property. If it is not given the DefaultValue will be null</param>
        /// <param name="minValue">If the Value is numeric, the minimum Value can be given. When the maximum Value is lower than the minimum value or equal, there is no limitation. The DefaultValue is 0</param>
        /// <param name="maxValue">If the Value is numeric, the maximum Value can be given. When the maximum Value is lower than the minimum value or equal, there is no limitation. the DefaultValue is 0</param>
        /// <param name="decimalPlaces">If the Value is Numeric: Defines the accuracy, which can be set for this property. If the Value is negative, the predecimal Places are meant. The DefaultValue is 0</param>
        public SettingAttribute(string path, string name, object defaultValue = null, double minValue = 0, double maxValue = 0, sbyte decimalPlaces = 0)
        {
            Path = path;
            DefaultValue = defaultValue;
            Name = name;
            DecimalPlaces = decimalPlaces;
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}
