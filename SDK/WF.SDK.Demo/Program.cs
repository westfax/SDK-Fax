using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.SDK.Fax.Demo
{
  class Program
  {
    static void Main(string[] args)
    {

      {
        //https://api2.westfax.com/REST/{0}/json
        //Set the URL Template
        WF.SDK.Fax.FaxInterface.RestUrlTemplate = ConfigInfo.RestUrlTemplate;
      }

      string result = "";

      Console.Write("Run the Workflow Demo? [n]");
      result = Console.ReadLine();
      if (result.ToLower().StartsWith("y"))
      {
        var t = new DemoWorkflow();
        t.RunDemo();
        Console.WriteLine( "Workflow Demo done.");
      }

      Console.Write("Hit [Enter] to exit demo.");
      result = Console.ReadLine();

    }
  }
}
