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
        public LockScreenControl LockScreen { get; internal set; }


        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20;
                return cp;
            }
        }

        public LockScreenWidget()
        {
            InitializeComponent();
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        { /* Do Nothing */ }

        protected void InvalidateEx()
        {
            if (Parent == null)
                Invalidate(true);
            else
                Parent.Invalidate(true);
        }


    }
}
