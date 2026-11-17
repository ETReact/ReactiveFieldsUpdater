using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using XrmToolBox.Extensibility;

namespace ReactiveFieldsUpdater
{
    public partial class RFUPluginControl : PluginControlBase
    {
        private Settings _settings;

        // Per-instance state: XrmToolBox can open several instances of the tool on different connections
        private readonly List<Operation> _operations = new List<Operation>();
        private readonly Dictionary<string, EntityMetadata> _entityCache = new Dictionary<string, EntityMetadata>(StringComparer.OrdinalIgnoreCase);
        private List<ListViewItem> _allEntityItems = new List<ListViewItem>();
        private List<ListViewItem> _allFieldItems = new List<ListViewItem>();

        private string _selectedEntity;
        private AttributeMetadata _selectedField;
        private bool _suppressCheckEvents;

        public RFUPluginControl()
        {
            InitializeComponent();
        }

        /*---------------------------------------------------------------------------------   LIFECYCLE   */

        private void MyPluginControl_Load(object sender, EventArgs e)
        {
            if (!SettingsManager.Instance.TryLoad(GetType(), out _settings) || _settings == null)
            {
                _settings = new Settings();
                LogWarning("Settings not found => a new settings file has been created!");
            }
            else
            {
                LogInfo("Settings found and loaded");
            }

            if (Uri.TryCreate(_settings.CustomUrl, UriKind.Absolute, out var learnMoreUri))
                ShowInfoNotification("Learn more --->", learnMoreUri);

            RFUHelper.InitListView(entitiesListView, "Logical Name", "Display Name", "Type");
            RFUHelper.InitListView(fieldsListView, "Field Logical Name", "Field Display Name", "Field Type");
            RFUHelper.InitListView(operationsListView, "", "Entity", "Field", "Attribute", "Current Value", "New Value", "Status");
            operationsListView.CheckBoxes = true;

            ListViewColumnSorter.Attach(entitiesListView);
            ListViewColumnSorter.Attach(fieldsListView);

            RFUHelper.SetPlaceholder(txtFilterEntities, "Filter entities...");
            RFUHelper.SetPlaceholder(txtFilterFields, "Filter fields...");

            attributesGridView.AutoGenerateColumns = true;
            attributesGridView.RowHeadersVisible = false;
            attributesGridView.AllowUserToDeleteRows = false;
            attributesGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            attributesGridView.EditMode = DataGridViewEditMode.EditOnEnter;

            toolStripButtonPublish.Checked = _settings.PublishAfterUpdate;

            RefreshOperationsList();

            if (Service != null)
                ExecuteMethod(GetEntities);
        }

        public override void ClosingPlugin(PluginCloseInfo info)
        {
            var pending = _operations.Count(o => !o.Succeeded);
            if (pending > 0)
            {
                var answer = MessageBox.Show(
                    $"There are {pending} operation(s) not applied yet. Close the tool anyway?",
                    "Pending operations", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (answer != DialogResult.Yes)
                {
                    info.Cancel = true;
                    return;
                }
            }

            if (_settings != null)
                SettingsManager.Instance.Save(GetType(), _settings);

            base.ClosingPlugin(info);
        }

        /// <summary>
        /// This event occurs when the connection has been updated in XrmToolBox
        /// </summary>
        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);

            if (_settings == null)
                return; // control not loaded yet, Load will take care of it

            if (detail != null)
            {
                _settings.LastUsedOrganizationWebappUrl = detail.WebApplicationUrl;
                LogInfo("Connection has changed to: {0}", detail.WebApplicationUrl);
            }

            // Metadata and operations belong to the previous organization
            if (_operations.Count > 0)
                LogWarning("{0} pending operation(s) discarded because the connection changed", _operations.Count);

            ResetState();
            ExecuteMethod(GetEntities);
        }

