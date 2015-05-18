using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxProfileAssistant.Utils;
using NUnit.Framework;
using TaxProfileAssistant.Models;
using TaxProfileAssistant.Controllers;
using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Security.Tokens;
using Microsoft.Web.Services3.Addressing;
using Avalara.AvaTax.Services.Proxies.AccountSvcProxy;
using Avalara.AvaTax.Adapter.TaxService;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;
using System.Web.Mvc;



namespace TaxProfileAssistantTest.Utils.Tests
{
    [TestFixture()]
    public class AccountServiceTests
    {
        
        AccountInfo accountInfo;
        TPAScaffold input;
        TaxProfileAssistantController tpaController;
        XmlReader xmlReader;
        
       
       [SetUp]//Initializes data required for each test and runs once per test method.
        public  void InitializeData()
       {
            //Arrange
            accountInfo = new AccountInfo();

            xmlReader = XmlReader.Create(@"..\XMLTestFiles\TPA_500.xml");
            var tpa = new TPAScaffold();
            var serializer = new XmlSerializer(tpa.GetType());
            input = (TPAScaffold)serializer.Deserialize(xmlReader);

            tpaController = new TaxProfileAssistantController();
            tpaController.ControllerContext = new ControllerContext();
            string resultstr = tpaController.Post(input);
            string key = resultstr.Substring(38);
            key = key.Replace(Constants.APPLICATION_URL_DUMMY_TEXT, "/");
            accountInfo = AvaTaxProfileAssistantHelper.GetAccountInfoBySecureKey(key);

  
           
        }

       
        [Test()]//Test to check if a connection to AccountSvcWse is established.
        public void Utils_AccountServiceTest_CheckIfServiceConnectionIsSuccessful()
        {
            
            //Act
            AccountService accountService = new AccountService(accountInfo.Webservice, accountInfo.UserName, accountInfo.Password, accountInfo.AccountNumber, accountInfo.LicenseKey, accountInfo.ERPName);

            //Assert
            Assert.IsTrue(accountService.accSvc.isConnected,"Service Connection failed!!");

        }


        [Test()]//Negative Test to check if connection to AccountSvcWse fails if wrong password is passed .
        public void Utils_AccountServiceTest_CheckIfServiceConnectionFailsWithWrongPassword()
        {
            accountInfo.Password = "A";
                   
             //Act
            AccountService accountService = new AccountService(accountInfo.Webservice, accountInfo.UserName, accountInfo.Password, accountInfo.AccountNumber, accountInfo.LicenseKey, accountInfo.ERPName);

            //Assert
            Assert.IsFalse(accountService.accSvc.isConnected, "Service connection should fail!Password does'nt match!!");

        }


        [Test()]//Negative Test to check if connection to AccountSvcWse fails if wrong username is passed.
        public void Utils_AccountServiceTest_CheckIfServiceConnectionFailsWithWrongUserName()
        {
            accountInfo.UserName = "xyz";
            //Act
            AccountService accountService = new AccountService(accountInfo.Webservice, accountInfo.UserName, accountInfo.Password, accountInfo.AccountNumber, accountInfo.LicenseKey, accountInfo.ERPName);

            //Assert
            Assert.IsFalse(accountService.accSvc.isConnected, "Service connection should fail!Incorrect Username!!");

        }


        [Test()]//Negative Test to check if connection to AccountSvcWse fails if wrong accountNumber is passed.
        public void Utils_AccountServiceTest_CheckIfServiceConnectionFailsWithWrongAccountNumber()
        {
            accountInfo.AccountNumber = "1234";

            //Act
            AccountService accountService = new AccountService(accountInfo.Webservice, accountInfo.UserName, accountInfo.Password, accountInfo.AccountNumber, accountInfo.LicenseKey, accountInfo.ERPName);

            //Assert
            Assert.IsFalse(accountService.accSvc.isConnected, "Service connection should fail!Incorrect AccountNumber!!");

        }


