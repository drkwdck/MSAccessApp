using System;

namespace MSAccessApp.Forms
{
    partial class StartMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            AllEntitiesFromTableFormButton.Click -= HadnleOpenFormClick;
            AddEntityFormButton.Click -= HadnleOpenFormClick;

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.AllEntitiesFromTableFormButton = new System.Windows.Forms.Button();
            this.AddEntityFormButton = new System.Windows.Forms.Button();
            this.RemoveEntityFromTableFormButton = new System.Windows.Forms.Button();
            this.EditEntityFromTableFormButton = new System.Windows.Forms.Button();
            this.MSysObjectsFormsButton = new System.Windows.Forms.Button();
            this.QueryAndFromsFromButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AllEntitiesFromTableFormButton
            // 
            this.AllEntitiesFromTableFormButton.Location = new System.Drawing.Point(235, 102);
            this.AllEntitiesFromTableFormButton.Name = "AllEntitiesFromTableFormButton";
            this.AllEntitiesFromTableFormButton.Size = new System.Drawing.Size(119, 58);
            this.AllEntitiesFromTableFormButton.TabIndex = 0;
            this.AllEntitiesFromTableFormButton.Text = "Список всех записей таблицы";
            this.AllEntitiesFromTableFormButton.UseVisualStyleBackColor = true;
            this.AllEntitiesFromTableFormButton.Click += HadnleOpenFormClick;

            // 
            // AddEntityFormButton
            // 
            this.AddEntityFormButton.Location = new System.Drawing.Point(421, 102);
            this.AddEntityFormButton.Name = "AddEntityFormButton";
            this.AddEntityFormButton.Size = new System.Drawing.Size(140, 58);
            this.AddEntityFormButton.TabIndex = 1;
            this.AddEntityFormButton.Text = "Добавить запись в таблицу";
            this.AddEntityFormButton.UseVisualStyleBackColor = true;
            this.AddEntityFormButton.Click += HadnleOpenFormClick;

            // 
            // RemoveEntityFromTableFormButton
            // 
            this.RemoveEntityFromTableFormButton.Location = new System.Drawing.Point(162, 190);
            this.RemoveEntityFromTableFormButton.Name = "RemoveEntityFromTableFormButton";
            this.RemoveEntityFromTableFormButton.Size = new System.Drawing.Size(140, 59);
            this.RemoveEntityFromTableFormButton.TabIndex = 2;
            this.RemoveEntityFromTableFormButton.Text = "Удалить запись из таблицы";
            this.RemoveEntityFromTableFormButton.UseVisualStyleBackColor = true;
            this.RemoveEntityFromTableFormButton.Click += HadnleOpenFormClick;

            // 
            // EditEntityFromTableFormButton
            // 
            this.EditEntityFromTableFormButton.Location = new System.Drawing.Point(472, 190);
            this.EditEntityFromTableFormButton.Name = "EditEntityFromTableFormButton";
            this.EditEntityFromTableFormButton.Size = new System.Drawing.Size(133, 58);
            this.EditEntityFromTableFormButton.TabIndex = 3;
            this.EditEntityFromTableFormButton.Text = "Изменить запись в таблице";
            this.EditEntityFromTableFormButton.UseVisualStyleBackColor = true;
            this.EditEntityFromTableFormButton.Click += HadnleOpenFormClick;

            // 
            // MSysObjectsFormsButton
            // 
            this.MSysObjectsFormsButton.Location = new System.Drawing.Point(235, 278);
            this.MSysObjectsFormsButton.Name = "MSysObjectsFormsButton";
            this.MSysObjectsFormsButton.Size = new System.Drawing.Size(119, 49);
            this.MSysObjectsFormsButton.TabIndex = 4;
            this.MSysObjectsFormsButton.Text = "MSysObjects";
            this.MSysObjectsFormsButton.UseVisualStyleBackColor = true;
            this.MSysObjectsFormsButton.Click += HadnleOpenFormClick;

            // 
            // QueryAndFromsFromButton
            // 
            this.QueryAndFromsFromButton.Location = new System.Drawing.Point(421, 279);
            this.QueryAndFromsFromButton.Name = "QueryAndFromsFromButton";
            this.QueryAndFromsFromButton.Size = new System.Drawing.Size(140, 48);
            this.QueryAndFromsFromButton.TabIndex = 5;
            this.QueryAndFromsFromButton.Text = "Запросы и формы";
            this.QueryAndFromsFromButton.UseVisualStyleBackColor = true;
            this.QueryAndFromsFromButton.Click += HadnleOpenFormClick;
            // 
            // StartMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.QueryAndFromsFromButton);
            this.Controls.Add(this.MSysObjectsFormsButton);
            this.Controls.Add(this.EditEntityFromTableFormButton);
            this.Controls.Add(this.RemoveEntityFromTableFormButton);
            this.Controls.Add(this.AddEntityFormButton);
            this.Controls.Add(this.AllEntitiesFromTableFormButton);
            this.Name = "StartMenu";
            this.Text = "2";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button AllEntitiesFromTableFormButton;
        private System.Windows.Forms.Button AddEntityFormButton;
        private System.Windows.Forms.Button RemoveEntityFromTableFormButton;
        private System.Windows.Forms.Button EditEntityFromTableFormButton;
        private System.Windows.Forms.Button MSysObjectsFormsButton;
        private System.Windows.Forms.Button QueryAndFromsFromButton;
    }
}