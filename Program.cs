using System;
using Newtonsoft.Json;
using System.Configuration;
using System.IO;
using System.Net;


namespace Service_Now
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ServiceNowLogger sl = new ServiceNowLogger();
                string inc = sl.CreateIncidentTicket("title","desc");
                System.Console.WriteLine("Incident Created");
                System.Console.WriteLine("Incident Number: " + inc);
            }
            catch(Exception ex)
            {
                System.Console.WriteLine("Exception Caught while creating an Incident");
                System.Console.WriteLine(ex);
            }
        }
    }
}