        private void ResetState()
        {
            _operations.Clear();
            _entityCache.Clear();
            _allEntityItems = new List<ListViewItem>();
            _allFieldItems = new List<ListViewItem>();
            _selectedEntity = null;
            _selectedField = null;

            ApplyEntityFilter();
            ApplyFieldFilter();
            ShowAttributes();
            RefreshOperationsList();
        }

        /*---------------------------------------------------------------------------------   UI EVENTS   */

        private void toolStripButtonClose_Click(object sender, EventArgs e) => CloseTool();

        private void toolStripButtonLoad_Click(object sender, EventArgs e) => ExecuteMethod(GetEntities);

        private void toolStripButtonUpdate_Click(object sender, EventArgs e) => ExecuteMethod(UpdateMetadata);

        private void btnUpdateMetadata_Click(object sender, EventArgs e) => ExecuteMethod(UpdateMetadata);

        private void toolStripButtonClear_Click(object sender, EventArgs e) => ClearOperations();

        private void btnClearOperations_Click(object sender, EventArgs e) => ClearOperations();

        private void lnkSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => ToggleAllOperations(true);

        private void lnkUnselectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => ToggleAllOperations(false);

        private void toolStripButtonPublish_CheckedChanged(object sender, EventArgs e)
        {
            if (_settings != null)
                _settings.PublishAfterUpdate = toolStripButtonPublish.Checked;
        }

        private void txtFilterEntities_TextChanged(object sender, EventArgs e) => ApplyEntityFilter();

        private void txtFilterFields_TextChanged(object sender, EventArgs e) => ApplyFieldFilter();

        private void entitiesListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (entitiesListView.SelectedItems.Count == 0)
                return;

            var entityName = entitiesListView.SelectedItems[0].Text;
            if (entityName == _selectedEntity)
                return;

