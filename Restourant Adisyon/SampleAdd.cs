using System;
using System.Drawing;
using System.Windows.Forms;
using Restourant_Adisyon.Business.Managers;
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
            ApplyTheme();
            LocalizationService.Instance.OnLanguageChanged += (s, args) => ApplyLocalization();
            ThemeManager.Instance.OnThemeChanged += (s, theme) => ApplyTheme();
            ApplyLocalization();

            if (btnSave != null)
                AppTheme.StyleButton(btnSave, ButtonRole.Success);
            if (btnClose != null)
                AppTheme.StyleButton(btnClose, ButtonRole.Danger);
        }

        public virtual void ApplyTheme()
        {
            this.BackColor = AppTheme.CardBackground;
            this.ForeColor = AppTheme.TextPrimary;
            if (label1 != null) label1.ForeColor = Color.White;
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
