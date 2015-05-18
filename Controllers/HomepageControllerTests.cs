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
using System.Web;
using System.IO;
using System.Security.Principal;





namespace TaxProfileAssistantTest.Controllers.Tests
{

    [TestFixture()]
    public class HomepageControllerTests
    {
        TaxProfileAssistantController tpaController;
        HomepageController homeController;
        TPAScaffold input;
        AccountInfo accInfo;
       
        bool IsConnected ;
        bool IsChoseCompany ;
        bool WhereToCollectTax ;
        bool NexusWarning ;



        [SetUp]//Initializes data required for each test and runs once per test method.
        public void InitializeData()
        {
            //Arrange
            XmlReader xmlReader = XmlReader.Create(@"..\XMLTestFiles\TPA_500.xml");
            var tpa = new TPAScaffold();
            var serializer = new XmlSerializer(tpa.GetType());
            input = (TPAScaffold)serializer.Deserialize(xmlReader);
            homeController = new HomepageController();
            tpaController = new TaxProfileAssistantController();
            tpaController.ControllerContext = new ControllerContext();
            
            IsConnected = false;
            IsChoseCompany = false;
            WhereToCollectTax = false;
            NexusWarning = false;

        }

       
        [Test]//Checks if all boolean variables required to complete taxprofile are set to true or false
        public void HomepageController_Index_Test_With_ValidID()
        {
           

            //Act
            String resultStr = tpaController.Post(input);
            string ID = resultStr.Substring(38);
            var result = homeController.Index(ID) as ViewResult;
            var homepage = (HomepageInfo)result.ViewData.Model;
            

            //Assert
            Assert.IsTrue(homepage.AddressValidation,"AddressValidation is false!");
            Assert.IsTrue(homepage.ChoseCompany,"Company not Chosen!!");
            Assert.IsTrue(homepage.CollectTax,"CollectTax is false!!");
            Assert.IsTrue(homepage.IsConnected,"Service Connection Failed!!");
            Assert.IsTrue(homepage.NexusWarning,"NexusWarning is false!!");
            Assert.IsFalse(homepage.AllChecked,"AllChecked is true!!");
            Assert.IsTrue(homepage.ItemTaxability,"ItemTaxability is false!!");
            Assert.IsFalse(homepage.MapCustomer,"MapCustomer is true!!");
            Assert.IsTrue(homepage.TaxProfileCompleted,"Tax Profile is incomplete!!");
            Assert.IsTrue(homepage.TaxSchedule,"Tax Schedule is not set to AVATAX!!");
            Assert.IsFalse(homepage.AvaERPSettings,"AvaERPSettings is true!");

        }


        [Test()]//Checks that empty view is returned when id passed is null.
        public void HomepageController_Index_Return_Empty_View_Test()
        {
            //Arrange
            string id = "";

            //Act
            var result = homeController.Index(id) as ViewResult;

            //Assert
            Assert.AreEqual("", result.ViewName,"Expected view not returned!");

        }


        [Test]//Checks if correct model type is returned through view
        public void HomepageController_IndexTest_CheckIfCorrectModelTypeIsReturned()
        {
            

            //Act
            String resultStr = tpaController.Post(input);
            string ID = resultStr.Substring(38);
            var result = homeController.Index(ID) as ViewResult;
            var homepage = (HomepageInfo)result.ViewData.Model;

            //Assert
            //Assert.IsInstanceOfType(result.Model, typeof(HomepageInfo),"Incorrect Model type returned!");
            Assert.IsInstanceOf<HomepageInfo>(result.Model, "Incorrect Model type returned!");

        }


