using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.SDK.Common
{
  public static class CommonRandom
  {
    private static Random rnd = new Random();
    private static object sync = new object();

    public static int Next(int max)
    {
      lock (CommonRandom.sync)
      {
        return rnd.Next(max);
      }
    }

    public static int Next(int min, int max)
    {
      lock (CommonRandom.sync)
      {
        return rnd.Next(min, max);
      }
    }

    public static void NextBytes(byte[] buffer)
    {
      lock (CommonRandom.sync)
      {
        rnd.NextBytes(buffer);
      }
    }

    public static double NextDouble()
    {
      lock (CommonRandom.sync)
      {
        return rnd.NextDouble();
      }
    }




  }
}
