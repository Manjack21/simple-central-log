/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Info
 * Datum: 25.08.2017
 * Zeit: 13:08
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */
using System;

namespace SimpleCentralLog.Persister
{
    /// <summary>
    /// Description of IEntryReader.
    /// </summary>
    public interface IEntryReader
    {
        Entry[] AllEntries();
    }
}
