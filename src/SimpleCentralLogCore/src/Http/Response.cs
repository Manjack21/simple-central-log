/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Info
 * Datum: 26.08.2017
 * Zeit: 11:22
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */
using System;
using System.Collections.Generic;

namespace SimpleCentralLog.Http
{
    /// <summary>
    /// Description of Response.
    /// </summary>
    public class Response : IResponse
    {
        public Response() {
            this.Header = new Dictionary<string, string>();
            this.Body = string.Empty;
            this.Status = string.Empty;
        }
        
        public string Body { get; set; }
        public Dictionary<string, string> Header { get; private set; }
        public string Status { get; set; }
        public Int32 Statuscode { get; set; }
    }
}