        [Test]//Positive test to check that DoConnection sets all boolean parameters to true when account information is correctly passed to it
        public void HomepageController_DoConnectionMethodTestWithAllParameters()
        {
            //Arrange
            string resultstr = tpaController.Post(input);
            string key = resultstr.Substring(38);
            key = key.Replace(Constants.APPLICATION_URL_DUMMY_TEXT, "/");
            accInfo = AvaTaxProfileAssistantHelper.GetAccountInfoBySecureKey(key);

            //Act
            homeController.DoConnection(ref IsConnected, ref IsChoseCompany, ref WhereToCollectTax, ref NexusWarning, accInfo);

            //Assert
            Assert.IsTrue(IsConnected,"Service Connection Failed!");
            Assert.IsTrue(IsChoseCompany,"Company not Chosen!");
            Assert.IsTrue(WhereToCollectTax,"WhereToCollectTax not specified!");
            Assert.IsTrue(NexusWarning,"NexusWarning is not set to true!");

        }


        [Test]//Negative test to check if DoConnection connects to service if accountnumber is null or wrong
        public void HomepageController_DoConnectionMethodTestWithInvalidAccountInfoParameter()
        {
            //Arrange
            string resultstr = tpaController.Post(input);
            string key = resultstr.Substring(38);
            key = key.Replace(Constants.APPLICATION_URL_DUMMY_TEXT, "/");
            accInfo = AvaTaxProfileAssistantHelper.GetAccountInfoBySecureKey(key);
            accInfo.AccountNumber = "";


            //Act
            homeController.DoConnection(ref IsConnected, ref IsChoseCompany, ref WhereToCollectTax, ref NexusWarning, accInfo);

            //Assert
            Assert.IsFalse(IsConnected, "AccountNumber Incorrect or Empty!!");
                
        }

        [Test]//Test to check if DoConnection throws an exception if accountnumber is not passed
        public void HomepageController_DoConnectionMethod_ThrowsExceptionTest()
        {
            //Arrange
            string resultstr = tpaController.Post(input);
            string key = resultstr.Substring(38);
            key = key.Replace(Constants.APPLICATION_URL_DUMMY_TEXT, "/");
            accInfo = AvaTaxProfileAssistantHelper.GetAccountInfoBySecureKey(key);
            accInfo.AccountNumber = "";
            Exception e=new Exception();

            //Act

            homeController.DoConnection(ref IsConnected, ref IsChoseCompany, ref WhereToCollectTax, ref NexusWarning, null);
            
            //Assert

            //AssertFailedException.Equals("Object reference not set to an instance of an object.",e);
            
        }


        [Test]//Negative test to check if DoConnection Connects to service  if username is null or wrong
        public void HomepageController_DoConnectionMethodTestWithInvalidUsernameParameter()
        {
            //Arrange
            string resultstr = tpaController.Post(input);
            string key = resultstr.Substring(38);
            key = key.Replace(Constants.APPLICATION_URL_DUMMY_TEXT, "/");
            accInfo = AvaTaxProfileAssistantHelper.GetAccountInfoBySecureKey(key);
            accInfo.UserName = "";
            
            //Act
            homeController.DoConnection(ref IsConnected, ref IsChoseCompany, ref WhereToCollectTax, ref NexusWarning, accInfo);

            //Assert
            Assert.IsFalse(IsConnected, "Username Incorrect or Empty!!");

        }


        [Test]//Negative test to check if DoConnection Connects to service  if password is null or wrong
        public void HomepageController_DoConnectionMethodTest_WithWrongAccountPassword()
        {
            //Arrange
            string resultstr = tpaController.Post(input);
            string key = resultstr.Substring(38);
            key = key.Replace(Constants.APPLICATION_URL_DUMMY_TEXT, "/");
            accInfo = AvaTaxProfileAssistantHelper.GetAccountInfoBySecureKey(key);
            accInfo.Password = "";
           
            //Act
            homeController.DoConnection(ref IsConnected, ref IsChoseCompany, ref WhereToCollectTax, ref NexusWarning, accInfo);

            //Assert
            Assert.IsFalse(IsConnected, "Wrong Password!!");
        
        }


