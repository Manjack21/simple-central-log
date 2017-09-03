/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Info
 * Datum: 26.08.2017
 * Zeit: 11:20
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */
using System;
using System.Collections.Generic;

namespace SimpleCentralLog.Http
{
    /// <summary>
    /// Description of IResponse.
    /// </summary>
    public interface IResponse
    {
        string Body { get; set; }
        Dictionary<string, string> Header { get; }
        string Status { get; set; }
        Int32 Statuscode { get; set; }        
    }
}
