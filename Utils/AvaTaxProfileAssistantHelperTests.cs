using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using TaxProfileAssistant.Controllers;
using NUnit.Framework;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using TaxProfileAssistant.Models;
using TaxProfileAssistant.Utils;
using System.Data.Entity;

using System.Web;
using Moq;


namespace TaxProfileAssistantTest.Utils.Tests
{
    [TestFixture()]
    public class AvaTaxProfileAssistantHelperTests:Controller
    {
        AccountInfo accountInfo;
        TPAScaffold input;
        XmlReader xmlReader;

        [SetUp]
        public void InitializeData()
        {
            xmlReader = XmlReader.Create(@"..\XMLTestFiles\TPA_500.xml");
            var tpa = new TPAScaffold();
            var serializer = new XmlSerializer(tpa.GetType());
            input = (TPAScaffold)serializer.Deserialize(xmlReader);

        }

        [Test()]//Checks if correct type of result is returned.
        public void InsertAccountInfoTestToCheckReturnType()
        {
                      

            //Act
            var result = AvaTaxProfileAssistantHelper.InsertAccountInfo(input.AccountCredentials);

            //Assert
            //Assert.IsInstanceOfType(result, typeof(AccountInfo), "Incorrect type of result returned! ");
            Assert.IsInstanceOf<AccountInfo>(result, "Incorrect type of result returned! ");
           
        }

        [Test()]//Checks if data is inserted in database.
        public void InsertAccountInfoTestToCheckInsertionOfRecordInDatabase()
        {
                     

            //Act
            var actualresult = AvaTaxProfileAssistantHelper.InsertAccountInfo(input.AccountCredentials);
            var expectedresult = AvaTaxProfileAssistantHelper.GetAccountInfo(actualresult.ID);

            //Assert
            Assert.AreEqual(expectedresult.ID, actualresult.ID, "Insertion of account information in database failed!!Incorrect Account ID!");
            Assert.AreEqual(expectedresult.AccountNumber, actualresult.AccountNumber, "Insertion of account information in database failed!!Incorrect AccountNumber!");
            Assert.AreEqual(expectedresult.UserName, actualresult.UserName, "Insertion of account information in database failed!!Incorrect Username!");
            Assert.AreEqual(expectedresult.Password, actualresult.Password, "Insertion of account information in database failed!!Incorrect Password!");
            Assert.AreEqual(expectedresult.LicenseKey, actualresult.LicenseKey, "Insertion of account information in database failed!!Incorrect License key!");
            Assert.AreEqual(expectedresult.CompanyCode, actualresult.CompanyCode, "Insertion of account information in database failed!!Incorrect Company Code!");
            Assert.AreEqual(expectedresult.Webservice, actualresult.Webservice, "Insertion of account information in database failed!!Incorrect WebService!");


        }

        [Test()]//Check if the help links are being inserted in the database from xml.
        public void InsertHelpLinkTestToCheckInsertionOfHelpLinksInDatabase()
        {
           
            //Act
            var accinfo = AvaTaxProfileAssistantHelper.InsertAccountInfo(input.AccountCredentials);
            foreach (Link hl in input.HelpLink.Links)
            {
                var actualresult=AvaTaxProfileAssistantHelper.InsertHelpLink(hl, accinfo.ID);

                //Assert
                Assert.AreEqual(true, actualresult, "HelpLink insertion in database failed!!");
            }
                   
        }

        [Test()]//Check if the method returns a list of HelpLinks.
        public void GetLinkListTestToCheckIfItReturnsListOfHelpLinks()
        {
           //Arrange
            var accinfo = AvaTaxProfileAssistantHelper.InsertAccountInfo(input.AccountCredentials);

            //Act
            var actualresult = AvaTaxProfileAssistantHelper.GetLinkList(accinfo.ID);
            List<HelpLink> expectedresult = new List<HelpLink>();

            //Assert
            Assert.AreEqual(expectedresult.GetType(), actualresult.GetType(),"The method failed to return a list of helplinks!!");
            Assert.IsNotNull(actualresult, "The list does not contain any records!!");
                   
          
        }

