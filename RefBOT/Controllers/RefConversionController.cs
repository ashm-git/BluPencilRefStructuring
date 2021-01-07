using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;


namespace RefBOT.Controllers
{
    public class RefConversionController : ApiController
    {
        [System.Web.Http.Cors.EnableCors(origins: "http://blupencildev.luminad.com/", headers: "*", methods: "*")]
        //[System.Web.Http.Cors.EnableCors(origins: "https://submissionchecker.luminad.com", headers: "*", methods: "*")]
        //[System.Web.Http.Cors.EnableCors(origins: "http://localhost:56277", headers: "*", methods: "*")]
        // GET: RefConversion
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //api/RefConversion/values?input=Manworren, R. C., Anderson, M. N., Girard, E. D., Ruscher, K. A., Verissimo, A. M., Palac, H., ... &#x0026; Hight, D. (2018). Postoperative Pain Outcomes After Nuss Procedures: Comparison of Epidural Analgesia, Continuous Infusion of Local Anesthetic, and Preoperative Self-Hypnosis Training. <i>Journal of Laparoendoscopic &#x0026; Advanced Surgical Techniques</i>, DOI: 10.1089/lap.2017.0699
        //updated by Dakshinamoorthy on 2020-Nov-24
        //public string Get(string refContent, string userName, string batchId, string docFullPath, string selectionMode, string appName, string appVersion)
        public string Get(string refContent, string userName, string batchId, string docFullPath, string selectionMode, string appName, string appVersion, string customerName)
        {
            string sOutputContent = refContent;

            try
            {
                AutoStructReferences objRef = new AutoStructReferences();
                string sRefContent = refContent;
                string sUserName = userName;
                string sBatchId = batchId;
                string sDocFullPath = docFullPath;
                string sSelectionMode = selectionMode;
                string sAppName = appName;
                string sAppVersion = appVersion;
                //added by Dakshinamoorthy on 2020-Nov-24 (for handling customer specific requirement)
                string sCustomerName = customerName;

                //added by Dakshinamoorthy on 2020-Nov-24 (for loading Author Names)
                AutoStructRefAuthor.LoadAuthorGroupPatterns();


                //sOutputContent = objRef.DoReferenceConversion(sRefContent, sUserName, sBatchId, sDocFullPath, sSelectionMode, sAppName, sAppVersion);
                sOutputContent = objRef.DoReferenceConversion(sRefContent, sUserName, sBatchId, sDocFullPath, sSelectionMode, sAppName, sAppVersion, sCustomerName);

            }
            catch (Exception ex)
            {
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(ex.Message),
                    ReasonPhrase = "Failed"
                };
                throw new HttpResponseException(response);
            }

            return sOutputContent;
        }


        public HttpResponseMessage Post()
        //public HttpResponseMessage Post(string userName, string batchId, string docFullPath, string selectionMode, string appName, string appVersion)
        //public async Task<HttpResponseMessage> Post()
        {
            string sUploadFileName = string.Empty;

            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                var docfiles = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filePath = HttpContext.Current.Server.MapPath("~/" + postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    docfiles.Add(filePath);
                }
                result = Request.CreateResponse(HttpStatusCode.Created, docfiles);

                //Read the File into a Byte Array.
                sUploadFileName = docfiles[0];
                string sInputContent = File.ReadAllText(sUploadFileName);

                AutoStructReferences objRef = new AutoStructReferences();
                string sRefContent = sInputContent;

                string sUserName = string.Empty;
                if (!string.IsNullOrEmpty(httpRequest.Form["userName"]))
                {
                    sUserName = httpRequest.Form["userName"];
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                string sBatchId = string.Empty;
                if (!string.IsNullOrEmpty(httpRequest.Form["batchId"]))
                {
                    sBatchId = httpRequest.Form["batchId"];
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                string sDocFullPath = string.Empty;
                if (!string.IsNullOrEmpty(httpRequest.Form["docFullPath"]))
                {
                    sDocFullPath = httpRequest.Form["docFullPath"];
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                string sSelectionMode = string.Empty;
                if (!string.IsNullOrEmpty(httpRequest.Form["selectionMode"]))
                {
                    sSelectionMode = httpRequest.Form["selectionMode"];
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                string sAppName = string.Empty;
                if (!string.IsNullOrEmpty(httpRequest.Form["appName"]))
                {
                    sAppName = httpRequest.Form["appName"];
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                string sAppVersion = string.Empty;
                if (!string.IsNullOrEmpty(httpRequest.Form["appVersion"]))
                {
                    sAppVersion = httpRequest.Form["appVersion"];
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                //added by Dakshinamoorthy on 2020-Nov-24
                string sCustomerName = string.Empty;
                if (!string.IsNullOrEmpty(httpRequest.Form["customerName"]))
                {
                    sCustomerName = httpRequest.Form["customerName"];
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                StringBuilder sbOutputContent = new StringBuilder();
                //updated by Dakshinamoorthy on 2020-Nov-24
                //sbOutputContent.AppendLine(objRef.DoReferenceConversion_File(sRefContent, sUserName, sBatchId, sDocFullPath, sSelectionMode, sAppName, sAppVersion));
                sbOutputContent.AppendLine(objRef.DoReferenceConversion_File(sRefContent, sUserName, sBatchId, sDocFullPath, sSelectionMode, sAppName, sAppVersion, sCustomerName));

                byte[] bytes = Encoding.ASCII.GetBytes(sbOutputContent.ToString());

                //Set the Response Content.
                result.Content = new ByteArrayContent(bytes);

                //Set the Response Content Length.
                result.Content.Headers.ContentLength = bytes.LongLength;
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (File.Exists(sUploadFileName))
            {
                File.Delete(sUploadFileName);
            }
            return result;
        }
    }
}