using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomGallery
{
    class HoverButton : Button
    {
        public HoverButton() : base()
        {
            ForeColor = Color.White;
            Font = new Font("Microsoft YaHei UI",
             20.25F,
             FontStyle.Bold,
             GraphicsUnit.Point,
             0);
        }
        private Color color = Color.SkyBlue;

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            // отрисовка прямоугольника
            pe.Graphics.FillRectangle(new SolidBrush(color), ClientRectangle);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            color = Color.Blue;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            color = Color.SkyBlue;
        }

    }
}
