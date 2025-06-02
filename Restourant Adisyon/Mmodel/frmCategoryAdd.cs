using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restourant_Adisyon.Mmodel
{
    public partial class frmCategoryAdd : SampleAdd
    {
        public frmCategoryAdd()
        {
            InitializeComponent();
        }


        public int id = 0;
        public override void btnSave_Click(object sender, EventArgs e)
        {
            String qry = "";

            if (id == 0)
            {
                qry = "Insert into category Values (@Name)";
            }
            else
            {
                qry = "Update category Set cataName = @Name where catID = @id ";
            }
            Hashtable ht = new Hashtable();
            ht.Add("@id", id);
            ht.Add("@Name", txtName.Text);

            if (MainClass.Sql(qry, ht) > 0)
            {
                MessageBox.Show("Saved successfully..");
                id= 0;
                txtName.Focus();
            }
        } 
    }
}