        [Test()]//Negative Test to check if connection to AccountSvcWse fails if wrong LicenseKey is passed.
        public void Utils_AccountServiceTest_CheckIfServiceConnectionFailsWithWrongLicenseKey()
        {
            accountInfo.LicenseKey = "00abcd";
            //Act
            AccountService accountService = new AccountService(accountInfo.Webservice, accountInfo.UserName, accountInfo.Password, accountInfo.AccountNumber, accountInfo.LicenseKey, accountInfo.ERPName);

            //Assert
            Assert.IsFalse(accountService.accSvc.isConnected, "Service connection should fail!Incorrect LicenseKey!!");

        }


        [Test()]//Negative Test to check if connection to AccountSvcWse fails if WebServiceUrl is null.
        public void Utils_AccountServiceTest_CheckIfServiceConnectionFailsWithNoWebService()
        {
            accountInfo.Webservice = "";
            
            //Act
            AccountService accountService = new AccountService(accountInfo.Webservice, accountInfo.UserName, accountInfo.Password, accountInfo.AccountNumber, accountInfo.LicenseKey, accountInfo.ERPName);

            //Assert
            Assert.IsFalse(accountService.accSvc.isConnected, "Service connection should fail!WebService URL not specified!!");

        }


        [Test()]//Check if this method returns a servicelist for the the given accountID and company code.
        public void Utils_FetchServiceTest_CheckIfServiceFetchResultObjectIsReturned()
        {
           
            //Act
            AccountService accountService = new AccountService(accountInfo.Webservice, accountInfo.UserName, accountInfo.Password, accountInfo.AccountNumber, accountInfo.LicenseKey, accountInfo.ERPName);
            var result = accountService.FetchService(accountInfo.ID, accountInfo.CompanyCode);

            //Assert
           // Assert.IsInstanceOfType(result, typeof(ServiceFetchResult), "Incorrect type of result returned by FetchService");
            Assert.IsInstanceOf<ServiceFetchResult>(result, "Incorrect type of result returned by FetchService");
        }

        [Test()]//Checks that no records are returned when negative Account ID is passed.
        public void Utils_FetchServiceTest_CheckIfNoRecordsAreReturnedWithInvalidAccountID()
        {

            //Act
            AccountService accountService = new AccountService(accountInfo.Webservice, accountInfo.UserName, accountInfo.Password, accountInfo.AccountNumber, accountInfo.LicenseKey, accountInfo.ERPName);
            var result = accountService.FetchService(-2, accountInfo.CompanyCode);

            //Assert
            Assert.AreEqual(result.RecordCount, 0 , "No records should be returned when negative accountid is passed!!");
        }

     
        [Test()]//checks if it returns a result of type CompanySaveResult
        public void Utils_AccountServiceCreateCompanyTest_CheckIfCompanySaveResultIsReturned()
        {
            //Arrange
            Avalara.AvaTax.Services.Proxies.AccountSvcProxy.Company company = new Avalara.AvaTax.Services.Proxies.AccountSvcProxy.Company();

            company.AccountId = accountInfo.ID;
            company.CompanyCode = Guid.NewGuid().ToString().Substring(0, 8);
           
            company.CompanyName = "Systems of America";
            Random rnd = new Random();
            company.CompanyId = rnd.Next(1,300);
            company.ParentId = rnd.Next(1000, 2000);

            //Act
            AccountService accountService = new AccountService(accountInfo.Webservice, accountInfo.UserName, accountInfo.Password, accountInfo.AccountNumber, accountInfo.LicenseKey, accountInfo.ERPName);
            var result = accountService.CreateCompany(company);

            //Assert
            //Assert.IsInstanceOfType(result, typeof(CompanySaveResult), "Incorrect result type returned!");
            Assert.IsInstanceOf<CompanySaveResult>(result, "Incorrect result type returned!");
        }

