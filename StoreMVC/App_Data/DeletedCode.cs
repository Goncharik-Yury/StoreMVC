using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace StoreMVC.Deleted
{
	public class Deleted
	{
		// Compressing Image
		private bool ImageCompress(Image image, int quality, string imagePathFull)
		{
			Encoder encoder = Encoder.Quality;
			EncoderParameters encoderParameters = new EncoderParameters(1);

			ImageCodecInfo jpgEncoder = null;

			ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
			foreach (ImageCodecInfo codec in codecs)
				if (codec.FormatID == ImageFormat.Jpeg.Guid)
					jpgEncoder = codec;

			EncoderParameter encoderParameter = new EncoderParameter(encoder, quality);
			encoderParameters.Param[0] = encoderParameter;
			image.Save(imagePathFull, jpgEncoder, encoderParameters);

			return true;
		}

		// Scaling image
		static Image ImageScale(Image source, int width, int height)
		{

			Image dest = new Bitmap(width, height);
			using (Graphics gr = Graphics.FromImage(dest))
			{
				gr.FillRectangle(Brushes.Transparent, 0, 0, width, height);  // Очищаем экран
				gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

				float srcwidth = source.Width;
				float srcheight = source.Height;
				float dstwidth = width;
				float dstheight = height;

				if (srcwidth <= dstwidth && srcheight <= dstheight)  // Исходное изображение меньше целевого
				{
					int left = (width - source.Width) / 2;
					int top = (height - source.Height) / 2;
					gr.DrawImage(source, left, top, source.Width, source.Height);
				}
				else if (srcwidth / srcheight > dstwidth / dstheight)  // Пропорции исходного изображения более широкие
				{
					float cy = srcheight / srcwidth * dstwidth;
					float top = ((float)dstheight - cy) / 2.0f;
					if (top < 1.0f) top = 0;
					gr.DrawImage(source, 0, top, dstwidth, cy);
				}
				else  // Пропорции исходного изображения более узкие
				{
					float cx = srcwidth / srcheight * dstheight;
					float left = ((float)dstwidth - cx) / 2.0f;
					if (left < 1.0f) left = 0;
					gr.DrawImage(source, left, 0, cx, dstheight);
				}

				return dest;
			}
		}
	}
}