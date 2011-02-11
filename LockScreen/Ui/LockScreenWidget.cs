using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LockScreen
{
    public abstract partial class LockScreenWidget : Panel
    {
        protected internal LockScreenControl LockScreen { get; internal set; }

        protected LockScreenWidget()
        {
            InitializeComponent();
            base.DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);

            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            base.BackColor = Color.Transparent;
        }

        protected sealed override void OnPaint(PaintEventArgs e)
        {
           // base.OnPaint(e);
            e.Graphics.TextRenderingHint =
            System.Drawing.Text.TextRenderingHint.AntiAlias;
            e.Graphics.InterpolationMode =
                System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            e.Graphics.PixelOffsetMode =
                System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            e.Graphics.SmoothingMode =
                System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            OnPaintWidget(e);
        }

        protected abstract void OnPaintWidget(PaintEventArgs e);

        protected void InvalidateEx()
        {
            if (Parent == null)
                Invalidate(true);
            else
                Parent.Invalidate(true);
        }


    }
}
