using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxProfileAssistant.Controllers;
using NUnit.Framework;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using TaxProfileAssistant.Models;
using TaxProfileAssistant.Utils;
using System.Data.Entity;

namespace TaxProfileAssistantTest.Controllers.Tests
{
    //[TestFixture()]
    //public class MapAvataxControllerTests
    //{
    //    AccountInfo accountInfo;
    //    MapAvataxController MapAvatax;
    //    TempDataDictionary TempData;
    //    TPAScaffold input;
    //    TaxProfileAssistantController tpaController;

    //    [SetUp]//Initializes data required for each test and runs once per test method.
    //    public void InitializeData()
    //    {
    //        accountInfo = new AccountInfo();
    //        XmlReader xmlReader = XmlReader.Create(@"..\XMLTestFiles\TPA_500.xml");
    //        var tpa = new TPAScaffold();
    //        var serializer = new XmlSerializer(tpa.GetType());
    //        input = (TPAScaffold)serializer.Deserialize(xmlReader);

    //        tpaController = new TaxProfileAssistantController();
    //        tpaController.ControllerContext = new ControllerContext();
    //        string resultstr = tpaController.Post(input);
    //        string key = resultstr.Substring(38);
    //        key = key.Replace(Constants.APPLICATION_URL_DUMMY_TEXT, "/");
    //        accountInfo = AvaTaxProfileAssistantHelper.GetAccountInfoBySecureKey(key);
    //        MapAvatax = new MapAvataxController();
    //        TempData = new TempDataDictionary();
    //        MapAvatax.TempData.Add("AccountInfo", accountInfo);
    //    }
        
    //    [Test()]//Check if Index method redirects to address validation setup
    //    public void MapAvataxController_Index_RedirectTo_AddressValidationSetup_Test()
    //    {                     
         
    //       //Act
    //       var indexResult= MapAvatax.Index();

    //       //Assert
    //       RedirectToRouteResult routeResult = indexResult as RedirectToRouteResult;
    //       Assert.AreEqual(routeResult.RouteValues["action"], "AddressValidationSetup", "Index Method of MapAvatax Controller failed to redirect to correct action!!");
          
    //     }


    //    [Test()]//Check if AddressValidationSetupMethod returns an object of AvaERPSettingsData
    //    public void MapAvataxController_AddressValidationSetupTest_ReturnsCorrectModelType()
    //    {
                       
    //        //Act
    //        var indexResult = MapAvatax.AddressValidationSetup() as ViewResult;

    //        //Assert
    //        Assert.IsInstanceOfType(indexResult.Model, typeof(AvaERPSettingsData),"Method should return model of type AvaErpSettingsData");

        
    //    }

    //    [Test()]//Checks if CustomerMapping method returns an object of correct model type
    //    public void MapAvataxController_CustomerMappingTest_ReturnsCorrectModelType()
    //    {
           
    //        //Act
    //        var indexResult = MapAvatax.CustomerMapping() as ViewResult;
            
    //        //Assert
    //        Assert.IsInstanceOfType(indexResult.Model, typeof(AvaERPSettingsData), "Method should return model of type AvaErpSettingsData");
        
    //    }

    //    [Test()]//Checks if ItemTaxability method returns an object of type AvaErpSettingsData
    //    public void MapAvataxController_ItemTaxabilityMappingTest_ReturnsCorrectModelType()
    //    {

    //        //Act
    //        var indexResult = MapAvatax.ItemTaxabilityMapping() as ViewResult;
            
    //        //Assert
    //        Assert.IsInstanceOfType(indexResult.Model, typeof(AvaERPSettingsData), "Method should return model of type AvaErpSettingsData");

        
    //    }
    //}
}
