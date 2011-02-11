using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LockScreen.Ui
{
    class AppleUnlockWidget : LockScreenWidget
    {
        private static readonly SolidBrush BackgroundBrush = new SolidBrush(Color.FromArgb(178, 60, 62, 71));

        private Rectangle _sliderBackgroundBounds;
        public Rectangle SliderBackgroundBounds
        {
            get { return _sliderBackgroundBounds; }
        }


        public Rectangle SliderButtonBounds
        {
            get
            {
                // SliderBackgroundBounds.Left -> 0
                // (SliderBackgroundBounds.Right - WidgetResources.slider_button.Width) -> 1
                return new Rectangle(RelativeButtonToAbsolute(_dragPosition),
                                     (Height - WidgetResources.slider_button.Height) / 2,
                                     WidgetResources.slider_button.Width,
                                     WidgetResources.slider_button.Height);
            }
        }

        private int RelativeButtonToAbsolute(float relative)
        {
            // SliderBackgroundBounds.Left -> 0
            // (SliderBackgroundBounds.Right - WidgetResources.slider_button.Width) -> 1

            int min = SliderBackgroundBounds.Left;
            int max = (SliderBackgroundBounds.Right - WidgetResources.slider_button.Width);

            return (int)((max - min) * relative + min);
        }


        private float AbsoluteButtonToRelative(int absolute)
        {
            // SliderBackgroundBounds.Left -> 0
            // (SliderBackgroundBounds.Right - WidgetResources.slider_button.Width) -> 1

            int x = absolute - SliderBackgroundBounds.Left;
            int width = SliderBackgroundBounds.Width - WidgetResources.slider_button.Width;

            // width -> 1
            // x = ?
            return x / (float)width;
        }

        private float _dragPosition = 0;
        private bool _lockDragging = false;
        private int _startOffset;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (SliderButtonBounds.Contains(e.Location))
            {
                _lockDragging = true;
                _startOffset = e.Location.X - SliderBackgroundBounds.X;
                Capture = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_lockDragging)
            {
                // take care of start offset
                int x = e.X - _startOffset;
                _dragPosition = Math.Min(1, Math.Max(0, AbsoluteButtonToRelative(x)));
                Invalidate(SliderBackgroundBounds);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            bool unlock = _dragPosition == 1;

            Capture = false;
            _lockDragging = false;
            _dragPosition = 0;
            Invalidate();

            if (unlock)
            {
                LockForm.Instance.Unlock();
            }
        }

        protected override void OnResize(EventArgs eventargs)
        {
            _sliderBackgroundBounds = new Rectangle((Width - WidgetResources.SliderBg.Width) / 2,
                                           (Height - WidgetResources.SliderBg.Height) / 2,
                                           WidgetResources.SliderBg.Width,
                                           WidgetResources.SliderBg.Height);
        }


        #region Overrides of LockScreenWidget

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, width, 90, specified);
        }

        protected override void OnPaintWidget(PaintEventArgs e)
        {
             e.Graphics.FillRectangle(BackgroundBrush, DisplayRectangle);

            // draw slider bg
            e.Graphics.DrawImage(WidgetResources.SliderBg, SliderBackgroundBounds,
                                 new Rectangle(Point.Empty, WidgetResources.SliderBg.Size), GraphicsUnit.Pixel);

            //// draw text
            //string s = "Entriegeln";
            //float alpha = Math.Max(0, 1 - _dragPosition - 0.5f);

            //Font f = new Font(SystemFonts.MenuFont.FontFamily, 29, FontStyle.Regular, GraphicsUnit.Pixel);
            //SizeF size = e.Graphics.MeasureString(s, f);
            //using (Brush b = new SolidBrush(Color.FromArgb((int)(255 * alpha), Color.White)))
            //{
            //    int x = SliderBackgroundBounds.X + SliderButtonBounds.Width;
            //    int w = SliderBackgroundBounds.Width - SliderButtonBounds.Width;
            //    e.Graphics.DrawString(s, f, b, x + ((w - size.Width)/2), (int)((Height - size.Height) / 2));
            //}

            // draw slider button
            e.Graphics.DrawImage(WidgetResources.slider_button, SliderButtonBounds,
                     new Rectangle(Point.Empty, WidgetResources.slider_button.Size), GraphicsUnit.Pixel);

          
        }

        #endregion
    }
}
