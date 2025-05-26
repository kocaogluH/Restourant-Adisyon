namespace Restourant_Adisyon.Mmodel
{
    partial class frmCategoryAdd
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
            this.SuspendLayout();
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.Size = new System.Drawing.Size(800, 100);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(135, 34);
            this.label1.Size = new System.Drawing.Size(108, 25);
            this.label1.Text = "Kategori Ekle";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 12);
            this.btnSave.Size = new System.Drawing.Size(180, 45);
            this.btnSave.Text = "KAYDET";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(244, 12);
            this.btnClose.Size = new System.Drawing.Size(180, 45);
            this.btnClose.Text = "KAPAT";
            // 
            // frmCategoryAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "frmCategoryAdd";
            this.Text = "Kategori Ekle";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}