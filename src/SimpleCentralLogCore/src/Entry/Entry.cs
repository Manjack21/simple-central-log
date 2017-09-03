/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Info
 * Datum: 22.08.2017
 * Zeit: 18:06
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */
using System;

namespace SimpleCentralLog
{        
    public class Entry
    {
        public Entry(Messageclass Class, string Message, string Application, DateTime Logdate) {
            this.Id = Id;
            this.Class = Class;        
            this.Message = Message ?? string.Empty;
            this.Application = Application ?? string.Empty;
            this.Logdate = Logdate;
        }
        
        public string Application { get; private set; }
        public Messageclass Class { get; private set; }
        public UInt64 Id { get; set; }
        public DateTime Logdate { get; private set; }
        public string Message { get; private set; }
    }
}
