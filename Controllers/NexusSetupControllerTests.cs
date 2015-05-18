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
    public class NexusSetupControllerTests
    {
        NexusSetupController nexusController;
        AccountInfo accountInfo;
        AccountService accountService;
        TempDataDictionary TempData;
        TPAScaffold input;
        TaxProfileAssistantController tpaController;
        string key;
       
        [SetUp]//Initializes data required for each test and runs once per test method.
        public void InitializeData()
        {
            //Arrange
            accountInfo = new AccountInfo();

            XmlReader xmlReader = XmlReader.Create(@"..\XMLTestFiles\TPA_500.xml");
            var tpa = new TPAScaffold();
            var serializer = new XmlSerializer(tpa.GetType());
            input = (TPAScaffold)serializer.Deserialize(xmlReader);

            tpaController = new TaxProfileAssistantController();
            tpaController.ControllerContext = new ControllerContext();
            string resultstr = tpaController.Post(input);
            key = resultstr.Substring(38);
            key = key.Replace(Constants.APPLICATION_URL_DUMMY_TEXT, "/");
            accountInfo = AvaTaxProfileAssistantHelper.GetAccountInfoBySecureKey(key);
          
            nexusController = new NexusSetupController();
            accountService = new AccountService(accountInfo.Webservice, accountInfo.UserName, accountInfo.Password, accountInfo.AccountNumber, accountInfo.LicenseKey, accountInfo.ERPName);
            TempData = new TempDataDictionary();
            nexusController.TempData.Add("AccountInfo", accountInfo);
            nexusController.TempData.Add("AccountService", accountService);
        }

       [Test()]//Check if the method returns nexus list
        public void NexusSetupController_GetNexusList_ReturnsListOfNexusMasterTest()
        {
            //Act
            var nexusresult = nexusController.GetNexusList(key);

            //Assert
           // Assert.IsInstanceOfType(nexusresult, typeof(List<NexusMaster>));
            Assert.IsInstanceOf<List<NexusMaster>>(nexusresult);
        }         
                              
       
       [Test()]//Check if the method returns nexus list and nexus list is not null
       public void NexusSetupController_IndexMethod_ReturnsNexusListThroughViewTest()
       {
           //Act
           var nexusresult =nexusController.Index(key);
         
           //Assert
           Assert.IsInstanceOf<List<NexusMaster>>(((ViewResult)nexusresult).Model, "Incorrect result type.Method should return a list of type NexusMaster.!!");
           //Assert.IsInstanceOfType(((ViewResult)nexusresult).Model,typeof(List<NexusMaster>),"Incorrect result type.Method should return a list of type NexusMaster.!!");
           Assert.IsNotNull(nexusresult,"Result returned is empty!!");
       }

       //[Test()]//Check if the method throws proper exception
       //[ExpectedException(typeof(System.NullReferenceException))]
       //public void NexusSetupController_GetNexusList_ThrowsExceptionTest()
       //{
          
       //    NexusSetupController nexusController = new NexusSetupController();

       //    //Act
       //    var nexusresult = nexusController.GetNexusList();
         

       //}

        //[Test()]//Check if the correct model type is returned by view of this method
        //[ExpectedException(typeof(System.NullReferenceException))]
        //public void NexusSetupController_IndexMethod_ThrowsExceptionTest()
        //{
        //    //Arrange
        //    NexusSetupController nexusController = new NexusSetupController();
         
        //    //Act
        //    var nexusList = nexusController.Index();
                            
           
        //}
      
       
    }
}
