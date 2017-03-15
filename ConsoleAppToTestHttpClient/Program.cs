using ConsoleAppToTestHttpClient.gov.ny.svc.test.daws;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleAppToTestHttpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //CallWebServ();



            BatchRequest batch = new BatchRequest();
            SearchRequest search = new SearchRequest();
            Filter filter = new Filter();
            dsmlQueryService client = new dsmlQueryService();
            client.Url = "https://qadaws.svc.ny.gov/daws/services/dsmlSoapQuery";
            batch.searchRequest = new SearchRequest[1] { search };

            //First stopped here and got 401 not authorized.  Which is good so going to add creds
            client.Credentials = new NetworkCredential("prxwsTL1HESC", "sfvwRMnB7N");

            //After just adding creds and no search info error.message is DN missing in request: &lt;searchRequest derefAliases="neverDerefAliases" scope="baseObject" xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns="urn:oasis:names:tc:DSML:2:0:core"/>
            //so going to just add a DN
            search.dn = "'o=ny, c=us'";

            //Can't believe it but it just made a tiny little complaint about a filter even with a big DN of all NY
            //Exactly one search filter needs to be in a search request: &lt;searchRequest derefAliases="neverDerefAliases" dn="&amp;apos;o = ny, c = us" scope="baseObject" xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns="urn:oasis:names:tc:DSML:2:0:core"/>
            //so time to add afilter but it is going to be blank
            //search.filter = filter;

            //suprisingly just adding a blank filter threw an exception...finally.
            // at System.Web.Services.Protocols.SoapHttpClientProtocol.ReadResponse(SoapClientMessage message, WebResponse response, Stream responseStream, Boolean asyncCall)
            //         at System.Web.Services.Protocols.SoapHttpClientProtocol.Invoke(String methodName, Object[] parameters)
            //at ConsoleAppToTestHttpClient.gov.ny.svc.test.daws.dsmlQueryService.directoryRequest(BatchRequest batchRequest) in C: \Users\mjordan\Documents\Visual Studio 2015\Projects\ConsoleAppToTestHttpClient\ConsoleAppToTestHttpClient\Web References\gov.ny.svc.test.daws\Reference.cs:line 80
            //at ConsoleAppToTestHttpClient.Program.Main(String[] args) in C: \Users\mjordan\Documents\Visual Studio 2015\Projects\ConsoleAppToTestHttpClient\ConsoleAppToTestHttpClient\Program.cs:line 42
            //at System.AppDomain._nExecuteAssembly(RuntimeAssembly assembly, String[] args)
            //at System.AppDomain.ExecuteAssembly(String assemblyFile, Evidence assemblySecurity, String[] args)
            //at Microsoft.VisualStudio.HostingProcess.HostProc.RunUsersAssembly()
            //at System.Threading.ThreadHelper.ThreadStart_Context(Object state)
            //at System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
            //at System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
            //at System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state)
            //at System.Threading.ThreadHelper.ThreadStart()
            // Going to make a nice filter now
            AttributeValueAssertion ava = new AttributeValueAssertion();
            ava.name = "uid";
            ava.value = "jjtester3";
            filter.ItemElementName = ItemChoiceType.equalityMatch;
            filter.Item = ava;
            search.filter = filter;








            BatchResponse response = client.directoryRequest(batch);
            ErrorResponse[] eResponses = response.errorResponse;

            //After adding a attribute value assertion and fitler to the search the error response ends up null so make a check for that
            if (eResponses != null)
            {
                if (eResponses.Length > 0)
                { 
                System.Diagnostics.Debug.WriteLine("Error Response");
                for (int i = 0; i < eResponses.Length; i++)
                {
                    ErrorResponse error = eResponses[i];
                    System.Diagnostics.Debug.WriteLine(error.message);
                    System.Diagnostics.Debug.WriteLine(error.detail);
                    System.Diagnostics.Debug.WriteLine(error.type);
                }
            }
        }









            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://qadaws.svc.ny.gov/daws/services/dsmlSoapQuery");
            ////WebProxy myproxy = new WebProxy("proxy-internet.cio.state.nyenet", 80);
            ////myproxy.BypassProxyOnLocal = false;
            ////request.Proxy = myproxy;
            //request.Method = "POST";
            //request.Credentials = new NetworkCredential("prxwsTL1HESC", "sfvwRMnB7N");
            ////request.PreAuthenticate = true;
            //request.Headers.Add("SOAPAction", "");
            //request.ContentType = "text/xml;charset=\"utf-8\"";
            //request.Accept = "text/xml";
            //request.Method = "POST";


            ////HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://qadaws.svc.ny.gov/daws/services/dsmlSoapQuery");
            ////request.Headers.Add("SOAPAction", "");
            ////request.ContentType = "text/xml;charset=\"utf-8\"";
            ////request.Accept = "text/xml";
            ////request.Method = "POST";
            ////request.Credentials = new NetworkCredential("prxwsTL1HESC", "sfvwRMnB7N");




            ////request.Headers.Add("SOAPAction", "https://qadaws.svc.ny.gov/daws/services/dsmlSoapQuery");
            ////request.ContentType = "text/xml";
            //System.Diagnostics.Debug.WriteLine("Making soap env");
            //XmlDocument soapEnvelopeXml = CreateSoapEnvelope();
            //System.Diagnostics.Debug.WriteLine("inserting soap into request");
            //InsertSoapEnvIntoWebReq(soapEnvelopeXml, request);
            //System.Diagnostics.Debug.WriteLine("Making an async result");

            ////You must supply a request body if not made correctly seems to be when you add the proxy info to the request.
            ////{"You must provide a request body if you set ContentLength>0 or SendChunked==true.  Do this by calling [Begin]GetRequestStream before [Begin]GetResponse."}
            //IAsyncResult asyncResult = request.BeginGetResponse(null, null);

            //// suspend this thread until call is complete. You might want to
            //// do something usefull here like update your UI.
            //asyncResult.AsyncWaitHandle.WaitOne();

            //try
            //{
            //    using (WebResponse stream = request.EndGetResponse(asyncResult))
            //    {
            //        //soapEnvelopeXml.Save(stream);
            //        using (WebResponse response = request.GetResponse())
            //        {
            //            using (StreamReader rd = new StreamReader(response.GetResponseStream()))
            //            {
            //                string soapResult = rd.ReadToEnd();
            //                System.Diagnostics.Debug.WriteLine(soapResult);
            //            }
            //        }
            //    }
            //}
            //catch (WebException ex)
            //{
            //    System.Diagnostics.Debug.WriteLine("issues with that damn request stream again");
            //    string message = ((HttpWebResponse)ex.Response).StatusDescription;
            //    System.Diagnostics.Debug.WriteLine(message);
            //}





            //InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, request);

            //System.Diagnostics.Debug.WriteLine("Going to call url");
            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //System.Diagnostics.Debug.WriteLine("Called to get response:" + response);
            //System.Diagnostics.Debug.WriteLine(response.Headers);
            //System.Diagnostics.Debug.WriteLine(response.StatusCode);


            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.sheldonbrown.com/web_sample1.html");
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.google.com");
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://qadaws.svc.ny.gov/daws/services/dsmlSoapQuery");
            //WebProxy myproxy = new WebProxy("proxy-internet.cio.state.nyenet", 80);
            //myproxy.BypassProxyOnLocal = false;
            //request.Proxy = myproxy;
            //request.Credentials = new NetworkCredential("prxwsTL1HESC", "sfvwRMnB7N");
            //request.PreAuthenticate = true;
            ////request.Headers.Add("username", "prxwsTL1HESC");
            ////request.Headers.Add("password", "sfvwRMnB7N");
            //request.Method = "POST";
            //System.Diagnostics.Debug.WriteLine("Going to call url");
            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //System.Diagnostics.Debug.WriteLine("Called to get response:" + response);
            //System.Diagnostics.Debug.WriteLine(response.Headers);
            //System.Diagnostics.Debug.WriteLine(response.StatusCode);

            //System.Diagnostics.Debug.WriteLine(CallWebService());
        }







        public static void CallWebServ()
        {
            var _url = "https://qadaws.svc.ny.gov/daws/services/dsmlSoapQuery";
            var _action = "";

            XmlDocument soapEnvelopeXml = CreateSoapEnv();
            HttpWebRequest webRequest = CreateWebReq(_url, _action);
            InsertSoapEnvIntoWebReq(soapEnvelopeXml, webRequest);

            // begin async call to web request.
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            // suspend this thread until call is complete. You might want to
            // do something usefull here like update your UI.
            asyncResult.AsyncWaitHandle.WaitOne();

            // get the response from the completed web request.
            string soapResult;
            //endGetResponse throws unauthorized if no credentials put on request
            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                System.Diagnostics.Debug.WriteLine("Got web response");
                try
                {
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        System.Diagnostics.Debug.WriteLine("Got stream reader from response");
                        soapResult = rd.ReadToEnd();
                        System.Diagnostics.Debug.WriteLine(soapResult);
                    }
                }
                catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("get response stream poop" + e);
                }
                
            }
        }

        private static HttpWebRequest CreateWebReq(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            webRequest.Credentials = new NetworkCredential("prxwsTL1HESC", "sfvwRMnB7N");
            return webRequest;
        }

        private static XmlDocument CreateSoapEnv()
        {
            XmlDocument soapEnvelop = new XmlDocument();
            soapEnvelop.LoadXml(@"
<soap-env:Envelope
xmlns:xsd='http://www.w3.org/2001/XMLSchema'
xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'
xmlns:soap-env='http://schemas.xmlsoap.org/soap/envelope/'>
<soap-env:Body>
<batchRequest xmlns='urn:oasis:names:tc:DSML:2:0:core'>
            <searchRequest dn='o = ny, c = us' scope='wholeSubtree' derefAliases='neverDerefAliases' timeLimit='0' sizeLimit='0'>
            	<filter>
            		<substrings name = 'mail'>
<initial>mark.mossman@its.ny.gov</initial>
</substrings>
</filter>
<attributes>
<attribute name = 'uid'/>
<attribute name = 'sn'/>
</attributes>
</searchRequest>
</batchRequest>
</soap-env:Body></soap-env:Envelope>
");
            return soapEnvelop;
        }

        private static void InsertSoapEnvIntoWebReq(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            try
            {
                using (Stream stream = webRequest.GetRequestStream())
                {
                    soapEnvelopeXml.Save(stream);
                }
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("get response stream poop" + e);
            }
        }


















        public static String CallWebService()
        {
            String returnValue = "";


            var _url = "https://qadaws.svc.ny.gov/daws/services/dsmlSoapQuery";
            var _action = "";

            XmlDocument soapEnvelopeXml = CreateSoapEnvelope();
            HttpWebRequest webRequest = CreateWebRequest(_url, _action);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            // begin async call to web request.
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            // suspend this thread until call is complete. You might want to
            // do something usefull here like update your UI.
            asyncResult.AsyncWaitHandle.WaitOne();

            // get the response from the completed web request.
            string soapResult;
            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResult = rd.ReadToEnd();
                }
                Console.Write(soapResult);
                returnValue = returnValue + " " + soapResult;
            }


            return returnValue;
        }

        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            //string MyProxyHostString = "199.168.151.10";
            string MyProxyHostString = "proxy-internet.cio.state.nyenet";

            int MyProxyPort = 80;


            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Proxy = new WebProxy(MyProxyHostString, MyProxyPort);
            webRequest.Proxy.Credentials = new NetworkCredential("mjordan", "fuckU023$6");

            webRequest.Headers.Add("SOAPAction", "");
            webRequest.Headers.Add("username", "prxwsTL1HESC");
            webRequest.Headers.Add("password", "sfvwRMnB7N");
            webRequest.ContentType = "text/xml";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        private static XmlDocument CreateSoapEnvelope()
        {

            XmlDocument soapEnvelop = new XmlDocument();
            soapEnvelop.LoadXml(@"

<soap-env:Envelope
xmlns:xsd='http://www.w3.org/2001/XMLSchema'
xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'
xmlns:soap-env='http://schemas.xmlsoap.org/soap/envelope/'>
<soap-env:Body>
<batchRequest xmlns='urn:oasis:names:tc:DSML:2:0:core'>
            <searchRequest dn='o = ny, c = us' scope='wholeSubtree' derefAliases='neverDerefAliases' timeLimit='0' sizeLimit='0'>
            	<filter>
            		<equalityMatch name = 'uid'>
                        <value>jjtester3</value>
                    </equalityMatch>
</filter>
<attributes>
<attribute name = 'uid'/>
<attribute name = 'sn'/>
</attributes>
</searchRequest>
</batchRequest>
</soap-env:Body></soap-env:Envelope>
");
            return soapEnvelop;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }





    }
}
