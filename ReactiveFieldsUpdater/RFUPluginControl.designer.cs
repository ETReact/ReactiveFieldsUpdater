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
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tssSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSample = new System.Windows.Forms.ToolStripButton();
            this.attributesGridView = new System.Windows.Forms.DataGridView();
            this.btnGetEntities = new System.Windows.Forms.Button();
            this.entitiesListView = new System.Windows.Forms.ListView();
            this.fieldsListView = new System.Windows.Forms.ListView();
            this.btnUpdateMetadata = new System.Windows.Forms.Button();
            this.toolStripMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.attributesGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.tssSeparator1,
            this.tsbSample});
            this.toolStripMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStripMenu.Size = new System.Drawing.Size(1448, 25);
            this.toolStripMenu.TabIndex = 1;
            this.toolStripMenu.Text = "toolStrip1";
            // 
            // tsbClose
            // 
            this.tsbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(86, 22);
            this.tsbClose.Text = "Close this tool";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // tssSeparator1
            // 
            this.tssSeparator1.Name = "tssSeparator1";
            this.tssSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbSample
            // 
            this.tsbSample.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbSample.Name = "tsbSample";
            this.tsbSample.Size = new System.Drawing.Size(46, 22);
            this.tsbSample.Text = "Try me";
            // 
            // attributesGridView
            // 
            this.attributesGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.attributesGridView.Location = new System.Drawing.Point(561, 353);
            this.attributesGridView.Margin = new System.Windows.Forms.Padding(2);
            this.attributesGridView.Name = "attributesGridView";
            this.attributesGridView.RowHeadersWidth = 51;
            this.attributesGridView.RowTemplate.Height = 24;
            this.attributesGridView.Size = new System.Drawing.Size(750, 249);
            this.attributesGridView.TabIndex = 6;
            this.attributesGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.SetNewAttributeValue);
            // 
            // btnGetEntities
            // 
            this.btnGetEntities.Location = new System.Drawing.Point(28, 353);
            this.btnGetEntities.Margin = new System.Windows.Forms.Padding(2);
            this.btnGetEntities.Name = "btnGetEntities";
            this.btnGetEntities.Size = new System.Drawing.Size(500, 51);
            this.btnGetEntities.TabIndex = 4;
            this.btnGetEntities.Text = "GET ENTITIES";
            this.btnGetEntities.UseVisualStyleBackColor = true;
            this.btnGetEntities.Click += new System.EventHandler(this.btnGetEntities_Click);
            // 
            // entitiesListView
            // 
            this.entitiesListView.HideSelection = false;
            this.entitiesListView.Location = new System.Drawing.Point(28, 46);
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
            this.fieldsListView.Location = new System.Drawing.Point(561, 46);
            this.fieldsListView.Margin = new System.Windows.Forms.Padding(2);
            this.fieldsListView.Name = "fieldsListView";
            this.fieldsListView.Size = new System.Drawing.Size(750, 268);
            this.fieldsListView.TabIndex = 3;
            this.fieldsListView.UseCompatibleStateImageBehavior = false;
            this.fieldsListView.SelectedIndexChanged += new System.EventHandler(this.fieldsListView_SelectedIndexChanged);
            // 
            // btnUpdateMetadata
            // 
            this.btnUpdateMetadata.Location = new System.Drawing.Point(28, 426);
            this.btnUpdateMetadata.Margin = new System.Windows.Forms.Padding(2);
            this.btnUpdateMetadata.Name = "btnUpdateMetadata";
            this.btnUpdateMetadata.Size = new System.Drawing.Size(500, 51);
            this.btnUpdateMetadata.TabIndex = 5;
            this.btnUpdateMetadata.Text = "UPDATE !";
            this.btnUpdateMetadata.UseVisualStyleBackColor = true;
            this.btnUpdateMetadata.Click += new System.EventHandler(this.btnUpdateMetadata_Click);
            // 
            // RFUPluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.entitiesListView);
            this.Controls.Add(this.fieldsListView);
            this.Controls.Add(this.btnGetEntities);
            this.Controls.Add(this.btnUpdateMetadata);
            this.Controls.Add(this.attributesGridView);
            this.Controls.Add(this.toolStripMenu);
            this.Name = "RFUPluginControl";
            this.Size = new System.Drawing.Size(1448, 776);
            this.Load += new System.EventHandler(this.MyPluginControl_Load);
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.attributesGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        private ToolStrip toolStripMenu;
        private ToolStripButton tsbClose;
        private ToolStripButton tsbSample;
        private ToolStripSeparator tssSeparator1;
        private DataGridView attributesGridView;
        private Button btnGetEntities;
        private ListView entitiesListView;
        private ListView fieldsListView;
        private Button btnUpdateMetadata;
    }
}
