using CustomGallery.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomGallery
{
    class RecGallery : HoverButton
    {
        List<Bitmap> bitmaps = new List<Bitmap>();
        int indexRes = 0;
        public RecGallery() : base()
        {
            bitmaps.Add(new Bitmap(Resources._1));
            bitmaps.Add(new Bitmap(Resources._2));
            bitmaps.Add(new Bitmap(Resources._3));
            bitmaps.Add(new Bitmap(Resources._4));
            bitmaps.Add(new Bitmap(Resources._5));
            bitmaps.Add(new Bitmap(Resources._6));
            bitmaps.Add(new Bitmap(Resources._7));
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            // получение картинки из ресурсов
            Bitmap bitmap = bitmaps.ElementAt(indexRes);
            // отрисовка картинки в точке (0,0)
            pe.Graphics.DrawImage(bitmap, 0, 0);
            pe.Graphics.DrawString(Text, new Font("Arial", 16), Brushes.Blue, 50, 50);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.Text = "lol";
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.Text = "";
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            changeBGImage();
        }

        private void changeBGImage()
        {
            if (indexRes != 6)
            {
                indexRes++;
            }
            else { indexRes = 0; }
            Bitmap bitmap = bitmaps.ElementAt(indexRes);
            // отрисовка картинки в точке (0,0)
            this.BackgroundImage = bitmap;
        }
    }
}
