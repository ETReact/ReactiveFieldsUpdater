using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Windows.Forms;
using Microsoft.Xrm.Sdk.Client;
using System.Reflection;
using System.Xml.Linq;
using System.Drawing;
using System.Data;
using System.Configuration;
using XrmToolBox.Extensibility;

namespace ReactiveFieldsUpdater
{
    internal class RFUHelper
    {
        private static List<FieldAttribute> myAttributesList;

        private static List<string> CONFIG_PROPS = new List<string>()
        {
            "DisplayName",
            "Description",
            "IsRequired",
            "Searchable",
            "MaxLength",
            "Format",
            "MaxValue",
            "MinValue",
            "Audit",
            "Column Security",
            "Dashboard global filter",
            "IsSortable"
        };

        public static EntityMetadata[] RetrieveAllEntitiesMetadata(IOrganizationService service)
        {
            RetrieveAllEntitiesRequest request = new RetrieveAllEntitiesRequest();
            request.EntityFilters = EntityFilters.Entity;
            RetrieveAllEntitiesResponse response = (RetrieveAllEntitiesResponse)service.Execute(request);

            return response.EntityMetadata;
        }

        public static EntityMetadata RetrieveEntitiyMetadata(string entityName, IOrganizationService service)
        {
            RetrieveEntityRequest request = new RetrieveEntityRequest
            {
                EntityFilters = EntityFilters.All,
                LogicalName = entityName,
                RetrieveAsIfPublished = true
            };
            RetrieveEntityResponse response = (RetrieveEntityResponse)service.Execute(request);

            return response.EntityMetadata;
        }

        public static List<ListViewItem> GetMetadataLabels(EntityMetadata[] allMetadata)
        {
            return allMetadata
            .Where(val => val.DisplayName?.UserLocalizedLabel != null)
            .Select(val => new ListViewItem(new[] { val.LogicalName, val.SchemaName }) { Tag = val })
            .ToList();
        }

        public static List<ListViewItem> GetEntityFields(string entityName, IOrganizationService service)
        {
            var currentEntity = RetrieveEntitiyMetadata(entityName, service);

            return currentEntity.Attributes
                .Select(attribute => new ListViewItem(new[]
                    {
                        attribute.LogicalName,
                        attribute.SchemaName,
                        attribute.AttributeTypeName?.Value.Replace("Type", "") ?? "Unknown"
                    }
                )
                { Tag = attribute }
                ).ToList();
        }

        public static List<FieldAttribute> GetFieldAttributes(string entityName, string fieldName, IOrganizationService service)
        {
            var currentEntity = RetrieveEntitiyMetadata(entityName, service);
            var currentField = currentEntity.Attributes.FirstOrDefault(p => p.LogicalName == fieldName);

            if (currentField == null)
                return new List<FieldAttribute>();

            return currentField.GetType()
                .GetProperties()
                .Where(p => CONFIG_PROPS.Contains(p.Name))
                .OrderBy(p => p.Name)
                .Select(p => new FieldAttribute
                {
                    AttributeName = p.Name,
                    AttributeValue = p.GetValue(currentField) ?? "N/A"
                })
                .ToList();
        }

        public static void UpdateEntityListView(ListView listView, List<ListViewItem> listViewItems)
        {
            listView.BeginUpdate();
            listView.Items.Clear();

            if (listView.Columns.Count == 0)
            {
                listView.View = View.Details;
                listView.LabelEdit = false;
                listView.AllowColumnReorder = true;
                listView.CheckBoxes = false;
                listView.FullRowSelect = true;
                listView.GridLines = true;
                listView.Sorting = SortOrder.Ascending;
            }

            listView.Columns.Add("Logical Name", 250, HorizontalAlignment.Left);
            listView.Columns.Add("Schema Name", 250, HorizontalAlignment.Left);

            //Add the items to the ListView.
            listView.Items.AddRange(listViewItems.ToArray());
            listView.EndUpdate();
        }

