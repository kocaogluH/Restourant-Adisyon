using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Restourant_Adisyon.Mmodel
{
    public partial class frmCheckout : Form
    {
        public frmCheckout()
        {
            InitializeComponent();
        }

        public double amt;
        public int MainID;
        public bool isSuccess = false;

        private void frmCheckout_Load(object sender, EventArgs e)
        {
            txtBillAmount.Text = amt.ToString("N2");
        }

        private void txtReceived_TextChanged(object sender, EventArgs e)
        {
            double amt_val = 0;
            double receipt = 0;

            double.TryParse(txtBillAmount.Text, out amt_val);
            double.TryParse(txtReceived.Text, out receipt);

            if (receipt > 0)
            {
                txtChange.Text = (receipt - amt_val).ToString("N2");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtReceived.Text))
            {
                MessageBox.Show("Please enter received amount");
                return;
            }

            string qry = @"Update tblMain set status = 'Paid', received = @received, change = @change 
                           where MainID = @ID";

            Hashtable ht = new Hashtable();
            ht.Add("@ID", MainID);
            ht.Add("@received", Convert.ToDouble(txtReceived.Text));
            ht.Add("@change", Convert.ToDouble(txtChange.Text));

            if (MainClass.Sql(qry, ht) > 0)
            {
                MessageBox.Show("Bill Paid Successfully");
                isSuccess = true;
                this.Close();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
