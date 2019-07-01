using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace WF.SDK.Common
{
  public static class IPAddressExtensions
  {
    public static IPAddress GetBroadcastAddress(this IPAddress address, IPAddress subnetMask)
    {
      byte[] ipAdressBytes = address.GetAddressBytes();
      byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

      if (ipAdressBytes.Length != subnetMaskBytes.Length)
        throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

      byte[] broadcastAddress = new byte[ipAdressBytes.Length];
      for (int i = 0; i < broadcastAddress.Length; i++)
      {
        broadcastAddress[i] = (byte)(ipAdressBytes[i] | (subnetMaskBytes[i] ^ 255));
      }
      return new IPAddress(broadcastAddress);
    }

    public static IPAddress GetNetworkAddress(this IPAddress address, IPAddress subnetMask)
    {
      byte[] ipAdressBytes = address.GetAddressBytes();
      byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

      if (ipAdressBytes.Length != subnetMaskBytes.Length)
        throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

      byte[] broadcastAddress = new byte[ipAdressBytes.Length];
      for (int i = 0; i < broadcastAddress.Length; i++)
      {
        broadcastAddress[i] = (byte)(ipAdressBytes[i] & (subnetMaskBytes[i]));
      }
      return new IPAddress(broadcastAddress);
    }

    public static bool IsInSameSubnet(this IPAddress address2, IPAddress address, IPAddress subnetMask)
    {
      IPAddress network1 = address.GetNetworkAddress(subnetMask);
      IPAddress network2 = address2.GetNetworkAddress(subnetMask);

      return network1.Equals(network2);
    }

    public static bool IsInSameSubnetAsAny(this List<IPNetworkAddress> addrs, IPAddress address)
    {
      return addrs.FirstOrDefault(i => i.IsInSameSubnet(address)) != null ? true : false;
    }

    public static bool IsInSameSubnet(this IPNetworkAddress addr, IPAddress address)
    {
      return addr.Network.Equals(address.GetNetworkAddress(addr.Netmask));
    }

    public static bool IsPrivate(this IPAddress addr)
    {
      return IPNetworkAddress.PrivateNetworks.IsInSameSubnetAsAny(addr);
    }
  }
}
