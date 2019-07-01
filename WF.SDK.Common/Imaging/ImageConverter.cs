using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;


namespace WF.SDK.Common.Imaging
{
	/// <summary>
	/// Summary description for ImageConverter.
	/// </summary>
	public class ImageConverter
	{
		private const string MODNAME = "WF.ImageTools.ImageConverter";
		//Static Only
		private ImageConverter() { }

		/// <summary>
		/// Receives a page image list and returns a faxable page image list.
		/// </summary>
		/// <param name="pages"></param>
		/// <param name="paperSize"></param>
		/// <param name="faxQuality"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		internal static List<PageImage> CreateFaxTiff(List<PageImage> pages, PaperSize paperSize, FaxQuality faxQuality, ImageOperationResult result)
		{
			paperSize = ImageConverter.GetBestFitPaperSizeForList(pages, paperSize);
			if (faxQuality == FaxQuality.Default) { faxQuality = FaxQuality.Normal; }
			if (faxQuality == FaxQuality.Undefined) { faxQuality = FaxQuality.Normal; }

			bool fastTrack = CanFastTrackPageImageList(pages, paperSize);

			List<PageImage> ret = new List<PageImage>();

			for (int i = 0; i < pages.Count; i++)
			{
				ret.Add(CreateFaxTiff(pages[i], paperSize, faxQuality, result, fastTrack));
			}
			return ret;
		}

		private static bool CanFastTrackPageImageList(List<PageImage> pages, PaperSize paperSize)
		{
			bool fastTrack = true;
			PaperSize paperSizeCheck = pages[0].PageInfo.GetStandardPaperSize;
			foreach (PageImage pi in pages)
			{
				//if (pi.PageInfo.GetStandardPaperSize != PaperSize.Auto
				//  && pi.PageInfo.GetStandardPaperSize != paperSizeCheck) { fastTrack = false; break; }

				if (pi.PageInfo.GetStandardPaperSize != paperSizeCheck) { fastTrack = false; break; }

				if (pi.PageInfo.GetStandardPaperSize == PaperSize.Undefined) { fastTrack = false; break; }
			}

			return fastTrack;
		}

		private static PageImage CreateFaxTiff(PageImage page, PaperSize paperSize, FaxQuality faxQuality, ImageOperationResult result, bool fastTrack)
		{
			PageImage ret = new PageImage();
			//FastTrack
			//if(page.PageInfo.IsStandardFaxTiff)
			if (fastTrack)
			{
				Trace.WriteLine("FastTracking tiff creation", MODNAME);
				return CreateFaxTiffFastTrack(page, paperSize, faxQuality, result);
			}
			else
			{
				Trace.WriteLine("SlowTracking tiff creation", MODNAME);
				return CreateFaxTiffSlowTrack(page, paperSize, faxQuality, result);
			}
		}

		private static PageImage CreateFaxTiffFastTrack(PageImage page, PaperSize paperSize, FaxQuality faxQuality, ImageOperationResult result)
		{
			PageInfo inf = null;
			PageImage ret = new PageImage();
			Bitmap src = null;
			Bitmap destroy = null;

			src = BitmapHelper.CreateCopy1BppIndexed(page._sourceBmp);
			inf = new PageInfo(src);

			//If the size is not right copy to other size (padding or reducing)
			if (inf.GetStandardPaperSize != paperSize)
			{
				if (inf.GetStandardPaperSize == PaperSize.Legal && paperSize == PaperSize.Letter)
				{
					destroy = src;
					src = BitmapHelper.CreateCopyFaxGeometry(src, faxQuality, paperSize, ImageUtility.InterpolationMode);
					if (destroy != null) { destroy.Dispose(); destroy = null; }
					inf = new PageInfo(src);
				}
				if (inf.GetStandardPaperSize == PaperSize.Letter && paperSize == PaperSize.Legal)
				{
					destroy = src;
					src = BitmapHelper.CreateCopyFaxGeometryPadding(src, faxQuality, paperSize);
					if (destroy != null) { destroy.Dispose(); destroy = null; }
					inf = new PageInfo(src);
				}
			}

			//Make sure its 1bpp
			if (inf.PixelFormat != PixelFormat.Format1bppIndexed)
			{
				destroy = src;
				src = BitmapHelper.CreateCopy1BppIndexed(src);
				if (destroy != null) { destroy.Dispose(); destroy = null; }
				inf = new PageInfo(src);
			}

			//Reduce or increase quality as needed
			if (inf.GetStandardFaxQuality != faxQuality)
			{
				if (inf.GetStandardFaxQuality == FaxQuality.Fine && faxQuality == FaxQuality.Normal)
				{
					destroy = src;
					src = BitmapHelper.ConvertTiffHiToTiffLow(src, ImageUtility.HighToLowScaleMethod);
					if (destroy != null) { destroy.Dispose(); destroy = null; }
					inf = new PageInfo(src);
				}
				if (inf.GetStandardFaxQuality == FaxQuality.Normal && faxQuality == FaxQuality.Fine)
				{
					destroy = src;
					src = BitmapHelper.ConvertTiffLowToTiffHi(src);
					if (destroy != null) { destroy.Dispose(); destroy = null; }
					inf = new PageInfo(src);
				}
			}

			ret._pageInfo = null;
			ret._sourceBmp = src;

			return ret;
		}

