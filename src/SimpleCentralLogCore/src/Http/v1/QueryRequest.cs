/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Info
 * Datum: 02.09.2017
 * Zeit: 07:14
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */
using System;
using System.Collections.Generic;

namespace SimpleCentralLog.Http
{
    /// <summary>
    /// Description of QueryRequest.
    /// </summary>
    public class QueryRequest
    {
        public QueryRequest()
        {
        }
        
        public string Application { get; set; }
        public string Message { get; set; }
        public Messageclass[] Messageclass { get; set; }
        public DateTime LogdateFrom { get; set; }
        public DateTime LogdateTo { get; set; }
        
        public Predicate<Entry>[] Filter() {
            List<Predicate<Entry>> Result = new List<Predicate<Entry>>();
            
            if (!string.IsNullOrEmpty(Application)) Result.Add(EntryFilter.ApplicationFilter(Application));
            if (!string.IsNullOrEmpty(Message)) Result.Add(EntryFilter.MessageFilter(Message));
            if (Messageclass != null) Result.Add(EntryFilter.MessageclassFilter(Messageclass));
            
            DateTime Enddate = LogdateTo == DateTime.MinValue ? DateTime.MaxValue : LogdateTo;
            Result.Add(EntryFilter.LogdateFilter(LogdateFrom, Enddate));
            
            return Result.ToArray();
        }
    }
}
