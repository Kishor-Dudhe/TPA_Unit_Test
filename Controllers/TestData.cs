using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxProfileAssistant.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using TaxProfileAssistant.Models;
using TaxProfileAssistant.Utils;
using System.Data.Entity;

namespace TaxProfileAssistantTests
{
    public  class TestData
    {

        public AccountInfo InitializeTestData()
        {
            var _temp = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
            XmlReader xmlReader = XmlReader.Create(@"..\XMLTestFiles\TPA_500.xml");
            var tpa = new TPAScaffold();
            var serializer = new XmlSerializer(tpa.GetType());
            TPAScaffold input = (TPAScaffold)serializer.Deserialize(xmlReader);
            TaxProfileAssistantController tpaController = new TaxProfileAssistantController();
            String resultStr = tpaController.Post(input);
            string ID = resultStr.Substring(38);
            AccountInfo accountInfo = AvaTaxProfileAssistantHelper.GetAccountInfoBySecureKey(ID);
            return accountInfo;
        }
           
            
        
    }
}