        [Test()]//Check if it returns the correct accountInfo details.
        public void GetAccountInfoBySecureKeyTestToCheckIfCorrectAccountInfoDetailsAreFetched()
        {
            //Arrange
            TaxProfileAssistantController tpaController = new TaxProfileAssistantController();
            tpaController.ControllerContext = new ControllerContext();
            string resultstr = tpaController.Post(input);
            string key = resultstr.Substring(38);

            key = key.Replace(Constants.APPLICATION_URL_DUMMY_TEXT, "/");

            //Act
            AccountInfo accInfo = AvaTaxProfileAssistantHelper.GetAccountInfoBySecureKey(key);
            
            //Assert
            Assert.AreEqual(input.AccountCredentials.AccountNumber, accInfo.AccountNumber, "Incorrect AccountNumber returned!!");
            Assert.AreEqual(input.AccountCredentials.UserName, accInfo.UserName, "Incorrect UserName returned!!");
            Assert.AreEqual(input.AccountCredentials.Password, accInfo.Password, "Incorrect Password returned!!");
            Assert.AreEqual(input.AccountCredentials.LicenseKey, accInfo.LicenseKey, "Incorrect LicenseKey returned!!");


        }


        [Test()]//Check if newly cretaed company info are being inserted in the database from xml.
        public void InsertCreateCompanyInfoTestToCheckCompanyInfoInsertionInDatabase()
        {
          
            //Act
            var accinfo = AvaTaxProfileAssistantHelper.InsertAccountInfo(input.AccountCredentials);
            var CompanyInfo = AvaTaxProfileAssistantHelper.InsertCreateCompanyInfo(input.Company,accinfo.ID);

            //Assert
            Assert.AreEqual(true, CompanyInfo, "Company info insertion in database failed!!");
        }

        [Test()]//Check if it returns the correct Company Info details.
        public void GetCreateCompanyInfoTestToCheckIfCorrectCompanyDetailsAreReturned()
        {
          
            //Act
            var AccountInfo = AvaTaxProfileAssistantHelper.InsertAccountInfo(input.AccountCredentials);
            var InsertCompanyInfo = AvaTaxProfileAssistantHelper.InsertCreateCompanyInfo(input.Company,AccountInfo.ID);
            var CompanyInfo = AvaTaxProfileAssistantHelper.GetCreateCompanyInfo(AccountInfo.ID);

            //Assert
            Assert.AreEqual(input.Company.TIN, CompanyInfo.TIN,"Incorrect TIN!!");
            Assert.AreEqual(input.Company.CompanyName, CompanyInfo.CompanyName,"Incorrect Company name!!");
            
        }

        [Test()]//Check if AvaErpSettings are being inserted in database
        public void InsertAvaERPSettingTestToCheckInsertionOfAvaERPSettingsInDatabase()
        {
           
            //Act
            var AccountInfo = AvaTaxProfileAssistantHelper.InsertAccountInfo(input.AccountCredentials);
            var AvaERPSetting = AvaTaxProfileAssistantHelper.InsertAvaERPSetting(input.AvaERPSettings, AccountInfo.ID);


            //Assert
            Assert.IsNotNull(AvaERPSetting, "No records inserted!!");

        }
        [Test()]//Check if AvaErpSettingsID returned matches the one in database.
        public void InsertAvaERPSettingTestToCheckCorrectAvaERPSettingsIDReturned()
        {
           
            //Act
            var AccountInfo = AvaTaxProfileAssistantHelper.InsertAccountInfo(input.AccountCredentials);
            var AvaERPSetting = AvaTaxProfileAssistantHelper.InsertAvaERPSetting(input.AvaERPSettings, AccountInfo.ID);
            var expectedID = AvaTaxProfileAssistantHelper.GetAvaERPSettingMaster(AccountInfo.ID);


            //Assert
            Assert.AreEqual(expectedID.ID, AvaERPSetting, "The inserted and fetched ID does not match!!");

        }
        [Test()]//Check if the method successfully inserts customer records in database
        public void InsertCustomersTestToCheckInsertionOfCustomerRecordsInDatabase()
        {
            //Arrange
            xmlReader = XmlReader.Create(@"..\XMLTestFiles\TPA_New.xml");
            var tpa = new TPAScaffold();
            var serializer = new XmlSerializer(tpa.GetType());
            input = (TPAScaffold)serializer.Deserialize(xmlReader);

            var AccountInfo = AvaTaxProfileAssistantHelper.InsertAccountInfo(input.AccountCredentials);
            var AvaERPSetting = AvaTaxProfileAssistantHelper.InsertAvaERPSetting(input.AvaERPSettings, AccountInfo.ID);
           

            //Act
            foreach (AvaCustomer cust in input.AvaERPSettings.Customers)
            {
                var actualResult=AvaTaxProfileAssistantHelper.InsertCustomers(cust, AccountInfo.ID, AvaERPSetting);

                //Assert
                Assert.AreEqual(true, actualResult, "Customer Insertion failed!!");
            }
           
        }

