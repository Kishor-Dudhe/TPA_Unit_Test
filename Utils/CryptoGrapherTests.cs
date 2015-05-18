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

namespace TaxProfileAssistant.Utils.Tests
{
    [TestFixture()]
    public class CryptoGrapherTests
    {
        TPAScaffold input;
        TaxProfileAssistantController tpaController;
        XmlReader xmlReader;
        CryptoGrapher crypt;

        [Test()]//Test working of decrypt function in CryptoGrapher
        public void DecryptTest()
        {
            //Arrange
            xmlReader = XmlReader.Create(@"..\XMLTestFiles\TPA_500.xml");
            var tpa = new TPAScaffold();
            var serializer = new XmlSerializer(tpa.GetType());
            input = (TPAScaffold)serializer.Deserialize(xmlReader);
            crypt = new CryptoGrapher();

            tpaController = new TaxProfileAssistantController();
            tpaController.ControllerContext = new ControllerContext();
            string resultstr = tpaController.Post(input);
            string key = resultstr.Substring(38);
            key = key.Replace(Constants.APPLICATION_URL_DUMMY_TEXT, "/");

            //Act
            string DecryptedKey = crypt.Decrypt(key);

            //Assert
            Assert.AreNotEqual(key, DecryptedKey);

        }



        [Test()]//Test working of Encrypt function in CryptoGrapher
        public void EncryptTest()
        {
            //Arrange
            crypt = new CryptoGrapher();
            string Key = DateTime.Now.ToString("ddmmyyyyhhMMssfffffff");

            //Act
            string EncryptedKey = crypt.Encrypt(Key);

            //Assert
            Assert.AreNotEqual(Key, EncryptedKey);

        }
    }
}
