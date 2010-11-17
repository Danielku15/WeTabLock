using System;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace LockScreen
{
    [Docking(DockingBehavior.Ask)]
    public partial class LockScreenControl : Control
    {
        public LockScreenControl()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ContainerControl, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            pevent.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            base.OnPaintBackground(pevent);
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            LockScreenWidget widget = e.Control as LockScreenWidget;
            if(widget == null)
            {
                throw new InvalidOperationException("Only Widgets can be added to this container!");
            }
            widget.LockScreen = this;
            base.OnControlAdded(e);
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            LockScreenWidget widget = e.Control as LockScreenWidget;
            if(widget != null)
            {
                widget.LockScreen = null;
            }
            base.OnControlRemoved(e);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

    }
}
