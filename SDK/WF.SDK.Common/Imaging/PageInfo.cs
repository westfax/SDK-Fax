using System;
using System.Drawing;
using System.Drawing.Imaging;


namespace WF.SDK.Common.Imaging
{
	/// <summary>
	/// Summary description for ImageDiscovery.
	/// </summary>
	public class PageInfo
	{
		public float HorizontalResolution = 96;
		public int WidthPixels = 0;
		public float WidthInches = 0;

		public float VerticalResolution = 96;
		public int HeightPixels = 0;
		public float HeightInches = 0;

		public float AspectRatio = 0;

		public ImageFlags ImageFlags = 0;

		public PixelFormat PixelFormat = PixelFormat.Undefined;

		public ImageFormat ImageFormat = null;
		public ImageFormatNames ImageFormatName = ImageFormatNames.Undefined;

		public PaletteFlags PaletteFlags = 0;
		public Color[] PaletteEntries = new Color[0];

		public ImageProperty[] ImageProperties = new ImageProperty[0];

		public object Tag = null;

		internal PageInfo(Image img)
		{
			this.HorizontalResolution = img.HorizontalResolution;
			this.WidthPixels = (int)img.PhysicalDimension.Width;
			this.WidthInches = img.PhysicalDimension.Width / this.HorizontalResolution;

			this.VerticalResolution = img.VerticalResolution;
			this.HeightPixels = (int)img.PhysicalDimension.Height;
			this.HeightInches = img.PhysicalDimension.Height / this.VerticalResolution;

			this.AspectRatio = this.WidthInches / this.HeightInches;

			this.ImageFlags = (ImageFlags)img.Flags;

			this.PixelFormat = img.PixelFormat;

			this.ImageFormat = ImageHelper.GetImageFormat_Instance(img.RawFormat.Guid);
			this.ImageFormatName = ImageHelper.GetImageFormat_Name(img);

			this.PaletteFlags = (PaletteFlags)img.Palette.Flags;
			this.PaletteEntries = img.Palette.Entries;

			this.ImageProperties = new ImageProperty[img.PropertyIdList.Length];

			for (int i = 0; i < img.PropertyIdList.Length; i++)
			{
				int propid = img.PropertyIdList[i];
				PropertyItem prop = img.GetPropertyItem(propid);
				this.ImageProperties[i] = new ImageProperty(prop);
			}
		}

		public string PageInfoDescription
		{
			get
			{
				System.Text.StringBuilder sb = new System.Text.StringBuilder();

				sb.Append("Width (Pixels): " + this.WidthPixels.ToString() + "\r\n");
				sb.Append("Height (Pixels): " + this.HeightPixels.ToString() + "\r\n");

				sb.Append("Horizontal Resolution: " + this.HorizontalResolution.ToString() + "\r\n");
				sb.Append("Vertical Resolution: " + this.VerticalResolution.ToString() + "\r\n");

				sb.Append("Width (Inches): " + this.WidthInches.ToString() + "\r\n");
				sb.Append("Height (Inches): " + this.HeightInches.ToString() + "\r\n");

				sb.Append("Aspect Ratio (w/h): " + this.AspectRatio.ToString() + "\r\n");

				sb.Append("Image Flags: " + Enum.Format(typeof(ImageFlags), this.ImageFlags, "g") + "\r\n");

				sb.Append("Pixel Format: " + this.PixelFormat.ToString() + "\r\n");
				sb.Append("Raw Format: " + this.ImageFormatName.ToString() + "\r\n");

				sb.Append("Palette Flags : " + Enum.Format(typeof(PaletteFlags), this.PaletteFlags, "g") + "\r\n");
				sb.Append("Palette Entries : \r\n");

				foreach (Color color in this.PaletteEntries)
				{
					sb.Append("\tColor: " + color.Name + "\r\n");
				}

				sb.Append("Image Property Items : \r\n");

				foreach (ImageProperty prop in this.ImageProperties)
				{
					sb.Append(prop.Description + "\r\n");
				}

				return sb.ToString();
			}
		}

