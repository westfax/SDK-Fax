using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using NUnit.Framework;

using WF.SDK.Fax;
using WF.SDK.Models;


namespace WF.SDK.Fax.Demo
{
  public class DemoWorkflow
  {
    public string TestDoc1 = "..\\TestDocuments\\Lorem ipsum.docx";
    public string TestDoc2 = "..\\TestDocuments\\Lorem ipsum 2.docx";

    public DemoWorkflow()
    {
    }

    
    public void RunDemo()
    {
      this.Authenticate();
      this.GetProductAndAccountInfo();
      this.SendFax();
      this.RetrieveInboundFax();
    }


    public void Authenticate()
    {
      Console.WriteLine("Basic Authentication");
      {
        //Ping - See if API is alive.
        var ret1 = FaxInterface.Ping("Ping!");
        Assert.IsTrue(ret1.Success);
        Assert.IsTrue(ret1.Result.Contains("Ping!"));
      }

      {
        //Authenticate basic
        var ret1 = FaxInterface.Authenticate(ConfigInfo.User, ConfigInfo.Pass);
        Assert.IsTrue(ret1.Success);
      }

      {
        //Authenticate with product - Product Id is optional on this method, but is required on many other methods.
        //You can use the product Id here to validate that you can connect to the product that you want to.
        var ret1 = FaxInterface.Authenticate(ConfigInfo.User, ConfigInfo.Pass, ConfigInfo.ProductId);
        Assert.IsTrue(ret1.Success);
      }

      {
        //Authenticate with product - This one will fail.  BadProductId.
        var ret1 = FaxInterface.Authenticate(ConfigInfo.User, ConfigInfo.Pass, Guid.NewGuid());
        Assert.IsFalse(ret1.Success);
      }
    }

    public void GetProductAndAccountInfo()
    {
      Console.WriteLine("Get Some Info about the products and account.");

      {
        //Display the Account Info
        var ret1 = FaxInterface.GetAccountInfo(ConfigInfo.User, ConfigInfo.Pass);
        Assert.IsTrue(ret1.Success);
        var result1 = ret1.Result;
        Console.WriteLine("---------Showing Account Information for this user---------");
        Console.WriteLine("AccountName: " + result1.AccountName + " AcctNumber: " + result1.AccountNumber + " State: " + result1.AccountState.ToString());
      }

      {
        //Display all the Products avialable to this login.
        var ret1 = FaxInterface.GetProductList(ConfigInfo.User, ConfigInfo.Pass);
        Assert.IsTrue(ret1.Success);
        var result1 = ret1.Result;
        Console.WriteLine("---------Showing Product Information for this user---------");
        foreach (var prod in result1)
        {
          Console.WriteLine("ProductName: " + prod.Name + " ProdType: " + prod.ProductType + " State: " + prod.ProductState.ToString() + " Id: " + prod.Id);
        }
      }

      {
        //Display all the fax to Email products avialable to this login.
        var ret1 = FaxInterface.GetF2EProductList(ConfigInfo.User, ConfigInfo.Pass);
        Assert.IsTrue(ret1.Success);
        var result1 = ret1.Result;
        Console.WriteLine("---------Showing Fax To Email Product Information for this user---------");
        foreach (var prod in result1)
        {
          Console.WriteLine("ProductName: " + prod.Name + " ProdType: " + prod.ProductType + " State: " + prod.ProductState.ToString() + " Id: " + prod.Id);
        }
      }

      {
        //Get even more detail for a particular Fax To Email Product.
        var temp = FaxInterface.GetF2EProductList(ConfigInfo.User, ConfigInfo.Pass).Result;
        var prodId = Guid.Empty;
        if (temp != null && temp.Count > 0)
        {
          prodId = temp[0].Id;        //Discover an Id.
          var ret1 = FaxInterface.GetF2EProductDetail(ConfigInfo.User, ConfigInfo.Pass, prodId);  //Now query it
          Assert.IsTrue(ret1.Success);
          var result1 = ret1.Result;
          Console.WriteLine("---------Showing Detail Fax To Email Product Information---------");
          Console.WriteLine("ProductName: " + result1.Name + " ProdType: " + result1.ProductType + " State: " + result1.ProductState.ToString() + " Id: " + result1.Id);
          Console.WriteLine("Qty Inbound: " + result1.QuantityInbound + " Qty Outbound: " + result1.QuantityOutbound + " Number: " + result1.InboundNumber);
        }
      }

      {
        //Get Some Info About me (the user)
        var ret1 = FaxInterface.GetUserProfile(ConfigInfo.User, ConfigInfo.Pass);  //Get my info
         Assert.IsTrue(ret1.Success);
        var result1 = ret1.Result;
        Console.WriteLine("---------Showing User Profile Detail---------");
        Console.WriteLine("FirstName: " + result1.FirstName + " FirstName: " + result1.LastName + " Fax: " + result1.Fax + " Email: " + result1.Email);
      }

      {
        //Get Some Info About me (the user) - enhanced.  Has login and ACL permissions.
        var ret1 = FaxInterface.GetLoginInfo(ConfigInfo.User, ConfigInfo.Pass);  //Get my info
        Assert.IsTrue(ret1.Success);
        var result1 = ret1.Result;
        Console.WriteLine("---------Showing User Profile Detail---------");
        Console.WriteLine("FirstName: " + result1.Contact.FirstName + " FirstName: " + result1.Contact.LastName + " Fax: " + result1.Contact.Fax + " Email: " + result1.Contact.Email);
        Console.WriteLine("UsernName: " + result1.UserName + " Id: " + result1.Id.ToString());
        //Acctss Control List contains permissions on various objects.
        foreach (var acl in result1.AclList)
        {
          Console.WriteLine("Type: " + acl.ACLType.ToString() + " Role: " + acl.UserRoleLevel.ToString() + " ACL Item: " + acl.EntityName + " Id: " + acl.EntityId.ToString());
        }
      }

      {
        //Users can sent a fax to a stored Contact.  Get that list here.  This list can be product qualified.
        var ret1 = FaxInterface.GetContactList(ConfigInfo.User, ConfigInfo.Pass);  //Get the contacts I can see.
        Assert.IsTrue(ret1.Success);
        var result1 = ret1.Result;
        Console.WriteLine("---------Showing Contacts---------");
        foreach (var contact in result1)
        {
          Console.WriteLine("FirstName: " + contact.FirstName + " FirstName: " + contact.LastName + " Fax: " + contact.FaxNumber + " Email: " + contact.Email);
          Console.WriteLine("Type: " + contact.Type + " Owner: " + contact.OwnerId); 
        }
      }

      {
        //Show details of the selected product
        var ret1 = FaxInterface.GetProductList(ConfigInfo.User, ConfigInfo.Pass);  //Get the products I can see.
        Assert.IsTrue(ret1.Success);
        var product = ret1.Result.FirstOrDefault(i => i.Id == ConfigInfo.ProductId);
        Console.WriteLine("---------Showing Selected Product---------");
        Console.WriteLine("ProductName: " + product.Name + " ProdType: " + product.ProductType + " State: " + product.ProductState.ToString() + " Id: " + product.Id);       
      }

    }

