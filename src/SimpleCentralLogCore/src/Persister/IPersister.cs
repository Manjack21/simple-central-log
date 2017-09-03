/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Info
 * Datum: 02.09.2017
 * Zeit: 10:16
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */
using System;

namespace SimpleCentralLog.Persister
{
    /// <summary>
    /// Description of IPersister.
    /// </summary>
    public interface IPersister : IEntryReader, IEntryWriter
    {
    }
}
