using System;
using System.IO;
using Xunit;
using OrderBot;
using Microsoft.Data.Sqlite;

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
        public void TestProductFalse()
        {
            Session oSession = new Session("12345");
            oSession.OnMessage("hello");
            String sInput=oSession.OnMessage("1");
            sInput=oSession.OnMessage("abcd");
            Assert.True(sInput.ToLower().Contains("invalid input"));
             sInput=oSession.OnMessage("123");
            Assert.True(sInput.ToLower().Contains("invalid input"));
            sInput=oSession.OnMessage("chicken");
            Assert.True(sInput.ToLower().Contains("cannot be processed"));
        }

        [Fact]
        public void TestProductTrue()
        {
            Session oSession = new Session("12345");
            oSession.OnMessage("hello");
            String sInput=oSession.OnMessage("2");
            sInput=oSession.OnMessage("chicken");
            Assert.True(sInput.ToLower().Contains("apart from chicken"));
            sInput=oSession.OnMessage("beef");
            Assert.True(sInput.ToLower().Contains("apart from chicken, beef"));
            sInput=oSession.OnMessage("beef");
            Assert.True(sInput.ToLower().Contains("already selected"));
            sInput=oSession.OnMessage("y");
            Assert.True(sInput.ToLower().Contains("what is the quality criteria"));
            Assert.True(sInput.ToLower().Contains("chicken beef"));
        }

        [Fact]
        public void TestQualityFalse()
        {
            Session oSession = new Session("12345");
            oSession.OnMessage("hello");
            String sInput=oSession.OnMessage("2");
            sInput=oSession.OnMessage("chicken");
            sInput=oSession.OnMessage("beef");
            sInput=oSession.OnMessage("y");
            sInput=oSession.OnMessage("123");
            Assert.True(sInput.ToLower().Contains("invalid input"));
            sInput=oSession.OnMessage("abcd");
            Assert.True(sInput.ToLower().Contains("invalid input"));
            sInput=oSession.OnMessage("soil");
            Assert.True(sInput.ToLower().Contains("not a valid quality criteria"));
        }

        [Fact]
        public void TestQualityTrue()
        {
            Session oSession = new Session("12345");
            oSession.OnMessage("hello");
            String sInput=oSession.OnMessage("2");
            sInput=oSession.OnMessage("chicken");
            sInput=oSession.OnMessage("beef");
            sInput=oSession.OnMessage("y");
            sInput=oSession.OnMessage("fat");
            Assert.True(sInput.ToLower().Contains("apart from"));
            sInput=oSession.OnMessage("fat");
            Assert.True(sInput.ToLower().Contains("already selected"));
            sInput=oSession.OnMessage("fungus");
            Assert.True(sInput.ToLower().Contains("apart from"));
            sInput=oSession.OnMessage("y");
            Assert.True(sInput.ToLower().Contains("almost close"));
        }

         [Fact]
        public void TestBookAppointmentFalse()
        {
            Session oSession = new Session("12345");
            oSession.OnMessage("hello");
            String sInput=oSession.OnMessage("2");
            sInput=oSession.OnMessage("chicken");
            sInput=oSession.OnMessage("beef");
            sInput=oSession.OnMessage("y");
            sInput=oSession.OnMessage("fat");
            sInput=oSession.OnMessage("fungus");
            sInput=oSession.OnMessage("y");
            sInput=oSession.OnMessage("123");
            Assert.True(sInput.ToLower().Contains("date is not available"));
            sInput=oSession.OnMessage("abcd");
            Assert.True(sInput.ToLower().Contains("invalid input"));
        }
        
        [Fact]
        public void TestBookAppointmentTrue()
        {
            Session oSession = new Session("12345");
            oSession.OnMessage("hello");
            String sInput=oSession.OnMessage("2");
            sInput=oSession.OnMessage("chicken");
            sInput=oSession.OnMessage("beef");
            sInput=oSession.OnMessage("y");
            sInput=oSession.OnMessage("fat");
            sInput=oSession.OnMessage("fungus");
            sInput=oSession.OnMessage("y");
            sInput=oSession.OnMessage("2");
            Assert.True(sInput.ToLower().Contains("successfully confirmed"));
            Assert.True(sInput.ToLower().Contains("am"));
            Assert.True(sInput.ToLower().Contains("chicken beef"));
            Assert.True(sInput.ToLower().Contains("fat fungus"));
            sInput=oSession.OnMessage("a");
            Assert.True(sInput.ToLower().Contains("appointment is confirmed"));
            sInput=oSession.OnMessage("1");
            Assert.True(sInput.ToLower().Contains("appointment is confirmed"));
        }
     
        [Fact]
        public void TestDB()
        {

            //book appointment
            Session oSession = new Session("12345");
            oSession.OnMessage("hello");
            String sInput=oSession.OnMessage("2");
            sInput=oSession.OnMessage("chicken");
            sInput=oSession.OnMessage("beef");
            sInput=oSession.OnMessage("y");
            sInput=oSession.OnMessage("fat");
            sInput=oSession.OnMessage("fungus");
            sInput=oSession.OnMessage("y");
            sInput=oSession.OnMessage("2");

            int firstStringPosition = sInput.ToLower().IndexOf("appointment id: ");  
               
            string appointmentID = sInput.Substring(firstStringPosition+16,     
            9);
            string sPath = DB.GetConnectionString();
            SqliteConnection connection=new SqliteConnection(sPath);
            connection.Open();
            string query="Select * from Orders where appointmentID="+appointmentID;
            SqliteCommand cmd=new SqliteCommand(query, connection);
            SqliteDataReader reader=cmd.ExecuteReader();
            while(reader.Read()){
            Assert.True(reader.HasRows);
            //Console.WriteLine(reader["appointmentId"]+" "+appointmentID);
            Assert.True(reader["appointmentId"].Equals(appointmentID));
            }
        }
    }
}
