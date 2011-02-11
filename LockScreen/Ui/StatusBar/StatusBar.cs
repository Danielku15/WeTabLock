using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace LockScreen
{
    [ProvideProperty("DisplayIndex", typeof(Control))]
    public class StatusBar : LockScreenWidget, IExtenderProvider
    {
        private readonly Timer _timer;

        public StatusBar()
        {
            _timer = new Timer();
            _timer.Interval = 900;
            _timer.Tick += OnTimerTick;
            _timer.Start();
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                _timer.Stop();
                _timer.Dispose();
            }
            base.Dispose(disposing);
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            Invalidate();
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, width, 110, specified);
        }

        #region Overrides of LockScreenWidget

        protected override void OnPaintWidget(PaintEventArgs e)
        {
            using (LinearGradientBrush background = new LinearGradientBrush(DisplayRectangle, Color.FromArgb(178, 30, 30, 30), 
                                Color.FromArgb(178, 96, 96, 96), LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(background, DisplayRectangle);
            }

            // time
            Font timeFont = new Font(SystemFonts.MenuFont.FontFamily, 60, FontStyle.Bold, GraphicsUnit.Pixel);
            string time = DateTime.Now.ToString("HH:mm");
            SizeF timeSize = e.Graphics.MeasureString(time, timeFont);

            int x = (int) ((Width - timeSize.Width)/2);
            int y = 0;

            e.Graphics.DrawString(time, timeFont, Brushes.DarkGray, x + 1, y + 1);
            e.Graphics.DrawString(time, timeFont, Brushes.White, x, y);

            // date
            Font dateFont = new Font(SystemFonts.MenuFont.FontFamily, 23, FontStyle.Bold, GraphicsUnit.Pixel);
            string date = DateTime.Now.ToString("dddd, dd. MMMM yyyy");
            SizeF dateSize = e.Graphics.MeasureString(date, dateFont);

            x = (int) ((Width - dateSize.Width)/2);

            y += 70;

            e.Graphics.DrawString(date, dateFont, Brushes.DarkGray, x + 1, y + 1);
            e.Graphics.DrawString(date, dateFont, Brushes.White, x, y);
        }

        #endregion

        public override LayoutEngine LayoutEngine
        {
            get
            {
                return StatusBarLayout.Instance;
            }
        }


        class StatusBarLayout : LayoutEngine
        {
            private static StatusBarLayout _instance;
            public static StatusBarLayout Instance
            {
                get { return _instance ?? (_instance = new StatusBarLayout()); }
            }

            private StatusBarLayout() { }

            public override bool Layout(object container, LayoutEventArgs layoutEventArgs)
            {
                StatusBar parent = container as StatusBar;
                if (parent == null) return true;

                Rectangle parentDisplayRectangle = parent.DisplayRectangle;
                Point nextControlLocation = parentDisplayRectangle.Location;

                int maxX = 0;

                Control[] controlArray = new Control[parent.Controls.Count];
                parent.Controls.CopyTo(controlArray, 0);
                Array.Sort(controlArray, (a,b) => parent.GetDisplayIndex(a).CompareTo(parent.GetDisplayIndex(b)));

                foreach (Control c in controlArray)
                {
                    // skip hidden control
                    if (!c.Visible)
                        continue;

                    // add control margin to offset
                    nextControlLocation.Offset(c.Margin.Left, c.Margin.Top);

                    // calculate size of control
                    if (c.AutoSize)
                    {
                        c.Size = c.GetPreferredSize(parentDisplayRectangle.Size);
                    }

                    // check if control will overlap 
                    int bottom = nextControlLocation.Y + c.Height + c.Margin.Bottom;
                    if (bottom > parentDisplayRectangle.Bottom)
                    {
                        // place it on the next line
                        nextControlLocation.Y = parentDisplayRectangle.Y;
                        nextControlLocation.X = maxX;
                    }

                    // place it to current position
                    c.Location = nextControlLocation;
                    // calculate new max 
                    maxX = Math.Max(maxX, nextControlLocation.X + c.Width + c.Margin.Right);

                    // move to next row
                    nextControlLocation.X -= c.Margin.Left;
                    nextControlLocation.Y += c.Height + c.Margin.Bottom;
                }

                return false;
            }
        }

        #region Implementation of IExtenderProvider

        public bool CanExtend(object extendee)
        {
            return extendee is Control;
        }


        [Category("Appearance")]
        [ExtenderProvidedProperty]
        public int GetDisplayIndex(Control w)
        {
            return EnsurePropertiesExists(w).DisplayIndex;
        }

        [ExtenderProvidedProperty]
        public void SetDisplayIndex(Control w, int index)
        {
            EnsurePropertiesExists(w).DisplayIndex = index;
            PerformLayout();
        }


        private readonly Dictionary<Control, Properties> _properties = new Dictionary<Control, Properties>();
        private Properties EnsurePropertiesExists(Control key)
        {
            if(!_properties.ContainsKey(key))
            {
                _properties.Add(key, new Properties());
            }
            return _properties[key];
        }

        private class Properties
        {
            public int DisplayIndex;

            public Properties()
            {
                DisplayIndex = 0;
            }
        }

        #endregion
    }
}
