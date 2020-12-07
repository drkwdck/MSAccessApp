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
            this.SuspendLayout();
            // 
            // AllEntitiesFromTableFormButton
            // 
            this.AllEntitiesFromTableFormButton.Location = new System.Drawing.Point(37, 46);
            this.AllEntitiesFromTableFormButton.Name = "AllEntitiesFromTableFormButton";
            this.AllEntitiesFromTableFormButton.Size = new System.Drawing.Size(119, 59);
            this.AllEntitiesFromTableFormButton.TabIndex = 0;
            this.AllEntitiesFromTableFormButton.Text = "Список всех записей таблицы";
            this.AllEntitiesFromTableFormButton.UseVisualStyleBackColor = true;
            this.AllEntitiesFromTableFormButton.Click += HadnleOpenFormClick;
            // 
            // AddEntityFormButton
            // 
            this.AddEntityFormButton.Location = new System.Drawing.Point(208, 46);
            this.AddEntityFormButton.Name = "AddEntityFormButton";
            this.AddEntityFormButton.Size = new System.Drawing.Size(140, 59);
            this.AddEntityFormButton.TabIndex = 1;
            this.AddEntityFormButton.Text = "Добавить запись в таблицу";
            this.AddEntityFormButton.UseVisualStyleBackColor = true;
            this.AddEntityFormButton.Click += HadnleOpenFormClick;
            // 
            // StartMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.AddEntityFormButton);
            this.Controls.Add(this.AllEntitiesFromTableFormButton);
            this.Name = "StartMenu";
            this.Text = "StartMenu";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button AllEntitiesFromTableFormButton;
        private System.Windows.Forms.Button AddEntityFormButton;
    }
}