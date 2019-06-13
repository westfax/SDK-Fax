using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;


using System.Collections.Generic;

namespace WF.SDK.Common.Imaging
{
	/// <summary>
	/// Summary description for BitmapHelper.
	/// </summary>
	public class BitmapHelper
	{
		private BitmapHelper()
		{ }

		/// <summary>
		/// Returns a PaperSize based on the attributes of the source image.  If it does not exactly
		/// fit a standard legal or letter page, then PaperSize.Undefined will be returned.
		/// </summary>
		/// <param name="bmp">The image to check.</param>
		/// <returns>The paper size.</returns>
		public static PaperSize GetStandardPaperSize(Bitmap bmp)
		{
			if (bmp.Width != ImageUtility.FAX_TIF_HOR_PX) { return PaperSize.Undefined; }
			if (bmp.HorizontalResolution != ImageUtility.FAX_TIF_HOR_RES) { return PaperSize.Undefined; }
			if (bmp.VerticalResolution == ImageUtility.FAX_TIF_VER_RES_HI)
			{
				if (bmp.Height == ImageUtility.FAX_TIF_VER_PX_LTR_HI) { return PaperSize.Letter; }
				if (bmp.Height == ImageUtility.FAX_TIF_VER_PX_LGL_HI) { return PaperSize.Legal; }
			}
			if (bmp.VerticalResolution == ImageUtility.FAX_TIF_VER_RES_LOW)
			{
				if (bmp.Height == ImageUtility.FAX_TIF_VER_PX_LTR_LOW) { return PaperSize.Letter; }
				if (bmp.Height == ImageUtility.FAX_TIF_VER_PX_LGL_LOW) { return PaperSize.Legal; }
			}
			return PaperSize.Undefined;
		}

    /// <summary>
    /// Rotates the bitmap to the correct orientation if the given bitmap has the correct geometry for a Fax, with the correct resolution, and the correct size.
    /// </summary>
    public static void RotateBitmapIfFaxMatch (Bitmap bmp, System.Drawing.RotateFlipType type = RotateFlipType.Rotate90FlipNone) 
    {
      if (
       (bmp.HorizontalResolution == ImageUtility.FAX_TIF_HOR_RES)
       && (bmp.Width == ImageUtility.FAX_TIF_HOR_PX)
       && (bmp.VerticalResolution == ImageUtility.FAX_TIF_VER_RES_HI || bmp.VerticalResolution == ImageUtility.FAX_TIF_VER_RES_LOW)
       && (bmp.Height == ImageUtility.FAX_TIF_VER_PX_LGL_HI || bmp.Height == ImageUtility.FAX_TIF_VER_PX_LGL_LOW || bmp.Height == ImageUtility.FAX_TIF_VER_PX_LTR_HI || bmp.Height == ImageUtility.FAX_TIF_VER_PX_LTR_LOW)
       ) //This is a standard fax.  No need to rotate.
      { return; }

      if (
        (bmp.VerticalResolution == ImageUtility.FAX_TIF_HOR_RES)
        && (bmp.Height == ImageUtility.FAX_TIF_HOR_PX)
        && (bmp.HorizontalResolution == ImageUtility.FAX_TIF_VER_RES_HI || bmp.HorizontalResolution == ImageUtility.FAX_TIF_VER_RES_LOW)
        && (bmp.Width == ImageUtility.FAX_TIF_VER_PX_LGL_HI || bmp.Width == ImageUtility.FAX_TIF_VER_PX_LGL_LOW || bmp.Width == ImageUtility.FAX_TIF_VER_PX_LTR_HI || bmp.Width == ImageUtility.FAX_TIF_VER_PX_LTR_LOW)
        )//This is a standard fax on its side.  Rotate it.
      {
        bmp.RotateFlip(type);
        return;
      }

      throw new Exception("Tiff does not conform to a Fax size.");
    }

    public static bool IsStandardFaxSize(Bitmap bmp)
    {
      if (
        (bmp.Width == ImageUtility.FAX_TIF_HOR_PX)
      && (bmp.Height == ImageUtility.FAX_TIF_VER_PX_LGL_HI || bmp.Height == ImageUtility.FAX_TIF_VER_PX_LGL_LOW || bmp.Height == ImageUtility.FAX_TIF_VER_PX_LTR_HI || bmp.Height == ImageUtility.FAX_TIF_VER_PX_LTR_LOW)
      ) //This is a standard fax.  No need to crop.
      { return true; }
      return false;
    }


		#region CreateBitMap Overloads
		/// <summary>
		/// Creates an empty Bitmap that will have the same size and resolution 
		/// of the source bitmap with the indicated pixel format.
		/// </summary>
		/// <param name="bmp">The bitmap to copy.</param>
		/// <param name="pixelFormat">The pixel format</param>
		/// <returns>The new bitmap.</returns>
		internal static Bitmap CreateBitMap(Bitmap bmp, PixelFormat pixelFormat)
		{
			return ImageHelper.IsIndexedFormat(bmp) ? BitmapHelper.CreateBitMap(bmp.Width, bmp.Height, bmp.HorizontalResolution, bmp.VerticalResolution, bmp.Palette, pixelFormat)
				: BitmapHelper.CreateBitMap(bmp.Width, bmp.Height, bmp.HorizontalResolution, bmp.VerticalResolution, pixelFormat);
		}

