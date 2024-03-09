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

namespace ReactiveFieldsUpdater
{
    internal class RFUHelper
    {
        private static List<FieldAttribute> myAttributesList = new List<FieldAttribute>();

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
            List<ListViewItem> listViewItems = new List<ListViewItem>();

            foreach (EntityMetadata val in allMetadata)
            {
                if (val.DisplayName.UserLocalizedLabel != null)
                {
                    ListViewItem listViewItem = new ListViewItem
                    {
                        Text = val.LogicalName,
                        Tag = (object)val
                    };
                    listViewItem.SubItems.Add(val.SchemaName);
                    listViewItems.Add(listViewItem);
                }
            }

            return listViewItems;
        }

        public static List<ListViewItem> GetEntityFields(string entityName, IOrganizationService service)
        {
            List<ListViewItem> listViewItems = new List<ListViewItem>();

            EntityMetadata currentEntity = RetrieveEntitiyMetadata(entityName, service);

            foreach (AttributeMetadata attribute in currentEntity.Attributes)
            {
                ListViewItem listViewItem = new ListViewItem
                {
                    Text = attribute.LogicalName,
                    Tag = (object)attribute
                };
                listViewItem.SubItems.Add(attribute.SchemaName);
                listViewItem.SubItems.Add(attribute.AttributeTypeName.Value.Replace("Type", ""));
                listViewItems.Add(listViewItem);
            }

            return listViewItems;
        }

        public static List<object> GetFieldAttributes(string entityName, string fieldName, IOrganizationService service)
        {
            List<object> listFieldAttribute = new List<object>();

            EntityMetadata currentEntity = RetrieveEntitiyMetadata(entityName, service);

            AttributeMetadata currentField = currentEntity.Attributes.FirstOrDefault(p => p.LogicalName == fieldName);

            var attributes = currentField.GetType().GetProperties().OrderBy(x => x.Name);

            foreach (var attribute in attributes)
            {
                var attributeValue = attribute.GetValue(currentField)?.ToString();
                if (!String.IsNullOrEmpty(attributeValue))
                {
                    listFieldAttribute.Add(new FieldAttribute()
                    {
                        AttributeName = attribute.Name,
                        AttributeValue = attributeValue
                    });
                }
            }

            return listFieldAttribute;
        }

        public static void SetNewAttributeValue(DataGridView dataGridView, DataGridViewCellEventArgs e, IOrganizationService service)
        {
            var rowIndex = e.RowIndex;

            DataGridViewRow updatedDataRow = dataGridView.Rows[rowIndex];
            var updatedAttribute = updatedDataRow.Cells[0].Value?.ToString();
            var updatedAttributeValue = updatedDataRow.Cells[1].Value?.ToString();

            var newAttr = new FieldAttribute()
            {
                AttributeName = updatedAttribute,
                AttributeValue = updatedAttributeValue
            };

            myAttributesList.Add(newAttr);
        }

        public static void UpdateMetadata(string entityName, string fieldName, IOrganizationService service)
        {
            if (String.IsNullOrEmpty(entityName) || String.IsNullOrEmpty(fieldName))
                return;

            EntityMetadata currentEntity = RetrieveEntitiyMetadata(entityName, service);
            AttributeMetadata currentField = currentEntity.Attributes.FirstOrDefault(p => p.LogicalName == fieldName);

            // -> updating the property of the current field with the new value (only works with INT)

            foreach (var attributeUp in myAttributesList)
            {
                PropertyInfo propertyInfo = currentField.GetType().GetProperty(attributeUp.AttributeName);
                Type nonNullableType = Nullable.GetUnderlyingType(propertyInfo.PropertyType);

                propertyInfo.SetValue(currentField, Convert.ChangeType(Convert.ToInt32(attributeUp.AttributeValue), nonNullableType));

                UpdateAttributeRequest updReq = new UpdateAttributeRequest
                {
                    Attribute = currentField,
                    EntityName = entityName,
                    MergeLabels = true
                };
                service.Execute(updReq);
            }
        }

        public static void UpdateListView(ListView listView, List<ListViewItem> listViewItems)
        {
            // Set the view to show details.
            listView.View = View.Details;
            // Allow the user to edit item text.
            listView.LabelEdit = false;
            // Allow the user to rearrange columns.
            listView.AllowColumnReorder = true;
            // Display check boxes.
            listView.CheckBoxes = false;
            // Select the item and subitems when selection is made.
            listView.FullRowSelect = true;
            // Display grid lines.
            listView.GridLines = true;
            // Sort the items in the list in ascending order.
            listView.Sorting = SortOrder.Ascending;


            //Add the items to the ListView.
            listView.Items.AddRange(listViewItems.ToArray());
        }

        public static void UpdateDataGridView(DataGridView dataGridView, List<object> rows)
        {
            //use binding source to hold dummy data
            BindingSource binding = new BindingSource();
            binding.DataSource = rows;

            //bind datagridview to binding source
            dataGridView.DataSource = binding;

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }
    }

    class FieldAttribute
    {
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }

    }
}
