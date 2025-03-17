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
            this.btnGetEntities = new System.Windows.Forms.Button();
            this.entitiesListView = new System.Windows.Forms.ListView();
            this.fieldsListView = new System.Windows.Forms.ListView();
            this.btnUpdateMetadata = new System.Windows.Forms.Button();
            this.btnClearOperations = new System.Windows.Forms.Button();
            this.operationsListView = new System.Windows.Forms.ListView();
            this.entitiesBox = new System.Windows.Forms.GroupBox();
            this.fieldsBox = new System.Windows.Forms.GroupBox();
            this.attributesBox = new System.Windows.Forms.GroupBox();
            this.operationsBox = new System.Windows.Forms.GroupBox();
            this.buttonsBox = new System.Windows.Forms.GroupBox();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonClose = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonLoad = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonUpdate = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonClear = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.attributesGridView)).BeginInit();
            this.entitiesBox.SuspendLayout();
            this.fieldsBox.SuspendLayout();
            this.attributesBox.SuspendLayout();
            this.operationsBox.SuspendLayout();
            this.buttonsBox.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // attributesGridView
            // 
            this.attributesGridView.AllowUserToAddRows = false;
            this.attributesGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.attributesGridView.Location = new System.Drawing.Point(4, 23);
            this.attributesGridView.Margin = new System.Windows.Forms.Padding(2);
            this.attributesGridView.Name = "attributesGridView";
            this.attributesGridView.RowHeadersWidth = 51;
            this.attributesGridView.RowTemplate.Height = 24;
            this.attributesGridView.Size = new System.Drawing.Size(400, 267);
            this.attributesGridView.TabIndex = 6;
            this.attributesGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.SetNewOperation);
            // 
            // btnGetEntities
            // 
            this.btnGetEntities.Location = new System.Drawing.Point(4, 25);
            this.btnGetEntities.Margin = new System.Windows.Forms.Padding(2);
            this.btnGetEntities.Name = "btnGetEntities";
            this.btnGetEntities.Size = new System.Drawing.Size(499, 51);
            this.btnGetEntities.TabIndex = 8;
            this.btnGetEntities.Text = "GET ENTITIES";
            this.btnGetEntities.UseVisualStyleBackColor = true;
            this.btnGetEntities.Click += new System.EventHandler(this.btnGetEntities_Click);
            // 
            // entitiesListView
            // 
            this.entitiesListView.HideSelection = false;
            this.entitiesListView.Location = new System.Drawing.Point(4, 23);
            this.entitiesListView.Margin = new System.Windows.Forms.Padding(2);
            this.entitiesListView.Name = "entitiesListView";
            this.entitiesListView.Size = new System.Drawing.Size(500, 268);
            this.entitiesListView.TabIndex = 2;
            this.entitiesListView.UseCompatibleStateImageBehavior = false;
            this.entitiesListView.SelectedIndexChanged += new System.EventHandler(this.entitiesListView_SelectedIndexChanged);
            // 
            // fieldsListView
            // 
            this.fieldsListView.HideSelection = false;
            this.fieldsListView.Location = new System.Drawing.Point(4, 23);
            this.fieldsListView.Margin = new System.Windows.Forms.Padding(2);
            this.fieldsListView.Name = "fieldsListView";
            this.fieldsListView.Size = new System.Drawing.Size(500, 268);
            this.fieldsListView.TabIndex = 4;
            this.fieldsListView.UseCompatibleStateImageBehavior = false;
            this.fieldsListView.SelectedIndexChanged += new System.EventHandler(this.fieldsListView_SelectedIndexChanged);
            // 
            // btnUpdateMetadata
            // 
            this.btnUpdateMetadata.Location = new System.Drawing.Point(4, 89);
            this.btnUpdateMetadata.Margin = new System.Windows.Forms.Padding(2);
            this.btnUpdateMetadata.Name = "btnUpdateMetadata";
            this.btnUpdateMetadata.Size = new System.Drawing.Size(499, 51);
            this.btnUpdateMetadata.TabIndex = 9;
            this.btnUpdateMetadata.Text = "UPDATE !";
            this.btnUpdateMetadata.UseVisualStyleBackColor = true;
            this.btnUpdateMetadata.Click += new System.EventHandler(this.btnUpdateMetadata_Click);
            // 
            // btnClearOperations
            // 
            this.btnClearOperations.ForeColor = System.Drawing.Color.Red;
            this.btnClearOperations.Location = new System.Drawing.Point(4, 236);
            this.btnClearOperations.Margin = new System.Windows.Forms.Padding(2);
            this.btnClearOperations.Name = "btnClearOperations";
            this.btnClearOperations.Size = new System.Drawing.Size(499, 51);
            this.btnClearOperations.TabIndex = 10;
            this.btnClearOperations.Text = "CLEAR";
            this.btnClearOperations.UseVisualStyleBackColor = true;
            this.btnClearOperations.Click += new System.EventHandler(this.btnClearOperations_Click);
            // 
            // operationsListView
            // 
            this.operationsListView.HideSelection = false;
            this.operationsListView.Location = new System.Drawing.Point(4, 25);
            this.operationsListView.Margin = new System.Windows.Forms.Padding(2);
            this.operationsListView.Name = "operationsListView";
            this.operationsListView.Size = new System.Drawing.Size(913, 262);
            this.operationsListView.TabIndex = 12;
            this.operationsListView.UseCompatibleStateImageBehavior = false;
            this.operationsListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.operationsListView_ItemChecked);
            // 
            // entitiesBox
            // 
            this.entitiesBox.Controls.Add(this.entitiesListView);
            this.entitiesBox.Location = new System.Drawing.Point(8, 33);
            this.entitiesBox.Margin = new System.Windows.Forms.Padding(2);
            this.entitiesBox.Name = "entitiesBox";
            this.entitiesBox.Padding = new System.Windows.Forms.Padding(2);
            this.entitiesBox.Size = new System.Drawing.Size(508, 297);
            this.entitiesBox.TabIndex = 1;
            this.entitiesBox.TabStop = false;
            this.entitiesBox.Text = "Entities";
            // 
            // fieldsBox
            // 
            this.fieldsBox.Controls.Add(this.fieldsListView);
            this.fieldsBox.Location = new System.Drawing.Point(520, 33);
            this.fieldsBox.Margin = new System.Windows.Forms.Padding(2);
            this.fieldsBox.Name = "fieldsBox";
            this.fieldsBox.Padding = new System.Windows.Forms.Padding(2);
            this.fieldsBox.Size = new System.Drawing.Size(508, 296);
            this.fieldsBox.TabIndex = 3;
            this.fieldsBox.TabStop = false;
            this.fieldsBox.Text = "Fields";
            // 
            // attributesBox
            // 
            this.attributesBox.Controls.Add(this.attributesGridView);
            this.attributesBox.Location = new System.Drawing.Point(1032, 33);
            this.attributesBox.Margin = new System.Windows.Forms.Padding(2);
            this.attributesBox.Name = "attributesBox";
            this.attributesBox.Padding = new System.Windows.Forms.Padding(2);
            this.attributesBox.Size = new System.Drawing.Size(409, 296);
            this.attributesBox.TabIndex = 5;
            this.attributesBox.TabStop = false;
            this.attributesBox.Text = "Attributes";
            // 
            // operationsBox
            // 
            this.operationsBox.Controls.Add(this.operationsListView);
            this.operationsBox.Location = new System.Drawing.Point(520, 334);
            this.operationsBox.Margin = new System.Windows.Forms.Padding(2);
            this.operationsBox.Name = "operationsBox";
            this.operationsBox.Padding = new System.Windows.Forms.Padding(2);
            this.operationsBox.Size = new System.Drawing.Size(921, 293);
            this.operationsBox.TabIndex = 11;
            this.operationsBox.TabStop = false;
            this.operationsBox.Text = "Operations";
            // 
            // buttonsBox
            // 
            this.buttonsBox.Controls.Add(this.btnClearOperations);
            this.buttonsBox.Controls.Add(this.btnUpdateMetadata);
            this.buttonsBox.Controls.Add(this.btnGetEntities);
            this.buttonsBox.Location = new System.Drawing.Point(8, 334);
            this.buttonsBox.Margin = new System.Windows.Forms.Padding(2);
            this.buttonsBox.Name = "buttonsBox";
            this.buttonsBox.Padding = new System.Windows.Forms.Padding(2);
            this.buttonsBox.Size = new System.Drawing.Size(508, 293);
            this.buttonsBox.TabIndex = 7;
            this.buttonsBox.TabStop = false;
            // 
            // toolStrip
            // 
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonClose,
            this.toolStripSeparator,
            this.toolStripButtonLoad,
            this.toolStripButtonUpdate,
            this.toolStripButtonClear});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1448, 27);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip";
            // 
            // toolStripButtonClose
            // 
            this.toolStripButtonClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonClose.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonClose.Image")));
            this.toolStripButtonClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonClose.Name = "toolStripButtonClose";
            this.toolStripButtonClose.Size = new System.Drawing.Size(24, 24);
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
            this.toolStripButtonLoad.Size = new System.Drawing.Size(90, 24);
            this.toolStripButtonLoad.Text = "Get Entities";
            this.toolStripButtonLoad.Click += new System.EventHandler(this.toolStripButtonLoad_Click);
            // 
            // toolStripButtonUpdate
            // 
            this.toolStripButtonUpdate.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonUpdate.Image")));
            this.toolStripButtonUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonUpdate.Name = "toolStripButtonUpdate";
            this.toolStripButtonUpdate.Size = new System.Drawing.Size(72, 24);
            this.toolStripButtonUpdate.Text = "Update!";
            this.toolStripButtonUpdate.Click += new System.EventHandler(this.toolStripButtonUpdate_Click);
            // 
            // toolStripButtonClear
            // 
            this.toolStripButtonClear.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonClear.Image")));
            this.toolStripButtonClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonClear.Name = "toolStripButtonClear";
            this.toolStripButtonClear.Size = new System.Drawing.Size(58, 24);
            this.toolStripButtonClear.Text = "Clear";
            this.toolStripButtonClear.Click += new System.EventHandler(this.toolStripButtonClear_Click);
            // 
            // RFUPluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.attributesBox);
            this.Controls.Add(this.fieldsBox);
            this.Controls.Add(this.entitiesBox);
            this.Controls.Add(this.operationsBox);
            this.Controls.Add(this.buttonsBox);
            this.Name = "RFUPluginControl";
            this.Size = new System.Drawing.Size(1448, 776);
            this.Load += new System.EventHandler(this.MyPluginControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.attributesGridView)).EndInit();
            this.entitiesBox.ResumeLayout(false);
            this.fieldsBox.ResumeLayout(false);
            this.attributesBox.ResumeLayout(false);
            this.operationsBox.ResumeLayout(false);
            this.buttonsBox.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ListView entitiesListView;
        private ListView fieldsListView;
        private DataGridView attributesGridView;
        private ListView operationsListView;
        private Button btnGetEntities;
        private Button btnUpdateMetadata;
        private Button btnClearOperations;
        private GroupBox entitiesBox;
        private GroupBox fieldsBox;
        private GroupBox attributesBox;
        private GroupBox operationsBox;
        private GroupBox buttonsBox;
        private ToolStrip toolStrip;
        private ToolStripButton toolStripButtonClose;
        private ToolStripButton toolStripButtonLoad;
        private ToolStripButton toolStripButtonUpdate;
        private ToolStripButton toolStripButtonClear;
        private ToolStripSeparator toolStripSeparator;
    }
}
