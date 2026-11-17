using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;

namespace ReactiveFieldsUpdater
{
    /// <summary>
    /// Stateless helpers: metadata retrieval, value conversion/validation and ListView handling.
    /// All state (operations, caches) lives in the plugin control instance, so several
    /// instances of the tool connected to different organizations never share data.
    /// </summary>
    internal static class RFUHelper
    {
        private const int CheckBoxColumnWidth = 30;
        private const int StringMaxLengthLimit = 4000;
        private const int MemoMaxLengthLimit = 1048576;

        private static readonly HashSet<AttributeTypeCode> SupportedTypes = new HashSet<AttributeTypeCode>
        {
            AttributeTypeCode.String,
            AttributeTypeCode.Memo,
            AttributeTypeCode.Integer,
            AttributeTypeCode.Decimal,
            AttributeTypeCode.Double,
            AttributeTypeCode.Money
        };

        /*---------------------------------------------------------------------------------   SERVER CALLS   */

        public static EntityMetadata[] RetrieveAllEntitiesMetadata(IOrganizationService service)
        {
            var response = (RetrieveAllEntitiesResponse)service.Execute(new RetrieveAllEntitiesRequest
            {
                EntityFilters = EntityFilters.Entity,
                RetrieveAsIfPublished = true
            });

            return response.EntityMetadata;
        }

        public static EntityMetadata RetrieveEntityMetadata(string entityName, IOrganizationService service)
        {
            var response = (RetrieveEntityResponse)service.Execute(new RetrieveEntityRequest
            {
                EntityFilters = EntityFilters.Attributes,
                LogicalName = entityName,
                RetrieveAsIfPublished = true
            });

            return response.EntityMetadata;
        }

        public static AttributeMetadata RetrieveAttributeMetadata(string entityName, string fieldName, IOrganizationService service)
        {
            var response = (RetrieveAttributeResponse)service.Execute(new RetrieveAttributeRequest
            {
                EntityLogicalName = entityName,
                LogicalName = fieldName,
                RetrieveAsIfPublished = true
            });

            return response.AttributeMetadata;
        }

        public static void PublishEntities(IEnumerable<string> entityNames, IOrganizationService service)
        {
            var entities = string.Concat(entityNames.Select(e => "<entity>" + SecurityElement.Escape(e) + "</entity>"));
            service.Execute(new PublishXmlRequest
            {
                ParameterXml = "<importexportxml><entities>" + entities + "</entities></importexportxml>"
            });
        }

        /*---------------------------------------------------------------------------------   LIST ITEMS   */

        public static List<ListViewItem> GetEntityItems(IEnumerable<EntityMetadata> allMetadata)
        {
            return allMetadata
                .Where(e => e.DisplayName?.UserLocalizedLabel != null && e.IsIntersect != true)
                .OrderBy(e => e.LogicalName)
                .Select(e => new ListViewItem(new[]
                {
                    e.LogicalName,
                    e.DisplayName.UserLocalizedLabel.Label,
                    e.IsCustomEntity == true ? "Custom" : "System"
                })
                { Tag = e })
                .ToList();
        }

        public static List<ListViewItem> GetFieldItems(EntityMetadata entity)
        {
            return entity.Attributes
                .Where(IsEditableField)
                .OrderBy(a => a.LogicalName)
                .Select(a => new ListViewItem(new[]
                {
                    a.LogicalName,
                    a.DisplayName.UserLocalizedLabel.Label,
                    a.AttributeType?.ToString() ?? "Unknown"
                })
                { Tag = a })
                .ToList();
        }

        private static bool IsEditableField(AttributeMetadata attribute)
        {
            return attribute.DisplayName?.UserLocalizedLabel != null
                && attribute.AttributeType.HasValue
                && SupportedTypes.Contains(attribute.AttributeType.Value)
                && attribute.AttributeOf == null                       // skip generated/virtual companions (e.g. *_base, yomi)
                && attribute.IsCustomizable?.Value != false;
        }

