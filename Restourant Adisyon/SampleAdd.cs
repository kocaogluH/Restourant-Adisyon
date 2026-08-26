using System;
using System.Windows.Forms;
using Restourant_Adisyon.Business.Services;
using Restourant_Adisyon.UI.Theme;

namespace Restourant_Adisyon
{
    public partial class SampleAdd : Form
    {
        public SampleAdd()
        {
            InitializeComponent();
            this.Load += SampleAdd_Load;
        }

        private void SampleAdd_Load(object sender, EventArgs e)
        {
            LocalizationService.Instance.OnLanguageChanged += (s, args) => ApplyLocalization();
            ApplyLocalization();

            if (btnSave != null)
                AppTheme.StyleButton(btnSave, ButtonRole.Success);
            if (btnClose != null)
                AppTheme.StyleButton(btnClose, ButtonRole.Danger);
        }

        public virtual void ApplyLocalization()
        {
            if (btnSave != null)
                btnSave.Text = LocalizationService.Instance.GetString("Save");
            if (btnClose != null)
                btnClose.Text = LocalizationService.Instance.GetString("Close");
        }

        public virtual void btnSave_Click(object sender, EventArgs e)
        {
        }

        public virtual void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
