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
using System.Threading;


namespace TaxProfileAssistantTest.Controllers.Tests
{
    [TestFixture()]
    public class ThreadProcessControllerTests
    {
        ThreadProcessController threadProcess;
        XmlReader xmlReader;
        TPAScaffold input;
        TaxProfileAssistantController tpaController;
        AccountInfo accountInfo;
      
        
        //Please pass accountinfo.id where thread status is 1.Check from database.
        [Test()]//Check if this method sets the respective thread status to true.
        public void ThreadProcessControllerGetThreadProcessStatus_ToCheckIfItSetsTheThreadStatusToTrue()
        {
            threadProcess = new ThreadProcessController();
            xmlReader = XmlReader.Create(@"..\XMLTestFiles\TPA_500.xml");
            var tpa = new TPAScaffold();
            var serializer = new XmlSerializer(tpa.GetType());
            input = (TPAScaffold)serializer.Deserialize(xmlReader);
           
            accountInfo = new AccountInfo();
            accountInfo.ID=206;


            //Act
            Thread PushWareHousLocThread = new Thread(() => tpaController.PushWareHouseLocation(input,accountInfo.ID));
            PushWareHousLocThread.Start();
            
            string tname = "PushWareHousLocThread";

            bool status = threadProcess.GetThreadProcessStatus(tname,accountInfo.ID);

            //Assert
            Assert.IsTrue(status, "Thread status not set to true!!");


        }

        [Test()]//Negative test to Check if this method sets the respective thread status to false if thread name is null.
        public void ThreadProcessControllerGetThreadProcessStatus_ToCheckIfItSetsTheThreadStatusToFalse()
        {
            threadProcess = new ThreadProcessController();
            xmlReader = XmlReader.Create(@"..\XMLTestFiles\TPA_500.xml");
            var tpa = new TPAScaffold();
            var serializer = new XmlSerializer(tpa.GetType());
            input = (TPAScaffold)serializer.Deserialize(xmlReader);
           
            accountInfo = new AccountInfo();
            accountInfo.ID=206;
                                 
            
            //Act
            Thread PushWareHousLocThread = new Thread(() => tpaController.PushWareHouseLocation(input, accountInfo.ID));
            PushWareHousLocThread.Start();

            string tname = "";

            bool status = threadProcess.GetThreadProcessStatus(tname, accountInfo.ID);

            //Assert
            Assert.IsFalse(status, "Thread status must be false if thread name is null!!");


        }


        [Test()]//Negative test to Check if this method sets the respective thread status to false if thread name contains special characters.
        public void GetThreadProcessStatus_ToCheckIfItSetsTheThreadStatusToFalseWithInvalidThreadNameParameter()
        {

            threadProcess = new ThreadProcessController();
            xmlReader = XmlReader.Create(@"..\XMLTestFiles\TPA_500.xml");
            var tpa = new TPAScaffold();
            var serializer = new XmlSerializer(tpa.GetType());
            input = (TPAScaffold)serializer.Deserialize(xmlReader);
            
            accountInfo = new AccountInfo();
            accountInfo.ID=206;

            //Act
            Thread PushWareHousLocThread = new Thread(() => tpaController.PushWareHouseLocation(input, accountInfo.ID));
            PushWareHousLocThread.Start();

            string tname = "@@###";

            bool status = threadProcess.GetThreadProcessStatus(tname, accountInfo.ID);

            //Assert
            Assert.IsFalse(status, "Thread status must be false if thread name is null!!");


        }

    }
}
