using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;

 public class ServiceNowLogger
 {
     //REST API Method Creating Incident on ServiceNow
     public string CreateIncidentTicket(string incidentTitle, string incidentDescription )
     {
         try
         {
             // HTTP API Creadentials
             string username = ConfigurationManager.AppSettings["ServiceNowUserName"];
             string password = ConfigurationManager.AppSettings["ServiceNowPassword"];
             string url = ConfigurationManager.AppSettings["ServiceNowUrl"];
             var authentication = "Basic" + Convert.ToBase64String(Encoding.Default.GetBytes(username + ":" + password));

            // Passing Auth Credentials through Header.
             HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
             webRequest.Headers.Add("Authorization",authentication);
             webRequest.Method="Post";

            // Create a Json Object of the Ticket Details
             using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
             {
                 string jsonObj = JsonConvert.SerializeObject(new{
                     description = incidentTitle + Environment.NewLine + Environment.NewLine + incidentDescription,
                     short_description = ConfigurationManager.AppSettings["ServiceNowTicketShortDescription"],
                     contact_type = ConfigurationManager.AppSettings["ServiceNowContactType"],
                     category = ConfigurationManager.AppSettings["ServiceNowCategory"],
                     subcategory = ConfigurationManager.AppSettings["ServiceNowSubCategory"],
                     assignment_group = ConfigurationManager.AppSettings["ServiceNowAssignmentGroup"],
                     impact = ConfigurationManager.AppSettings["ServiceNowIncidentImpact"],
                     priority = ConfigurationManager.AppSettings["ServiceNowIncidentPriority"],
                     caller_id = ConfigurationManager.AppSettings["ServiceNowCallerId"],
                     cmdb_ci = ConfigurationManager.AppSettings["ServiceNowCatalogueName"],
                     comments = ConfigurationManager.AppSettings["ServiceNowTicketShortDescription"]
                 });

                 streamWriter.Write(jsonObj);
             }

             using(HttpWebResponse Webresponse = webRequest.GetResponse() as HttpWebResponse)
             {
                 var postResponse = new StreamReader(Webresponse.GetResponseStream()).ReadToEnd();

                 JObject parseResponse = JObject.Parse(postResponse.ToString());
                 // Check only Result Object 
                 JObject resultObj = (JObject)parseResponse["result"];
                 //Get Result Value
                 string incidentNumberCreated = ((JValue)resultObj.SelectToken("number")).Value.ToString();
                 return incidentNumberCreated;
             }
         }
         catch(Exception ex)
         {
            System.Console.WriteLine("Exception:" + ex);
            return ex.ToString();
         }
     }
 }