//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TaxProfileAssistant.Controllers;
//using NUnit.Framework;
//using System.Web.Mvc;
//using System.Xml;
//using System.Xml.Serialization;
//using TaxProfileAssistant.Models;
//using TaxProfileAssistant.Utils;
//using System.Data.Entity;
//using System.Web;




//namespace TaxProfileAssistantTest.Controllers.Tests
//{
//    [TestFixture()]
//    public class CredentialsControllerTests
//    {
//        AccountInfo accountInfo;
//        //CredentialsController credentialsController;
//        TPAScaffold input;
//        TaxProfileAssistantController tpaController;
//        [SetUp]//Initializes data required for each test and runs once per test method.
//        public void InitializeData()
//        {
//            //Arrange
//            accountInfo = new AccountInfo();
           
//            XmlReader xmlReader = XmlReader.Create(@"..\XMLTestFiles\TPA_500.xml");
//            var tpa = new TPAScaffold();
//            var serializer = new XmlSerializer(tpa.GetType());
//            input = (TPAScaffold)serializer.Deserialize(xmlReader);
            
//            tpaController = new TaxProfileAssistantController();
//            tpaController.ControllerContext = new ControllerContext();
//            string resultstr = tpaController.Post(input);
//            string key = resultstr.Substring(38);
//            key= key.Replace(Constants.APPLICATION_URL_DUMMY_TEXT, "/");
//            accountInfo = AvaTaxProfileAssistantHelper.GetAccountInfoBySecureKey(key);
                     

//            credentialsController = new CredentialsController();
//        }

//       [Test()]//Check if index method redirects to index method of NexusSetupController
//        public void CredentialsControllerIndexTest_RedirectstoNexusSetupController()
//        {
                                  
//            //Act
//            var credentialsresult = credentialsController.DoConnection(accountInfo);
//            var indexresult = credentialsController.Index();
            
//           //Assert
        
//            RedirectToRouteResult routeResult = indexresult as RedirectToRouteResult;
//            Assert.AreEqual(routeResult.RouteValues["action"], "Index","Index Method of Credentials Controller failed to redirect to correct action!!");
//            Assert.AreEqual(routeResult.RouteValues["controller"], "NexusSetup","Index Method of Credentials Controller failed to redirect to correct controller!!");
           

//        }

//       [Test()]//Check if index method returns a type of RedirectToRouteResult
//       public void CredentialsControllerIndexTest_ReturnsRedirectToRouteResult()
//       {
//           //Act

//           var credentialsresult = credentialsController.DoConnection(accountInfo);
//           var indexresult = credentialsController.Index();

//           //Assert
//           Assert.IsInstanceOf<RedirectToRouteResult>(indexresult, "Returned result is not of type RedirectToRouteResult!!");
//           //Assert.IsInstanceOfType(indexresult, typeof(RedirectToRouteResult),"Returned result is not of type RedirectToRouteResult!!");
//           Assert.IsNotNull(indexresult, "Redirect action of credentials controller needs to redirect to the Index action of nexussetup controller");
//       }

//       [Test()]//Check if DoConnection method returns the correct error message if some mandatory accountinfo parameters are not passed
//       public void CredentialsControllerDoConnectionTest_ThrowsCorrectErrorMessage()
//       {
//           accountInfo.AccountNumber = "";
          
//           //Act
//           var credentialsresult = credentialsController.DoConnection(accountInfo);

//           //Assert
//           Assert.AreEqual("Error: Attempt to invoke web service method failed. No Account to the web service has been provided. Check the configuration and try again.",credentialsresult);
           

//       }
//       [Test()]//Check if Index method returns the correct view if accountinfo object is null
//       public void CredentialsControllerIndexTest_ReturnsCorrectViewWhenAccountInfoObjectIsNull()
//       {
//           AccountInfo accountInfoNull = new AccountInfo();
//           accountInfoNull = null;

//           //Act
//           var indexresult = credentialsController.Index();


//           //Assert
//           //Assert.IsInstanceOfType(indexresult,typeof(ViewResult));
//           Assert.IsInstanceOf<ViewResult>(indexresult);

//       }
            
                 
//        [Test()]//Checks if DoConnection Returns NexusSetup string when correct accountinfo is passed
//        public void DoConnectionTestToCheckIfItReturnsTheNexusSetupString()
//        {
            
//            //Act
//            var credentialsresult = credentialsController.DoConnection(accountInfo);

//            //Assert

//            Assert.AreEqual("NexusSetup", credentialsresult);

//        }

//        [Test()]//Checks if DoConnection Returns CreateCompany string when incorrect CompanyCode is passed
//        public void DoConnectionTestToCheckIfItReturnsTheCreateCompanyString()
//        {

//            accountInfo.CompanyCode = "##";

//            //Act
          
//            var credentialsresult = credentialsController.DoConnection(accountInfo);

//            //Assert

//            Assert.AreEqual("CreateCompany", credentialsresult);
//        }


      
        
//    }
//}