        [Test()]//Check if it returns a list of customers.
        public void GetCustomersListTestToCheckIfItReturnsListOfCustomers()
        {
            //Arrange
            var AccountInfo = AvaTaxProfileAssistantHelper.InsertAccountInfo(input.AccountCredentials);

            //Act
            var actualResult = AvaTaxProfileAssistantHelper.GetCustomersList(AccountInfo.ID);

            //Assert
            //Assert.IsInstanceOfType(actualResult, typeof(List<Customers>), "Incorrect result type returned!!");
            Assert.IsInstanceOf<List<Customers>>(actualResult, "Incorrect result type returned!!");
           
        }
        [Test()]//Check if it returns correct number of customers as inserted from xml.
        public void GetCustomersListTestToCheckIfItReturnsCorrectNumberOfCustomers()
        {
            //Arrange
            xmlReader = XmlReader.Create(@"..\XMLTestFiles\TPA_New.xml");
            var tpa = new TPAScaffold();
            var serializer = new XmlSerializer(tpa.GetType());
            input = (TPAScaffold)serializer.Deserialize(xmlReader);

            var AccountInfo = AvaTaxProfileAssistantHelper.InsertAccountInfo(input.AccountCredentials);
            var AvaERPSetting = AvaTaxProfileAssistantHelper.InsertAvaERPSetting(input.AvaERPSettings, AccountInfo.ID);
           

            //Act
            foreach (AvaCustomer cust in input.AvaERPSettings.Customers)
            {
                var actualResult = AvaTaxProfileAssistantHelper.InsertCustomers(cust, AccountInfo.ID, AvaERPSetting);
            }

            var Result = AvaTaxProfileAssistantHelper.GetCustomersList(AccountInfo.ID);

            Assert.AreEqual(Result.Count, input.AvaERPSettings.Customers.Count, "Incorrect number of customers returned!!");

        }            
          
               
        [Test()]//Check if it returns a AvaERPSettingMaster
        public void GetAvaERPSettingMasterTestToCheckIfItReturnsAvaERPSettingMaster()
        {
            //Arrange
            var AccountInfo = AvaTaxProfileAssistantHelper.InsertAccountInfo(input.AccountCredentials);
            var AvaERPSetting = AvaTaxProfileAssistantHelper.InsertAvaERPSetting(input.AvaERPSettings, AccountInfo.ID);
            
            //Act
            var result = AvaTaxProfileAssistantHelper.GetAvaERPSettingMaster(AccountInfo.ID);

            //Assert
            //Assert.IsInstanceOfType(result, typeof(AvaERPSettingsMaster), "Incorrect result type returned!!");
            Assert.IsInstanceOf<AvaERPSettingsMaster>(result, "Incorrect result type returned!!");
        }

       
        //[Test()]RETURNS VOID
        //public void InsertNexusCompanyLocationTest()
        //{
        //    //Arrange
        //    XmlReader xmlReader = XmlReader.Create(@"..\XMLTestFiles\TPA_500.xml");
        //    var tpa = new TPAScaffold();
        //    var serializer = new XmlSerializer(tpa.GetType());
        //    input = (TPAScaffold)serializer.Deserialize(xmlReader);
        //    var AccountInfo = AvaTaxProfileAssistantHelper.InsertAccountInfo(input.AccountCredentials);
        //    int TotCntCmpLoc = input.Nexus.CompanyLocations.Count();
        //    bool SaveNow = false;
            
        //    //Act
                  
        //    for (int i = 0; i < TotCntCmpLoc; i++)
        //        {
        //            if (i % 500 == 0 || i == (TotCntCmpLoc - 1))
        //            {
        //                SaveNow = true;
        //            }
        //            else
        //                SaveNow = false;
        //            AvaTaxProfileAssistantHelper.InsertNexusCompanyLocation(input.Nexus.CompanyLocations[i], AccountInfo.ID, SaveNow);
        //        }
           
        //    //Assert
          

        
        //}


        //[Test()]RETURNS VOID
        //public void InsertNexusWareHourseLocationTest()
        //{
           
        //}

        //[Test()]RETURNS VOID
        //public void InsertNexusPreviousCustomerLocationTest()
        //{
           
        //}


