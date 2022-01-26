using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace VMS.Codec.Lib
{
    public class ImageControl  // 20150304 클래스로 묶음.
    {
        private System.Windows.Controls.Image _image;
        private WriteableBitmap _bitmap;
        private int _width;
        private int _height;
        private PixelFormat _format;



        private void Prepare(int width, int height, PixelFormat format)
        {
            this._bitmap = new WriteableBitmap(
                width, height,
                96, 96,
                format, null);

            this._image.Source = this._bitmap;
            this._width = width;
            this._height = height;
            this._format = format;
        }

        private void Prepare(Image image)
        {
            if (image == null)
                return;
            BitmapSource bs = image.Source as BitmapSource;
            this._bitmap = new WriteableBitmap(bs);
            this._bitmap.Freeze();

            this._width = (int)image.Source.Width;
            this._height = (int)image.Source.Height;
            this._image.Source = this._bitmap;
        }

        private int PixelBytes(PixelFormat format)
        {
            if (format == PixelFormats.Bgr32)
                return 4;

            if (format == PixelFormats.Bgr24)
                return 3;

            return 4; // 디폴트는 그냥  PixelFormats.Bgr32 인걸로.
        }

        public void Update(Image image)
        {
            Prepare(image);
        }

        public void Update(IntPtr srcimage, int width, int height, PixelFormat format)
        {
            Update(srcimage, width, height, format, new Int32Rect(0, 0, width, height));
        }

        public void Clear()
        {
            Prepare(800, 600, PixelFormats.Bgr32);
        }

        public void Update(IntPtr srcimage, int width, int height, PixelFormat format, Int32Rect rect)
        {
            if (this._width != width ||
                this._height != height ||
                this._format != format ||
                this._bitmap.IsFrozen == true)
                Prepare(width, height, format);

            this._bitmap.Lock();
            try
            {
                this._bitmap.WritePixels(rect,
                                    srcimage,
                                    width * height * PixelBytes(format),
                                    width * PixelBytes(format));
            }
            finally
            {
                this._bitmap.Unlock();
            }
        }



        public ImageControl(Image image)
        {
            this._image = image;
        }
    }
}
