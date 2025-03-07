﻿using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using System.Data;

namespace ReactiveFieldsUpdater
{
    internal class RFUHelper
    {
        private static List<Operation> operationsList = new List<Operation>();


        /*---------------------------------------------------------------------------------   GET METHODS   */
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
            .Select(val => new ListViewItem(new[] { val.LogicalName }) { Tag = val })
            .ToList();
        }

        public static List<ListViewItem> GetEntityFields(string entityName, IOrganizationService service)
        {
            var currentEntity = RetrieveEntitiyMetadata(entityName, service);

            return currentEntity.Attributes
                .Where(attribute =>
                (bool)attribute.AttributeTypeName?.Value.Contains("String")
                ||
                (bool)attribute.AttributeTypeName?.Value.Contains("Integer")
                )
                .Select(attribute => new ListViewItem(new[]
                    {
                        attribute.LogicalName,
                        attribute.AttributeTypeName?.Value.Replace("Type", "") ?? "Unknown"
                    }
                )
                { Tag = attribute }
                ).ToList();
        }

        public static List<AttributesListItem> GetFieldAttributes(string entityName, string fieldName, IOrganizationService service)
        {
            var currentEntity = RetrieveEntitiyMetadata(entityName, service);
            var currentField = currentEntity.Attributes.FirstOrDefault(p => p.LogicalName == fieldName);

            if (currentField == null)
                return new List<AttributesListItem>();

            return currentField.GetType()
                .GetProperties()
                .Where(p => RFUPluginControl.mySettings.Config_Props.Contains(p.Name))
                .OrderBy(p => p.Name)
                .Select(p => new AttributesListItem
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
            switch (listView?.Name)
            {
                case "entitiesListView":
                    listView.Columns.Add("Logical Name", 500, HorizontalAlignment.Left);
                    break;
                case "fieldsListView":
                    listView.Columns.Add("Field Logical Name", 250, HorizontalAlignment.Left);
                    listView.Columns.Add("Field Type", 250, HorizontalAlignment.Left);
                    break;
                case "operationsListView":
                    listView.Columns.Add("Entity", 200, HorizontalAlignment.Left);
                    listView.Columns.Add("Field", 200, HorizontalAlignment.Left);
                    listView.Columns.Add("Attribute", 200, HorizontalAlignment.Left);
                    listView.Columns.Add("Value", 200, HorizontalAlignment.Left);

                    listView.CheckBoxes = true;

                    break;
                default:
                    break;
            }

            return listView;
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
        public static List<ListViewItem> SetNewOperation(DataGridView dataGridView, string entityName, string fieldName, int rowIndex, IOrganizationService service)
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
                    EntityName = entityName,
                    EntityField = fieldName,
                    AttributeName = updatedAttribute,
                    AttributeValue = updatedAttributeValue.ToString()
                });
            }

            // returns all the operations as a ListViewItem List<>
            return operationsList
                .Select(operation => new ListViewItem(new[]
                    {
                        operation.EntityName,
                        operation.EntityField,
                        operation.AttributeName,
                        (string)operation.AttributeValue
                    }
                )
                { Tag = operation }
                ).ToList();
        }


        /*---------------------------------------------------------------------------------   UPDATE METADATA   */
        public static bool UpdateMetadata(string entityName, string fieldName, IOrganizationService service)
        {
            if (string.IsNullOrEmpty(entityName) || string.IsNullOrEmpty(fieldName))
                return false;

            var currentEntity = RetrieveEntitiyMetadata(entityName, service);
            var currentField = currentEntity.Attributes.FirstOrDefault(p => p.LogicalName == fieldName);

            if (currentField == null)
                return false;

            foreach (var attributeUp in operationsList)
            {
                PropertyInfo propertyInfo = currentField.GetType().GetProperty(attributeUp.AttributeName);
                if (propertyInfo == null)
                    continue;

                Type targetType = propertyInfo.PropertyType;
                if (Nullable.GetUnderlyingType(targetType) != null)
                    targetType = Nullable.GetUnderlyingType(targetType);

                object convertedValue;
                try
                {
                    convertedValue = Convert.ChangeType(attributeUp.AttributeValue, targetType);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Conversion Error for value: {ex.Message}", "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    continue;
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

            return ClearOperations();
        }

        public static bool ClearOperations()
        {
            operationsList.Clear();
            return true;
        }

    }

    class AttributesListItem
    {
        public string AttributeName { get; set; }
        public dynamic AttributeValue { get; set; }

    }

    class Operation
    {
        public string EntityName { get; set; }
        public string EntityField { get; set; }
        public string AttributeName { get; set; }
        public dynamic AttributeValue { get; set; }

    }
}