		/// <summary>
		/// Creates a Bitmap that will have the indicated size, resolution and pixel format
		/// </summary>
		/// <param name="widthPixels">The width in pixels</param>
		/// <param name="heightPixels">The height in pixels</param>
		/// <param name="horizontalResolution">The resolution</param>
		/// <param name="verticalResolution">The resolution</param>
		/// <param name="pixelFormat">The pixel format</param>
		/// <returns>The new bitmap.</returns>
		internal static Bitmap CreateBitMap(int widthPixels, int heightPixels, float horizontalResolution, float verticalResolution, ColorPalette palette, PixelFormat pixelFormat)
		{
			Bitmap ret = new Bitmap(widthPixels, heightPixels, pixelFormat);
			ret.SetResolution(horizontalResolution, verticalResolution);
			ret.Palette = palette;
			return ret;
		}

		/// <summary>
		/// Creates a Bitmap that will have the indicated size, resolution and pixel format
		/// </summary>
		/// <param name="widthPixels">The width in pixels</param>
		/// <param name="heightPixels">The height in pixels</param>
		/// <param name="horizontalResolution">The resolution</param>
		/// <param name="verticalResolution">The resolution</param>
		/// <param name="pixelFormat">The pixel format</param>
		/// <returns>The new bitmap.</returns>
		internal static Bitmap CreateBitMap(int widthPixels, int heightPixels, float horizontalResolution, float verticalResolution, PixelFormat pixelFormat)
		{
			Bitmap ret = new Bitmap(widthPixels, heightPixels, pixelFormat);
			ret.SetResolution(horizontalResolution, verticalResolution);
			return ret;
		}

		/// <summary>
		/// Creates a Bitmap that will have the indicated pixel format, size and resolution
		/// </summary>
		/// <param name="paperSize">The required paper size</param>
		/// <param name="faxQuality">The required fax quality</param>
		/// <param name="pixelFormat">The needed pixel format</param>
		/// <returns>The new image with the correct settings.</returns>
		internal static Bitmap CreateBitMap(PaperSize paperSize, FaxQuality faxQuality, PixelFormat pixelFormat)
		{
			return CreateBitMap(paperSize, faxQuality, null, pixelFormat);
		}

		/// <summary>
		/// Creates a Bitmap that will have the indicated pixel format, size and resolution
		/// </summary>
		/// <param name="paperSize">The required paper size</param>
		/// <param name="faxQuality">The required fax quality</param>
		/// <param name="pixelFormat">The needed pixel format</param>
		/// <returns>The new image with the correct settings.</returns>
		internal static Bitmap CreateBitMap(PaperSize paperSize, FaxQuality faxQuality, ColorPalette palette, PixelFormat pixelFormat)
		{
			Bitmap ret = null;
			switch (paperSize)
			{
				case PaperSize.Legal:
					{
						switch (faxQuality)
						{
							case FaxQuality.Fine: 
								{ 
									ret = 
										palette == null ?  CreateBitMap(ImageUtility.FAX_TIF_HOR_PX, ImageUtility.FAX_TIF_VER_PX_LGL_HI, ImageUtility.FAX_TIF_HOR_RES, ImageUtility.FAX_TIF_VER_RES_HI, pixelFormat)
										: CreateBitMap(ImageUtility.FAX_TIF_HOR_PX, ImageUtility.FAX_TIF_VER_PX_LGL_HI, ImageUtility.FAX_TIF_HOR_RES, ImageUtility.FAX_TIF_VER_RES_HI, palette, pixelFormat); 
									break; 
								}
							default: 
								{
									ret = palette == null ? CreateBitMap(ImageUtility.FAX_TIF_HOR_PX, ImageUtility.FAX_TIF_VER_PX_LGL_LOW, ImageUtility.FAX_TIF_HOR_RES, ImageUtility.FAX_TIF_VER_RES_LOW, pixelFormat)
										: CreateBitMap(ImageUtility.FAX_TIF_HOR_PX, ImageUtility.FAX_TIF_VER_PX_LGL_LOW, ImageUtility.FAX_TIF_HOR_RES, ImageUtility.FAX_TIF_VER_RES_LOW, palette, pixelFormat); 
									break; 
								}
						}
						break;
					}
				default:
					{
						switch (faxQuality)
						{
							case FaxQuality.Fine: 
								{
									ret = palette == null ? BitmapHelper.CreateBitMap(ImageUtility.FAX_TIF_HOR_PX, ImageUtility.FAX_TIF_VER_PX_LTR_HI, ImageUtility.FAX_TIF_HOR_RES, ImageUtility.FAX_TIF_VER_RES_HI, pixelFormat)
										: CreateBitMap(ImageUtility.FAX_TIF_HOR_PX, ImageUtility.FAX_TIF_VER_PX_LTR_HI, ImageUtility.FAX_TIF_HOR_RES, ImageUtility.FAX_TIF_VER_RES_HI, palette, pixelFormat); 
									break; 
								}
							default: 
								{
									ret = palette == null ? BitmapHelper.CreateBitMap(ImageUtility.FAX_TIF_HOR_PX, ImageUtility.FAX_TIF_VER_PX_LTR_LOW, ImageUtility.FAX_TIF_HOR_RES, ImageUtility.FAX_TIF_VER_RES_LOW, pixelFormat)
										: CreateBitMap(ImageUtility.FAX_TIF_HOR_PX, ImageUtility.FAX_TIF_VER_PX_LTR_LOW, ImageUtility.FAX_TIF_HOR_RES, ImageUtility.FAX_TIF_VER_RES_LOW, palette, pixelFormat); 
									break; 
								}
						}
						break;
					}
			}
			return ret;
		}

