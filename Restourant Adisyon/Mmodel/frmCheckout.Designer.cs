namespace Restourant_Adisyon.Mmodel
{
    partial class frmCheckout
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnExit = new Guna.UI2.WinForms.Guna2ControlBox();
            this.txtBillAmount = new Guna.UI2.WinForms.Guna2TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtReceived = new Guna.UI2.WinForms.Guna2TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtChange = new Guna.UI2.WinForms.Guna2TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSave = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.Controls.Add(this.btnExit);
            this.guna2Panel1.Controls.Add(this.label1);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2Panel1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.guna2Panel1.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(400, 80);
            this.guna2Panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Checkout";
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(152)))), ((int)(((byte)(166)))));
            this.btnExit.IconColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(343, 12);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(45, 29);
            this.btnExit.TabIndex = 1;
            // 
            // txtBillAmount
            // 
            this.txtBillAmount.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtBillAmount.DefaultText = "";
            this.txtBillAmount.DisabledState.FillColor = System.Drawing.Color.White;
            this.txtBillAmount.DisabledState.ForeColor = System.Drawing.Color.Black;
            this.txtBillAmount.Enabled = false;
            this.txtBillAmount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtBillAmount.Location = new System.Drawing.Point(42, 131);
            this.txtBillAmount.Name = "txtBillAmount";
            this.txtBillAmount.Size = new System.Drawing.Size(315, 36);
            this.txtBillAmount.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Bill Amount";
            // 
            // txtReceived
            // 
            this.txtReceived.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtReceived.DefaultText = "";
            this.txtReceived.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtReceived.Location = new System.Drawing.Point(42, 201);
            this.txtReceived.Name = "txtReceived";
            this.txtReceived.Size = new System.Drawing.Size(315, 36);
            this.txtReceived.TabIndex = 3;
            this.txtReceived.TextChanged += new System.EventHandler(this.txtReceived_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 185);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Received Amount";
            // 
            // txtChange
            // 
            this.txtChange.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtChange.DefaultText = "";
            this.txtChange.DisabledState.FillColor = System.Drawing.Color.White;
            this.txtChange.DisabledState.ForeColor = System.Drawing.Color.Black;
            this.txtChange.Enabled = false;
            this.txtChange.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtChange.Location = new System.Drawing.Point(42, 271);
            this.txtChange.Name = "txtChange";
            this.txtChange.Size = new System.Drawing.Size(315, 36);
            this.txtChange.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(39, 255);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Change";
            // 
            // btnSave
            // 
            this.btnSave.AutoRoundedCorners = true;
            this.btnSave.BorderRadius = 21;
            this.btnSave.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(85)))), ((int)(((byte)(126)))));
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(125, 330);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(150, 45);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Pay Bill";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // frmCheckout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(400, 400);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtChange);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtReceived);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtBillAmount);
            this.Controls.Add(this.guna2Panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmCheckout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmCheckout";
            this.Load += new System.EventHandler(this.frmCheckout_Load);
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2ControlBox btnExit;
        private Guna.UI2.WinForms.Guna2TextBox txtBillAmount;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2TextBox txtReceived;
        private System.Windows.Forms.Label label3;
        private Guna.UI2.WinForms.Guna2TextBox txtChange;
        private System.Windows.Forms.Label label4;
        private Guna.UI2.WinForms.Guna2Button btnSave;
    }
}
