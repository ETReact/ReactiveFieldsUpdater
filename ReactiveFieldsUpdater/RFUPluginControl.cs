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

            entitiesListView.Columns.Add("Logical Name", 250, HorizontalAlignment.Left);
            entitiesListView.Columns.Add("Schema Name", 250, HorizontalAlignment.Left);

            fieldsListView.Columns.Add("Field Logical Name", 250, HorizontalAlignment.Left);
            fieldsListView.Columns.Add("Field Schema Name", 250, HorizontalAlignment.Left);
            fieldsListView.Columns.Add("Field Type", 250, HorizontalAlignment.Left);
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
            ListView.SelectedListViewItemCollection selectedItems = entitiesListView.SelectedItems;
            string value = String.Empty;
            foreach (ListViewItem item in selectedItems)
            {
                value = item.Text;
            }
            entityDummyLabel.Text = value;
            if (!String.IsNullOrEmpty(value))
                ExecuteMethod(GetFields);
        }

        private void fieldsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection selectedItems = fieldsListView.SelectedItems;
            string value = String.Empty;
            foreach (ListViewItem item in selectedItems)
            {
                value = item.Text;
            }
            fieldDummyLabel.Text = value;
            if (!String.IsNullOrEmpty(value))
                ExecuteMethod(GetAttributes);
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
                    }

                    var result = args.Result as List<ListViewItem>;
                    if (result != null)
                    {
                        try
                        {
                            entitiesListView.Items.Clear();
                            RFUHelper.UpdateListView(entitiesListView, result);
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
            });
        }

        private void GetFields()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Getting fields...",
                Work = (worker, args) =>
                {
                    args.Result = RFUHelper.GetEntityFields(entityDummyLabel.Text, Service);
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    var result = args.Result as List<ListViewItem>;
                    if (result != null)
                    {
                        try
                        {
                            fieldsListView.Items.Clear();
                            RFUHelper.UpdateListView(fieldsListView, result);
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
            });
        }

        private void GetAttributes()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Getting attributes...",
                Work = (worker, args) =>
                {
                    args.Result = RFUHelper.GetFieldAttributes(entityDummyLabel.Text, fieldDummyLabel.Text, Service);
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    var result = args.Result as List<object>;
                    if (result != null)
                    {
                        try
                        {
                            RFUHelper.UpdateDataGridView(attributesGridView, result);
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
            });
        }

        private void SetNewAttributeValue(object sender, EventArgs e)
        {
            WorkAsync(new WorkAsyncInfo
            {
                Work = (worker, args) =>
                {
                    DataGridViewCellEventArgs handler = e as DataGridViewCellEventArgs;
                    RFUHelper.SetNewAttributeValue(attributesGridView, handler, Service);
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    try
                    {
                        attributesGridView.Refresh();
                    }
                    catch (Exception)
                    {
                        throw;
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
                    RFUHelper.UpdateMetadata(entityDummyLabel.Text, fieldDummyLabel.Text, Service);
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    try
                    {
                        attributesGridView.Refresh();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            });
        }


    }
}