		/// <summary>
		/// Creates a Bitmap that will have the indicated pixel format, size and resolution
		/// </summary>
		/// <param name="widthInches">The width in inches</param>
		/// <param name="heightInches">The height in inches</param>
		/// <param name="verticalResolution">The verticalResolution in pixels per inch</param>
		/// <param name="horizontalResolution">The horizontalResolution in pixels per inch</param>
		/// <param name="pixelFormat">The needed pixel format</param>
		/// <returns>The new image with the correct settings.</returns>
		internal static Bitmap CreateBitMap(float widthInches, float heightInches, float horizontalResolution, float verticalResolution, ColorPalette palette, PixelFormat pixelFormat)
		{
			Bitmap ret = null;

			ret = new Bitmap((int)(widthInches * horizontalResolution), (int)(heightInches * verticalResolution), pixelFormat);
			ret.SetResolution(horizontalResolution, verticalResolution);
			//if dest pixelformat is 1bpp, then setting the palette is not necessary (black and white is only possible palette)
			ret.Palette = palette;

			return ret;
		}

		/// <summary>
		/// Creates a Bitmap that will have the indicated pixel format, size and resolution
		/// </summary>
		/// <param name="widthInches">The width in inches</param>
		/// <param name="heightInches">The height in inches</param>
		/// <param name="verticalResolution">The verticalResolution in pixels per inch</param>
		/// <param name="horizontalResolution">The horizontalResolution in pixels per inch</param>
		/// <param name="pixelFormat">The needed pixel format</param>
		/// <returns>The new image with the correct settings.</returns>
		internal static Bitmap CreateBitMap(float widthInches, float heightInches, float horizontalResolution, float verticalResolution, PixelFormat pixelFormat)
		{
			Bitmap ret = null;

			ret = new Bitmap((int)(widthInches * horizontalResolution), (int)(heightInches * verticalResolution), pixelFormat);
			ret.SetResolution(horizontalResolution, verticalResolution);

			return ret;
		}
		#endregion

		#region CreateCopy1BppIndexed Overloads
		public static Bitmap CreateCopy1BppIndexed(Bitmap bmp)
		{
			return CreateCopy1BppIndexed(bmp, ImageUtility.Threshold); 
		}

		

		/// <summary>
		/// Reduces the PixelFormat to 1bppIndexed and will use threshold
		/// for color reduction.
		/// </summary>
		/// <param name="bmp">The source bitmap</param>
		/// <param name="threshold">The threshold to use</param>
		/// <returns>A new bitmap.</returns>
		public static Bitmap CreateCopy1BppIndexed(Bitmap bmp, int threshold)
		{
			//If its already in the format we need, make a copy and return.
			if (bmp.PixelFormat == PixelFormat.Format1bppIndexed) { return BitmapHelper.CreateCopyExact(bmp); }

			//If its not in 32bppArgb format then throw error
			if (bmp.PixelFormat != PixelFormat.Format32bppArgb)
			{
				throw new Exception("Source format must be 32BppArgb.");
			}

      //Destination bitmap
			Bitmap ret = BitmapHelper.CreateBitMap(bmp, PixelFormat.Format1bppIndexed);

			// Lock source bitmap in memory
			BitmapData sourceData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

			// Copy image data to binary array
			int imageSize = sourceData.Stride * sourceData.Height;
			byte[] sourceBuffer = new byte[imageSize];
			Marshal.Copy(sourceData.Scan0, sourceBuffer, 0, imageSize);

			// Unlock source bitmap
			bmp.UnlockBits(sourceData);

			// Lock destination bitmap in memory
			BitmapData destinationData = ret.LockBits(new Rectangle(0, 0, ret.Width, ret.Height), ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed);

			// Create destination buffer
			imageSize = destinationData.Stride * destinationData.Height;
			byte[] destinationBuffer = new byte[imageSize];

			int sourceIndex = 0;
			int destinationIndex = 0;
			int pixelTotal = 0;
			byte destinationValue = 0;
			int pixelValue = 128;
			int height = bmp.Height;
			int width = bmp.Width;

			// Iterate lines
			for (int y = 0; y < height; y++)
			{
				sourceIndex = y * sourceData.Stride;
				destinationIndex = y * destinationData.Stride;
				destinationValue = 0;
				pixelValue = 128;

				// Iterate pixels
				for (int x = 0; x < width; x++)
				{
					// Compute pixel brightness (i.e. total of Red, Green, and Blue values)
					pixelTotal = sourceBuffer[sourceIndex + 1] + sourceBuffer[sourceIndex + 2] + sourceBuffer[sourceIndex + 3];
					if (pixelTotal > threshold)
					{
						destinationValue += (byte)pixelValue;
					}
					if (pixelValue == 1)
					{
						destinationBuffer[destinationIndex] = destinationValue;
						destinationIndex++;
						destinationValue = 0;
						pixelValue = 128;
					}
					else
					{
						pixelValue >>= 1;
					}
					sourceIndex += 4;
				}
				if (pixelValue != 128)
				{
					destinationBuffer[destinationIndex] = destinationValue;
				}
			}

			// Copy binary image data to destination bitmap
			Marshal.Copy(destinationBuffer, 0, destinationData.Scan0, imageSize);

			// Unlock destination bitmap
			ret.UnlockBits(destinationData);
			return ret;
		}
		#endregion

		#region CreateCopy****  Functions
		/// <summary>
		/// A copy of the original with a 32bpp pixel format.
		/// </summary>
		/// <param name="bmp">Bitmap object to copy.</param>
		/// <returns>A new bitmap object</returns>
		internal static Bitmap CreateCopy32Bit(Bitmap bmp)
		{
			Bitmap ret = BitmapHelper.CreateBitMap(bmp, PixelFormat.Format32bppArgb);
			Graphics g = Graphics.FromImage(ret);
			g.DrawImage(bmp, 0F, 0F);
			g.Dispose();

			return ret;
		}

