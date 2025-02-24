using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Windows.Documents;
using System.Windows.Forms;
using XrmToolBox.Extensibility;

namespace ReactiveFieldsUpdater
{
    public partial class RFUPluginControl : PluginControlBase
    {
        private Settings mySettings;

        private string _selectedEntity = string.Empty;
        private string _selectedField = string.Empty;

        public RFUPluginControl()
        {
            InitializeComponent();
        }

        private void MyPluginControl_Load(object sender, EventArgs e)
        {
            ShowInfoNotification("Learn more --->", new Uri("https://www.activadigital.it/"));

            // Loads or creates the settings for the plugin
            if (!SettingsManager.Instance.TryLoad(GetType(), out mySettings))
            {
                mySettings = new Settings();

                LogWarning("Settings not found => a new settings file has been created!");
            }
            else
            {
                LogInfo("Settings found and loaded");
            }
        }

        /// <summary>
        /// This event occurs when the plugin is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyPluginControl_OnCloseTool(object sender, EventArgs e)
        {
            // Before leaving, save the settings
            SettingsManager.Instance.Save(GetType(), mySettings);
        }

        /// <summary>
        /// This event occurs when the connection has been updated in XrmToolBox
        /// </summary>
        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);

            if (mySettings != null && detail != null)
            {
                mySettings.LastUsedOrganizationWebappUrl = detail.WebApplicationUrl;
                LogInfo("Connection has changed to: {0}", detail.WebApplicationUrl);
            }
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }

        private void btnGetEntities_Click(object sender, EventArgs e)
        {
            ExecuteMethod(GetEntities);
        }

        private void btnUpdateMetadata_Click(object sender, EventArgs e)
        {
            ExecuteMethod(UpdateMetadata);
        }

        private void entitiesListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (entitiesListView.SelectedItems.Count == 0)
                return;

            _selectedEntity = entitiesListView.SelectedItems[0].Text;
            ExecuteMethod(() => GetFields(_selectedEntity));
        }

        private void fieldsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fieldsListView.SelectedItems.Count == 0)
                return;

            _selectedField = fieldsListView.SelectedItems[0].Text;
            ExecuteMethod(() => GetAttributes(_selectedEntity, _selectedField));
        }

        private void GetEntities()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Getting entities...",
                Work = (worker, args) =>
                {
                    var allEntities = RFUHelper.RetrieveAllEntitiesMetadata(Service);
                    args.Result = RFUHelper.GetMetadataLabels(allEntities);
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var result = args.Result as List<ListViewItem>;
                    if (result != null)
                    {
                        RFUHelper.UpdateEntityListView(entitiesListView, result);
                    }
                }
            });
        }

        private void GetFields(string selectedEntity)
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Getting fields...",
                Work = (worker, args) =>
                {
                    args.Result = RFUHelper.GetEntityFields(selectedEntity, Service);
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var result = args.Result as List<ListViewItem>;
                    if (result != null)
                    {
                        RFUHelper.UpdateFieldListView(fieldsListView, result);
                    }
                }
            });
        }

        private void GetAttributes(string selectedEntity, string selectedField)
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Getting attributes...",
                Work = (worker, args) =>
                {
                    args.Result = RFUHelper.GetFieldAttributes(selectedEntity, selectedField, Service);
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (args.Result is List<FieldAttribute> result)
                    {
                        attributesGridView.BeginInvoke(new Action(() =>
                        {
                            RFUHelper.UpdateDataGridView(attributesGridView, result);
                        }));
                    }
                }
            });
        }

        private void SetNewAttributeValue(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            WorkAsync(new WorkAsyncInfo
            {
                Work = (worker, args) =>
                {
                    RFUHelper.SetNewAttributeValue(attributesGridView, e.RowIndex, e.ColumnIndex, Service);
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            });
        }

        private void UpdateMetadata()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Updating...",
                Work = (worker, args) =>
                {
                    RFUHelper.UpdateMetadata(_selectedEntity, _selectedField, Service);
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            });
        }
    }
}