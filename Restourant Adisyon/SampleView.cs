using System;
using System.Windows.Forms;
using Restourant_Adisyon.Business.Services;

namespace Restourant_Adisyon
{
    public partial class SampleView : Form
    {
        public SampleView()
        {
            InitializeComponent();
            this.Load += SampleView_Load;
        }

        private void SampleView_Load(object sender, EventArgs e)
        {
            LocalizationService.Instance.OnLanguageChanged += (s, args) => ApplyLocalization();
            ApplyLocalization();
        }

        public virtual void ApplyLocalization()
        {
            if (txtSearch != null)
                txtSearch.PlaceholderText = LocalizationService.Instance.GetString("Search");
            if (btnAdd != null)
                btnAdd.Text = "+ " + LocalizationService.Instance.GetString("Add_New");
            if (label2 != null && !string.IsNullOrEmpty(label2.Text))
                label2.Text = LocalizationService.Instance.GetString(label2.Text);
        }

        public virtual void btnAdd_Click(object sender, EventArgs e)
        {
            // Base implementation
        }

        public virtual void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Base implementation
        }

        protected Guna.UI2.WinForms.Guna2TextBox SearchTextBox => txtSearch;
    }
}
