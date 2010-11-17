using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;
using LockScreen.Ui;

namespace LockScreen
{
    public class ClockWidget : LockScreenWidget
    {
        private static readonly StringFormat ClockFormat;
        static ClockWidget()
        {
            ClockFormat = new StringFormat(StringFormat.GenericTypographic);
            ClockFormat.Alignment = StringAlignment.Center;
        }
        private const int ClockSize = 115;

        private int _timeSize = 0;
        private string _time = "";
        private string _date = "";
        private Timer _updateTimer;

        private Font _timeFont;
        public Font TimeFont
        {
            get { return _timeFont; }
            set
            {
                _timeFont = value;
                _timeSize = (int)CreateGraphics().MeasureString(_time, value, Size, ClockFormat).Height;

                Invalidate();
            }
        }

        private Font _dateFont;
        public Font DateFont
        {
            get { return _dateFont; }
            set
            {
                _dateFont = value;
                Invalidate();
            }
        }

        public ClockWidget()
        {
            TimeFont = new Font(SystemFonts.MenuFont.FontFamily, 60, FontStyle.Regular, GraphicsUnit.Pixel);
            DateFont = new Font(SystemFonts.MenuFont.FontFamily, 25, FontStyle.Regular, GraphicsUnit.Pixel);
            _updateTimer = new Timer { Interval = 1000 };
            _updateTimer.Tick += UpdateTime;
            _updateTimer.Start();
            UpdateTime(null, null);
        }

        private void UpdateTime(object sender, EventArgs e)
        {
            string oldTime = _time;
            _time = DateTime.Now.ToString("hh:mm");
            _date = DateTime.Now.ToString("dddd, dd. MMMM yyyy");
            //if (oldTime != _time)
                InvalidateEx();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_updateTimer != null)
                {
                    _updateTimer.Dispose();
                    _updateTimer = null;
                }
            }
            base.Dispose(disposing);
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, width, ClockSize, specified);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            try
            {
                Graphics g = pe.Graphics;
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                g.DrawImage(WidgetResources.ClockBg,
                    new Rectangle(Point.Empty, Size),
                    new Rectangle(Point.Empty, WidgetResources.ClockBg.Size),
                    GraphicsUnit.Pixel);

                Rectangle timeRect = new Rectangle(1, 1, Width, ClockSize);
                Rectangle dateRect = new Rectangle(1, 1 + (int)(_timeSize*0.8), Width, ClockSize);
                using (SolidBrush b = new SolidBrush(Color.FromArgb(34, 34, 34)))
                {
                    g.DrawString(_time, TimeFont, b, timeRect, ClockFormat);
                    g.DrawString(_date, DateFont, b, dateRect, ClockFormat);

                    timeRect.Offset(0, 1);
                    dateRect.Offset(0, 1);
                    b.Color = Color.White;

                    g.DrawString(_time, TimeFont, b, timeRect, ClockFormat);
                    g.DrawString(_date, DateFont, b, dateRect, ClockFormat);
                }
            }
            catch (Exception e)
            {

                Debug.WriteLine(e.ToString());
            }



        }
    }
}