		/// <summary>
		/// Returns a PaperSize based on the attributes of the source image.  If it does not exactly
		/// fit a standard legal or letter page, then PaperSize.Undefined will be returned.
		/// </summary>
		/// <returns>The paper size.</returns>
		public PaperSize GetStandardPaperSize
		{
			get
			{
				if (this.WidthPixels != ImageUtility.FAX_TIF_HOR_PX) { return PaperSize.Undefined; }
				if (this.HorizontalResolution != ImageUtility.FAX_TIF_HOR_RES) { return PaperSize.Undefined; }
				if (this.VerticalResolution == ImageUtility.FAX_TIF_VER_RES_HI)
				{
					if (this.HeightPixels == ImageUtility.FAX_TIF_VER_PX_LTR_HI) { return PaperSize.Letter; }
					if (this.HeightPixels == ImageUtility.FAX_TIF_VER_PX_LGL_HI) { return PaperSize.Legal; }
				}
				if (this.VerticalResolution == ImageUtility.FAX_TIF_VER_RES_LOW)
				{
					if (this.HeightPixels == ImageUtility.FAX_TIF_VER_PX_LTR_LOW) { return PaperSize.Letter; }
					if (this.HeightPixels == ImageUtility.FAX_TIF_VER_PX_LGL_LOW) { return PaperSize.Legal; }
				}
				return PaperSize.Undefined;
			}
		}

		/// <summary>
		/// Returns a Fax based on the attributes of the source image.  If it does not exactly
		/// fit a standard legal or letter page, then FaxQuality.Undefined will be returned.
		/// </summary>
		/// <returns>The fax quality.</returns>
		public FaxQuality GetStandardFaxQuality
		{
			get
			{
				if (this.WidthPixels != ImageUtility.FAX_TIF_HOR_PX) { return FaxQuality.Undefined; }
				if (this.HorizontalResolution != ImageUtility.FAX_TIF_HOR_RES) { return FaxQuality.Undefined; }
				if (this.VerticalResolution == ImageUtility.FAX_TIF_VER_RES_HI)
				{
					if (this.HeightPixels == ImageUtility.FAX_TIF_VER_PX_LTR_HI) { return FaxQuality.Fine; }
					if (this.HeightPixels == ImageUtility.FAX_TIF_VER_PX_LGL_HI) { return FaxQuality.Fine; }
				}
				if (this.VerticalResolution == ImageUtility.FAX_TIF_VER_RES_LOW)
				{
					if (this.HeightPixels == ImageUtility.FAX_TIF_VER_PX_LTR_LOW) { return FaxQuality.Normal; }
					if (this.HeightPixels == ImageUtility.FAX_TIF_VER_PX_LGL_LOW) { return FaxQuality.Normal; }
				}
				return FaxQuality.Undefined;
			}
		}

		public bool IsStandardFaxTiff
		{
			get
			{
				if (this.GetStandardPaperSize == PaperSize.Undefined) { return false; }
				else { return true; }
			}
		}

		public PaperSize GetBestFitPaperSize
		{
			get
			{
				float inch_variance = .30F;
				bool islandscape = this.AspectRatio > 1;
				PaperSize temp = PaperSize.Letter;

				if (islandscape && this.WidthInches < ImageUtility.FAX_TIF_HOR_IN + inch_variance)
				{
					islandscape = false;
					temp = PaperSize.Letter;
				}

				if (islandscape && this.WidthInches > ImageUtility.FAX_TIF_VER_IN_LTR + inch_variance)
				{
					islandscape = true;
					temp = PaperSize.Legal;
				}

				if (!islandscape && this.HeightInches > ImageUtility.FAX_TIF_VER_IN_LTR + inch_variance)
				{
					islandscape = false;
					temp = PaperSize.Legal;
				}
				return temp;
			}
		}

		public int GetBestFitRotation
		{
			get
			{
				float inch_variance = .20F;
				bool islandscape = this.AspectRatio > 1;
				PaperSize temp = PaperSize.Letter;

				if (islandscape && this.WidthInches < ImageUtility.FAX_TIF_HOR_IN + inch_variance)
				{
					islandscape = false;
					temp = PaperSize.Letter;
				}

				if (islandscape && this.WidthInches > ImageUtility.FAX_TIF_VER_IN_LTR + inch_variance)
				{
					islandscape = true;
					temp = PaperSize.Legal;
				}

				if (!islandscape && this.HeightInches > ImageUtility.FAX_TIF_VER_IN_LTR + inch_variance)
				{
					islandscape = false;
					temp = PaperSize.Legal;
				}
				if (islandscape) { return 90; }
				else { return 0; }
			}
		}

		public bool IsIndexedFormat
		{
			get
			{
				if (this.PixelFormat == System.Drawing.Imaging.PixelFormat.Indexed ||
					this.PixelFormat == System.Drawing.Imaging.PixelFormat.Format1bppIndexed ||
					this.PixelFormat == System.Drawing.Imaging.PixelFormat.Format4bppIndexed ||
					this.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
				{ return true; }
				else { return false; }
			}
		}
	}
}
