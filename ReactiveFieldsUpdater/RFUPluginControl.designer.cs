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
            this.attributesGridView = new System.Windows.Forms.DataGridView();
            this.btnGetEntities = new System.Windows.Forms.Button();
            this.entitiesListView = new System.Windows.Forms.ListView();
            this.fieldsListView = new System.Windows.Forms.ListView();
            this.btnUpdateMetadata = new System.Windows.Forms.Button();
            this.btnClearOperations = new System.Windows.Forms.Button();
            this.operationsListView = new System.Windows.Forms.ListView();
            this.lblEntities = new System.Windows.Forms.Label();
            this.lblFields = new System.Windows.Forms.Label();
            this.lblAttributes = new System.Windows.Forms.Label();
            this.lblOperations = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.attributesGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // attributesGridView
            // 
            this.attributesGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.attributesGridView.Location = new System.Drawing.Point(1454, 45);
            this.attributesGridView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.attributesGridView.Name = "attributesGridView";
            this.attributesGridView.RowHeadersWidth = 51;
            this.attributesGridView.RowTemplate.Height = 24;
            this.attributesGridView.Size = new System.Drawing.Size(426, 329);
            this.attributesGridView.TabIndex = 6;
            this.attributesGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.SetNewOperation);
            this.attributesGridView.AllowUserToAddRows = false;

            // 
            // btnGetEntities
            // 
            this.btnGetEntities.Location = new System.Drawing.Point(29, 439);
            this.btnGetEntities.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnGetEntities.Name = "btnGetEntities";
            this.btnGetEntities.Size = new System.Drawing.Size(667, 63);
            this.btnGetEntities.TabIndex = 4;
            this.btnGetEntities.Text = "GET ENTITIES";
            this.btnGetEntities.UseVisualStyleBackColor = true;
            this.btnGetEntities.Click += new System.EventHandler(this.btnGetEntities_Click);
            // 
            // entitiesListView
            // 
            this.entitiesListView.HideSelection = false;
            this.entitiesListView.Location = new System.Drawing.Point(29, 45);
            this.entitiesListView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.entitiesListView.Name = "entitiesListView";
            this.entitiesListView.Size = new System.Drawing.Size(665, 329);
            this.entitiesListView.TabIndex = 2;
            this.entitiesListView.UseCompatibleStateImageBehavior = false;
            this.entitiesListView.SelectedIndexChanged += new System.EventHandler(this.entitiesListView_SelectedIndexChanged);
            // 
            // fieldsListView
            // 
            this.fieldsListView.HideSelection = false;
            this.fieldsListView.Location = new System.Drawing.Point(740, 45);
            this.fieldsListView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.fieldsListView.Name = "fieldsListView";
            this.fieldsListView.Size = new System.Drawing.Size(665, 329);
            this.fieldsListView.TabIndex = 3;
            this.fieldsListView.UseCompatibleStateImageBehavior = false;
            this.fieldsListView.SelectedIndexChanged += new System.EventHandler(this.fieldsListView_SelectedIndexChanged);
            // 
            // btnUpdateMetadata
            // 
            this.btnUpdateMetadata.Location = new System.Drawing.Point(29, 529);
            this.btnUpdateMetadata.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnUpdateMetadata.Name = "btnUpdateMetadata";
            this.btnUpdateMetadata.Size = new System.Drawing.Size(667, 63);
            this.btnUpdateMetadata.TabIndex = 5;
            this.btnUpdateMetadata.Text = "UPDATE !";
            this.btnUpdateMetadata.UseVisualStyleBackColor = true;
            this.btnUpdateMetadata.Click += new System.EventHandler(this.btnUpdateMetadata_Click);
            // 
            // btnClearOperations
            // 
            this.btnClearOperations.ForeColor = System.Drawing.Color.Red;
            this.btnClearOperations.Location = new System.Drawing.Point(29, 619);
            this.btnClearOperations.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClearOperations.Name = "btnClearOperations";
            this.btnClearOperations.Size = new System.Drawing.Size(667, 63);
            this.btnClearOperations.TabIndex = 5;
            this.btnClearOperations.Text = "CLEAR";
            this.btnClearOperations.UseVisualStyleBackColor = true;
            this.btnClearOperations.Click += new System.EventHandler(this.btnClearOperations_Click);
            // 
            // operationsListView
            // 
            this.operationsListView.HideSelection = false;
            this.operationsListView.Location = new System.Drawing.Point(740, 439);
            this.operationsListView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.operationsListView.Name = "operationsListView";
            this.operationsListView.Size = new System.Drawing.Size(1140, 243);
            this.operationsListView.TabIndex = 3;
            this.operationsListView.UseCompatibleStateImageBehavior = false;
            this.operationsListView.ItemChecked += new ItemCheckedEventHandler(this.operationsListView_ItemChecked);
            // 
            // lblEntities
            // 
            this.lblEntities.AutoSize = true;
            this.lblEntities.Location = new System.Drawing.Point(29, 24);
            this.lblEntities.Name = "lblEntities";
            this.lblEntities.Size = new System.Drawing.Size(68, 16);
            this.lblEntities.TabIndex = 7;
            this.lblEntities.Text = "ENTITIES";
            // 
            // lblFields
            // 
            this.lblFields.AutoSize = true;
            this.lblFields.Location = new System.Drawing.Point(737, 24);
            this.lblFields.Name = "lblFields";
            this.lblFields.Size = new System.Drawing.Size(53, 16);
            this.lblFields.TabIndex = 7;
            this.lblFields.Text = "FIELDS";
            // 
            // lblAttributes
            // 
            this.lblAttributes.AutoSize = true;
            this.lblAttributes.Location = new System.Drawing.Point(1451, 24);
            this.lblAttributes.Name = "lblAttributes";
            this.lblAttributes.Size = new System.Drawing.Size(93, 16);
            this.lblAttributes.TabIndex = 7;
            this.lblAttributes.Text = "ATTRIBUTES";
            // 
            // lblOperations
            // 
            this.lblOperations.AutoSize = true;
            this.lblOperations.Location = new System.Drawing.Point(737, 418);
            this.lblOperations.Name = "lblOperations";
            this.lblOperations.Size = new System.Drawing.Size(126, 16);
            this.lblOperations.TabIndex = 7;
            this.lblOperations.Text = "OPERATIONS LIST";
            // 
            // RFUPluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblOperations);
            this.Controls.Add(this.lblAttributes);
            this.Controls.Add(this.lblFields);
            this.Controls.Add(this.lblEntities);
            this.Controls.Add(this.entitiesListView);
            this.Controls.Add(this.operationsListView);
            this.Controls.Add(this.fieldsListView);
            this.Controls.Add(this.btnGetEntities);
            this.Controls.Add(this.btnClearOperations);
            this.Controls.Add(this.btnUpdateMetadata);
            this.Controls.Add(this.attributesGridView);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "RFUPluginControl";
            this.Size = new System.Drawing.Size(1931, 955);
            this.Load += new System.EventHandler(this.MyPluginControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.attributesGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Label lblEntities;
        private Label lblFields;
        private Label lblAttributes;
        private Label lblOperations;
        private ListView entitiesListView;
        private ListView fieldsListView;
        private DataGridView attributesGridView;
        private ListView operationsListView;
        private Button btnGetEntities;
        private Button btnUpdateMetadata;
        private Button btnClearOperations;
    }
}
