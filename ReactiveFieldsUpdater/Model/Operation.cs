using System;

namespace ReactiveFieldsUpdater
{
    /// <summary>
    /// A pending change of a single metadata property (e.g. MaxLength) on a single field.
    /// </summary>
    internal class Operation
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool Checked { get; set; }
        public string EntityName { get; set; }
        public string EntityField { get; set; }
        public string AttributeName { get; set; }

        /// <summary>Value on the server when the operation was created (invariant culture).</summary>
        public string CurrentValue { get; set; }

        /// <summary>Value to apply (invariant culture).</summary>
        public string AttributeValue { get; set; }

        /// <summary>Error from the last update attempt, null if none.</summary>
        public string Error { get; set; }

        /// <summary>True once the operation has been applied successfully.</summary>
        public bool Succeeded { get; set; }

        public bool Matches(string entityName, string fieldName, string attributeName)
        {
            return string.Equals(EntityName, entityName, StringComparison.OrdinalIgnoreCase)
                && string.Equals(EntityField, fieldName, StringComparison.OrdinalIgnoreCase)
                && string.Equals(AttributeName, attributeName, StringComparison.Ordinal);
        }
    }
}
