/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Info
 * Datum: 23.08.2017
 * Zeit: 10:27
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleCentralLog
{
    /// <summary>
    /// Description of EntryFilter.
    /// </summary>
    public static class EntryFilter
    {
        public static Predicate<Entry> MessageclassFilter(params Messageclass[] Classes) {
            return (Entry e) => Classes.Contains(e.Class);
        }
        
        public static Predicate<Entry> MessageFilter(string MessageExpression) {
            return (Entry e) => System.Text.RegularExpressions.Regex.IsMatch(e.Message, MessageExpression);
        }
        
        public static Predicate<Entry> ApplicationFilter(string ApplicationExpression) {
            return (Entry e) => System.Text.RegularExpressions.Regex.IsMatch(e.Application, ApplicationExpression);
        }
        
        public static Predicate<Entry> LogdateFilter(DateTime from, DateTime to) {
            return (Entry e) => e.Logdate >= from && e.Logdate <= to;
        }
    }
}