            _selectedEntity = entityName;
            ExecuteMethod(() => LoadFields(entityName, null));
        }

        private void fieldsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fieldsListView.SelectedItems.Count == 0)
                return;

            // Field metadata is already in the item Tag: no server round trip needed
            _selectedField = fieldsListView.SelectedItems[0].Tag as AttributeMetadata;
            ShowAttributes();
        }

        private void operationsListView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (_suppressCheckEvents)
                return;

            if (e.Item.Tag is Operation operation)
                operation.Checked = e.Item.Checked;

            UpdateButtons();
        }

        private void operationsListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && operationsListView.SelectedItems.Count > 0)
            {
                var toRemove = operationsListView.SelectedItems.Cast<ListViewItem>()
                    .Select(i => i.Tag as Operation)
                    .Where(o => o != null)
                    .ToList();

                _operations.RemoveAll(toRemove.Contains);
                RefreshOperationsList();
                ShowAttributes();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.A && e.Control)
            {
                ToggleAllOperations(true);
                e.Handled = true;
            }
        }

        private void plugin_Resize(object sender, EventArgs e)
        {
            RFUHelper.ResizeListViewColumns(entitiesListView);
            RFUHelper.ResizeListViewColumns(fieldsListView);
            RFUHelper.ResizeListViewColumns(operationsListView);
        }

        /*---------------------------------------------------------------------------------   ATTRIBUTES GRID   */

        private bool IsNewValueColumn(int columnIndex)
        {
            return columnIndex >= 0
                && attributesGridView.Columns[columnIndex].DataPropertyName == nameof(AttributeListItem.NewValue);
        }

        private void attributesGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex < 0 || !IsNewValueColumn(e.ColumnIndex) || _selectedField == null)
                return;

            var row = attributesGridView.Rows[e.RowIndex];
            var item = row.DataBoundItem as AttributeListItem;
            var text = Convert.ToString(e.FormattedValue);

            row.ErrorText = string.Empty;
            if (item == null || string.IsNullOrWhiteSpace(text))
                return;

            string error;
            if (!RFUHelper.TryConvert(text, item.ValueType, out var value, out error))
            {
                row.ErrorText = error;
                e.Cancel = true;
                return;
            }

            error = RFUHelper.Validate(_selectedField, item.AttributeName, value);
            if (error != null)
            {
                row.ErrorText = error;
                e.Cancel = true;
            }
        }

        private void attributesGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                attributesGridView.Rows[e.RowIndex].ErrorText = string.Empty;
        }

        private void attributesGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || !IsNewValueColumn(e.ColumnIndex) || _selectedField == null)
                return;

            var row = attributesGridView.Rows[e.RowIndex];
            if (!(row.DataBoundItem is AttributeListItem item))
                return;

            var text = Convert.ToString(row.Cells[e.ColumnIndex].Value)?.Trim();

            // Normalize the value to invariant culture (validated already in CellValidating)
            string newValue = null;
            if (!string.IsNullOrEmpty(text) && RFUHelper.TryConvert(text, item.ValueType, out var converted, out _))
                newValue = RFUHelper.FormatValue(converted);

            var entityName = _selectedField.EntityLogicalName;
            var fieldName = _selectedField.LogicalName;
            var existing = _operations.FirstOrDefault(o => o.Matches(entityName, fieldName, item.AttributeName));

            if (newValue == null || newValue == item.CurrentValue)
            {
                // Cleared or set back to the current value: nothing to do
                if (existing != null)
                    _operations.Remove(existing);
            }
            else if (existing != null)
            {
                existing.AttributeValue = newValue;
                existing.Checked = true;
                existing.Error = null;
                existing.Succeeded = false;
            }
            else
            {
                _operations.Add(new Operation
                {
                    Checked = true,
                    EntityName = entityName,
                    EntityField = fieldName,
                    AttributeName = item.AttributeName,
                    CurrentValue = item.CurrentValue,
                    AttributeValue = newValue
                });
            }

            RefreshOperationsList();
        }

        private void ShowAttributes()
        {
            var rows = RFUHelper.GetFieldAttributes(_selectedField, _settings?.Config_Props, _operations);

            attributesGridView.DataSource = null;
            if (_selectedField == null)
            {
                attributesBox.Text = "Attributes";
                return;
            }

            attributesGridView.DataSource = new BindingSource { DataSource = rows };
            foreach (DataGridViewColumn column in attributesGridView.Columns)
            {
                var editable = column.DataPropertyName == nameof(AttributeListItem.NewValue);
                column.ReadOnly = !editable;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                if (editable)
                    column.DefaultCellStyle.BackColor = System.Drawing.Color.LightYellow;
            }

            attributesBox.Text = $"Attributes - {_selectedField.EntityLogicalName}.{_selectedField.LogicalName}";
        }

        /*---------------------------------------------------------------------------------   LISTS   */

        private void ApplyEntityFilter()
        {
            RFUHelper.SetItems(entitiesListView, RFUHelper.Filter(_allEntityItems, txtFilterEntities.Text));
            entitiesBox.Text = $"Entities ({entitiesListView.Items.Count}/{_allEntityItems.Count})";
        }

        private void ApplyFieldFilter()
        {
            RFUHelper.SetItems(fieldsListView, RFUHelper.Filter(_allFieldItems, txtFilterFields.Text));
            fieldsBox.Text = _selectedEntity == null
                ? "Fields"
                : $"Fields - {_selectedEntity} ({fieldsListView.Items.Count}/{_allFieldItems.Count})";
        }

        private void RefreshOperationsList()
        {
            _suppressCheckEvents = true;
            try
            {
                // Newest operations first
                RFUHelper.SetItems(operationsListView, _operations.AsEnumerable().Reverse().Select(RFUHelper.ToListViewItem));
            }
            finally
            {
                _suppressCheckEvents = false;
            }

            UpdateButtons();
        }

        private void UpdateButtons()
        {
            var checkedCount = _operations.Count(o => o.Checked);
            var anyChecked = checkedCount > 0;

            btnClearOperations.Enabled = anyChecked;
            btnUpdateMetadata.Enabled = anyChecked;
            toolStripButtonClear.Enabled = anyChecked;
            toolStripButtonUpdate.Enabled = anyChecked;

            operationsBox.Text = $"Operations ({_operations.Count}, {checkedCount} selected)";
        }

        private void ToggleAllOperations(bool value)
        {
            foreach (var operation in _operations)
                operation.Checked = value;

            RefreshOperationsList();
        }

        private void ClearOperations()
        {
            if (_operations.RemoveAll(o => o.Checked) == 0)
                return;

            RefreshOperationsList();
            ShowAttributes();
        }

        /*---------------------------------------------------------------------------------   SERVER WORK   */

        private void GetEntities()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Getting entities...",
                Work = (worker, args) =>
                {
                    args.Result = RFUHelper.GetEntityItems(RFUHelper.RetrieveAllEntitiesMetadata(Service));
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        ShowError("Unable to retrieve entities", args.Error);
                        return;
                    }

                    _entityCache.Clear();
                    _allEntityItems = args.Result as List<ListViewItem> ?? new List<ListViewItem>();
                    _allFieldItems = new List<ListViewItem>();
                    _selectedEntity = null;
                    _selectedField = null;

                    ApplyEntityFilter();
                    ApplyFieldFilter();
                    ShowAttributes();
                }
            });
        }

        /// <summary>
        /// Loads the fields of an entity (from cache when available) and optionally reselects a field.
        /// </summary>
        private void LoadFields(string entityName, string fieldToSelect)
        {
            if (_entityCache.TryGetValue(entityName, out var cached))
            {
                DisplayFields(entityName, cached, fieldToSelect);
                return;
            }

            WorkAsync(new WorkAsyncInfo
            {
                Message = $"Getting fields of {entityName}...",
                Work = (worker, args) =>
                {
                    args.Result = RFUHelper.RetrieveEntityMetadata(entityName, Service);
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        if (entityName == _selectedEntity)
                            _selectedEntity = null; // allow retrying by clicking the entity again
                        ShowError($"Unable to retrieve fields of {entityName}", args.Error);
                        return;
                    }

                    var metadata = (EntityMetadata)args.Result;
                    _entityCache[entityName] = metadata;

                    // The user may have selected another entity in the meantime
                    if (entityName == _selectedEntity)
                        DisplayFields(entityName, metadata, fieldToSelect);
                }
            });
        }

        private void DisplayFields(string entityName, EntityMetadata metadata, string fieldToSelect)
        {
            _selectedEntity = entityName;
            _selectedField = null;
            _allFieldItems = RFUHelper.GetFieldItems(metadata);
            ApplyFieldFilter();

            if (fieldToSelect != null)
            {
                var item = fieldsListView.Items.Cast<ListViewItem>().FirstOrDefault(i => i.Text == fieldToSelect);
                if (item != null)
                {
                    item.Selected = true;
                    item.EnsureVisible();
                    _selectedField = item.Tag as AttributeMetadata;
                }
            }

            ShowAttributes();
        }

        private void UpdateMetadata()
        {
            // Make sure a value being edited in the grid is committed first
            attributesGridView.EndEdit();

            var toApply = _operations.Where(o => o.Checked).ToList();
            if (toApply.Count == 0)
                return;

            var publish = _settings.PublishAfterUpdate;
            var fieldCount = toApply.Select(o => o.EntityName + "." + o.EntityField).Distinct(StringComparer.OrdinalIgnoreCase).Count();
            var entityCount = toApply.Select(o => o.EntityName).Distinct(StringComparer.OrdinalIgnoreCase).Count();

            var confirm = MessageBox.Show(
                $"Apply {toApply.Count} operation(s) on {fieldCount} field(s) of {entityCount} entit{(entityCount == 1 ? "y" : "ies")}?" +
                (publish ? "\n\nThe updated entities will be published afterwards." : string.Empty),
                "Confirm update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
                return;

            var service = Service;

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Updating metadata...",
                Work = (worker, args) =>
                {
                    var updatedEntities = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                    // One retrieve + one update per field, whatever the number of properties changed on it
                    var groups = toApply
                        .GroupBy(o => new { Entity = o.EntityName.ToLowerInvariant(), Field = o.EntityField.ToLowerInvariant() })
                        .ToList();

                    for (var i = 0; i < groups.Count; i++)
                    {
                        var group = groups[i];
                        worker.ReportProgress((i * 100) / groups.Count,
                            $"Updating {group.Key.Entity}.{group.Key.Field} ({i + 1}/{groups.Count})...");

                        try
                        {
                            var field = RFUHelper.RetrieveAttributeMetadata(group.Key.Entity, group.Key.Field, service);

                            var applicable = new List<Operation>();
                            foreach (var operation in group)
                            {
                                try
                                {
                                    RFUHelper.ApplyValue(field, operation.AttributeName, operation.AttributeValue);
                                    applicable.Add(operation);
                                }
                                catch (Exception ex)
                                {
                                    operation.Error = ex.Message;
                                    operation.Succeeded = false;
                                }
                            }

                            if (applicable.Count == 0)
                                continue;

                            service.Execute(new Microsoft.Xrm.Sdk.Messages.UpdateAttributeRequest
                            {
                                Attribute = field,
                                EntityName = group.Key.Entity,
                                MergeLabels = true
                            });

                            foreach (var operation in applicable)
                            {
                                operation.Error = null;
                                operation.Succeeded = true;
                            }

                            updatedEntities.Add(group.Key.Entity);
                        }
                        catch (Exception ex)
                        {
                            foreach (var operation in group)
                            {
                                operation.Error = ex.Message;
                                operation.Succeeded = false;
                            }
                        }
                    }

                    args.Result = updatedEntities;

                    if (publish && updatedEntities.Count > 0)
                    {
                        worker.ReportProgress(100, $"Publishing {updatedEntities.Count} entit{(updatedEntities.Count == 1 ? "y" : "ies")}...");
                        RFUHelper.PublishEntities(updatedEntities, service);
                    }
                },
                ProgressChanged = (e) => SetWorkingMessage(e.UserState as string),
                PostWorkCallBack = (args) =>
                {
                    var succeeded = toApply.Where(o => o.Succeeded).ToList();
                    var failed = toApply.Where(o => !o.Succeeded).ToList();
                    var updatedEntities = toApply.Where(o => o.Succeeded)
                        .Select(o => o.EntityName)
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToList();

                    _operations.RemoveAll(o => o.Succeeded);

                    foreach (var entity in updatedEntities)
                        _entityCache.Remove(entity);

                    RefreshOperationsList();

                    // Reload the current field so the grid shows the new server values
                    if (_selectedEntity != null && updatedEntities.Contains(_selectedEntity, StringComparer.OrdinalIgnoreCase))
                        LoadFields(_selectedEntity, _selectedField?.LogicalName);
                    else
                        ShowAttributes();

                    foreach (var operation in succeeded)
                        LogInfo("Updated {0}.{1}.{2} = {3}", operation.EntityName, operation.EntityField, operation.AttributeName, operation.AttributeValue);
                    foreach (var operation in failed)
                        LogError("Failed {0}.{1}.{2}: {3}", operation.EntityName, operation.EntityField, operation.AttributeName, operation.Error);

                    var summary = $"{succeeded.Count} operation(s) applied successfully.";
                    if (failed.Count > 0)
                        summary += $"\n{failed.Count} operation(s) failed and are still listed (hover them to see the error).";

                    if (args.Error != null)
                    {
                        ShowError(summary + "\n\nPublishing failed", args.Error);
                    }
                    else if (failed.Count > 0)
                    {
                        MessageBox.Show(summary, "Update completed with errors", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show(summary + (publish && updatedEntities.Count > 0 ? "\nEntities published." : string.Empty),
                            "Update completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            });
        }

        private void ShowError(string message, Exception error)
        {
            LogError("{0}: {1}", message, error);
            MessageBox.Show($"{message}:\n\n{error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
