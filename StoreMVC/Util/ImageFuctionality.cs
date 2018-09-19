using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;
using System.Diagnostics;

namespace StoreMVC.Util
{
	public static class ImageFuctionality
	{
		// Uploading files on server
		public static string UploadImage(HttpPostedFileBase file, string serverMapPath, string imagesDirectoryPath)
		{
			string filePath = serverMapPath + imagesDirectoryPath;
			string fileName = "default.png";

			Image image = null;
			try
			{
				image = Image.FromStream(file.InputStream);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("File to Image convertation exception" + ex);
				return filePath + fileName;
			}

			fileName = Path.GetFileName(file.FileName);

			do
				fileName = String.Format(@"{0}{1}", Guid.NewGuid(), Path.GetExtension(file.FileName));
			while (System.IO.File.Exists(filePath + fileName));

			int requiredHeight = 500;
			int requiredWidth = 500;

			image = ImageScale(image, requiredHeight, requiredWidth);
			image.Save(filePath + fileName);
			return fileName;
		}

		// Scaling the Image
		public static Image ImageScale(Image originalBitmap, int requiredHeight, int requiredWidth)
		{
			int[] heightWidthRequiredDimensions;

			// Pass dimensions to worker method depending on image type required
			heightWidthRequiredDimensions = ImageRightSize(originalBitmap.Height, originalBitmap.Width, requiredHeight, requiredWidth);


			Bitmap resizedBitmap = new Bitmap(heightWidthRequiredDimensions[1],
											   heightWidthRequiredDimensions[0]);

			const float resolution = 72;

			resizedBitmap.SetResolution(resolution, resolution);

			Graphics graphic = Graphics.FromImage((Image)resizedBitmap);

			graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			graphic.DrawImage(originalBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height);

			graphic.Dispose();
			originalBitmap.Dispose();
			//resizedBitmap.Dispose(); // Still in use


			return resizedBitmap;
		}

		// Finding correct image size
		private static int[] ImageRightSize(int originalHeight, int originalWidth, int requiredHeight, int requiredWidth)
		{
			int imgHeight = 0;
			int imgWidth = 0;

			imgWidth = requiredHeight;
			imgHeight = requiredWidth;


			int requiredHeightLocal = originalHeight;
			int requiredWidthLocal = originalWidth;

			double ratio = 0;

			// Check height first
			// If original height exceeds maximum, get new height and work ratio.
			if (originalHeight > imgHeight)
			{
				ratio = double.Parse(((double)imgHeight / (double)originalHeight).ToString());
				requiredHeightLocal = imgHeight;
				requiredWidthLocal = (int)((decimal)originalWidth * (decimal)ratio);
			}

			// Check width second. It will most likely have been sized down enough
			// in the previous if statement. If not, change both dimensions here by width.
			// If new width exceeds maximum, get new width and height ratio.
			if (requiredWidthLocal >= imgWidth)
			{
				ratio = double.Parse(((double)imgWidth / (double)originalWidth).ToString());
				requiredWidthLocal = imgWidth;
				requiredHeightLocal = (int)((double)originalHeight * (double)ratio);
			}

			int[] heightWidthDimensionArr = { requiredHeightLocal, requiredWidthLocal };

			return heightWidthDimensionArr;
		}

		public static bool DeleteImageFromServer(string imgName, string serverMapPath, string imagesDirectoryPath)
		{
			if (imgName != "default.png")
			{
				try
				{
					System.IO.File.Delete(serverMapPath + imagesDirectoryPath + imgName);
				}
				catch (Exception ex)
				{
					Debug.WriteLine("Unable to delete the Image from server." + ex);
					return false;
				}
			}
			return true;
		}

	}
}