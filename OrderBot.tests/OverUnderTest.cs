using System;
using System.IO;
using Xunit;
using OrderBot;

namespace OrderBot.tests
{
    public class OverUnderTest
    {
        [Fact]
        public void TestWelcome()
        {
            Session oSession = new Session("12345");
            String sInput = oSession.OnMessage("hello");
            Assert.True(sInput.Contains("Welcome"));
            Assert.True(sInput.Contains("P&P Imaging A"));
        }
        [Fact]
        public void TestMachineTypeTrue()
        {
            Session oSession = new Session("12345");
            oSession.OnMessage("hello");
            String sInput=oSession.OnMessage("2");
            Assert.True(sInput.ToLower().Contains("food products"));
            Assert.True(sInput.ToLower().Contains("1583"));
        }
        [Fact]
        public void TestMachineTypeFalse()
        {
            Session oSession = new Session("12345");
            oSession.OnMessage("hello");
            String sInput=oSession.OnMessage("5");
            Assert.True(sInput.ToLower().Contains("wrong input"));
            sInput=oSession.OnMessage("abcd");
            Assert.True(sInput.ToLower().Contains("wrong input"));
        }
        [Fact]
        public void TestSize()
        {
            Session oSession = new Session("12345");
            String sInput = oSession.OnMessage("hello");
            Assert.True(sInput.ToLower().Contains("choice"));
        }
              
        public void TestChicken()
        {
            string sPath = DB.GetConnectionString();
            Session oSession = new Session("12345");
            oSession.OnMessage("hello");
            oSession.OnMessage("large");
            String sInput = oSession.OnMessage("chicken");
            Assert.True(sInput.ToLower().Contains("toppings"));
            Assert.True(sInput.ToLower().Contains("large"));
            Assert.True(sInput.ToLower().Contains("chicken"));
        }
    }
}
