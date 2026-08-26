using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace Restourant_Adisyon.UI.Theme
{
    public static class AppTheme
    {
        // ── MERKEZİ RENK PALETİ ──────────────────────────────────────────────────
        public static readonly Color Background        = Color.FromArgb(34, 38, 58);   // Koyu Indigo Arka Plan
        public static readonly Color CardBackground    = Color.FromArgb(44, 49, 74);   // Kart & Panel Arka Planı
        public static readonly Color HeaderBackground  = Color.FromArgb(245, 247, 250); // Aydınlık Üst Bar
        public static readonly Color TextPrimary       = Color.White;
        public static readonly Color TextSecondary     = Color.FromArgb(175, 185, 210);

        // ── ROL BAZLI RENKLER ────────────────────────────────────────────────────
        public static readonly Color BrandPrimary      = Color.FromArgb(241, 85, 126); // Pembe (#F1557E)
        public static readonly Color Success           = Color.FromArgb(16, 185, 129); // Yeşil (#10B981)
        public static readonly Color Danger            = Color.FromArgb(239, 68, 68);  // Kırmızı (#EF4444)
        public static readonly Color Info              = Color.FromArgb(94, 148, 255); // Mavi (#5E94FF)
        public static readonly Color Warning           = Color.FromArgb(245, 158, 11); // Turuncu (#F59E0B)
        public static readonly Color Secondary         = Color.FromArgb(55, 62, 92);   // Nötr Gri/Koyu

        // ── YARDIMCI METOTLAR ────────────────────────────────────────────────────
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
                gBtn.HoverState.FillColor = ControlPaint.Dark(baseColor, 0.1f);
                gBtn.CheckedState.FillColor = ControlPaint.Dark(baseColor, 0.2f);
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

        public static void ApplyFormTheme(Form form)
        {
            if (form == null) return;
            form.BackColor = Color.White;
            form.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
        }
    }
}