        public static void UpdateFieldListView(ListView listView, List<ListViewItem> listViewItems)
        {
            listView.BeginUpdate();
            listView.Items.Clear();

            if (listView.Columns.Count == 0)
            {
                listView.View = View.Details;
                listView.LabelEdit = false;
                listView.AllowColumnReorder = true;
                listView.CheckBoxes = false;
                listView.FullRowSelect = true;
                listView.GridLines = true;
                listView.Sorting = SortOrder.Ascending;
            }

            listView.Columns.Add("Field Logical Name", 250, HorizontalAlignment.Left);
            listView.Columns.Add("Field Schema Name", 250, HorizontalAlignment.Left);
            listView.Columns.Add("Field Type", 250, HorizontalAlignment.Left);

            //Add the items to the ListView.
            listView.Items.AddRange(listViewItems.ToArray());
            listView.EndUpdate();
        }

        public static void UpdateDataGridView<T>(DataGridView dataGridView, List<T> rows)
        {
            if (dataGridView.InvokeRequired)
            {
                dataGridView.BeginInvoke(new Action(() => UpdateDataGridView(dataGridView, rows)));
                return;
            }

            var binding = new BindingSource { DataSource = rows };
            dataGridView.DataSource = binding;

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        /* ------------------------------------------------------------------------------------
           UPDATE DATA */

        public static void SetNewAttributeValue(DataGridView dataGridView, int rowIndex, int columnIndex, IOrganizationService service)
        {
            var updatedDataRow = dataGridView.Rows[rowIndex];
            var updatedAttribute = updatedDataRow.Cells[0].Value?.ToString();
            var updatedAttributeValue = updatedDataRow.Cells[1].Value;

            if (string.IsNullOrEmpty(updatedAttribute))
                return;

            var existingAttr = myAttributesList.FirstOrDefault(a => a.AttributeName == updatedAttribute);
            if (existingAttr != null)
            {
                existingAttr.AttributeValue = updatedAttributeValue;
            }
            else
            {
                myAttributesList.Add(new FieldAttribute()
                {
                    AttributeName = updatedAttribute,
                    AttributeValue = updatedAttributeValue
                });
            }
        }

        public static void UpdateMetadata_OLD(string entityName, string fieldName, IOrganizationService service)
        {
            if (String.IsNullOrEmpty(entityName) || String.IsNullOrEmpty(fieldName))
                return;

            EntityMetadata currentEntity = RetrieveEntitiyMetadata(entityName, service);
            AttributeMetadata currentField = currentEntity.Attributes.FirstOrDefault(p => p.LogicalName == fieldName);

            // -> updating the property of the current field with the new value
            foreach (var attributeUp in myAttributesList)
            {
                PropertyInfo propertyInfo = currentField.GetType().GetProperty(attributeUp.AttributeName);
                Type nonNullableType = Nullable.GetUnderlyingType(propertyInfo.PropertyType);

                propertyInfo.SetValue(currentField, Convert.ChangeType(attributeUp.AttributeValue, nonNullableType));

                UpdateAttributeRequest updReq = new UpdateAttributeRequest
                {
                    Attribute = currentField,
                    EntityName = entityName,
                    MergeLabels = true
                };
                service.Execute(updReq);
            }
        }

        public static void UpdateMetadata(string entityName, string fieldName, IOrganizationService service)
        {
            if (string.IsNullOrEmpty(entityName) || string.IsNullOrEmpty(fieldName))
                return;

            var currentEntity = RetrieveEntitiyMetadata(entityName, service);
            var currentField = currentEntity.Attributes.FirstOrDefault(p => p.LogicalName == fieldName);

            if (currentField == null)
                return;

            foreach (var attributeUp in myAttributesList)
            {
                PropertyInfo propertyInfo = currentField.GetType().GetProperty(attributeUp.AttributeName);
                if (propertyInfo == null)
                    continue; // Evita errori se la proprietà non esiste

                // Determina il tipo corretto per il cast
                Type targetType = propertyInfo.PropertyType;
                if (Nullable.GetUnderlyingType(targetType) != null)
                    targetType = Nullable.GetUnderlyingType(targetType); // Ottiene il tipo non nullable

                object convertedValue;
                try
                {
                    convertedValue = Convert.ChangeType(attributeUp.AttributeValue, targetType);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Errore nella conversione del valore: {ex.Message}", "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    continue; // Salta l'attributo con errore
                }

                propertyInfo.SetValue(currentField, convertedValue);

                var updateRequest = new UpdateAttributeRequest
                {
                    Attribute = currentField,
                    EntityName = entityName,
                    MergeLabels = true
                };
                service.Execute(updateRequest);
            }
        }
    }

    class FieldAttribute
    {
        public string AttributeName { get; set; }
        public dynamic AttributeValue { get; set; }

    }
}