		private static PageImage CreateFaxTiffSlowTrack(PageImage page, PaperSize paperSize, FaxQuality faxQuality, ImageOperationResult result)
		{
			PageInfo inf = null;
			PageImage ret = new PageImage();
			Bitmap src = null;
			Bitmap destroy = null;

			Trace.WriteLine("SlowTrack: CreateCopyExact...", MODNAME);
			src = BitmapHelper.CreateCopyExact(page._sourceBmp);
			Trace.WriteLine("SlowTrack: CreateCopyExact done.", MODNAME);
			inf = new PageInfo(src);

			if (inf.GetBestFitRotation != 0)
			{
				Trace.WriteLine("SlowTrack: Rotating...", MODNAME);
				destroy = src;
        src = BitmapHelper.CreateCopyRotate(src, 90);
				if (destroy != null) { destroy.Dispose(); destroy = null; }
				inf = new PageInfo(src);
				Trace.WriteLine("SlowTrack: Rotating done.", MODNAME);
			}
			
			destroy = src;
			Trace.WriteLine("SlowTrack: CreateCopyFaxGeometry...", MODNAME);
			src = BitmapHelper.CreateCopyFaxGeometry(src, faxQuality, paperSize, ImageUtility.InterpolationMode);
			Trace.WriteLine("SlowTrack: CreateCopyFaxGeometry done.", MODNAME);
			if (destroy != null) { destroy.Dispose(); destroy = null; }
			inf = new PageInfo(src);

			destroy = src;
			Trace.WriteLine("SlowTrack: CreateCopy1BppIndexed: " + ImageUtility.ConvertTo1BppMethod.ToString() + "...", MODNAME);
			src = BitmapHelper.CreateCopy1BppIndexed(src);
			Trace.WriteLine("SlowTrack: CreateCopy1BppIndexed done.", MODNAME);
			if (destroy != null) { destroy.Dispose(); destroy = null; }
			inf = new PageInfo(src);

			ret._pageInfo = null;
			ret._sourceBmp = src;
			return ret;
		}

		internal static List<PageImage> ConvertToFaxablePageImageList(List<PageImage> pages, FaxQuality quality, PaperSize paperSize, ImageOperationResult result)
		{
			if (pages.Count == 0) { return new List<PageImage>(); }

			FaxQuality targetquality = quality;
			PaperSize targetsize = ImageConverter.GetBestFitPaperSizeForList(pages, PaperSize.Auto);

			List<PageImage> ret = new List<PageImage>();
			for (int i = 0; i < pages.Count; i++)
			{
				ret.Add(new PageImage());
			}

			for (int i = 0; i < ret.Count; i++)
			{
				ret[i] = CreateFaxTiffSlowTrack(pages[i], targetsize, targetquality, result);
			}

			return ret;
		}

		#region ConvertPixelFormat
		/// <summary>
		/// Creates a new PageImageList containing pages with the new pixel format.  The 
		/// source PageImageList is not altered.
		/// </summary>
		/// <param name="pages">The PageImageList to alter.</param>
		/// <param name="pixelFormat">The pixel format to use.</param>
		/// <returns>The new PageImageList.</returns>
		private static List<PageImage> ConvertPixelFormat(List<PageImage> pages, PixelFormat pixelFormat)
		{
			return ImageConverter.ConvertPixelFormat(pages, pixelFormat, 500);
		}

