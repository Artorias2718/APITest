using RestSharp;
using RestSharp.Serializers.Utf8Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace APITest.Utilities
{
    public class APIManager
    {
        public static string RestCredentials { get; set; } = "";
        public static string GraphQLCredentials { get; set; } = "";

        /// <summary>
        /// Make a REST API call.
        /// </summary>
        /// <param name="i_sApiBase">Base URI for API.</param>
        /// <param name="i_sApiEndpoint">API Method.</param>
        /// <param name="i_sHttpMethod">HTTP Method to use (GET, POST, PUT, etc.)</param>
        /// <param name="i_sPayload">For POST requests, a JSON string containing input data for the API request.</param>
        /// <returns>JSON-formatted string containing a response object (if any) or null if nothing was sent back.</returns>
        /// 
        public static IRestResponse QueryRest(string i_sApiBase, string i_sApiEndpoint, Method i_sHttpMethod, string i_sPayload = "")
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string sApiUri = $"{i_sApiBase}/{i_sApiEndpoint}";
            var client = new RestClient(sApiUri);

            client.PreAuthenticate = true;
            var request = new RestRequest(i_sHttpMethod);

            request.AddHeader("Authorization", $"Basic {RestCredentials}"); //must include the access token on every call
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.AddHeader("X-hedtech-Media-Type", "application/vnd.hedtech.v4+json");

            if (!string.IsNullOrEmpty(i_sPayload))
            {
                request.AddJsonBody(i_sPayload);
            }

            IRestResponse oRestResponse = client.Execute(request);

            if (oRestResponse.ErrorException != null)
            {
                string message = $"Rest API Error: {sApiUri}";

                string body = $"An error occured when attempting to call {sApiUri}";

                if (!string.IsNullOrEmpty(i_sPayload))
                {
                    body += $" with input data<br/>{i_sPayload}<br/>{oRestResponse}";
                }

                //TODO turn this back on

                //EmailManager.SendEmail(
                //    new Email
                //    {
                //        FromName = "Weber State University",
                //        FromEmail = "noreply@weber.edu",
                //        ToName = "OCE Marketing",
                //        ToEmail = "oce-marketing+web@weber.edu",
                //        Subject = "REST API Call",
                //        Body = body
                //    }
                //);

                throw new Exception(message + ". " + body);
            }

            return oRestResponse;
        }

        /// <summary>
        /// Accepts a string format for the GraphQL query, initializes the connections, and returns the response
        /// </summary>
        /// <param name="i_sPayload">GraphQL string without the header information</param>
        /// <returns>Query results in JSON format</returns>
        public static IRestResponse QueryGraphQL(string i_sApiBase, string i_sPayload)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var oClient = new RestClient(i_sApiBase);
            oClient.UseUtf8Json();
            var oRequest = new RestRequest(Method.POST);

            oRequest.AddHeader("Authorization", "Bearer " + GraphQLCredentials); //must include the access token on every call
            oRequest.AddParameter("application/json", i_sPayload, ParameterType.RequestBody);

            IRestResponse oResponse = oClient.Execute(oRequest);

            if (oResponse.ErrorException != null)
            {
                string message = "GraphQL API Error " + oResponse.ErrorException.Message;
                var bannerException = new ApplicationException(message, oResponse.ErrorException);
                throw bannerException;
            }

            if (oResponse.StatusCode != HttpStatusCode.OK)
            {
                string message = "GraphQL API Error: The remote server returned a " + oResponse.StatusCode.ToString() + " response.";
                var bannerException = new ApplicationException(message, oResponse.ErrorException);
                throw bannerException;
            }

            return oResponse;
        }
    }
}
