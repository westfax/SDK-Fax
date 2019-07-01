using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.SDK.Models.Internal
{
  /// <summary>
  /// Fax to Email Product Item detail
  /// </summary>
  [Serializable]
  public class F2EProductItem
  {
    public Guid Id;
    public String Name;
    public String ProductType;
    public String ProductState;
    public String InboundNumber;
    public String TimeZone;
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