        [Test()]//Check if it returns a list of NexusMaster.
        public void GetNexusesTestToCheckIfItReturnsNexusMaster()
        {
            //Arrange
            var AccountInfo = AvaTaxProfileAssistantHelper.InsertAccountInfo(input.AccountCredentials);
            int TotCntCmpLoc = input.Nexus.CompanyLocations.Count();
            bool SaveNow = false;

            //Act
            for (int i = 0; i < TotCntCmpLoc; i++)
            {
                if (i % 500 == 0 || i == (TotCntCmpLoc - 1))
                {
                    SaveNow = true;
                }
                else
                    SaveNow = false;
                AvaTaxProfileAssistantHelper.InsertNexusCompanyLocation(input.Nexus.CompanyLocations[i], AccountInfo.ID);
                AvaTaxProfileAssistantHelper.InsertNexusWareHourseLocation(input.Nexus.WareHouseLocations[i], AccountInfo.ID);
                AvaTaxProfileAssistantHelper.InsertNexusPreviousCustomerLocation(input.Nexus.PreviousCustomerLocations[i], AccountInfo.ID);
            }

            var result = AvaTaxProfileAssistantHelper.GetNexuses(AccountInfo.ID);

            //Assert
            //Assert.IsInstanceOfType(result, typeof(List<NexusMaster>), "Incorrect result type returned!!");
            Assert.IsInstanceOf<List<NexusMaster>>(result, "Incorrect result type returned!!");
        }

        //[Test()]RETURNS VOID
        //public void InsertAvaERPCountryMappingTest()
        //{
          


        //}

        //[Test()] RETURNS VOID
        //public void InsertEntityNameCodeTest()
        //{
        //    //Arrange
        //    XmlReader xmlReader = XmlReader.Create(@"..\XMLTestFiles\TPA_500.xml");
        //    var tpa = new TPAScaffold();
        //    var serializer = new XmlSerializer(tpa.GetType());
        //    input = (TPAScaffold)serializer.Deserialize(xmlReader);
        //    var AccountInfo = AvaTaxProfileAssistantHelper.InsertAccountInfo(input.AccountCredentials);
        //    int AvaERPSettingsID = AvaTaxProfileAssistantHelper.InsertAvaERPSetting(input.AvaERPSettings, AccountInfo.ID);

        //    //Act

        //     foreach (EntityNameCode ec in input.AvaERPSettings.MapItemCodes.NonTaxableItems.Items)
        //        {
        //            AvaTaxProfileAssistantHelper.InsertEntityNameCode(AccountInfo.ID, AvaERPSettingsID, ec, Constants.AVAERP_SETTINGS_ITEMS);
        //        }
            
            
        //}

        [Test()]//Check if it returns a list of AvaERPSettingsOtherDetails.
        public void GetAvaERPSettingOtherDetailsTestToCheckReturnType()
        {
            xmlReader = XmlReader.Create(@"..\XMLTestFiles\TPA_New.xml");
            var tpa = new TPAScaffold();
            var serializer = new XmlSerializer(tpa.GetType());
            input = (TPAScaffold)serializer.Deserialize(xmlReader);
            var AccountInfo = AvaTaxProfileAssistantHelper.InsertAccountInfo(input.AccountCredentials);
            int AvaERPSettingsID = AvaTaxProfileAssistantHelper.InsertAvaERPSetting(input.AvaERPSettings, AccountInfo.ID);

            //Act

            foreach (EntityNameCode ec in input.AvaERPSettings.MapItemCodes.NonTaxableItems.Items)
            {
                AvaTaxProfileAssistantHelper.InsertEntityNameCode(AccountInfo.ID, AvaERPSettingsID, ec, Constants.AVAERP_SETTINGS_ITEMS);
            }

            var result = AvaTaxProfileAssistantHelper.GetAvaERPSettingOtherDetails(AccountInfo.ID);

            //Assert

           // Assert.IsInstanceOfType(result, typeof(List<AvaERPSettingsOtherDetails>), "Incorrect result type returned!!");
            Assert.IsInstanceOf<List<AvaERPSettingsOtherDetails>>(result, "Incorrect result type returned!!");

        }