    public void SendFax()
    {
      Console.WriteLine("Send a fax to to the Test Number and then check its status.");

      var faxNumber = ConfigInfo.TestFax;

      //This is the id of the job we'll send.
      Guid JobId = Guid.Empty;

      {
        var file = new FileDetail();
        //Get the bytes from the file.
        file.FileContents = System.IO.File.ReadAllBytes(TestDoc1);
        //The file name doesn't matter.  The extension does help our system deal with various crap...  
        //We do some fancy filetype detection.  Extensions act as "powerful" hints to our file type detector.  Use them if you can.
        file.Filename = System.IO.Path.GetFileName(TestDoc1);

        //Send it.  Notice that the fax number is there 2 times.  This means that the fax will have 2 calls on it.  Yes, we'll call the same number twice.
        var ret1 = FaxInterface.SendFax(ConfigInfo.User, ConfigInfo.Pass, ConfigInfo.ProductId,
          new List<string>() { faxNumber, faxNumber },
          file, "Faxing Demo", "777-777-7000", DateTime.Now, "Fine", "Demo Job", "Demo Header",
          "Demo Billing Code", ConfigInfo.Email);

        Assert.IsTrue(ret1.Success);
        var result1 = ret1.Result;
        JobId = new Guid(result1);  //Save the Id.
      }


      //You can loop here until the job is done.
      //There is a cache on the server side.  Calls will only be updated every 3 minutes.  So just be patient.
      //Ask for FaxStatus of the job.  Really we should wait a few minutes.  There's no chance it'll be done right away.
      bool loop = true;
      while (loop)
      {
        Console.Write("\r\nEnter [y] to check the fax FaxStatus now.  Enter [n] to exit loop: ");
        var result = Console.ReadLine().ToLower();
        if (result == "y")
        {
          var desc = this.GetFaxDescription(JobId);
          Console.WriteLine("  --Job Name: " + desc.JobName + " Status:" + desc.FaxStatus.ToString());
          foreach (var call in desc.FaxCallInfoList)
          {
            Console.WriteLine("    --Call To: " + call.TermNumber + " Status:" + call.CallResult.ToString());
          }
        }
        if (result == "n") { loop = false; }
      }

    }

