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
using Avalara.AvaTax.Services.Proxies.AccountSvcProxy;

namespace TaxProfileAssistantTest.Utils.Tests
{
    [TestFixture()]
    public class AccountServiceHelperTests
    {
        XmlReader xmlReader;
        TPAScaffold input;
        AccountInfo accountInfo;
        AccountService accountService;
        CreateCompanyController createCompanyController;
        CreateCompany createCompany;
        NexusSetupController nexusController;
        [SetUp]
        public void InitializeData()
        {
            xmlReader = XmlReader.Create(@"..\XMLTestFiles\TPA_500.xml");
            var tpa = new TPAScaffold();
            var serializer = new XmlSerializer(tpa.GetType());
            input = (TPAScaffold)serializer.Deserialize(xmlReader);
            accountInfo = AvaTaxProfileAssistantHelper.InsertAccountInfo(input.AccountCredentials);
            accountService = new AccountService(accountInfo.Webservice, accountInfo.UserName, accountInfo.Password, accountInfo.AccountNumber, accountInfo.LicenseKey, accountInfo.ERPName);
            createCompanyController = new CreateCompanyController();
            createCompanyController.TempData.Add("AccountInfo", accountInfo);
            createCompanyController.TempData.Add("AccountService", accountService);
           
            createCompany = new CreateCompany();
            createCompany.AccountInfoID = accountInfo.ID;
            createCompany.AddressLine1 = "900 winslow way e";
            createCompany.AddressLine2 = "";
            createCompany.AddressLine3 = "";
            createCompany.CompanyCode = Guid.NewGuid().ToString().Substring(0, 8);
            createCompany.CompanyName = "Systems of America1";
            createCompany.Country = "US";
            createCompany.StateProvince = "WA";
            createCompany.TIN = "95-3657472";

            createCompany.FirstName = "abbott";
            createCompany.LastName = "abbott";
            createCompany.PhoneNumber = "9999999999";
            createCompany.Fax = "9999999999";
            createCompany.Email = "abc@abc.com";
            createCompany.City = "Bainbridge Island";
            createCompany.ZipCode = "98110";
            createCompany.MobileNumber = "";
            createCompany.Title = "";
            createCompany.BIN = "";
        }

        [Test()]//Check if the CompanySaveResultType Is Returned.
        public void CreateCompanyTestToCheckIfCorrectResultTypeIsReturned()
        {
                      

            //Act
            var actualResult = AccountServiceHelper.CreateCompany(createCompany, accountInfo, createCompanyController);

            //Assert
            //Assert.IsInstanceOfType(actualResult, typeof(CompanySaveResult), "Incorrect result type returned!!");
            Assert.IsInstanceOf<CompanySaveResult>(actualResult, "Incorrect result type returned!!");
        }

        [Test()]//Check if the Correct ResultCode Is Returned.
        public void CreateCompanyTestToCheckIfResultCodeIsReturned()
        {
            //Arrange
            var Success=SeverityLevel.Success;

            //Act
            var actualResult = AccountServiceHelper.CreateCompany(createCompany, accountInfo, createCompanyController);
            
            //Assert
            Assert.AreEqual(Success,actualResult.ResultCode,"ResultCode must be Success");
        }
        [Test()]//Check if the Correct CompanyID and TransactionID Is Returned.
        public void CreateCompanyTestToCheckIfCompanyID_And_TransactionIDIsReturned()
        {
            //Act
            var actualResult = AccountServiceHelper.CreateCompany(createCompany, accountInfo, createCompanyController);

            //Assert
            Assert.IsNotNull(actualResult.CompanyId, "Null CompanyID returned!!");
            Assert.IsNotNull(actualResult.TransactionId, "Null TransactionID returned!!");      
                  
          
        }


        [Test()]//check if resultset is not null.
        public void CreateNexusTestToCheckIfNullResultsetIsNotReturned()
        {
            //Arrange
            Dictionary<string, string> countryState = new Dictionary<string, string>();
            countryState.Add("IN", "MH");
            var companyResult = AccountServiceHelper.CreateCompany(createCompany, accountInfo, createCompanyController);
            int companyID = companyResult.CompanyId;
           
            nexusController = new NexusSetupController();
            nexusController.TempData.Add("AccountInfo", accountInfo);
            nexusController.TempData.Add("AccountService", accountService);

            //Act

            var actualResult = AccountServiceHelper.CreateNexus(accountInfo,countryState, companyID, nexusController);

            //Assert
            Assert.IsNotNull(actualResult, "resultset must not be null!!");


        }

        [Test()]//check if resultcode is success when unique nexus is created
        public void CreateNexusTestToCheckIfResultCodeIsSuccessForUniqueNexus()
        {
            //Arrange
            var companyResult = AccountServiceHelper.CreateCompany(createCompany, accountInfo, createCompanyController);
            int companyID = companyResult.CompanyId;
            Dictionary<string, string> countryState = new Dictionary<string, string>();
            countryState.Add("IN", "RJ");
            nexusController = new NexusSetupController();
            nexusController.TempData.Add("AccountInfo", accountInfo);
            nexusController.TempData.Add("AccountService", accountService);
           
            var Success = SeverityLevel.Success;

            //Act

            var actualResult = AccountServiceHelper.CreateNexus(accountInfo,countryState, companyID, nexusController);

            //Assert
            Assert.AreEqual(Success,actualResult[0].ResultCode, "resultcode must be success!!");


        }
    }
}
