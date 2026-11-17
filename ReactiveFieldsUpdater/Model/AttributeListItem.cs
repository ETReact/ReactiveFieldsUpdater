using System;
using System.ComponentModel;

namespace ReactiveFieldsUpdater
{
    /// <summary>
    /// Row of the Attributes grid: one editable metadata property of the selected field.
    /// </summary>
    internal class AttributeListItem
    {
        [DisplayName("Attribute")]
        public string AttributeName { get; set; }

        [DisplayName("Current Value")]
        public string CurrentValue { get; set; }

        [DisplayName("New Value")]
        public string NewValue { get; set; }

        [Browsable(false)]
        public Type ValueType { get; set; }
    }
}
