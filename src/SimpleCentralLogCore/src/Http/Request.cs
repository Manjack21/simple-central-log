/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Info
 * Datum: 26.08.2017
 * Zeit: 11:07
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */
using System;
using System.Collections.Specialized;

namespace SimpleCentralLog.Http
{
    /// <summary>
    /// Description of Request.
    /// </summary>
    public class Request : IRequest        
    {
        public Request() {
        }        
        public Request(System.Net.HttpListenerRequest Request) : this() {
            this.Method = Request.HttpMethod;
            this.URI = Request.Url.AbsolutePath;
            this.Header = Request.Headers;
            using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.InputStream)) {                
                this.Body = reader.ReadToEnd();
            }
        } 
        public Request(string Method, string URI) : this() {
            this.Method = Method;
            this.URI = URI;
        }
        public Request(string Method, string URI, string Body, params Tuple<string, string>[] Headers) : this(Method, URI) {
            this.Body = Body;
            this.Header = new NameValueCollection();
            foreach(Tuple<string, string> header in Headers) this.Header.Add(header.Item1, header.Item2);
        }
        
        public string Body { get; set; }
        public NameValueCollection Header { get; set; }
        public string Method { get; set; }
        public string URI { get; set; }
    }
}