    public FaxDesc GetFaxDescription(Guid jobId)
    {
      //Ask for FaxStatus of the job.
      var fax = new FaxId(jobId);
      var ret1 = FaxInterface.GetFaxDescriptions(ConfigInfo.User, ConfigInfo.Pass, ConfigInfo.ProductId,
        new List<IFaxId>() { fax });
      Assert.IsTrue(ret1.Success);
      Assert.IsTrue(ret1.Result.Count == 1);  //Just one
      return (FaxDesc)ret1.Result[0];  //We asked for 1 result, return it.
    }

    /// <summary>
    /// This method will retrieve a fax if there is one there, and then mark it deleted after retrieving it.
    /// Marking it deleted does not delete the fax.  It merely marks it as downloaded so that the next time you ask for 
    /// faxes it does not return it.  Basically, you are able to tell the API you aren't interested in it anymore.
    /// Mark as downloaded when you have successfully pulled and saved the fax.
    /// </summary>
    public void RetrieveInboundFax()
    {
      Console.WriteLine("Retreive a fax document from the API, see its properties and display it.  Need to have an inbound fax in your account for this to work.");

      //Get Inbound Faxes
      var ret1 = FaxInterface.GetInboundFaxIds(ConfigInfo.User, ConfigInfo.Pass, ConfigInfo.ProductId);
      //Check if we got any.
      if (ret1.Result.Count == 0)
      {
        Console.WriteLine("Did not find any inbound faxes to retrieve.  Please send one to your account.");
        return;
      }

      //Get the first one, and retrieve detail and retrieve the document.
      var fax = ret1.Result[0];

      //Retrieve the fax detail, and echo to screen.
      FaxInterface.GetFaxDescriptions(ConfigInfo.User, ConfigInfo.Pass, ConfigInfo.ProductId, new List<IFaxId>() { fax });
      if (ret1.Result.Count == 0)
      {
        Console.WriteLine("Could not retrieve the detail.  Something bad happened with the API.");
        return;
      }

      var faxDetail = (FaxDesc)ret1.Result[0];
      Console.WriteLine("A fax was found:");
      //Show the fax info
      Console.WriteLine("  Name: " + faxDetail.JobName + " Status:" + faxDetail.FaxStatus.ToString());  //There's more detail here not shown
      foreach (var call in faxDetail.FaxCallInfoList)
      {
        Console.WriteLine("    --Number: " + call.TermNumber + " Status:" + call.CallResult.ToString());  //There's more detail here not shown
      }

      Console.WriteLine("Retrieving and showing the docuement as a PDF.");
      //Now retrieve the document as a pdf and show it. Other formats are supported here.
      var ret3 = FaxInterface.GetFaxDocuments(ConfigInfo.User, ConfigInfo.Pass, ConfigInfo.ProductId, new List<IFaxId>() { fax }, FileFormat.Pdf);
      Assert.IsTrue(ret3.Success);
      Assert.IsTrue(ret3.Result.Count == 1);
      Assert.IsTrue(((FaxDesc)ret3.Result[0]).FaxFileList.Count > 0);
      //Show the file
      foreach (var item in ret3.Result)
      {
        Helper.DisplayFiles(Helper.WriteFiles(item));
      }


      Console.WriteLine("Marking this fax as deleted or removed.  and then marking it as read.");
      
      //Now delete this one.
      var ret4 = FaxInterface.MarkAsDeleted(ConfigInfo.User, ConfigInfo.Pass, ConfigInfo.ProductId, new List<IFaxId>() { fax });
      Assert.IsTrue(ret4.Result);  //It should work.
     
      //Now retrieve the faxes again.  The one we just got will be missing.
      ret1 = FaxInterface.GetInboundFaxIds(ConfigInfo.User, ConfigInfo.Pass, ConfigInfo.ProductId);
      //The one we just deleted will be gone!  Can't find it in the resulting collection.
      Assert.IsTrue(ret1.Result.FirstOrDefault(i => i.Id == fax.Id) == null);

      //Now mark it as read.
      ret4 = FaxInterface.MarkAsRead(ConfigInfo.User, ConfigInfo.Pass, ConfigInfo.ProductId, new List<IFaxId>() { fax });
      Assert.IsTrue(ret4.Result);  //It should work.

      //Now retrieve the faxes again.  This one will come back!  It is not really deleted.
      ret1 = FaxInterface.GetInboundFaxIds(ConfigInfo.User, ConfigInfo.Pass, ConfigInfo.ProductId);
      //The one we just deleted will be back!
      Assert.IsTrue(ret1.Result.FirstOrDefault(i => i.Id == fax.Id) != null);
      //Also it will be marked as Read (or "Retrieved") meaning that it has been retrieved from the server.
      Assert.IsTrue(ret1.Result[0].Tag == "Retrieved");
    }

    

  }
}


