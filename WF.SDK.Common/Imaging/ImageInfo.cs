using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;



namespace WF.SDK.Common.Imaging
{
	/// <summary>
	/// Summary description for ImageInfo.
	/// </summary>
	public class ImageInfo
	{
		public int PageCount = 0;
		public string FilePath = "(From Memory)";

		public FrameDimensionType FrameDimensionType = FrameDimensionType.None;

		public int PageDimensionCount = 0;
		public int ResolutionDimensionCount = 0;
		public int TimeDimensionCount = 0;

		public List<PageInfo> PageInfoItems = new List<PageInfo>();

		private ImageInfo() { }

		internal ImageInfo(string imageFilePath) : this(Image.FromFile(imageFilePath)) { this.FilePath = imageFilePath; }

		internal ImageInfo(Bitmap bmp) : this((Image)bmp) { }

		internal ImageInfo(Image img)
		{
			this.FrameDimensionType = ImageHelper.GetFrameDimensionTypes(img.FrameDimensionsList);

			//Get the Frame Dimensions if they exist
			if ((int)(this.FrameDimensionType & FrameDimensionType.Page) == (int)FrameDimensionType.Page)
			{
				this.PageDimensionCount = img.GetFrameCount(ImageHelper.FrameDimensionPage);
			}
			else { this.PageDimensionCount = 0; }

			if ((int)(this.FrameDimensionType & FrameDimensionType.Resolution) == (int)FrameDimensionType.Resolution)
			{
				this.ResolutionDimensionCount = img.GetFrameCount(ImageHelper.FrameDimensionResolution);
			}
			else { this.ResolutionDimensionCount = 0; }

			if ((int)(this.FrameDimensionType & FrameDimensionType.Time) == (int)FrameDimensionType.Time)
			{
				this.TimeDimensionCount = img.GetFrameCount(ImageHelper.FrameDimensionTime);
			}
			else { this.TimeDimensionCount = 0; }

			//Page count (PageDimensionFrameCount)
			this.PageCount = this.PageDimensionCount;

			try
			{
				//Foreach frame discover the image thats in it
				for (int i = 0; i < this.PageCount; i++)
				{
					img.SelectActiveFrame(ImageHelper.FrameDimensionPage, i);
					PageInfoItems.Add(new PageInfo(img));
				}
			}
			catch
			{
			}
			img.Dispose();
		}

		public string ImageInfoDescription
		{
			get
			{
				System.Text.StringBuilder sb = new System.Text.StringBuilder();

				sb.Append("---------Image Information----------\r\n");
				sb.Append("File Name: " + this.FilePath + "\r\n");
				sb.Append("Page Count: " + this.PageCount.ToString() + "\r\n");
				sb.Append("Frame Dimension Types: " + Enum.Format(typeof(FrameDimensionType), this.FrameDimensionType, "g") + "\r\n");
				sb.Append("Page Dimension Frame Count: " + this.PageDimensionCount.ToString() + "\r\n");
				sb.Append("Resolution Dimension Frame Count: " + this.ResolutionDimensionCount.ToString() + "\r\n");
				sb.Append("Time Dimension Frame Count: " + this.TimeDimensionCount.ToString() + "\r\n");
				sb.Append("---------Page Information-----------\r\n");

				for (int i = 0; i < this.PageInfoItems.Count; i++)
				{
					sb.Append("---Page #" + ((int)i + 1).ToString() + "---\r\n");
					sb.Append(this.PageInfoItems[i].PageInfoDescription);
				}

				return sb.ToString();
			}
		}

	}
}
