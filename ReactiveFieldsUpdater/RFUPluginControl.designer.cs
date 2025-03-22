using System.Windows.Forms;

namespace ReactiveFieldsUpdater
{
    partial class RFUPluginControl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RFUPluginControl));
            this.attributesGridView = new System.Windows.Forms.DataGridView();
            this.entitiesListView = new System.Windows.Forms.ListView();
            this.fieldsListView = new System.Windows.Forms.ListView();
            this.btnUpdateMetadata = new System.Windows.Forms.Button();
            this.btnClearOperations = new System.Windows.Forms.Button();
            this.operationsListView = new System.Windows.Forms.ListView();
            this.entitiesBox = new System.Windows.Forms.GroupBox();
            this.fieldsBox = new System.Windows.Forms.GroupBox();
            this.attributesBox = new System.Windows.Forms.GroupBox();
            this.operationsBox = new System.Windows.Forms.GroupBox();
            this.lnkUnselectAll = new System.Windows.Forms.LinkLabel();
            this.lnkSelectAll = new System.Windows.Forms.LinkLabel();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonClose = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonLoad = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonUpdate = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel_1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel_2 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.attributesGridView)).BeginInit();
            this.entitiesBox.SuspendLayout();
            this.fieldsBox.SuspendLayout();
            this.attributesBox.SuspendLayout();
            this.operationsBox.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.tableLayoutPanel_1.SuspendLayout();
            this.tableLayoutPanel_2.SuspendLayout();
            this.SuspendLayout();
            // 
            // attributesGridView
            // 
            this.attributesGridView.AllowUserToAddRows = false;
            this.attributesGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.attributesGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.attributesGridView.Location = new System.Drawing.Point(5, 28);
            this.attributesGridView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.attributesGridView.Name = "attributesGridView";
            this.attributesGridView.RowHeadersWidth = 51;
            this.attributesGridView.RowTemplate.Height = 24;
            this.attributesGridView.Size = new System.Drawing.Size(618, 329);
            this.attributesGridView.TabIndex = 6;
            this.attributesGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.SetNewOperation);
            // 
            // entitiesListView
            // 
            this.entitiesListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.entitiesListView.HideSelection = false;
            this.entitiesListView.Location = new System.Drawing.Point(13, 28);
            this.entitiesListView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.entitiesListView.Name = "entitiesListView";
            this.entitiesListView.Size = new System.Drawing.Size(619, 329);
            this.entitiesListView.TabIndex = 2;
            this.entitiesListView.UseCompatibleStateImageBehavior = false;
            this.entitiesListView.SelectedIndexChanged += new System.EventHandler(this.entitiesListView_SelectedIndexChanged);
            // 
            // fieldsListView
            // 
            this.fieldsListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fieldsListView.HideSelection = false;
            this.fieldsListView.Location = new System.Drawing.Point(5, 28);
            this.fieldsListView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.fieldsListView.Name = "fieldsListView";
            this.fieldsListView.Size = new System.Drawing.Size(626, 329);
            this.fieldsListView.TabIndex = 4;
            this.fieldsListView.UseCompatibleStateImageBehavior = false;
            this.fieldsListView.SelectedIndexChanged += new System.EventHandler(this.fieldsListView_SelectedIndexChanged);
            // 
            // btnUpdateMetadata
            // 
            this.btnUpdateMetadata.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateMetadata.Enabled = false;
            this.btnUpdateMetadata.Location = new System.Drawing.Point(1745, 13);
            this.btnUpdateMetadata.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnUpdateMetadata.Name = "btnUpdateMetadata";
            this.btnUpdateMetadata.Size = new System.Drawing.Size(165, 31);
            this.btnUpdateMetadata.TabIndex = 11;
            this.btnUpdateMetadata.Text = "Update!";
            this.btnUpdateMetadata.UseVisualStyleBackColor = true;
            this.btnUpdateMetadata.Click += new System.EventHandler(this.btnUpdateMetadata_Click);
            // 
            // btnClearOperations
            // 
            this.btnClearOperations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearOperations.Enabled = false;
            this.btnClearOperations.ForeColor = System.Drawing.Color.Red;
            this.btnClearOperations.Location = new System.Drawing.Point(1574, 13);
            this.btnClearOperations.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClearOperations.Name = "btnClearOperations";
            this.btnClearOperations.Size = new System.Drawing.Size(165, 31);
            this.btnClearOperations.TabIndex = 10;
            this.btnClearOperations.Text = "Clear";
            this.btnClearOperations.UseVisualStyleBackColor = true;
            this.btnClearOperations.Click += new System.EventHandler(this.btnClearOperations_Click);
            // 
            // operationsListView
            // 
            this.operationsListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.operationsListView.HideSelection = false;
            this.operationsListView.Location = new System.Drawing.Point(13, 49);
            this.operationsListView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.operationsListView.Name = "operationsListView";
            this.operationsListView.Size = new System.Drawing.Size(1897, 329);
            this.operationsListView.TabIndex = 12;
            this.operationsListView.UseCompatibleStateImageBehavior = false;
            this.operationsListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.operationsListView_ItemChecked);
            // 
            // entitiesBox
            // 
            this.entitiesBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.entitiesBox.Controls.Add(this.entitiesListView);
            this.entitiesBox.Location = new System.Drawing.Point(3, 2);
            this.entitiesBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.entitiesBox.Name = "entitiesBox";
            this.entitiesBox.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.entitiesBox.Size = new System.Drawing.Size(637, 364);
            this.entitiesBox.TabIndex = 1;
            this.entitiesBox.TabStop = false;
            this.entitiesBox.Text = "Entities";
            // 
            // fieldsBox
            // 
            this.fieldsBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fieldsBox.Controls.Add(this.fieldsListView);
            this.fieldsBox.Location = new System.Drawing.Point(646, 2);
            this.fieldsBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.fieldsBox.Name = "fieldsBox";
            this.fieldsBox.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.fieldsBox.Size = new System.Drawing.Size(637, 364);
            this.fieldsBox.TabIndex = 3;
            this.fieldsBox.TabStop = false;
            this.fieldsBox.Text = "Fields";
            // 
            // attributesBox
            // 
            this.attributesBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.attributesBox.Controls.Add(this.attributesGridView);
            this.attributesBox.Location = new System.Drawing.Point(1289, 2);
            this.attributesBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.attributesBox.Name = "attributesBox";
            this.attributesBox.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.attributesBox.Size = new System.Drawing.Size(638, 364);
            this.attributesBox.TabIndex = 5;
            this.attributesBox.TabStop = false;
            this.attributesBox.Text = "Attributes";
            // 
            // operationsBox
            // 
            this.operationsBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.operationsBox.Controls.Add(this.lnkUnselectAll);
            this.operationsBox.Controls.Add(this.lnkSelectAll);
            this.operationsBox.Controls.Add(this.btnClearOperations);
            this.operationsBox.Controls.Add(this.operationsListView);
            this.operationsBox.Controls.Add(this.btnUpdateMetadata);
            this.operationsBox.Location = new System.Drawing.Point(3, 2);
            this.operationsBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.operationsBox.Name = "operationsBox";
            this.operationsBox.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.operationsBox.Size = new System.Drawing.Size(1924, 530);
            this.operationsBox.TabIndex = 7;
            this.operationsBox.TabStop = false;
            this.operationsBox.Text = "Operations";
            // 
            // lnkUnselectAll
            // 
            this.lnkUnselectAll.AutoSize = true;
            this.lnkUnselectAll.Location = new System.Drawing.Point(83, 26);
            this.lnkUnselectAll.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lnkUnselectAll.Name = "lnkUnselectAll";
            this.lnkUnselectAll.Size = new System.Drawing.Size(78, 16);
            this.lnkUnselectAll.TabIndex = 9;
            this.lnkUnselectAll.TabStop = true;
            this.lnkUnselectAll.Text = "Unselect All";
            this.lnkUnselectAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkUnselectAll_LinkClicked);
            // 
            // lnkSelectAll
            // 
            this.lnkSelectAll.AutoSize = true;
            this.lnkSelectAll.Location = new System.Drawing.Point(16, 26);
            this.lnkSelectAll.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lnkSelectAll.Name = "lnkSelectAll";
            this.lnkSelectAll.Size = new System.Drawing.Size(60, 16);
            this.lnkSelectAll.TabIndex = 8;
            this.lnkSelectAll.TabStop = true;
            this.lnkSelectAll.Text = "SelectAll";
            this.lnkSelectAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSelectAll_LinkClicked);
            // 
            // toolStrip
            // 
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonClose,
            this.toolStripSeparator,
            this.toolStripButtonLoad,
            this.toolStripButtonClear,
            this.toolStripButtonUpdate});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1930, 27);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip";
            // 
            // toolStripButtonClose
            // 
            this.toolStripButtonClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonClose.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonClose.Image")));
            this.toolStripButtonClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonClose.Name = "toolStripButtonClose";
            this.toolStripButtonClose.Size = new System.Drawing.Size(29, 24);
            this.toolStripButtonClose.Text = "Close this tool";
            this.toolStripButtonClose.Click += new System.EventHandler(this.toolStripButtonClose_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripButtonLoad
            // 
            this.toolStripButtonLoad.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLoad.Image")));
            this.toolStripButtonLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLoad.Name = "toolStripButtonLoad";
            this.toolStripButtonLoad.Size = new System.Drawing.Size(108, 24);
            this.toolStripButtonLoad.Text = "Get Entities";
            this.toolStripButtonLoad.Click += new System.EventHandler(this.toolStripButtonLoad_Click);
            // 
            // toolStripButtonClear
            // 
            this.toolStripButtonClear.Enabled = false;
            this.toolStripButtonClear.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonClear.Image")));
            this.toolStripButtonClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonClear.Name = "toolStripButtonClear";
            this.toolStripButtonClear.Size = new System.Drawing.Size(67, 24);
            this.toolStripButtonClear.Text = "Clear";
            this.toolStripButtonClear.Click += new System.EventHandler(this.toolStripButtonClear_Click);
            // 
            // toolStripButtonUpdate
            // 
            this.toolStripButtonUpdate.Enabled = false;
            this.toolStripButtonUpdate.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonUpdate.Image")));
            this.toolStripButtonUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonUpdate.Name = "toolStripButtonUpdate";
            this.toolStripButtonUpdate.Size = new System.Drawing.Size(86, 24);
            this.toolStripButtonUpdate.Text = "Update!";
            this.toolStripButtonUpdate.Click += new System.EventHandler(this.toolStripButtonUpdate_Click);
            // 
            // tableLayoutPanel_1
            // 
            this.tableLayoutPanel_1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel_1.ColumnCount = 3;
            this.tableLayoutPanel_1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel_1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel_1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel_1.Controls.Add(this.attributesBox, 2, 0);
            this.tableLayoutPanel_1.Controls.Add(this.fieldsBox, 1, 0);
            this.tableLayoutPanel_1.Controls.Add(this.entitiesBox, 0, 0);
            this.tableLayoutPanel_1.Location = new System.Drawing.Point(0, 31);
            this.tableLayoutPanel_1.Name = "tableLayoutPanel_1";
            this.tableLayoutPanel_1.RowCount = 1;
            this.tableLayoutPanel_1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_1.Size = new System.Drawing.Size(1930, 375);
            this.tableLayoutPanel_1.TabIndex = 999;
            // 
            // tableLayoutPanel_2
            // 
            this.tableLayoutPanel_2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel_2.ColumnCount = 1;
            this.tableLayoutPanel_2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_2.Controls.Add(this.operationsBox, 0, 0);
            this.tableLayoutPanel_2.Location = new System.Drawing.Point(0, 413);
            this.tableLayoutPanel_2.Name = "tableLayoutPanel_2";
            this.tableLayoutPanel_2.RowCount = 1;
            this.tableLayoutPanel_2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_2.Size = new System.Drawing.Size(1930, 534);
            this.tableLayoutPanel_2.TabIndex = 999;
            // 
            // RFUPluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel_1);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.tableLayoutPanel_2);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimumSize = new System.Drawing.Size(1930, 0);
            this.Name = "RFUPluginControl";
            this.Size = new System.Drawing.Size(1930, 950);
            this.Load += new System.EventHandler(this.MyPluginControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.attributesGridView)).EndInit();
            this.entitiesBox.ResumeLayout(false);
            this.fieldsBox.ResumeLayout(false);
            this.attributesBox.ResumeLayout(false);
            this.operationsBox.ResumeLayout(false);
            this.operationsBox.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.tableLayoutPanel_1.ResumeLayout(false);
            this.tableLayoutPanel_2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ListView entitiesListView;
        private ListView fieldsListView;
        private DataGridView attributesGridView;
        private ListView operationsListView;
        private Button btnUpdateMetadata;
        private Button btnClearOperations;
        private GroupBox entitiesBox;
        private GroupBox fieldsBox;
        private GroupBox attributesBox;
        private GroupBox operationsBox;
        private ToolStrip toolStrip;
        private ToolStripButton toolStripButtonClose;
        private ToolStripButton toolStripButtonLoad;
        private ToolStripButton toolStripButtonUpdate;
        private ToolStripButton toolStripButtonClear;
        private ToolStripSeparator toolStripSeparator;
        private LinkLabel lnkUnselectAll;
        private LinkLabel lnkSelectAll;
        private TableLayoutPanel tableLayoutPanel_1;
        private TableLayoutPanel tableLayoutPanel_2;
    }
}