        [Test()]//Check if it returns a list of type AvaERPCountryMapping
        public void GetAvaERPCountryMappingTestToCheckIfItReturnsListOfAvaERPCountryMapping()
        {
            //Arrange          
            var AccountInfo = AvaTaxProfileAssistantHelper.InsertAccountInfo(input.AccountCredentials);
            int AvaERPSettingsID = AvaTaxProfileAssistantHelper.InsertAvaERPSetting(input.AvaERPSettings, AccountInfo.ID);

            //Act
            foreach (MappedCountry mc in input.AvaERPSettings.AddressValidation.MappedCountries)
            {
                
                AvaTaxProfileAssistantHelper.InsertAvaERPCountryMapping(AccountInfo.ID, AvaERPSettingsID, mc);
            }
            var result = AvaTaxProfileAssistantHelper.GetAvaERPCountryMapping(AccountInfo.ID);


            //Assert
            //Assert.IsInstanceOfType(result, typeof(List<AvaERPCountryMapping>), "Incorrect result type rturned!!");
            Assert.IsInstanceOf<List<AvaERPCountryMapping>>(result, "Incorrect result type returned!!");

            
        }

        [Test()]//Check if the method does not return an empty result
        public void GetAvaERPCountryMappingTestToCheckIfItDoesNotReturnEmptyResult()
        {
            //Arrange
            var AccountInfo = AvaTaxProfileAssistantHelper.InsertAccountInfo(input.AccountCredentials);
            int AvaERPSettingsID = AvaTaxProfileAssistantHelper.InsertAvaERPSetting(input.AvaERPSettings, AccountInfo.ID);

            //Act
            foreach (MappedCountry mc in input.AvaERPSettings.AddressValidation.MappedCountries)
            {
                AvaTaxProfileAssistantHelper.InsertAvaERPCountryMapping(AccountInfo.ID, AvaERPSettingsID, mc);
            }
            var result = AvaTaxProfileAssistantHelper.GetAvaERPCountryMapping(AccountInfo.ID);


            //Assert
            Assert.IsNotNull(result, "Empty result returned!!");



        }

        [Test()]//Check if the method returns correct number of mapped countries
        public void GetAvaERPCountryMappingTestToCheckIfItReturnsCorrectNumberOfMappedCountries()
        {
            //Arrange
            var AccountInfo = AvaTaxProfileAssistantHelper.InsertAccountInfo(input.AccountCredentials);
            int AvaERPSettingsID = AvaTaxProfileAssistantHelper.InsertAvaERPSetting(input.AvaERPSettings, AccountInfo.ID);

            //Act
            foreach (MappedCountry mc in input.AvaERPSettings.AddressValidation.MappedCountries)
            {
                AvaTaxProfileAssistantHelper.InsertAvaERPCountryMapping(AccountInfo.ID, AvaERPSettingsID, mc);
            }
            var result = AvaTaxProfileAssistantHelper.GetAvaERPCountryMapping(AccountInfo.ID);


            //Assert
            Assert.AreEqual(input.AvaERPSettings.AddressValidation.MappedCountries.Count, result.Count, "Incorrect number of mapped countries fetched!");


        }




        //[Test()] NOT BEING USED
        //public void GetCountriesTest()
        //{
            
        //}

        //[Test()] NOT BEING USED
        //public void GetCountryStateMappingTest()
        //{
        //    Assert.Fail();
        //}

        //[Test()] NOT BEING USED
        //public void GetCountriesByCodeTest()
        //{
        //    Assert.Fail();
        //}

        //[Test()] NOT BEING USED
        //public void GetCountryStateMappingByCodeTest()
        //{
        //    Assert.Fail();
        //}

        [Test()]//Check if the method returns correct state and company code
        public void ParseStateCountryTestToCheckIfItReturnsCorrectStateAndComapnyCode()
        {
            //Arrange
            string country = "USA";
            string state = "WASHINGTON";
            string StateCode="";
            string CountryCode = "";


            //Act
            AvaTaxProfileAssistantHelper.ParseStateCountry(state, country, out StateCode, out CountryCode);
            Assert.AreEqual("US", CountryCode, "Incorrect CountryCode returned!!");
            Assert.AreEqual("WA", StateCode, "Incorrect state code!!");
            
        }

        [Test()]//Check if the method returns correct state code
        public void GetStateCodeTestToCheckIfItReturnsCorrectStateCode()
        {
            //Arrange
            string state = "WASHINGTON";

            //Act
            var result = AvaTaxProfileAssistantHelper.GetStateCode(state);

            //Assert
            Assert.AreEqual("WA", result, "Incorrect state code returned!!");
           
        }

        [Test()]//Check if it trims the state code to first 2 letters.
        public void TrimCodeTestToCheckIfFirstTwoLettersOfStringAreReturned()
        {
            //Arrange
            string state = "WASHINGTON";
            string country = "USA";

            //Act
            var StateResult = AvaTaxProfileAssistantHelper.TrimCode(state);
            var CountryResult = AvaTaxProfileAssistantHelper.TrimCode(country);

            //Assert
            Assert.AreEqual("WA", StateResult, "Incorrect state code returned!!");
            Assert.AreEqual("US", CountryResult, "Incorrect country code returned!!");
        }

