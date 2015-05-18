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
    public class TaxProfileAssistantControllerTests
    {

        [Test]//Checks if Post method returns url if correct xml is provided
        public void PostMethod_ReturnURL_Test()
        {
            //Arrange
            //var _temp = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
            
            XmlReader xmlReader = XmlReader.Create(@"..\XMLTestFiles\TPA_500.xml");
            var tpa = new TPAScaffold();
            var serializer = new XmlSerializer(tpa.GetType());
            TPAScaffold input = (TPAScaffold)serializer.Deserialize(xmlReader);
            TaxProfileAssistantController tpaController = new TaxProfileAssistantController();
            tpaController.ControllerContext = new ControllerContext();

            //Act
            String resultStr = tpaController.Post(input);

            //Assert

            Assert.IsNotNull(resultStr,"Problem in returning url through post!!Returned url is empty!");

        }
       

                   
        [Test()]//Checks performance of post method for 10000 records
        public void TaxProfileAssistantController_Post_XML_Load_TestFor10000Records()
        {
            XmlReader xmlReader = XmlReader.Create(@"..\XMLTestFiles\TPA_10000.xml");
            var tpa = new TPAScaffold();
            var serializer = new XmlSerializer(tpa.GetType());
            TPAScaffold input = (TPAScaffold)serializer.Deserialize(xmlReader);
            TaxProfileAssistantController tpaController = new TaxProfileAssistantController();
            tpaController.ControllerContext = new ControllerContext();
            string ReturnURL = tpaController.Post(input);
            Assert.IsNotNull(ReturnURL,"Problem in returning URL through post method!!Returned url is empty!");
        }



        //[Test()]//Checks if method throws an exception on error and sends a proper message
        //[ExpectedException(typeof(System.NullReferenceException))]
        //public void TaxProfileAssistantController_PushWareHousLoc_ThrowsException_Test()
        //{
        //    //Arrange
        //    XmlReader xmlReader = XmlReader.Create(@"..\XMLTestFiles\TPA_500_NexusMissing.xml");
        //    var tpa = new TPAScaffold();
        //    var serializer = new XmlSerializer(tpa.GetType());
        //    TPAScaffold input = (TPAScaffold)serializer.Deserialize(xmlReader);
        //    TaxProfileAssistantController tpaController = new TaxProfileAssistantController();
        //    tpaController.ControllerContext = new ControllerContext();
        //    AccountInfo accountInfo = AvaTaxProfileAssistantHelper.InsertAccountInfo(input.AccountCredentials);
        //    int ID = accountInfo.ID;

        //    //Act
        //    tpaController.PushWareHousLoc(input, ID);
          
        //}

        //[Test()]//Checks if method throws an exception on error and sends a proper message
        //[ExpectedException(typeof(System.NullReferenceException))]
        //public void TaxProfileAssistantController_PushPrevCustLoc_ThrowsException_Test()
        //{
        //    //Arrange
        //    XmlReader xmlReader = XmlReader.Create(@"..\XMLTestFiles\TPA_500_NexusMissing.xml");
        //    var tpa = new TPAScaffold();
        //    var serializer = new XmlSerializer(tpa.GetType());
        //    TPAScaffold input = (TPAScaffold)serializer.Deserialize(xmlReader);
        //    TaxProfileAssistantController tpaController = new TaxProfileAssistantController();
        //    tpaController.ControllerContext = new ControllerContext();
        //    AccountInfo accountInfo = AvaTaxProfileAssistantHelper.InsertAccountInfo(input.AccountCredentials);
        //    int ID = accountInfo.ID;

        //    //Act
        //    tpaController.PushPrevCustLoc(input, ID);

        //}

        //[Test()]//Checks if method throws an exception on error and sends a proper message
        //[ExpectedException(typeof(System.NullReferenceException))]

        //public void TaxProfileAssistantController_PushCustomers_ThrowsException_Test()
        //{
        //    //Arrange
        //    XmlReader xmlReader = XmlReader.Create(@"..\XMLTestFiles\TPA_500_AvaERPSettingMissing.xml");
        //    var tpa = new TPAScaffold();
        //    var serializer = new XmlSerializer(tpa.GetType());
        //    TPAScaffold input = (TPAScaffold)serializer.Deserialize(xmlReader);
        //    TaxProfileAssistantController tpaController = new TaxProfileAssistantController();
        //    tpaController.ControllerContext = new ControllerContext();
        //    AccountInfo accountInfo = AvaTaxProfileAssistantHelper.InsertAccountInfo(input.AccountCredentials);
        //    int ID = accountInfo.ID;
        //    int AvaERPSettingsID = AvaTaxProfileAssistantHelper.InsertAvaERPSetting(input.AvaERPSettings, ID);
            

        //    //Act
        //    tpaController.PushCustomers(input, ID, AvaERPSettingsID);

        //    //Assert

            


        //}

        //[Test()]//Checks if method throws an exception on error and sends a proper message
        //[ExpectedException(typeof(System.NullReferenceException))]
        //public void TaxProfileAssistantController_PushNonMappedItems_ThrowsException_Test()
        //{
        //    //Arrange
        //    XmlReader xmlReader = XmlReader.Create(@"..\XMLTestFiles\TPA_500_AvaERPSettingMissing.xml");
        //    var tpa = new TPAScaffold();
        //    var serializer = new XmlSerializer(tpa.GetType());
        //    TPAScaffold input = (TPAScaffold)serializer.Deserialize(xmlReader);
        //    TaxProfileAssistantController tpaController = new TaxProfileAssistantController();
        //    tpaController.ControllerContext = new ControllerContext();
        //    AccountInfo accountInfo = AvaTaxProfileAssistantHelper.InsertAccountInfo(input.AccountCredentials);
        //    int ID = accountInfo.ID;
        //    int AvaERPSettingsID = AvaTaxProfileAssistantHelper.InsertAvaERPSetting(input.AvaERPSettings, ID);


        //    //Act
        //    tpaController.PushNonMappedItems(input, ID, AvaERPSettingsID);
        //}

        //[Test()]//Checks if method throws an exception on error and sends a proper message
        //[ExpectedException(typeof(System.NullReferenceException))]
        //public void TaxProfileAssistantController_PushMappedItems_ThrowsException_Test()
        //{
        //    //Arrange
        //    XmlReader xmlReader = XmlReader.Create(@"..\XMLTestFiles\TPA_500_AvaERPSettingMissing.xml");
        //    var tpa = new TPAScaffold();
        //    var serializer = new XmlSerializer(tpa.GetType());
        //    TPAScaffold input = (TPAScaffold)serializer.Deserialize(xmlReader);
        //    TaxProfileAssistantController tpaController = new TaxProfileAssistantController();
        //    tpaController.ControllerContext = new ControllerContext();
        //    AccountInfo accountInfo = AvaTaxProfileAssistantHelper.InsertAccountInfo(input.AccountCredentials);
        //    int ID = accountInfo.ID;
        //    int AvaERPSettingsID = AvaTaxProfileAssistantHelper.InsertAvaERPSetting(input.AvaERPSettings, ID);


        //    //Act
        //    tpaController.PushMappedItems(input, ID, AvaERPSettingsID);
        //}              
    }
}
