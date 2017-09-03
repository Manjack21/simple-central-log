/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Info
 * Datum: 26.08.2017
 * Zeit: 11:05
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */
using System;
using System.Collections;


namespace SimpleCentralLog.Http
{
    /// <summary>
    /// Description of IRequest.
    /// </summary>
    public interface IRequest
    {
        string Body { get; set; }
        System.Collections.Specialized.NameValueCollection Header { get; set; }
        string Method { get; set; }
        string URI { get; set; }
    }
}
