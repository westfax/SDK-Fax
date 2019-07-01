using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace WF.SDK.Common.Imaging
{
	/// <summary>
	/// Summary description for PageImage.
	/// </summary>
	public class PageImage : IDisposable
	{
		private object _tag = null;
		internal Bitmap _sourceBmp = null;
		internal PageInfo _pageInfo = null;
		public Guid TempId = Guid.NewGuid();

		internal PageImage()
		{
		}

		internal PageImage(Bitmap bmp)
			: this(bmp, true)
		{ }

		internal PageImage(Bitmap bmp, bool ConvertTo32Bit)
		{
			if (ConvertTo32Bit)
			{
				this._sourceBmp = BitmapHelper.CreateCopy32Bit(bmp);
			}
			else
			{
				this._sourceBmp = BitmapHelper.CreateCopyExact(bmp);
			}
		}

		public object Tag
		{
			get { return this._tag; }
			set { this._tag = value; }
		}

		public Bitmap Bitmap
		{
			get
			{
				return this._sourceBmp;
			}
		}

		public PageInfo PageInfo
		{
			get
			{
				if (this._pageInfo == null) { this._pageInfo = new PageInfo(this._sourceBmp); }
				return this._pageInfo;
			}
		}

		#region IDisposable Members

		public void Dispose()
		{
			if (this._sourceBmp != null) { this._sourceBmp.Dispose(); }
			this._sourceBmp = null;
		}

		#endregion
	}
}