		/// <summary>
		/// Creates an exact copy of the original bitmap with the same PixelFormat.  
		/// </summary>
		/// <param name="bmp">Bitmap object to copy.</param>
		/// <returns>A new bitmap object</returns>
		internal static Bitmap CreateCopyExact(Bitmap bmp)
		{
			//Copy size and geometry
			Bitmap ret = BitmapHelper.CreateBitMap(bmp, bmp.PixelFormat);

			// Lock source bitmap in memory
			BitmapData sourceData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
			// Copy image data to binary array
			int imageSize = sourceData.Stride * sourceData.Height;
			byte[] imageBuffer = new byte[imageSize];
			Marshal.Copy(sourceData.Scan0, imageBuffer, 0, imageSize);
			// Unlock source bitmap
			bmp.UnlockBits(sourceData);

			// Lock destination bitmap in memory
			BitmapData destinationData = ret.LockBits(new Rectangle(0, 0, ret.Width, ret.Height), ImageLockMode.WriteOnly, ret.PixelFormat);
			// Copy binary image data to destination bitmap
			Marshal.Copy(imageBuffer, 0, destinationData.Scan0, imageSize);
			// Unlock destination bitmap
			ret.UnlockBits(destinationData);

			return ret;
		}

		/// <summary>
		/// Draws the given Bitmap into a new Bitmap that has the indicated fax geometry. Always returns
		/// 32bpp pixel format.
		/// </summary>
		/// <param name="bmp">The source bitmap.</param>
		/// <param name="quality">The Fax Quality.  Default is Low.</param>
		/// <param name="paperSize">The Paper size.  Default is Auto.</param>
		/// <param name="interpolationMode">The Interpolation mode.  Default is High, but will use the current value in Image Utility, unless defined here. size.</param>
		/// <returns>The new PageImageList (always with 32bpp pixel format)</returns>
		internal static Bitmap CreateCopyFaxGeometry(Bitmap bmp, FaxQuality quality, PaperSize paperSize, InterpolationMode interpolationMode)
		{
			bool destroytemp = false;
			Bitmap tmp = null;
			Bitmap ret = null;

			if (bmp.PixelFormat == PixelFormat.Format8bppIndexed)
			{
				tmp = Convert8BppTo32Bpp(bmp); destroytemp = true;
			}
			else if (bmp.PixelFormat == PixelFormat.Format32bppArgb) { tmp = bmp; }
			else { tmp = BitmapHelper.CreateCopy32Bit(bmp); destroytemp = true; }
			//else if (bmp.PixelFormat != PixelFormat.Format32bppArgb) { tmp = bmp; }
			//else { tmp = BitmapHelper.CreateCopy32Bit(bmp); destroytemp = true; }

			ret = BitmapHelper.CreateBitMap(paperSize, quality, PixelFormat.Format32bppArgb);

			Graphics g = Graphics.FromImage(ret);
			g.InterpolationMode = interpolationMode;
			GraphicsUnit gu = GraphicsUnit.Pixel;
			g.DrawImage(tmp, ret.GetBounds(ref gu), tmp.GetBounds(ref gu), GraphicsUnit.Pixel);
			g.Dispose();
			if (destroytemp) { tmp.Dispose(); }
			return ret;
		}

		/// <summary>
		/// Creates a padded Fax page.  Source must be 1bpp.  Returned bitmap is 1bpp.
		/// </summary>
		/// <param name="bmp"></param>
		/// <param name="quality"></param>
		/// <param name="paperSize"></param>
		/// <returns></returns>
		public static Bitmap CreateCopyFaxGeometryPadding(Bitmap bmp, FaxQuality quality, PaperSize paperSize)
		{
			PageInfo inf = new PageInfo(bmp);
			if (!(inf.GetStandardFaxQuality == quality)) { throw new Exception("Fax quality must match source."); }
			if (!(inf.PixelFormat != PixelFormat.Format1bppIndexed)) { throw new Exception("Source bitmap must have 1bppIndexed format."); }

			if (!(inf.GetStandardPaperSize == paperSize))
			{
				return BitmapHelper.CreateCopyExact(bmp);
			}

			if (inf.GetStandardPaperSize == PaperSize.Legal && paperSize == PaperSize.Letter) { throw new Exception("Method cannnot reduce Paper size."); }

			Bitmap ret = BitmapHelper.CreateBitMap(paperSize, quality, PixelFormat.Format1bppIndexed);

			// Lock source bitmap in memory
			BitmapData sourceData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

			// Copy image data to binary array
			int imageSize = sourceData.Stride * sourceData.Height;
			byte[] sourceBuffer = new byte[imageSize];
			Marshal.Copy(sourceData.Scan0, sourceBuffer, 0, imageSize);

			// Unlock source bitmap
			bmp.UnlockBits(sourceData);

			// Lock destination bitmap in memory
			BitmapData destinationData = ret.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed);

			// Copy binary image data to destination bitmap
			Marshal.Copy(sourceBuffer, 0, destinationData.Scan0, imageSize);

			// Unlock destination bitmap
			ret.UnlockBits(destinationData);
			return ret;
		}

