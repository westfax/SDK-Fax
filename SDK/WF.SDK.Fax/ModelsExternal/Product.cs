using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WF.SDK.Common;

namespace WF.SDK.Models
{
  [Serializable]
  public class Product : ICacheControl, IId
  {
    //Cache Control properties - these do not go to the client.
    [Newtonsoft.Json.JsonIgnore]
    public HydrationFlag HydrationFlag { get; set; }
    [Newtonsoft.Json.JsonIgnore]
    public DateTime HydrationUTC { get; set; }
    [Newtonsoft.Json.JsonIgnore]
    public bool RequiresTimeExpiration { get; set; }
    //End Cache Control

    public Guid Id { get; set; }
    public String Name { get; set; }
    public String Detail { get; set; }
		public Guid PlanId { get; set; }

    public ProductType ProductType { get; set; }

    public string InboundCSID { get; set; }
    public string InboundNumber { get; set; }

    public string OutboundCSID { get; set; }
    public string OutboundANI { get; set; }

    public string FaxHeader { get; set; }
    public TimeZoneName TimeZone { get; set; }
    public ProductState ProductState { get; set; }
        
    public DateTime CurrentBillingPeriodStart;
    public DateTime CurrentBillingPeriodEnd;
    public DateTime FreeTrialEnd;
    public int PeriodicQuantity;
    public int QuantityInbound;
    public int QuantityOutbound;

    public bool ShowDeletedItemsFolder { get; set; }

    public Guid DefaultCoverPageId { get; set; }

    public Product()
    {
      this.HydrationUTC = DateTime.MinValue;
      this.RequiresTimeExpiration = false;
    }

    public Product(Guid id, string name, ProductType type, string inboundNumber = "") : this()
    {
      this.Id = id;
      this.Name = name;
      this.ProductType = type;
      this.InboundNumber = "";
    }

    public Product(Internal.F2EProductItem item)
			: this((Internal.ProductItem)item)
    {
      this.Id = item.Id;
      this.Name = item.Name;
			this.PlanId = item.PlanId;
      //Workaround for legacy product name.
      this.ProductType = EnumExtensionMethods.ConvertToSdkProdType(item.ProductType);
      //Convert Enums
      try { this.ProductState = (ProductState)Enum.Parse(typeof(ProductState), item.ProductState); }
      catch { this.ProductState = ProductState.OK; }
      this.InboundNumber = item.InboundNumber;
      try { this.TimeZone = (TimeZoneName)Enum.Parse(typeof(TimeZoneName), item.TimeZone); }
      catch { }
      this.CurrentBillingPeriodStart = item.CurrentBillingPeriodStart;
      this.CurrentBillingPeriodEnd = item.CurrentBillingPeriodEnd;
      this.PeriodicQuantity = item.PeriodicQuantity;
      this.QuantityInbound = item.QuantityInbound;
      this.QuantityOutbound = item.QuantityOutbound;
      this.FreeTrialEnd = item.FreeTrialEnd;
      this.ShowDeletedItemsFolder = item.ShowDeletedItemsFolder;

      this.DefaultCoverPageId = item.DefaultCoverPageId;
    }

    public Product(Internal.ProductItem item) : this()
    {
      this.Id = item.Id;
      this.Name = item.Name;
			this.PlanId = item.PlanId;
      //Workaround for legacy product name.
      this.ProductType = EnumExtensionMethods.ConvertToSdkProdType(item.ProductType);

      this.Detail = item.Detail;

      this.InboundCSID = item.InboundCSID;
      this.InboundNumber = item.InboundNumber;
      this.OutboundCSID = item.OutboundCSID;
      this.OutboundANI = item.OutboundANI;

      this.FaxHeader = item.FaxHeader;
      this.DefaultCoverPageId = item.DefaultCoverPageId;

      this.ShowDeletedItemsFolder = false;

      try { this.ProductState = (ProductState)Enum.Parse(typeof(ProductState), item.ProductState); }
      catch { this.ProductState = ProductState.OK; }

      try { this.TimeZone = (TimeZoneName)Enum.Parse(typeof(TimeZoneName), item.TimeZone); }
      catch { }
    }

    public string BillingDateLabel
    {
      get
      {
        if (this.FreeTrialEnd < DateTime.Now) { return "Billing Date"; }
        else { return "Free Trial Ends"; }
      }
    }

    public string BillingDateData
    {
      get
      {
        if (this.FreeTrialEnd < DateTime.Now) { return this.CurrentBillingPeriodEnd.ToShortDateString(); }
        else { return this.FreeTrialEnd.ToShortDateString(); }
      }
    }
  }

  public static class ProductInfoExtensions
  {
    public static Product ToProduct(this Internal.ProductItem obj)
    {
      if (obj == null) { return null; }
      var ret = new Product(obj);
      return ret;
    }

    public static Internal.ProductItem ToProductItem(this Product obj)
    {
      if (obj == null) { return null; }

      var ret = new Internal.ProductItem();
      ret.Id = obj.Id;
			ret.PlanId = obj.PlanId;
      ret.Detail = obj.Detail;
      ret.FaxHeader = obj.FaxHeader;
      ret.InboundCSID = obj.InboundCSID;
      ret.InboundNumber = obj.InboundNumber;
      ret.Name = obj.Name;
      ret.OutboundANI = obj.OutboundANI;
      ret.OutboundCSID = obj.OutboundCSID;
      ret.ProductState = obj.ProductState.ToString();
      ret.ProductType = obj.ProductType.ConvertToPolkaProdString();
      ret.TimeZone = obj.TimeZone.ToString();
      ret.ShowDeletedItemsFolder = obj.ShowDeletedItemsFolder;

      return ret;
    }
  }

}
