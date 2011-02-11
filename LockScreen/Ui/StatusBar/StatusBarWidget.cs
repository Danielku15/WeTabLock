using System.Drawing;
using System.Windows.Forms;

namespace LockScreen
{
    public abstract class StatusBarWidget : LockScreenWidget
    {
        private static readonly StringFormat TextFormat = new StringFormat(StringFormat.GenericTypographic)
                                                              {
                                                                  LineAlignment = StringAlignment.Center
                                                              };
        


        protected abstract Image Icon { get; }
        protected abstract string Label { get; }

        protected StatusBarWidget()
        {
            base.Font = new Font(SystemFonts.MenuFont.FontFamily, 12, FontStyle.Bold, GraphicsUnit.Pixel);
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, 32 + TextRenderer.MeasureText(Label, Font).Width + 5, 32, specified);
        }
        
        protected override void OnPaintWidget(PaintEventArgs e)
        {
            if (Icon != null)
            {
                e.Graphics.DrawImage(Icon, new Rectangle(0, 0, 32, 32), new Rectangle(Point.Empty, Icon.Size),
                                     GraphicsUnit.Pixel);
            }

            e.Graphics.DrawString(Label, Font, Brushes.White, new Rectangle(37, 0, Width - 37, 32), TextFormat);
        }
    }
}
