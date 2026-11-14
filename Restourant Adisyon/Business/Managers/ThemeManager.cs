using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Restourant_Adisyon.Core.Enums;
using Restourant_Adisyon.UI.Theme;

namespace Restourant_Adisyon.Business.Managers
{
    public class ThemeManager
    {
        private static ThemeManager _instance;
        public static ThemeManager Instance => _instance ?? (_instance = new ThemeManager());

        public ThemeMode CurrentTheme { get; private set; } = ThemeMode.CleanWhiteLight;

        public event EventHandler<ThemeMode> OnThemeChanged;

        // ── Renk Paleti Özellikleri ──────────────────────────────────────────
        public Color Background => GetColor(
            dark:    Color.FromArgb(15, 23, 42),   // #0F172A
            luxury:  Color.FromArgb(24, 24, 27),   // #18181B
            light:   Color.FromArgb(248, 250, 252) // #F8FAFC
        );

        public Color SurfaceCard => GetColor(
            dark:    Color.FromArgb(30, 41, 59),   // #1E293B
            luxury:  Color.FromArgb(39, 39, 42),   // #27272A
            light:   Color.FromArgb(255, 255, 255) // #FFFFFF
        );

        public Color SurfaceElevated => GetColor(
            dark:    Color.FromArgb(51, 65, 85),   // #334155
            luxury:  Color.FromArgb(63, 63, 70),   // #3F3F46
            light:   Color.FromArgb(241, 245, 249) // #F1F5F9
        );

        public Color BorderColor => GetColor(
            dark:    Color.FromArgb(51, 65, 85),   // #334155
            luxury:  Color.FromArgb(63, 63, 70),   // #3F3F46
            light:   Color.FromArgb(226, 232, 240) // #E2E8F0
        );

        public Color SidebarBackground => GetColor(
            dark:    Color.FromArgb(11, 17, 32),   // #0B1120
            luxury:  Color.FromArgb(18, 18, 20),   // #121214
            light:   Color.FromArgb(15, 23, 42)    // #0F172A
        );

        public Color HeaderBackground => GetColor(
            dark:    Color.FromArgb(20, 29, 47),   // #141D2F
            luxury:  Color.FromArgb(30, 30, 34),   // #1E1E22
            light:   Color.FromArgb(255, 255, 255) // #FFFFFF
        );

        public Color BrandPrimary => GetColor(
            dark:    Color.FromArgb(244, 63, 94),  // #F43F5E (Cyber Rose)
            luxury:  Color.FromArgb(245, 158, 11), // #F59E0B (Amber Gold)
            light:   Color.FromArgb(244, 63, 94)   // #F43F5E (Rose)
        );

        public Color BrandSecondary => GetColor(
            dark:    Color.FromArgb(99, 102, 241), // #6366F1 (Electric Indigo)
            luxury:  Color.FromArgb(225, 29, 72),  // #E11D48 (Velvet Rose)
            light:   Color.FromArgb(99, 102, 241)  // #6366F1 (Indigo)
        );

        public Color TextPrimary => GetColor(
            dark:    Color.FromArgb(248, 250, 252), // #F8FAFC
            luxury:  Color.FromArgb(250, 245, 255), // #FAF5FF
            light:   Color.FromArgb(15, 23, 42)     // #0F172A
        );

        public Color TextSecondary => GetColor(
            dark:    Color.FromArgb(148, 163, 184), // #94A3B8
            luxury:  Color.FromArgb(161, 161, 170), // #A1A1AA
            light:   Color.FromArgb(100, 116, 139)  // #64748B
        );

        public Color Success => Color.FromArgb(16, 185, 129); // #10B981 (Emerald)
        public Color Warning => Color.FromArgb(245, 158, 11); // #F59E0B (Amber)
        public Color Danger  => Color.FromArgb(239, 68, 68);  // #EF4444 (Coral Red)
        public Color Info    => Color.FromArgb(56, 189, 248);  // #38BDF8 (Cyan/Sky)

        // ── Tema Değiştirme ───────────────────────────────────────────────────
        public void SetTheme(ThemeMode theme)
        {
            CurrentTheme = theme;
            OnThemeChanged?.Invoke(this, CurrentTheme);
        }

        public void CycleNextTheme()
        {
            ThemeMode next = (ThemeMode)(((int)CurrentTheme + 1) % 3);
            SetTheme(next);
        }

        public string GetThemeName()
        {
            switch (CurrentTheme)
            {
                case ThemeMode.CyberSlateDark: return "Cyber Slate (Koyu)";
                case ThemeMode.LuxuryGourmet:  return "Luxury Amber (Lüks)";
                case ThemeMode.CleanWhiteLight: return "Clean SaaS (Açık)";
                default: return "Modern";
            }
        }

        public string GetThemeIcon()
        {
            switch (CurrentTheme)
            {
                case ThemeMode.CyberSlateDark: return "🌙";
                case ThemeMode.LuxuryGourmet:  return "👑";
                case ThemeMode.CleanWhiteLight: return "☀️";
                default: return "🎨";
            }
        }

        private Color GetColor(Color dark, Color luxury, Color light)
        {
            switch (CurrentTheme)
            {
                case ThemeMode.LuxuryGourmet:  return luxury;
                case ThemeMode.CleanWhiteLight: return light;
                case ThemeMode.CyberSlateDark:
                default:
                    return dark;
            }
        }
    }
}