        [Test()]//Check if correct code is returned for state or country
        public void ParseCodeTestToCheckIfCorrectCodeIsReturnedForStateOrCountry()
        {
            //Arrange
            string state = "MAHARASHTRA";
            string country = "CANADA";
            
            
            //Act
            var StateResult = AvaTaxProfileAssistantHelper.ParseCode(state,ParseCodeCollection.State);
            var CountryResult = AvaTaxProfileAssistantHelper.ParseCode(country,ParseCodeCollection.Country);

            //Assert
            Assert.AreEqual("MH", StateResult, "Incorrect state code returned!!");
            Assert.AreEqual("CA", CountryResult, "Incorrect country code returned!!");
            
        }


        public AvaTaxProfileAssistantHelperTests()
        {
            ControllerContext = (new Mock<ControllerContext>()).Object;
        }
 
        public bool TestTryValidateModel(object model)
        {
            return TryValidateModel(model);
        }


        [Test()]//check modelstate and if false return errorstring
        public void GetValidationErrorMessagesTestForInvalidModel()
        {
            //Arrange
            xmlReader = XmlReader.Create(@"..\XMLTestFiles\TPA_1000_AccountCredentialsMissing.xml");
            var tpa = new TPAScaffold();
            var serializer = new XmlSerializer(tpa.GetType());
            input = (TPAScaffold)serializer.Deserialize(xmlReader);
            AvaTaxProfileAssistantHelperTests taxProfileAsstHelper = new AvaTaxProfileAssistantHelperTests();
            TryValidateModel(input.AccountCredentials);
            string ErrorString = "";
            
            //Act
            AvaTaxProfileAssistantHelper.GetValidationErrorMessages(ModelState, ref ErrorString);
            
            //Assert
            Assert.AreEqual(false, ModelState.IsValid, "Model state cannot be true when invalid credentials are passed!!");
        }


        //[Test()]//check modelstate if valid credentials are passed
        //public void GetValidationErrorMessagesTestForValidModel()
        //{
        //    //Arrange

        //    AvaTaxProfileAssistantHelperTests taxProfileAsstHelper = new AvaTaxProfileAssistantHelperTests();
        //    TryValidateModel(input.AccountCredentials);
        //    string ErrorString = "";

        //    //Act
        //    AvaTaxProfileAssistantHelper.GetValidationErrorMessages(ModelState, ref ErrorString);

        //    //Assert
        //    //Assert.IsTrue(ModelState.IsValid, "Model state cannot be false when valid credentials are passed!!");
        //    Assert.IsTrue(ModelState.IsValid, "Model state cannot be false when valid credentials are passed!!");
        //}

        //[Test()]//Check if thread status is set to true when executed.
        //public void SaveThreadTestToCheckIfItSetsThreadStatusToTrue()
        //{
        //    //Arrange
        //    XmlReader xmlReader = XmlReader.Create(@"..\XMLTestFiles\TPA_500.xml");
        //    var tpa = new TPAScaffold();
        //    var serializer = new XmlSerializer(tpa.GetType());
        //    input = (TPAScaffold)serializer.Deserialize(xmlReader);
        //    var AccountInfo = AvaTaxProfileAssistantHelper.InsertAccountInfo(input.AccountCredentials);
          
        //    int ID = AccountInfo.ID;

        //    TaxProfileAssistantController tpaController = new TaxProfileAssistantController();
        //    tpaController.ControllerContext = new ControllerContext();
        //    int ActionFlag = 1;
        //    Thread PushWareHousLocThread = new Thread(() =>tpaController.PushWareHousLoc(input,ID));
        //    //PushWareHousLocThread.IsBackground = true;
        //    PushWareHousLocThread.Start();
        //    if (PushWareHousLocThread.ThreadState == ThreadState.Running)
        //    {
        //        AvaTaxProfileAssistantHelper.SaveThread("PushWareHousLocThread", ID, ActionFlag);
        //    }

        //    //Assert
        //  //  Assert.AreEqual(true, Tprocess.TState, "Thread status must be true!!");

            
        //}

        //[Test()]
        //public void GetThreadStatusTest()
        //{
        //    Assert.Fail();
        //}

        //[Test()]
        //public void SetTimeStampTest()
        //{
        //    Assert.Fail();
        //}
    }
}