		/// <summary>
		/// Returns a 32bppArgb image that only contains black and white based on the given
		/// threshold.  The new copy is the same size and resolution as the source
		/// </summary>
		/// <param name="bmp">The source bitmap</param>
		/// <param name="threshold">The threshold to use.</param>
		/// <returns>The new Monochrome Bitmap</returns>
		public static Bitmap CreateCopyMonochrome(Bitmap bmp, int threshold)
		{
			// If source and destination are not 32 BPP, ARGB format, then throw error
			if (bmp.PixelFormat != PixelFormat.Format32bppArgb)
			{
				throw new Exception("Conversion to Monochrome requires that the source have PixelFormat.Format32bppArgb.");
			}

			Bitmap ret = BitmapHelper.CreateBitMap(bmp, bmp.PixelFormat);

			// Lock source bitmap in memory
			BitmapData sourceData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

			// Copy image data to binary array
			int imageSize = sourceData.Stride * sourceData.Height;
			byte[] sourceBuffer = new byte[imageSize];
			Marshal.Copy(sourceData.Scan0, sourceBuffer, 0, imageSize);

			// Unlock source bitmap
			bmp.UnlockBits(sourceData);

			int pixelTotal = sourceData.Height * sourceData.Width;
			byte destinationValue = 0;
			// Iterate pixels
			for (int i = 0; i < pixelTotal; i++)
			{
				pixelTotal = sourceBuffer[i * 4 + 1] + sourceBuffer[i * 4 + 2] + sourceBuffer[i * 4 + 3];

				if (pixelTotal > threshold)
				{
					destinationValue = 255;
				}
				else
				{
					destinationValue = 0;
				}

				sourceBuffer[i * 4 + 1] = destinationValue;
				sourceBuffer[i * 4 + 2] = destinationValue;
				sourceBuffer[i * 4 + 3] = destinationValue;
			}

			// Lock source bitmap in memory
			BitmapData destinationData = ret.LockBits(new Rectangle(0, 0, ret.Width, ret.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

			// Copy binary image data to destination bitmap
			Marshal.Copy(sourceBuffer, 0, destinationData.Scan0, imageSize);

			// Unlock destination bitmap
			ret.UnlockBits(destinationData);
			return ret;
		}

		/// <summary>
		/// Creates a copy of the given bitmap after rotation using the supplied intermediate resolution
		/// and Angle.  Positive Angle is Clockwise.  The resulting image has the same horizonatal 
		/// and vertical resolution as the given page, but geometry may have changed (height and width).
		/// </summary>
		/// <param name="bmp">The Bitmap to rotate</param>
		/// <param name="intermediateResolution">The intermediate resolution to use.</param>
		/// <param name="rotationAngle">The angle to rotate the image</param>
		/// <returns></returns>
		public static Bitmap CreateCopyRotate(Bitmap bmp, float rotationAngle)
		{
      
			PageInfo bmpinfo = new PageInfo(bmp);

			PointF[] pts = new PointF[4];
			pts[0] = new PointF(0, 0);
			pts[1] = new PointF(bmpinfo.WidthInches, 0);
			pts[2] = new PointF(0, bmpinfo.HeightInches);
			pts[3] = new PointF(bmpinfo.WidthInches, bmpinfo.HeightInches);

			Matrix mx = new Matrix();
			mx.RotateAt(rotationAngle, new Point(0, 0), MatrixOrder.Append);
			mx.TransformPoints(pts);

			float width;
			float height;
			float dx;
			float dy;

			BitmapHelper.GetExtents(pts, out width, out height, out dx, out dy);

      //If the overall image is bigger than 5000 pixels in either direction, then lets scale it by adjusting the Intermediate resolution - We'll scale it to about 4000 px.
      float intermediateResolution = Math.Max(bmp.VerticalResolution, bmp.HorizontalResolution);
      if (Math.Max(bmpinfo.WidthPixels, bmpinfo.HeightPixels) > 5000)
      {
        intermediateResolution = (float)(int)(4000 / Math.Max(bmpinfo.HeightInches, bmpinfo.WidthInches));
      }

			mx.Translate((int)(dx * intermediateResolution), (int)(dy * intermediateResolution), MatrixOrder.Append);
			//Flip width and height
			Bitmap temp = BitmapHelper.CreateBitMap(width, height, intermediateResolution, intermediateResolution, PixelFormat.Format32bppArgb);
			{
				Graphics g = Graphics.FromImage(temp);
				g.ResetTransform();
				g.Transform = mx;
				g.DrawImage(bmp, new Point(0, 0));
				g.Dispose();
			}
			Bitmap ret = BitmapHelper.CreateBitMap(width, height, bmpinfo.HorizontalResolution, bmpinfo.VerticalResolution, PixelFormat.Format32bppArgb);
			{
				Graphics g = Graphics.FromImage(ret);
				g.ResetTransform();
				g.DrawImage(temp, new Point(0, 0));
				g.Dispose();
			}
			temp.Dispose();

			return ret;
		}


		/// <summary>
		/// Creates a copy of the given bitmap after cropping white pixels from top and bottom of image.
		/// Requires a 1bitPerPixel image format, and must be a regular sized fax bitmap (1728 pixels wide).
		/// after rotation using the supplied intermediate resolution
		/// and Angle.  Positive Angle is Clockwise.  The resulting image has the same horizonatal 
		/// and vertical resolution as the given page, but geometry may have changed (height and width).
		/// </summary>
		/// <param name="bmp">The Bitmap to rotate</param>
		/// <param name="intermediateResolution">The intermediate resolution to use.</param>
		/// <param name="rotationAngle">The angle to rotate the image</param>
		/// <returns></returns>
		public static Bitmap CreateCopyCrop(Bitmap bmp)
		{
			PageInfo bmpinfo = new PageInfo(bmp);
			if(bmpinfo.WidthPixels != ImageUtility.FAX_TIF_HOR_PX && bmpinfo.HorizontalResolution != ImageUtility.FAX_TIF_HOR_RES)
			{
				throw new Exception("Bitmap is not the correct size for cropping.");
			}

			if (bmp.PixelFormat != PixelFormat.Format1bppIndexed)
			{
				throw new Exception("Bitmap is not the correct pixel format for cropping.");
			}

			// Lock source bitmap in memory
			BitmapData sourceData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format1bppIndexed);

			// Copy image data to binary array
			int imageSize = sourceData.Stride * sourceData.Height;
			byte[] sourceBuffer = new byte[imageSize];
			Marshal.Copy(sourceData.Scan0, sourceBuffer, 0, imageSize);

			// Unlock source bitmap
			bmp.UnlockBits(sourceData);


			int firstDataRow = BitmapHelper.ScanForFirstNonWhiteRow(sourceBuffer, sourceData.Stride, true, false);
			int lastDataRow = BitmapHelper.ScanForFirstNonWhiteRow(sourceBuffer, sourceData.Stride, false, true);

			int totalRows = lastDataRow - firstDataRow + 1;
			int newHeight = 0;

			var quality = FaxQuality.Undefined;
			var paper = PaperSize.Undefined;
			if (bmp.VerticalResolution == ImageUtility.FAX_TIF_VER_RES_HI)
			{
				if (totalRows <= ImageUtility.FAX_TIF_VER_PX_LTR_HI) { newHeight = ImageUtility.FAX_TIF_VER_PX_LTR_HI; paper = PaperSize.Letter; }
				else if (totalRows <= ImageUtility.FAX_TIF_VER_PX_LGL_HI) { newHeight = ImageUtility.FAX_TIF_VER_PX_LGL_HI;paper = PaperSize.Legal;}
				else { newHeight = ImageUtility.FAX_TIF_VER_PX_LGL_HI; paper = PaperSize.Legal;}
				quality = FaxQuality.Fine;
			}
			if (bmp.VerticalResolution == ImageUtility.FAX_TIF_VER_RES_LOW)
			{
				if (totalRows <= ImageUtility.FAX_TIF_VER_PX_LTR_LOW) { newHeight = ImageUtility.FAX_TIF_VER_PX_LTR_LOW; paper = PaperSize.Letter;}
				else if (totalRows <= ImageUtility.FAX_TIF_VER_PX_LGL_LOW) { newHeight = ImageUtility.FAX_TIF_VER_PX_LGL_LOW; paper = PaperSize.Legal;}
				else { newHeight = ImageUtility.FAX_TIF_VER_PX_LGL_LOW; paper = PaperSize.Legal;}
				quality = FaxQuality.Normal;
			}

			//Make sure the first data row does not go negative.
			firstDataRow = Math.Max(0, firstDataRow - ((newHeight - totalRows) / 2));
			//Make sure last does not extend out of the original document range.
			lastDataRow = Math.Min(Math.Max(0,bmp.Height - 1), Math.Max(0, firstDataRow + newHeight - 1));
			totalRows = lastDataRow - firstDataRow + 1;

			var startindex = firstDataRow * sourceData.Stride;
			var length = totalRows * sourceData.Stride;
			var croppedBitmap = BitmapHelper.CreateBitMap(paper, quality, PixelFormat.Format1bppIndexed);

			// Lock destination bitmap in memory
			BitmapData destinationData = croppedBitmap.LockBits(new Rectangle(0, 0, croppedBitmap.Width, croppedBitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed);

			// Copy binary image data to destination bitmap
			Marshal.Copy(sourceBuffer, startindex, destinationData.Scan0, length);

			// Unlock destination bitmap
			croppedBitmap.UnlockBits(destinationData);

			return croppedBitmap;
		}

		/// <summary>
		/// Returns the first row that has a non white row searching in idicated direction.  Returns 0 if not found.
		/// </summary>
		private static int ScanForFirstNonWhiteRow(byte[] sourceData, int stride, bool scanTopDown = true, bool scanBottomUp = false)
		{
			if (scanTopDown)
			{
				int firstDataRow = 1;
				bool done = false;
				while (!done)
				{
					int rowIdx = (firstDataRow - 1) * stride;
					for (int i = 0; i < stride; i++)
					{
						if (sourceData[rowIdx + i] != 255) { done = true; continue; }
					}
					firstDataRow++;
				}
				return firstDataRow;
			}

			if (scanBottomUp)
			{
				int lastDataRow = sourceData.Length / stride;
				bool done = false;
				while (!done)
				{
					int rowIdx = (lastDataRow -1) * stride;
					for (int i = 0; i < stride; i++)
					{
						if (sourceData[rowIdx + i] != 255) { done = true; continue; }
					}
					lastDataRow--;
				}
				return lastDataRow;
			}

			return 0;
		}
		#endregion

		#region Convert****  Functions
		/// <summary>
		/// Creates a new Bitmap with the resolution reduced by half in the vertical direction using
		/// the indicated method.  Source must be 1bppIndexed.  Source must be a standard Fax size.
		/// </summary>
		/// <param name="bmp">Source Bitmap.</param>
		/// <param name="scaleMethod">The scaling method.</param>
		/// <returns>The new bitmap in 1bppIndexed format.</returns>
		internal static Bitmap ConvertTiffHiToTiffLow(Bitmap bmp, HighToLowScaleMethod scaleMethod)
		{
			PaperSize paperSize = BitmapHelper.GetStandardPaperSize(bmp);

			//If the paper size is not standard then throw error
			if (paperSize == PaperSize.Undefined) { throw new Exception("Source bitmap returned an incorrect paper size."); }

			//If its not in 32bppArgb format then throw error
			if (bmp.PixelFormat != PixelFormat.Format1bppIndexed)
			{
				throw new Exception("Source format must be Format1bppIndexed.");
			}

			//If its already a low resolution, then copy and return.
			if (bmp.VerticalResolution == ImageUtility.FAX_TIF_VER_RES_LOW)
			{
				return BitmapHelper.CreateCopyExact(bmp);
			}

			//No Scale requested.
			if (scaleMethod == HighToLowScaleMethod.NoScale)
			{
				return BitmapHelper.CreateCopyExact(bmp);
			}

			//If its in tiff high resolution, then throw error
			if (bmp.VerticalResolution != ImageUtility.FAX_TIF_VER_RES_HI)
			{
				throw new Exception("Source resolution is not FAX_TIF_VER_RES_HI.");
			}

			Bitmap ret = BitmapHelper.CreateBitMap(paperSize, FaxQuality.Normal, PixelFormat.Format1bppIndexed);

			// Lock source bitmap in memory
			BitmapData sourceData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format1bppIndexed);

			// Copy image data to binary array
			int imageSize = sourceData.Stride * sourceData.Height;
			byte[] sourceBuffer = new byte[imageSize];
			Marshal.Copy(sourceData.Scan0, sourceBuffer, 0, imageSize);

			// Unlock source bitmap
			bmp.UnlockBits(sourceData);

			// Lock destination bitmap in memory
			BitmapData destinationData = ret.LockBits(new Rectangle(0, 0, ret.Width, ret.Height), ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed);

			// Create destination buffer
			imageSize = destinationData.Stride * destinationData.Height;
			byte[] destinationBuffer = new byte[imageSize];

			int nnx_ratio = (int)((sourceData.Stride << 16) / destinationData.Stride) + 1;
			int nny_ratio = (int)((sourceData.Height << 16) / destinationData.Height) + 1;

			int A, B, C, D, x, y, index, gray;
			float x_ratio = ((float)(sourceData.Stride - 1)) / destinationData.Stride;
			float y_ratio = ((float)(sourceData.Height - 1)) / destinationData.Height;
			float x_diff, y_diff;
			int offset = 0;

			for (int j = 0; j < destinationData.Height; j++)  //For every row
			{
				for (int i = 0; i < destinationData.Stride; i++)   //For each byte in row
				{
					byte s1 = sourceBuffer[(j * 2) * destinationData.Stride + i];
					byte s2 = sourceBuffer[((j * 2) + 1) * destinationData.Stride + i];
					byte d1 = 0;

					switch (scaleMethod)
					{
						case HighToLowScaleMethod.ORing:
							{
								d1 = (byte)(s1 | s2);
								destinationBuffer[j * destinationData.Stride + i] = d1;
								break;
							}
						case HighToLowScaleMethod.ANDing:
							{
								d1 = (byte)(s1 & s2);
								destinationBuffer[j * destinationData.Stride + i] = d1;
								break;
							}
						case HighToLowScaleMethod.Elimination:
							{
								d1 = s1;
								destinationBuffer[j * destinationData.Stride + i] = d1;
								break;
							}
						case HighToLowScaleMethod.Averaging:
							{
								//d1 = (byte)(s1 | s2);
								//d1 = (byte)(s1 ^ s2);
								d1 = (byte)((s1 + s2) / 2);
								//int a1s1 = s1 >> 6 & 3;
								//int a1s2 = s2 >> 6 & 3;
								//int a2s1 = s1 >> 4 & 3;
								//int a2s2 = s2 >> 4 & 3;
								//int a3s1 = s1 >> 2 & 3;
								//int a3s2 = s2 >> 2 & 3;
								//int a4s1 = s1 & 3;
								//int a4s2 = s2 & 3;
								//d1 = (byte)(((a1s1 + a1s2) / 2 << 6) + ((a2s1 + a2s2) / 2 << 4) + ((a3s1 + a3s2) / 2 << 2) + ((a4s1 + a4s2) / 2));
								destinationBuffer[j * destinationData.Stride + i] = d1;
								break;
							}
						case HighToLowScaleMethod.NearestNeighbor:
							{
								int x2 = ((i * nnx_ratio) >> 16);
								int y2 = ((j * nny_ratio) >> 16);

								destinationBuffer[j * destinationData.Stride + i] = sourceBuffer[(y2 * sourceData.Stride) + x2];
								break;
							}
						case HighToLowScaleMethod.Bilinear:
							{
								x = (int)(x_ratio * i);
								y = (int)(y_ratio * j);
								x_diff = (x_ratio * i) - x;
								y_diff = (y_ratio * j) - y;
								index = y * sourceData.Stride + x;

								A = sourceBuffer[index] & 0xff;
								B = sourceBuffer[index + 1] & 0xff;
								C = sourceBuffer[index + sourceData.Stride] & 0xff;
								D = sourceBuffer[index + sourceData.Stride + 1] & 0xff;
								gray = (int)(
									 A * (1 - x_diff) * (1 - y_diff) + B * (x_diff) * (1 - y_diff) +
										C * (y_diff) * (1 - x_diff) + D * (x_diff * y_diff)
									);

								destinationBuffer[offset++] = (byte)gray;
								break;
							}
					}
				}
			}

			// Copy binary image data to destination bitmap
			Marshal.Copy(destinationBuffer, 0, destinationData.Scan0, imageSize);

			// Unlock destination bitmap
			ret.UnlockBits(destinationData);
			return ret;
		}

		/// <summary>
		/// Creates a new Bitmap with the resolution increased by 2 in the vertical direction using
		/// the indicated method.  Source must be 1bppIndexed.  Source must be a standard Fax size.
		/// </summary>
		/// <param name="bmp">Source Bitmap.</param>
		/// <returns>The new bitmap in 1bppIndexed format.</returns>
		internal static Bitmap ConvertTiffLowToTiffHi(Bitmap bmp)
		{
			PageInfo inf = new PageInfo(bmp);

			if (!inf.IsStandardFaxTiff) { throw new Exception("Source bitmap must be a standard fax tiff."); }
			if (inf.PixelFormat != PixelFormat.Format1bppIndexed) { throw new Exception("Source bitmap must be 1bpp."); }

			//If its already a hi resolution, then copy and return.
			if (inf.VerticalResolution == ImageUtility.FAX_TIF_VER_RES_HI)
			{
				return BitmapHelper.CreateCopyExact(bmp);
			}

			Bitmap ret = BitmapHelper.CreateBitMap(inf.GetStandardPaperSize, FaxQuality.Fine, PixelFormat.Format1bppIndexed);

			// Lock source bitmap in memory
			BitmapData sourceData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format1bppIndexed);

			// Copy image data to binary array
			int imageSize = sourceData.Stride * sourceData.Height;
			byte[] sourceBuffer = new byte[imageSize];
			Marshal.Copy(sourceData.Scan0, sourceBuffer, 0, imageSize);

			// Unlock source bitmap
			bmp.UnlockBits(sourceData);

			// Lock destination bitmap in memory
			BitmapData destinationData = ret.LockBits(new Rectangle(0, 0, ret.Width, ret.Height), ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed);

			// Copy image data to binary array
			byte[] destinationBuffer = new byte[imageSize * 2];
			int destinationIndex = 0;

			for (int j = 0; j < sourceData.Height; j++)  //For every row
			{
				for (int k = 0; k < 2; k++) //Do it twice
				{
					for (int i = 0; i < sourceData.Stride; i++) //For every element
					{
						byte s1 = sourceBuffer[(j) * sourceData.Stride + i];
						destinationBuffer[destinationIndex] = s1;
						destinationIndex++;
					}
				}
			}

			// Copy binary image data to destination bitmap
			Marshal.Copy(destinationBuffer, 0, destinationData.Scan0, destinationBuffer.Length);

			// Unlock destination bitmap
			ret.UnlockBits(destinationData);
			return ret;
		}

