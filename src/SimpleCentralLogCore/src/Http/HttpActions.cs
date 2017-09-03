/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Info
 * Datum: 26.08.2017
 * Zeit: 11:38
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */
using System;
using System.Net;
using Newtonsoft.Json;
using NUnit.Framework;

namespace SimpleCentralLog.Http
{
    /// <summary>
    /// Description of HttpActions.
    /// </summary>
    public static class HttpActions
    {
        public static Func<IRequest, IResponse> Route(IRequest Request, EntryRepository repo) {
            switch (Request.URI.ToLower()) {
                case "/log":
                    switch (Request.Method.ToUpper()) {
                        case "POST":
                            return LogAdd(repo);
                    }
                    break;
                case "/query":
                    switch (Request.Method.ToUpper()) {
                        case "POST":
                            return Query(repo);
                    }
                    break;
            }
            return Error404;
        }
    
        public static IResponse Error404(IRequest Request) {
            return Status(404, "page not found");
        }
        
        public static Func<IRequest, IResponse> LogAdd(EntryRepository Repo) {        
            return (IRequest Request) => {
                if (string.IsNullOrWhiteSpace(Request.Body)) return Status(400, "no data");
                
                Entry e = null;
                try {
                    e = (Entry)JsonSerializer.Create().Deserialize(new System.IO.StringReader(Request.Body), typeof(Entry));
                } catch (Exception ex) {                    
                    return Status(400, "invalid input", ex.Message);
                }
                
                Repo.Add(e);
                
                return OK();
            };
        }
        
        public static Func<IRequest, IResponse> Query(EntryRepository Repo) {        
            return (IRequest Request) => {
                if (string.IsNullOrWhiteSpace(Request.Body)) return Status(400, "no data");
                
                QueryRequest q = null;
                try {
                    q = (QueryRequest)JsonSerializer
                        .Create()
                        .Deserialize(new System.IO.StringReader(Request.Body), typeof(QueryRequest));
                } catch (Exception ex) {                    
                    return Status(400, "invalid input", ex.Message);
                }
                                
                Entry[] FilteredEntries = Repo.Query(q.Filter());                
                var Response = new System.Text.StringBuilder();
                JsonSerializer
                    .Create()
                    .Serialize(new System.IO.StringWriter(Response), FilteredEntries);
                
                return OK(Response.ToString());
            };
        }
        
        /*
         * Responsecode helper functions
        */
        static IResponse Status(int code, string status, string Body = "") {
            return new Response() { Status = status, Statuscode = code, Body = Body};
        }        
        static IResponse OK() {
            return OK(String.Empty);
        }      
        static IResponse OK(string Body) {
            return Status(200, "OK", Body);
        }
    }
}
