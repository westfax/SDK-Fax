using System;


namespace WF.SDK.Common.Imaging
{
	/// <summary>
	/// Summary description for ImageOperationResult.
	/// </summary>
	[Serializable]
	public class ImageOperationResult
	{
		public bool Success = true;
		public string ErrorDesc = "";
		public ImageOperationWarning ImageOperationWarning = ImageOperationWarning.None;
		public int Pages = 0;

		public ImageOperationResult() { }

	}
}
