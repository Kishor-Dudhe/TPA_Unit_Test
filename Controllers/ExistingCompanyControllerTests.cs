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
    [TestFixture()]
    public class ExistingCompanyControllerTests
    {
        [Test()]//Checks if Index method returns existing companies view
        public void ExistingCompanyController_IndexTest_CheckIfItReturnsCorrectView()
        {
            //Arrange          
            AccountInfo accountInfo = new AccountInfo();
            TPAScaffold input=new TPAScaffold();
            TaxProfileAssistantController tpaController=new TaxProfileAssistantController();

            XmlReader xmlReader = XmlReader.Create(@"..\XMLTestFiles\TPA_500.xml");
            var tpa = new TPAScaffold();
            var serializer = new XmlSerializer(tpa.GetType());
            input = (TPAScaffold)serializer.Deserialize(xmlReader);
 
            tpaController.ControllerContext = new ControllerContext();
            string resultstr = tpaController.Post(input);
            string key = resultstr.Substring(38);
            key = key.Replace(Constants.APPLICATION_URL_DUMMY_TEXT, "/");
            accountInfo = AvaTaxProfileAssistantHelper.GetAccountInfoBySecureKey(key);

            ExistingCompanyController existingCompany = new ExistingCompanyController();
            TempDataDictionary TempData = new TempDataDictionary();
            existingCompany.TempData.Add("AccountInfo", accountInfo);
            
            //Act
            var result = existingCompany.Index() as ViewResult;

            //Assert: Checks if Company Information is returned
            Assert.IsTrue(result.TempData.Count>0);
            //Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsInstanceOf<ViewResult>(result);

        }

        [Test()]//Check if this method redirects to nexustsetup controller's index method
        public void ExistingCompanyController_CompanySelected_Returns_RedirectToAction()
        {
            //Arrange
            FormCollection fc = new FormCollection();
            fc["m.CompanyName"] = "Systems-of-America";
                 
            ExistingCompanyController existingCompany = new ExistingCompanyController();
            
            //Act
            var result = existingCompany.CompanySelected(fc);

            //Assert

            RedirectToRouteResult routeResult = result as RedirectToRouteResult;
            Assert.AreEqual(routeResult.RouteValues["action"], "Index", "CompanySelected Method of ExistingCompany Controller failed to redirect to correct action!!");
            Assert.AreEqual(routeResult.RouteValues["controller"], "NexusSetup", "CompanySelected Method of ExistingCompany Controller failed to redirect to correct controller!!");

            

        }


        //[Test()]//Check if Index method returns an object of AvaERPSettingsData
        ////Pass XML with TaxSchedule ID NULL
        //public void MapAvataxController_Index_ReturnCorrectModelType()
        //{
        //    TempDataDictionary TempData = new TempDataDictionary();
        //    AccountInfo accInfo;
        //    TaxProfileAssistantController tpaController;
        //    TPAScaffold input;
        //    MapAvataxController MapAvatax = new MapAvataxController();
        //    accInfo = new AccountInfo();
        //    XmlReader xmlReader = XmlReader.Create(@"..\XMLTestFiles\TPA_500_AvaERPSettingMissing.xml");
        //    var tpa = new TPAScaffold();
        //    var serializer = new XmlSerializer(tpa.GetType());
        //    input = (TPAScaffold)serializer.Deserialize(xmlReader);

        //    tpaController = new TaxProfileAssistantController();
        //    tpaController.ControllerContext = new ControllerContext();
        //    string resultstr = tpaController.Post(input);
        //    string key = resultstr.Substring(38);
        //    key = key.Replace(Constants.APPLICATION_URL_DUMMY_TEXT, "/");
        //    accInfo = AvaTaxProfileAssistantHelper.GetAccountInfoBySecureKey(key);
        //    MapAvatax.TempData.Add("AccountInfo", accInfo);
         
        //    //Act
        //    var indexResult = MapAvatax.Index() as ViewResult;

        //    //Assert
        //    Assert.IsInstanceOfType(indexResult.Model, typeof(AvaERPSettingsData), "Method should return model of type AvaErpSettingsData");

        //}
    }
}