        [Test]//Negative test to check if DocConnection Connects to service  if LicenseKey is null or wrong
        public void HomepageController_DoConnectionMethodTest_With_Wrong_OR_Empty_LicenseKey()
        {
            //Arrange
            string resultstr = tpaController.Post(input);
            string key = resultstr.Substring(38);
            key = key.Replace(Constants.APPLICATION_URL_DUMMY_TEXT, "/");
            accInfo = AvaTaxProfileAssistantHelper.GetAccountInfoBySecureKey(key);
            accInfo.LicenseKey = "";
            
            //Act
            homeController.DoConnection(ref IsConnected, ref IsChoseCompany, ref WhereToCollectTax, ref NexusWarning, accInfo);

            //Assert
            Assert.IsFalse(IsConnected, "License Key Not Present or Wrong License Key is Entered!!");

        }


        [Test]//Negative test to check if DocConnection Connects to service  if Webservice is null or wrong
        public void HomepageController_DoConnectionMethodTest_Without_WebserviceURL()
        {
            //Arrange
            string resultstr = tpaController.Post(input);
            string key = resultstr.Substring(38);
            key = key.Replace(Constants.APPLICATION_URL_DUMMY_TEXT, "/");
            accInfo = AvaTaxProfileAssistantHelper.GetAccountInfoBySecureKey(key);
            accInfo.Webservice = "";


            //Act
            homeController.DoConnection(ref IsConnected, ref IsChoseCompany, ref WhereToCollectTax, ref NexusWarning, accInfo);

            //Assert
            Assert.IsFalse(IsConnected, "Webservice URL is not provided!!");

        }

        
        [Test]//Checks if index method of homecontroller works with 10000 records
        public void HomeController_IndexTestWithXMLOf10000Records()
        {
          
            XmlReader xmlReader = XmlReader.Create(@"..\XMLTestFiles\TPA_10000.xml");
            var tpa = new TPAScaffold();
            var serializer = new XmlSerializer(tpa.GetType());
            TPAScaffold input = (TPAScaffold)serializer.Deserialize(xmlReader);

                             
                         
            //Act
            string returnURL = tpaController.Post(input);
            string ID = returnURL.Substring(38);
            var result = homeController.Index(ID) as ViewResult;
            var homepage = (HomepageInfo)result.ViewData.Model;


            //Assert
            Assert.IsTrue(homepage.IsConnected,"Load test of 10000 records failed!!Service not connected!");

        }

        [Test]//Checks if index method of homecontroller works with 1000 records
        public void HomeController_IndexTestWithXMLOf1000Records()
        {
            //Arrange
            
            XmlReader xmlReader = XmlReader.Create(@"..\XMLTestFiles\TPA_1000.xml");
            var tpa = new TPAScaffold();
            var serializer = new XmlSerializer(tpa.GetType());
            TPAScaffold input = (TPAScaffold)serializer.Deserialize(xmlReader);
           

            //Act
            string returnURL = tpaController.Post(input);
            string ID = returnURL.Substring(38);
            var result = homeController.Index(ID) as ViewResult;

            var homepage = (HomepageInfo)result.ViewData.Model;


            //Assert
            Assert.IsTrue(homepage.IsConnected, "Load test of 1000 records failed!!Service not connected!");

        }


        [Test]//Checks if index method of homecontroller works with 2000 records
        public void HomeController_IndexTestWithXMLOf2000Records()
        {
            //Arrange
      
            XmlReader xmlReader = XmlReader.Create(@"..\XMLTestFiles\TPA_2000.xml");
            var tpa = new TPAScaffold();
            var serializer = new XmlSerializer(tpa.GetType());
            TPAScaffold input = (TPAScaffold)serializer.Deserialize(xmlReader);
          
            //Act
            string returnURL = tpaController.Post(input);
            string ID = returnURL.Substring(38);
            var result = homeController.Index(ID) as ViewResult;
            var homepage = (HomepageInfo)result.ViewData.Model;


            //Assert
            Assert.IsTrue(homepage.IsConnected,"Load test of 2000 records failed!!Service not connected!");

        }

    }
}
