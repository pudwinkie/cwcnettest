namespace Dah.Windows.Forms
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class LinearGradientPanel : Panel
    {
        private float _angle = 90f;
        private Color _color1 = Color.CornflowerBlue;
        private Color _color2 = Color.RoyalBlue;

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if ((e.ClipRectangle.Width > 0) && (e.ClipRectangle.Height > 0))
            {
                using (Brush brush = new LinearGradientBrush(e.ClipRectangle, this._color1, this._color2, this._angle))
                {
                    e.Graphics.FillRectangle(brush, e.ClipRectangle);
                }
            }
        }

        [Category("LinearGradientPanel")]
        public float LinearGradientAngle
        {
            get
            {
                return this._angle;
            }
            set
            {
                this._angle = value;
            }
        }

        [Category("LinearGradientPanel")]
        public Color LinearGradientColor1
        {
            get
            {
                return this._color1;
            }
            set
            {
                this._color1 = value;
            }
        }

        [Category("LinearGradientPanel")]
        public Color LinearGradientColor2
        {
            get
            {
                return this._color2;
            }
            set
            {
                this._color2 = value;
            }
        }
    }
}

