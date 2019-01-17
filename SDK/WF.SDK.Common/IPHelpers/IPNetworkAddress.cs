using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace WF.SDK.Common
{
  public class IPNetworkAddress
  {

    #region Static
    public static readonly IPNetworkAddress ClassA = IPNetworkAddress.Parse("10.0.0.0/8");
    public static readonly IPNetworkAddress ClassB = IPNetworkAddress.Parse("172.16.0.0/12");
    public static readonly IPNetworkAddress ClassC = IPNetworkAddress.Parse("192.168.0.0/16");

    public static List<IPNetworkAddress> PrivateNetworks = new List<IPNetworkAddress>() { ClassA, ClassB, ClassC };
    
    /// <summary>
    /// Parses addresses with trainlin subnet bits.
    /// </summary>
    public static IPNetworkAddress Parse(string addr)
    {
      return new IPNetworkAddress(addr);
    }
    #endregion
    
    public IPAddress Address = null;
    public IPAddress Netmask = null;
    public IPAddress Network = null;

    public IPNetworkAddress(){}

    public IPNetworkAddress(string addr )
    {
      this.Address = IPAddress.Parse(addr.Split('/')[0]);
      this.Netmask = SubnetMask.CreateByNetBitLength(int.Parse(addr.Split('/')[1]));
      this.Network = this.Address.GetNetworkAddress(this.Netmask);
    }
    
    public bool IsInSameSubnet(string addr)
    {
      return this.Address.IsInSameSubnet(IPAddress.Parse(addr), this.Netmask);
    }

  }
}