		/// <summary>
		/// Creates a new PageImageList containing pages with the new pixel format.  The 
		/// source PageImageList is not altered.
		/// </summary>
		/// <param name="pages">The PageImageList to alter.</param>
		/// <param name="pixelFormat">The pixel format to use.</param>
		/// /// <param name="threshold">The threshold to use for converting to 1bpp. Default is 500.</param>
		/// <returns>The new PageImageList.</returns>
		private static List<PageImage> ConvertPixelFormat(List<PageImage> pages, PixelFormat pixelFormat, int threshold)
		{
			List<PageImage> ret = new List<PageImage>();

			//If there are no images return.
			if (pages.Count == 0) { return ret; }

			bool consistentPixelFormat = ImageConverter.IsListPixelFormatConsistent(pages);
			PixelFormat pxfmt = pages[0].PageInfo.PixelFormat;

			if (consistentPixelFormat && pxfmt == pixelFormat)
			{
				return ImageConverter.CreateDeepCopy(pages);
			}

			if (pixelFormat == PixelFormat.Format1bppIndexed)  //Indexed format, need to do something special
			{
				ret = ImageConverter.CreateEmptyPageImageList(pages, pixelFormat);
				for (int i = 0; i < pages.Count; i++)
				{
					ConvertPixelFormatTo1bppIndexed(pages[i], ret[i]);
				}
				return ret;
			}
			else
			{
				ret = ImageConverter.CreateEmptyPageImageList(pages, pixelFormat);
				ImageConverter.DrawSourceToDestination(pages, ret);
				return ret;
			}
		}

