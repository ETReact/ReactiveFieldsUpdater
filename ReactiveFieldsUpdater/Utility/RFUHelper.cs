﻿using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using System.Data;
using Microsoft.Crm.Sdk.Messages;
using System.Web.Services.Description;

namespace ReactiveFieldsUpdater
{
    internal class RFUHelper
    {
        private static List<Operation> operationsList = new List<Operation>();
        private static readonly int checkBoxWidth = 50;

        /*---------------------------------------------------------------------------------   GET METHODS   */
        public static EntityMetadata[] RetrieveAllEntitiesMetadata(IOrganizationService service)
        {
            RetrieveAllEntitiesRequest request = new RetrieveAllEntitiesRequest();
            request.EntityFilters = EntityFilters.Entity;
            RetrieveAllEntitiesResponse response = (RetrieveAllEntitiesResponse)service.Execute(request);

            return response.EntityMetadata;
        }

        public static EntityMetadata RetrieveEntityMetadata(string entityName, IOrganizationService service)
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
            .Select(val => new ListViewItem(new[]
                {
                    val.LogicalName,
                    val.DisplayName?.UserLocalizedLabel?.Label
                }
            )
            { Tag = val }
            ).ToList();
        }

        public static List<ListViewItem> GetEntityFields(string entityName, IOrganizationService service)
        {
            var currentEntity = RetrieveEntityMetadata(entityName, service);

            return currentEntity.Attributes
                .Where(attribute =>
                attribute.DisplayName?.UserLocalizedLabel != null &&
                ((bool)attribute.AttributeTypeName?.Value.Contains("String")
                ||
                (bool)attribute.AttributeTypeName?.Value.Contains("Integer"))
                )
                .Select(attribute => new ListViewItem(new[]
                    {
                        attribute.LogicalName,
                        attribute.DisplayName?.UserLocalizedLabel?.Label,
                        attribute.AttributeTypeName?.Value.Replace("Type", "") ?? "Unknown"
                    }
                )
                { Tag = attribute }
                ).ToList();
        }

        public static List<AttributeListItem> GetFieldAttributes(string entityName, string fieldName, IOrganizationService service)
        {
            var currentEntity = RetrieveEntityMetadata(entityName, service);
            var currentField = currentEntity.Attributes.FirstOrDefault(p => p.LogicalName == fieldName);

            if (currentField == null)
                return new List<AttributeListItem>();

            return currentField.GetType()
                .GetProperties()
                .Where(p => RFUPluginControl.mySettings.Config_Props.Contains(p.Name))
                .OrderBy(p => p.Name)
                .Select(p => new AttributeListItem
                {
                    AttributeName = p.Name,
                    AttributeValue = p.GetValue(currentField) ?? "N/A"
                })
                .ToList();
        }


        /*---------------------------------------------------------------------------------   HANDLE VIEWS   */

        public static void UpdateListView(ListView listView, List<ListViewItem> listViewItems)
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

            if (listView.Columns.Count == 0)
                listView = AddListViewColumns(listView);

            if (listViewItems != null)
                listView.Items.AddRange(listViewItems.ToArray());

