/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Info
 * Datum: 24.08.2017
 * Zeit: 10:21
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Timers;
using Newtonsoft.Json;

namespace SimpleCentralLog.Persister
{
    /// <summary>
    /// Description of TextwriterEntrypersistor.
    /// </summary>
    public class JsonPersister : IPersister       
    {
        readonly Func<System.IO.TextWriter> getTextwriter;
        readonly Timer writeTimer;
        
        readonly ConcurrentBag<Entry> entries;
        readonly List<Entry> allEntries;
        bool isWriting;
        
        public JsonPersister(Func<System.IO.TextWriter> getTextWriter) : this(getTextWriter, 1000) {
        }
        public JsonPersister(Func<System.IO.TextWriter> getTextWriter, double writeIntervall) {
            this.getTextwriter = getTextWriter;
            this.entries = new ConcurrentBag<Entry>(); 
            this.allEntries = new List<Entry>();
            this.writeTimer = new Timer(writeIntervall);
            writeTimer.Elapsed += OnWriteTimerElapsed;
            writeTimer.AutoReset = true;
            writeTimer.Start();
                        
        }
        
        ~JsonPersister() {
            persist();
        }
        
        /// <summary>
        /// Add an Entry to the cache and try to persist the cached Items
        /// </summary>
        /// <param name="e"></param>
        public void WriteEntry(Entry e) {
            entries.Add(e);
        }
        
        /// <summary>
        /// Reads all Entries from the internal Entrylist
        /// </summary>
        /// <returns>Entry[]</returns>
        public Entry[] AllEntries() {
            return allEntries.ToArray();
        }
        
        
        /// <summary>
        /// Deserializes an Entry[] from a Textreader and append the items to the internal Entrylist. 
        /// </summary>
        /// <returns>Entry[]</returns>
        public void ReadEntries(System.IO.TextReader reader) {
            lock(allEntries) {
                Entry[] entriesFromFile = (Entry[])JsonSerializer.Create().Deserialize(reader, typeof(Entry).MakeArrayType());
                if (entriesFromFile != null) allEntries.AddRange(entriesFromFile);
            }
        }
        
        private void OnWriteTimerElapsed(Object sender, ElapsedEventArgs EventArgs) {
            this.persist();
        }
        
        /// <summary>
        /// writes the caches entries to a new TextWriter
        /// </summary>
        private void persist() {
            if (getTextwriter == null) throw new NullReferenceException("No IO.TextWriter provided. Use ctor Parameter.");
            
            if (entries.Count == 0) return;
            
            if (isWriting) return;
            
            lock (entries) {
                isWriting = true;
                
                try {                    
                    Entry currentEntry = null;
                    while (entries.TryTake(out currentEntry)) {
                        allEntries.Add(currentEntry);
                    }
                    
                    using (System.IO.TextWriter writer = getTextwriter.Invoke()) {
                        JsonSerializer.Create().Serialize(writer, allEntries.ToArray());                    
                    }
                } catch (Exception e) {
                    
                    Console.WriteLine(e.Message);
                } finally {
                    isWriting = false;                
                }
            }
        }
    }
}