		private static void ConvertPixelFormatTo1bppIndexed(PageImage source, PageImage destination)
		{

			// Lock source bitmap in memory
			BitmapData sourceData = source.Bitmap.LockBits(new Rectangle(0, 0, source.Bitmap.Width, source.Bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

			// Copy image data to binary array
			int imageSize = sourceData.Stride * sourceData.Height;
			byte[] sourceBuffer = new byte[imageSize];
			Marshal.Copy(sourceData.Scan0, sourceBuffer, 0, imageSize);

			// Unlock source bitmap
			source.Bitmap.UnlockBits(sourceData);

			// Lock destination bitmap in memory
			BitmapData destinationData = destination.Bitmap.LockBits(new Rectangle(0, 0, destination.Bitmap.Width, destination.Bitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed);

			// Create destination buffer
			imageSize = destinationData.Stride * destinationData.Height;
			byte[] destinationBuffer = new byte[imageSize];

			int sourceIndex = 0;
			int destinationIndex = 0;
			int pixelTotal = 0;
			byte destinationValue = 0;
			int pixelValue = 128;
			int height = source.Bitmap.Height;
			int width = source.Bitmap.Width;
			int threshold = 500;

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
			destination.Bitmap.UnlockBits(destinationData);
		}


		#endregion

		#region ApplyMonochromeFilter

		/// <summary>
		/// Creates a new PageImageList containing pages with only two colors (Black and White).
		/// The returned pixel format will be the same as the source pixel format.
		/// The given page image list is not altered.
		/// </summary>
		/// <param name="pages">The PageImageList to alter.</param>
		/// <param name="threshold">The threshold to use for converting to bitonal. Default is 500.</param>
		/// <returns>The new PageImageList.</returns>
		private static List<PageImage> ApplyMonochromeFilter(List<PageImage> pages, int threshold = 500)
		{
			//Get out if source is empty
			if (pages.Count == 0) { return new List<PageImage>(); }

			List<PageImage> ret = ImageConverter.CreateEmptyPageImageList(pages, PixelFormat.Format32bppArgb);

			List<PageImage> source = null;

			//If the pages are consistent 32 bit then continue
			bool consistentPixelFormat = ImageConverter.IsListPixelFormatConsistent(pages);
			if (consistentPixelFormat && pages[0].PageInfo.PixelFormat == PixelFormat.Format32bppArgb)
			{
				source = pages;
				for (int i = 0; i < source.Count; i++)
				{
					ImageConverter.ApplyMonochromeFilter(source[i], ret[i], threshold);
				}
			}
			else
			{
				source = ImageConverter.CreateEmptyPageImageList(pages, PixelFormat.Format32bppArgb);
				for (int i = 0; i < pages.Count; i++)
				{
					ImageConverter.DrawSourcePageToDestination(pages[i], source[i], ImageUtility.InterpolationMode);
				}
				for (int i = 0; i < source.Count; i++)
				{
					ImageConverter.ApplyMonochromeFilter(source[i], ret[i], threshold);
				}
				ImageUtility.Dispose(source);
			}

			return ret;
		}

		private static void ApplyMonochromeFilter(PageImage source, PageImage destination, int threshold)
		{
			// If source and destination are not 32 BPP, ARGB format, then throw error
			if (source.Bitmap.PixelFormat != PixelFormat.Format32bppArgb && destination.Bitmap.PixelFormat != PixelFormat.Format32bppArgb)
			{
				throw new Exception("Conversion to Monochrome requires that the source and destination both have PixelFormat.Format32bppArgb.");
			}

			// If source and destination are not the same size then throw error
			if (source.PageInfo.WidthPixels != destination.PageInfo.WidthPixels || source.PageInfo.HeightPixels != destination.PageInfo.HeightPixels)
			{
				throw new Exception("Conversion to Monochrome requires that the source and destination are both the same size.");
			}

			// Lock source bitmap in memory
			BitmapData sourceData = source.Bitmap.LockBits(new Rectangle(0, 0, source.Bitmap.Width, source.Bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

			// Copy image data to binary array
			int imageSize = sourceData.Stride * sourceData.Height;
			byte[] sourceBuffer = new byte[imageSize];
			Marshal.Copy(sourceData.Scan0, sourceBuffer, 0, imageSize);

			// Unlock source bitmap
			source.Bitmap.UnlockBits(sourceData);

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
			BitmapData destinationData = destination.Bitmap.LockBits(new Rectangle(0, 0, destination.Bitmap.Width, destination.Bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

			// Copy binary image data to destination bitmap
			Marshal.Copy(sourceBuffer, 0, destinationData.Scan0, imageSize);

			// Unlock destination bitmap
			destination.Bitmap.UnlockBits(destinationData);
		}

		#endregion

		#region ConvertToFaxGeometry
		/// <summary>
		/// Creates a new PageImageList containing pages that have a fax geometry.
		/// </summary>
		/// <param name="pages">The PageImageList to alter.</param>
		/// <param name="quality">The Fax Quality.  Default is Low.</param>
		/// <returns>The new PageImageList (always with 32bpp pixel format)</returns>
		private static List<PageImage> ConvertToFaxGeometry(List<PageImage> pages, FaxQuality quality = FaxQuality.Normal)
		{
			return ImageConverter.ConvertToFaxGeometry(pages, quality, ImageConverter.GetBestFitPaperSizeForList(pages, PaperSize.Auto));
		}


		/// <summary>
		/// Creates a new PageImageList containing pages that have a fax geometry.
		/// </summary>
		/// <param name="pages">The PageImageList to alter.</param>
		/// <param name="quality">The Fax Quality.  Default is Low.</param>
		/// <param name="paperSize">The Paper size.  Default is Auto.</param>
		/// <returns>The new PageImageList (always with 32bpp pixel format)</returns>
		private static List<PageImage> ConvertToFaxGeometry(List<PageImage> pages, FaxQuality quality, PaperSize paperSize)
		{
			return ImageConverter.ConvertToFaxGeometry(pages, quality, paperSize, ImageUtility.InterpolationMode);
		}

		/// <summary>
		/// Creates a new PageImageList containing pages that have a fax geometry.
		/// </summary>
		/// <param name="pages">The PageImageList to alter.</param>
		/// <param name="quality">The Fax Quality.  Default is Low.</param>
		/// <param name="paperSize">The Paper size.  Default is Auto.</param>
		/// <param name="interpolationMode">The Interpolation mode.  Default is High, but will use the current value in Image Utility, unless defined here. size.</param>
		/// <returns>The new PageImageList (always with 32bpp pixel format)</returns>
		private static List<PageImage> ConvertToFaxGeometry(List<PageImage> pages, FaxQuality quality, PaperSize paperSize, InterpolationMode interpolationMode)
		{
			List<PageImage> ret = ImageConverter.CreateEmptyPageImageList(pages.Count, PixelFormat.Format32bppArgb, pages[0].Bitmap.Palette, quality, paperSize);
			for (int i = 0; i < pages.Count; i++)
			{
				ImageConverter.ConvertToFaxGeometry(pages[i], ret[i], interpolationMode);
			}
			return ret;
		}

		private static void ConvertToFaxGeometry(PageImage source, PageImage destination, InterpolationMode interpolationMode)
		{
			Graphics g = Graphics.FromImage(destination.Bitmap);
			g.InterpolationMode = interpolationMode;
			GraphicsUnit gu = GraphicsUnit.Pixel;
			g.DrawImage(source.Bitmap, destination.Bitmap.GetBounds(ref gu), source.Bitmap.GetBounds(ref gu), GraphicsUnit.Pixel);
			g.Dispose();
		}

		#endregion


		/// <summary>
		/// Copies an existing PageImage List, including pixel data.  PixelFormat,
		/// Resolution, and color depth are not affected.
		/// </summary>
		/// <param name="list">The list to copy.</param>
		/// <returns>The new page image list.</returns>
		private static List<PageImage> CreateDeepCopy(List<PageImage> list)
		{
			return ImageUtility.LoadImage(list);
		}



		/// <summary>
		/// Creates a new PageImageList that contains empty images (no pixel data) with the 
		/// appropriate PixelFormat.  Image sizes and resolutions will be the same as the given list.
		/// </summary>
		/// <param name="list">The list to copy.</param>
		/// <param name="pixelFormat">The new pixel format.</param>
		/// <returns>The new page image list.</returns>
		private static List<PageImage> CreateEmptyPageImageList(List<PageImage> list, PixelFormat pixelFormat)
		{
			List<PageImage> ret = new List<PageImage>();
			//Get out if not pages in list.
			if (list.Count == 0) { return ret; }

			foreach (PageImage page in list)
			{
				PageImage newpage = ImageConverter.CreateEmptyPageImage(page, pixelFormat);
				ret.Add(newpage);
			}
			return ret;
		}

		/// <summary>
		/// Creates a new PageImage that contains an empty image (no pixel data) with the 
		/// appropriate PixelFormat.  Image size and resolution will be the same as the PageImage.
		/// </summary>
		/// <param name="page">The page image to copy.</param>
		/// <param name="pixelFormat">The new pixel format.</param>
		/// <returns>The new page image.</returns>
		private static PageImage CreateEmptyPageImage(PageImage page, PixelFormat pixelFormat)
		{
			PageImage newpage = new PageImage();
			newpage._sourceBmp = BitmapHelper.CreateBitMap(page.PageInfo.WidthPixels, page.PageInfo.HeightPixels, page.PageInfo.HorizontalResolution, page.PageInfo.VerticalResolution, page.Bitmap.Palette, pixelFormat);
			return newpage;
		}

		/// <summary>
		/// Creates a new PageImageList that contains empty images (no pixel data) with the 
		/// appropriate PixelFormat, Size and Resolution.
		/// </summary>
		/// <param name="pageCount">Page count to get.</param>
		/// <param name="pixelFormat">The new pixel format.</param>
		/// <param name="quality">The fax quality to use (sets resolution)</param>
		/// <param name="paperSize">The page size to use (width and height)</param>
		/// <returns>The new page image list.</returns>
		private static List<PageImage> CreateEmptyPageImageList(int pageCount, PixelFormat pixelFormat, ColorPalette palette, FaxQuality quality, PaperSize paperSize)
		{
			List<PageImage> ret = new List<PageImage>();
			//Get out if not pages in list.
			if (pageCount <= 0) { return ret; }

			int width = ImageUtility.FAX_TIF_HOR_PX;
			float hres = ImageUtility.FAX_TIF_HOR_RES;

			int height = 0;
			float vres = 0.0F;

			if (quality == FaxQuality.Fine)
			{
				vres = ImageUtility.FAX_TIF_VER_RES_HI;
				if (paperSize == PaperSize.Legal) { height = ImageUtility.FAX_TIF_VER_PX_LGL_HI; }
				else { height = ImageUtility.FAX_TIF_VER_PX_LTR_HI; }
			}
			else
			{
				vres = ImageUtility.FAX_TIF_VER_RES_LOW;
				if (paperSize == PaperSize.Legal) { height = ImageUtility.FAX_TIF_VER_PX_LGL_LOW; }
				else { height = ImageUtility.FAX_TIF_VER_PX_LTR_LOW; }
			}

			for (int i = 0; i < pageCount; i++)
			{
				PageImage newpage = new PageImage();
				newpage._sourceBmp = BitmapHelper.CreateBitMap(width, height, hres, vres, palette, pixelFormat);
				ret.Add(newpage);
			}
			return ret;
		}

		/// <summary>
		/// Creates a new PageImageList that contains empty images (no pixel data) with the 
		/// appropriate PixelFormat, Size and Resolution.
		/// </summary>
		/// <param name="pageCount">Page count to get.</param>
		/// <param name="pixelFormat">The new pixel format.</param>
		/// <param name="quality">The fax quality to use (sets resolution)</param>
		/// <param name="paperSize">The page size to use (width and height)</param>
		/// <returns>The new page image list.</returns>
		internal static List<PageImage> CreateCroppedPageImageList(List<PageImage> list)
		{
			List<PageImage> ret = new List<PageImage>();

			foreach (PageImage img in list)
			{
				PageImage newpage = new PageImage();
				newpage._sourceBmp = BitmapHelper.CreateCopyCrop(img._sourceBmp);
				ret.Add(newpage);
			}
			return ret;
		}

		private static void DrawSourceToDestination(List<PageImage> source, List<PageImage> destination)
		{
			ImageConverter.DrawSourceToDestination(source, destination, ImageUtility.InterpolationMode);
		}

		private static void DrawSourceToDestination(List<PageImage> source, List<PageImage> destination, InterpolationMode interpolationMode)
		{
			if (source.Count == 0) { return; }
			for (int i = 0; i < source.Count; i++)
			{
				ImageConverter.DrawSourcePageToDestination(source[i], destination[i], interpolationMode);
			}
		}

		private static void DrawSourcePageToDestination(PageImage source, PageImage destination, InterpolationMode interpolationMode)
		{
			Graphics g = Graphics.FromImage(destination.Bitmap);
			g.InterpolationMode = interpolationMode;
			GraphicsUnit gu = GraphicsUnit.Pixel;
			g.DrawImage(source.Bitmap, destination.Bitmap.GetBounds(ref gu), source.Bitmap.GetBounds(ref gu), GraphicsUnit.Pixel);
			g.Dispose();
		}

		/// <summary>
		/// When paper size is Auto, determines the appropriate fax paper size for the 
		/// all images contained in the imageInfo object. If paper size is not Auto, then it
		/// will returns the paper size.
		/// </summary>
		/// <param name="pages">The pages to check</param>
		/// <param name="paperSize">The paper size</param>
		private static PaperSize GetBestFitPaperSizeForList(List<PageImage> pages, PaperSize paperSize)
		{
			//If it's not auto then fix it to the requested size
			if (paperSize == PaperSize.Legal) { return PaperSize.Legal; }
			if (paperSize == PaperSize.Letter) { return PaperSize.Letter; }

			//Track the bigest that we find.
			PaperSize biggest = PaperSize.Letter;
			//Otherwise, attempt to determine the best size
			foreach (PageImage page in pages)
			{
				if (page.PageInfo.GetBestFitPaperSize == PaperSize.Legal)
				{
					biggest = PaperSize.Legal;
					break;
				}
			}

			return biggest;
		}

		/// <summary>
		/// Checks for the consistency of the Pixel format in each page in the Page Image list.
		/// If the list contains 0 or 1 page, always returns true.
		/// </summary>
		/// <param name="list">The list to check.</param>
		/// <returns>Whether it is consistent or not.</returns>
		private static bool IsListPixelFormatConsistent(List<PageImage> list)
		{
			bool ret = true;
			if (list.Count == 0) { return ret; }
			if (list.Count == 1) { return ret; }
			PixelFormat pxfmt = list[0].PageInfo.PixelFormat;
			foreach (PageImage page in list)
			{
				if (page.PageInfo.PixelFormat != pxfmt) { ret = false; }
			}
			return ret;
		}

		/// <summary>
		/// Checks for the consistency of the Pixel format in each page in the Page Image list.
		/// If the list contains 0 or 1 page, always returns true.
		/// </summary>
		/// <param name="list">The list to check.</param>
		/// <returns>Whether it is consistent or not.</returns>
		private static bool IsListStandardFax(List<PageImage> list)
		{
			bool ret = true;
			foreach (PageImage page in list)
			{
				if (!page.PageInfo.IsStandardFaxTiff) { ret = false; break; }
			}
			return ret;
		}

		/// <summary>
		/// Checks for the consistency of the Pixel format in each page in the Page Image list.
		/// If the list contains 0 or 1 page, always returns true.
		/// </summary>
		/// <param name="pages">The list to check.</param>
		/// <returns>Whether it is consistent or not.</returns>
		//internal static bool IsListPaperSizeConsistent(PageImageList pages)
		//{
		//  bool ret = false;
		//  if(pages.Count == 0){return ret;}
		//  if(!ImageConverter.IsListStandardFax(pages)){return ret;}

		//  PaperSize paperSize = PaperSize.Letter;
		//  paperSize = pages[0].PageInfo.GetStandardPaperSize;
		//  ret = true;
		//  foreach(PageImage page in pages)
		//  {
		//    if(paperSize != page.PageInfo.GetStandardPaperSize){ret = false;break;}
		//  }
		//  return ret;
		//}

	}
}
