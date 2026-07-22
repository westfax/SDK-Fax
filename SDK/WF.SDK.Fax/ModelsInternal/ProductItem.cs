using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.SDK.Models.Internal
{

  [Serializable]
  public class ProductItem
  {
    public Guid Id;
    public string Name;
    public string Detail;
		public Guid PlanId;

    public string ProductType;
		public string ProductState;

    public string InboundCSID;
    public string InboundNumber;

    public string OutboundCSID;
    public string OutboundANI;

    public string FaxHeader;
    public string TimeZone;
    public bool ShowDeletedItemsFolder;

    public Guid DefaultCoverPageId;
    public ProductItem()
    { }
  }

	/// <summary>
	/// Fax to Email Product Item detail
	/// </summary>
	[Serializable]
	public class F2EProductItem : ProductItem
	{
		public DateTime CurrentBillingPeriodStart;
		public DateTime CurrentBillingPeriodEnd;
		public DateTime FreeTrialEnd;
		public int PeriodicQuantity;
		public int QuantityInbound;
		public int QuantityOutbound;

    public F2EProductItem()
		{ }
	}

}
