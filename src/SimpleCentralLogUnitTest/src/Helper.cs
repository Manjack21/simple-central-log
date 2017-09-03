/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Info
 * Datum: 25.08.2017
 * Zeit: 10:11
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */
using System;
using SimpleCentralLog;

namespace SimpleCentralLogUnitTest
{
    /// <summary>
    /// Description of Helper.
    /// </summary>
    public static class Helper
    {
        
        public static Entry makeEntry(Messageclass Class = Messageclass.Error, string Message = "", string Application = "", string Logdate = "") {
            DateTime Ld;
            DateTime.TryParse(Logdate, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal, out Ld);
            return new Entry(Class, Message, Application, Ld);
        }
    }
}