        public static List<AttributeListItem> GetFieldAttributes(AttributeMetadata field, IEnumerable<string> editableProps, IEnumerable<Operation> pendingOperations)
        {
            if (field == null)
                return new List<AttributeListItem>();

            var props = new HashSet<string>(editableProps ?? Enumerable.Empty<string>());
            var pending = pendingOperations
                .Where(o => !o.Succeeded
                    && string.Equals(o.EntityName, field.EntityLogicalName, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(o.EntityField, field.LogicalName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return field.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => props.Contains(p.Name) && p.CanWrite)
                .OrderBy(p => p.Name)
                .Select(p => new AttributeListItem
                {
                    AttributeName = p.Name,
                    CurrentValue = FormatValue(p.GetValue(field)),
                    NewValue = pending.FirstOrDefault(o => o.AttributeName == p.Name)?.AttributeValue,
                    ValueType = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType
                })
                .ToList();
        }

        public static ListViewItem ToListViewItem(Operation operation)
        {
            var item = new ListViewItem(new[]
            {
                string.Empty,
                operation.EntityName,
                operation.EntityField,
                operation.AttributeName,
                operation.CurrentValue,
                operation.AttributeValue,
                operation.Error == null ? "Pending" : "Failed: " + operation.Error
            })
            {
                Tag = operation,
                Checked = operation.Checked
            };

            if (operation.Error != null)
            {
                item.ForeColor = Color.Firebrick;
                item.ToolTipText = operation.Error;
            }

            return item;
        }

        /*---------------------------------------------------------------------------------   VALUES   */

        public static string FormatValue(object value)
        {
            return value == null ? string.Empty : Convert.ToString(value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Parses user input (current culture first, then invariant) into the target type.
        /// </summary>
        public static bool TryConvert(string text, Type targetType, out object value, out string error)
        {
            value = null;
            error = null;

            foreach (var culture in new[] { CultureInfo.CurrentCulture, CultureInfo.InvariantCulture })
            {
                try
                {
                    value = Convert.ChangeType(text.Trim(), targetType, culture);
                    return true;
                }
                catch (Exception ex) when (ex is FormatException || ex is OverflowException || ex is InvalidCastException)
                {
                    error = ex is OverflowException
                        ? $"Value is out of range for {targetType.Name}."
                        : $"'{text}' is not a valid {targetType.Name}.";
                }
            }

            return false;
        }

        /// <summary>
        /// Business validation of a converted value. Returns an error message or null.
        /// </summary>
        public static string Validate(AttributeMetadata field, string propertyName, object value)
        {
            if (propertyName == "MaxLength" && value is int maxLength)
            {
                var limit = field is MemoAttributeMetadata ? MemoMaxLengthLimit : StringMaxLengthLimit;
                if (maxLength < 1 || maxLength > limit)
                    return $"MaxLength must be between 1 and {limit}.";
            }

            return null;
        }

        /// <summary>
        /// Converts and sets the value of an operation on a metadata object. Throws on invalid values.
        /// </summary>
        public static void ApplyValue(AttributeMetadata field, string propertyName, string value)
        {
            var property = field.GetType().GetProperty(propertyName);
            if (property == null || !property.CanWrite)
                throw new InvalidOperationException($"Property '{propertyName}' is not available on {field.GetType().Name}.");

            var targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

            // Operation values are stored in invariant culture
            object converted;
            try
            {
                converted = Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
            }
            catch (Exception ex) when (ex is FormatException || ex is OverflowException || ex is InvalidCastException)
            {
                throw new InvalidOperationException($"'{value}' is not a valid {targetType.Name}.", ex);
            }

            var validationError = Validate(field, propertyName, converted);
            if (validationError != null)
                throw new InvalidOperationException(validationError);

            property.SetValue(field, converted);
        }

        /*---------------------------------------------------------------------------------   LIST VIEWS   */

        public static void InitListView(ListView listView, params string[] columns)
        {
            listView.View = View.Details;
            listView.LabelEdit = false;
            listView.AllowColumnReorder = false;
            listView.FullRowSelect = true;
            listView.GridLines = true;
            listView.ShowItemToolTips = true;

            foreach (var column in columns)
                listView.Columns.Add(column, column.Length == 0 ? CheckBoxColumnWidth : 100, HorizontalAlignment.Left);

            ResizeListViewColumns(listView);
        }

        public static void SetItems(ListView listView, IEnumerable<ListViewItem> items)
        {
            listView.BeginUpdate();
            try
            {
                listView.Items.Clear();
                if (items != null)
                    listView.Items.AddRange(items.ToArray());
            }
            finally
            {
                listView.EndUpdate();
            }
        }

        public static IEnumerable<ListViewItem> Filter(IEnumerable<ListViewItem> items, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return items;

            text = text.Trim();
            return items.Where(item => item.SubItems
                .Cast<ListViewItem.ListViewSubItem>()
                .Any(sub => sub.Text != null && sub.Text.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0));
        }

        /// <summary>
        /// Columns with an empty header (checkbox column) keep their width, the others share the rest.
        /// </summary>
        public static void ResizeListViewColumns(ListView listView)
        {
            if (listView.Columns.Count == 0)
                return;

            var fixedColumns = listView.Columns.Cast<ColumnHeader>().Where(c => string.IsNullOrEmpty(c.Text)).ToList();
            var flexibleColumns = listView.Columns.Cast<ColumnHeader>().Except(fixedColumns).ToList();
            if (flexibleColumns.Count == 0)
                return;

            var available = listView.ClientSize.Width - fixedColumns.Sum(c => c.Width) - SystemInformation.VerticalScrollBarWidth;
            var width = Math.Max(60, available / flexibleColumns.Count);
            foreach (var column in flexibleColumns)
                column.Width = width;
        }

        /*---------------------------------------------------------------------------------   TEXTBOX CUE BANNER   */

        private const int EM_SETCUEBANNER = 0x1501;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

        public static void SetPlaceholder(TextBox textBox, string text)
        {
            if (textBox.IsHandleCreated)
                SendMessage(textBox.Handle, EM_SETCUEBANNER, IntPtr.Zero, text);
            else
                textBox.HandleCreated += (s, e) => SendMessage(textBox.Handle, EM_SETCUEBANNER, IntPtr.Zero, text);
        }
    }
}
