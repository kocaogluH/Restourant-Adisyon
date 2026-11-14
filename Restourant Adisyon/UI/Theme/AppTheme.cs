using Guna.UI2.WinForms;
using Restourant_Adisyon.Business.Managers;
using System.Drawing;
using System.Windows.Forms;

namespace Restourant_Adisyon.UI.Theme
{
    public static class AppTheme
    {
        // ── DİNAMİK MERKEZİ RENK PALETİ (ThemeManager ile Senkronize) ─────────────
        public static Color Background        => ThemeManager.Instance.Background;
        public static Color CardBackground    => ThemeManager.Instance.SurfaceCard;
        public static Color HeaderBackground  => ThemeManager.Instance.HeaderBackground;
        public static Color SidebarBackground => ThemeManager.Instance.SidebarBackground;
        public static Color TextPrimary       => ThemeManager.Instance.TextPrimary;
        public static Color TextSecondary     => ThemeManager.Instance.TextSecondary;
        public static Color BorderColor       => ThemeManager.Instance.BorderColor;

        // ── ROL BAZLI RENKLER ──────────────────────────────────────────────────
        public static Color BrandPrimary      => ThemeManager.Instance.BrandPrimary;
        public static Color BrandSecondary    => ThemeManager.Instance.BrandSecondary;
        public static Color Success           => ThemeManager.Instance.Success;
        public static Color Danger            => ThemeManager.Instance.Danger;
        public static Color Info              => ThemeManager.Instance.Info;
        public static Color Warning           => ThemeManager.Instance.Warning;
        public static Color Secondary         => ThemeManager.Instance.SurfaceElevated;

        // ── YARDIMCI METOTLAR ──────────────────────────────────────────────────
        public static void StyleButton(Control btn, ButtonRole role)
        {
            if (btn == null) return;

            Color baseColor;
            switch (role)
            {
                case ButtonRole.Primary:   baseColor = BrandPrimary; break;
                case ButtonRole.Success:   baseColor = Success; break;
                case ButtonRole.Danger:    baseColor = Danger; break;
                case ButtonRole.Info:      baseColor = Info; break;
                case ButtonRole.Warning:   baseColor = Warning; break;
                default:                   baseColor = Secondary; break;
            }

            if (btn is Guna2Button gBtn)
            {
                gBtn.Animated = true;
                gBtn.AutoRoundedCorners = true;
                gBtn.UseTransparentBackground = true;
                gBtn.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                gBtn.ForeColor = Color.White;
                gBtn.FillColor = baseColor;
                gBtn.HoverState.FillColor = ControlPaint.Dark(baseColor, 0.12f);
                gBtn.CheckedState.FillColor = ControlPaint.Dark(baseColor, 0.22f);
            }
            else if (btn is Button stdBtn)
            {
                stdBtn.FlatStyle = FlatStyle.Flat;
                stdBtn.FlatAppearance.BorderSize = 0;
                stdBtn.BackColor = baseColor;
                stdBtn.ForeColor = Color.White;
                stdBtn.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            }
        }

        public static void StyleCard(Control panel)
        {
            if (panel == null) return;
            if (panel is Guna2Panel gPanel)
            {
                gPanel.FillColor = CardBackground;
                gPanel.BorderRadius = 14;
                gPanel.BorderColor = BorderColor;
                gPanel.BorderThickness = 1;
            }
            else if (panel is Panel stdPanel)
            {
                stdPanel.BackColor = CardBackground;
            }
        }

        public static void ApplyFormTheme(Form form)
        {
            if (form == null) return;
            form.BackColor = Background;
            form.ForeColor = TextPrimary;
            form.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
        }
    }
}