        [Test()]//Checks if it returns a result of type CompanyFetchResult
        public void Utils_AccountServiceFetchCompanyTest_ChecksIfItReturnsCompanyFetchResult()
        {
            //Act
            AccountService accountService = new AccountService(accountInfo.Webservice, accountInfo.UserName, accountInfo.Password, accountInfo.AccountNumber, accountInfo.LicenseKey, accountInfo.ERPName);
            var result = accountService.FetchComapany(accountInfo.AccountNumber, accountInfo.CompanyCode);

            //Assert
            //Assert.IsInstanceOfType(result, typeof(CompanyFetchResult), "Incorrect result type returned!");
            Assert.IsInstanceOf<CompanyFetchResult>(result, "Incorrect result type returned!");
        }

        [Test()]//Checks if it returns a correct error message when invalid accountnumber is passed
        public void Utils_AccountServiceFetchCompanyTest_ChecksIfItReturnsCorrectErrorMessageWithInvalidAccounNumber()
        {
            //Act
            AccountService accountService = new AccountService(accountInfo.Webservice, accountInfo.UserName, accountInfo.Password, accountInfo.AccountNumber, accountInfo.LicenseKey, accountInfo.ERPName);
            var result = accountService.FetchComapany("abcd%$", "");

            //Assert
            Assert.IsInstanceOf<CompanyFetchResult>(result, "Incorrect result type returned!");
            //Assert.IsInstanceOfType(result, typeof(CompanyFetchResult), "Incorrect result type returned!");
            Assert.AreEqual(result.Messages[0].Details, "Invalid filter expression. Could not create tokens", "Incorrect error message!!");
            

        }

        [Test()]//Checks if it returns a result of type NexusFetchResult
        public void Utils_AccountServiceFetchNexusTest_ChecksIfItReturnsFetchNexusResult()
        {
            //Act
            AccountService asvc = new AccountService(accountInfo.Webservice, accountInfo.UserName, accountInfo.Password, accountInfo.AccountNumber, accountInfo.LicenseKey, accountInfo.ERPName);
            asvc.CompanyFetchResult = asvc.FetchComapany(accountInfo.AccountNumber, accountInfo.CompanyCode);
            int CompanyID = asvc.CompanyFetchResult.Companies[0].CompanyId;
            var result=asvc.FetchNexus(CompanyID,"");

            //Assert
            //Assert.IsInstanceOfType(result, typeof(NexusFetchResult), "Incorrect result type returned!");
            Assert.IsInstanceOf<NexusFetchResult>(result, "Incorrect result type returned!");
        }

        [Test()]//Negative test to Check if it returns a correct error message when invalid company id is passed
        public void Utils_AccountServiceFetchNexusTest_ChecksIfItReturnsCorrectErrorWithInvalidCompanyId()
        {
            //Act
            AccountService asvc = new AccountService(accountInfo.Webservice, accountInfo.UserName, accountInfo.Password, accountInfo.AccountNumber, accountInfo.LicenseKey, accountInfo.ERPName);
            int CompanyID = -10;
            var result = asvc.FetchNexus(CompanyID, "");

            //Assert
            Assert.AreEqual(result.Messages[0].Details, "Invalid filter expression. Could not create tokens", "Incorrect error message!!");
        }

      /*  [Test()]
        public void CreateNexusTest()
        {
            //Arrange
            AccountService asvc = new AccountService(accountInfo.Webservice, accountInfo.UserName, accountInfo.Password, accountInfo.AccountNumber, accountInfo.LicenseKey, accountInfo.ERPName);
            asvc.CompanyFetchResult = asvc.FetchComapany(accountInfo.AccountNumber, accountInfo.CompanyCode);
            int CompanyID = asvc.CompanyFetchResult.Companies[0].CompanyId;
           
            Avalara.AvaTax.Services.Proxies.AccountSvcProxy.Nexus[] nexusadd= new Avalara.AvaTax.Services.Proxies.AccountSvcProxy.Nexus[5];
            nexusadd[0].CompanyId=CompanyID;
            nexusadd[0].Country="US";
            nexusadd[0].HasLocalNexus=true;
            nexusadd[0].State="WA";
            
            
            

         

            var result = asvc.CreateNexus(nexusadd,CompanyID);
           
        }*/
    }
}
