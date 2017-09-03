/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Info
 * Datum: 24.08.2017
 * Zeit: 10:22
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */
using System;

namespace SimpleCentralLog.Persister
{
    /// <summary>
    /// Description of IPersister.
    /// </summary>
    public interface IEntryWriter
    {
        void WriteEntry(Entry entry);
        
    }
}