            listView.EndUpdate();
        }

        public static ListView AddListViewColumns(ListView listView)
        {
            var availableWidth = listView.ClientSize.Width;

            switch (listView?.Name)
            {
                case "entitiesListView":
                    listView.Columns.Add("Logical Name", availableWidth / 2, HorizontalAlignment.Left);
                    listView.Columns.Add("Display Name", availableWidth / 2, HorizontalAlignment.Left);
                    break;
                case "fieldsListView":
                    listView.Columns.Add("Field Logical Name", availableWidth / 3, HorizontalAlignment.Left);
                    listView.Columns.Add("Field Display Name", availableWidth / 3, HorizontalAlignment.Left);
                    listView.Columns.Add("Field Type", availableWidth / 3, HorizontalAlignment.Left);
                    break;
                case "operationsListView":
                    listView.CheckBoxes = true;
                    listView.Columns.Add("", checkBoxWidth, HorizontalAlignment.Center);
                    listView.Columns.Add("Entity", (availableWidth - checkBoxWidth) / 4, HorizontalAlignment.Left);
                    listView.Columns.Add("Field", (availableWidth - checkBoxWidth) / 4, HorizontalAlignment.Left);
                    listView.Columns.Add("Attribute", (availableWidth - checkBoxWidth) / 4, HorizontalAlignment.Left);
                    listView.Columns.Add("Value", (availableWidth - checkBoxWidth) / 4, HorizontalAlignment.Left);
                    break;
                default:
                    break;
            }

            return listView;
        }

        public static void ResizeListViewColumns(ListView listView)
        {
            if (listView.Columns.Count == 0) return;

            int availableWidth = listView.ClientSize.Width;
            int columnCount = listView.Columns.Count;

            for (int i = 0; i < columnCount; i++)
            {
                if (!String.IsNullOrEmpty(listView.Columns[i].Text))
                    listView.Columns[i].Width = availableWidth / columnCount;
            }
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

            dataGridView.Columns[0].ReadOnly = true;

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }


        /*---------------------------------------------------------------------------------   HANDLE OPERATIONS LIST   */
        public static List<ListViewItem> SetNewOperation(DataGridView dataGridView, string entityName, string fieldName, int rowIndex)
        {
            // gets the operation from the GridView
            var updatedDataRow = dataGridView.Rows[rowIndex];
            var updatedAttribute = updatedDataRow.Cells[0].Value?.ToString();
            var updatedAttributeValue = updatedDataRow.Cells[1].Value;

            if (string.IsNullOrEmpty(updatedAttribute))
                return new List<ListViewItem>();

            // adds a new operation to the global variable "operationsList"
            var existingOperation = operationsList.FirstOrDefault(a =>
            a.EntityName == entityName && a.EntityField == fieldName && a.AttributeName == updatedAttribute);
            if (existingOperation != null)
            {
                existingOperation.AttributeValue = updatedAttributeValue.ToString();
            }
            else
            {
                operationsList.Add(new Operation()
                {
                    Id = Guid.NewGuid(),
                    Checked = false,
                    EntityName = entityName,
                    EntityField = fieldName,
                    AttributeName = updatedAttribute,
                    AttributeValue = updatedAttributeValue.ToString()
                });
            }

            // returns all the operations as a ListViewItem List<>
            return GetItemsFromOperations();
        }

        public static void HandleCheckedItem(Guid operationId, bool value)
        {
            var existingOperation = operationsList.FirstOrDefault(a => a.Id == operationId);
            if (existingOperation != null)
            {
                existingOperation.Checked = value;
            }
        }

        public static void SelectAllOperations(bool toggle)
        {
            foreach (Operation operation in operationsList)
            {
                operation.Checked = toggle;
            }
        }

        public static List<ListViewItem> ClearOperations()
        {
            operationsList.RemoveAll(c => c.Checked);

            // returns all the operations as a ListViewItem List<>
            return GetItemsFromOperations();
        }

        public static List<ListViewItem> GetItemsFromOperations()
        {
            return operationsList
                .Select(operation => new ListViewItem(new[]
                    {
                        String.Empty,
                        operation.EntityName,
                        operation.EntityField,
                        operation.AttributeName,
                        (string)operation.AttributeValue
                    }
                )
                { Tag = operation }
                )
                .Select((item, index) => new { item, index })
                .OrderByDescending(x => x.index)
                .Select(x => x.item)
                .ToList();
        }

        /*---------------------------------------------------------------------------------   UPDATE METADATA   */
        public static void UpdateMetadata(IOrganizationService service)
        {
            foreach (var operation in operationsList)
            {
                if (!operation.Checked)
                    continue;

                if (string.IsNullOrEmpty(operation.EntityName) || string.IsNullOrEmpty(operation.EntityField))
                    continue;

                var currentEntityMetadata = RetrieveEntityMetadata(operation.EntityName, service);
                var currentField = currentEntityMetadata.Attributes.FirstOrDefault(p => p.LogicalName == operation.EntityField);

                if (currentField == null)
                    continue;

                PropertyInfo propertyInfo = currentField.GetType().GetProperty(operation.AttributeName);
                if (propertyInfo == null)
                    continue;

                Type targetType = propertyInfo.PropertyType;
                if (Nullable.GetUnderlyingType(targetType) != null)
                    targetType = Nullable.GetUnderlyingType(targetType);

                object convertedValue;
                try
                {
                    convertedValue = Convert.ChangeType(operation.AttributeValue, targetType);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Conversion Error for value: {ex.Message}", "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    continue;
                }

                propertyInfo.SetValue(currentField, convertedValue);

                try
                {
                    var updateRequest = new UpdateAttributeRequest
                    {
                        Attribute = currentField,
                        EntityName = operation.EntityName,
                        MergeLabels = true
                    };
                    service.Execute(updateRequest);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Updating Error for value: {ex.Message}", "Updating Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    continue;
                }
            }
        }
    }
}