		internal static Bitmap Convert8BppTo32Bpp(Bitmap bmp)
		{
			if (bmp.PixelFormat != PixelFormat.Format8bppIndexed) { throw new Exception("PixelFormat must be Format8bppIndexed."); }

			//Grayscale value (0-255) = R = G = B
			//So copy grayscale value to R/G/B bytes, setting A to 255
			BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
			int imageSize = bmpData.Stride * bmpData.Height;
			byte[] bmpBuffer = new byte[imageSize];
			Marshal.Copy(bmpData.Scan0, bmpBuffer, 0, imageSize);
			bmp.UnlockBits(bmpData);

			Bitmap destBmp = BitmapHelper.CreateBitMap(bmp, PixelFormat.Format32bppArgb);
			BitmapData destData = destBmp.LockBits(new Rectangle(0, 0, destBmp.Width, destBmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
			imageSize = destData.Stride * destData.Height;
			byte[] destBuffer = new byte[imageSize];

			Color[] colorArray = new Color[bmp.Palette.Entries.Length];
			for (int i = 0; i < bmp.Palette.Entries.Length; i++) { colorArray[i] = bmp.Palette.Entries[i]; } //Create array of palette for quick lookup below

			for (int y = 0; y < bmp.Height; y++)
			{
				for (int x = 0; x < bmp.Width; x++)
				{
					destBuffer[y * destData.Stride + (x * 4)]     = colorArray[bmpBuffer[y * bmpData.Stride + x]].B; //B //bmpBuffer[y * bmpData.Stride + x];
					destBuffer[y * destData.Stride + (x * 4) + 1] = colorArray[bmpBuffer[y * bmpData.Stride + x]].G; //G //bmpBuffer[y * bmpData.Stride + x];
					destBuffer[y * destData.Stride + (x * 4) + 2] = colorArray[bmpBuffer[y * bmpData.Stride + x]].R; //R //bmpBuffer[y * bmpData.Stride + x];
					destBuffer[y * destData.Stride + (x * 4) + 3] = colorArray[bmpBuffer[y * bmpData.Stride + x]].A; //A //255;
				}
			}

			Marshal.Copy(destBuffer, 0, destData.Scan0, imageSize);
			destBmp.UnlockBits(destData);
			return destBmp;
		}

		#endregion


		private static void GetExtents(PointF[] pts, out float Width, out float Height, out float dx, out float dy)
		{
			float minX = pts[0].X;
			float maxX = pts[0].X;
			float minY = pts[0].Y;
			float maxY = pts[0].Y;

			foreach (PointF pt in pts)
			{
				if (pt.X < minX) { minX = pt.X; }
				if (pt.X > maxX) { maxX = pt.X; }
				if (pt.Y < minY) { minY = pt.Y; }
				if (pt.Y > maxY) { maxY = pt.Y; }
			}

			Width = maxX - minX;
			Height = maxY - minY;

			dx = -minX;
			dy = -minY;
		}
		
	}
}
