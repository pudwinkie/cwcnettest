using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace dog
{
    public class FontListBox : ListBox
    {
        private ArrayList m_fonts;
        private Font m_labelFont;

        public FontListBox()
        {
            DrawMode = DrawMode.OwnerDrawVariable;
            m_labelFont = new Font("Arial", 8);

            CreateFonts();
        }

        protected void CreateFonts()
        {
            m_fonts = new ArrayList();
            
            foreach (FontFamily _font in FontFamily.Families)
            {
                if (_font.IsStyleAvailable(FontStyle.Regular))
                {
                    m_fonts.Add(new Font(_font, 14));
                    Items.Add(_font);
                }
            }
        }

        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= m_fonts.Count)
                return;

            e.ItemWidth = 300;
            Font _font = (Font) m_fonts[e.Index];

            if (_font == null)
                return;

            e.ItemHeight = (int) _font.GetHeight(e.Graphics) + 6 + (int) m_labelFont.GetHeight(e.Graphics);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if ((e.Index < 0) || (e.Index >= m_fonts.Count))
                return;

            Font _font = (Font) m_fonts[e.Index];
            
            e.DrawBackground();
            
            Graphics _g = e.Graphics;

            _g.DrawLine(Pens.Black, e.Bounds.Left, e.Bounds.Top, e.Bounds.Left + e.Bounds.Width, e.Bounds.Top);
            _g.DrawString(_font.Name, _font, Brushes.Black, e.Bounds.Left + 2, e.Bounds.Top + 2);
            int _y = e.Bounds.Top + 4 + (int) _font.GetHeight(_g);
            _g.DrawString(_font.Name, m_labelFont, Brushes.Black, e.Bounds.Left + 2, _y);
        }
    }
}