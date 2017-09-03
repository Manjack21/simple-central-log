/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Info
 * Datum: 22.08.2017
 * Zeit: 17:50
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */
using System;
using System.Linq;
using System.Collections.Generic;

namespace SimpleCentralLog
{
    /// <summary>
    /// Description of MyClass.
    /// </summary>
    public class EntryRepository        
    {
        System.Collections.Concurrent.ConcurrentDictionary<UInt64, Entry> entries;
        Persister.IEntryWriter entryWriter;
        
        public EntryRepository() {
            entries = new System.Collections.Concurrent.ConcurrentDictionary<UInt64, Entry>();
        }
        public EntryRepository(Persister.IEntryWriter EntryWriter) : this() {
            this.entryWriter = EntryWriter;
        }
        public EntryRepository(Persister.IEntryWriter EntryWriter, Entry[] Entries) : this(EntryWriter) {
            foreach (var e in Entries) {
                entries.TryAdd(e.Id, e);
            }
        }

        public bool Add(Entry entry) {
            bool wasAdded = false;
            Int16 tryCount = 0;
            while (!wasAdded & tryCount < 100) {                
                entry.Id = (ulong)DateTime.Now.Ticks;
                wasAdded = entries.TryAdd(entry.Id, entry);
                tryCount++;
                System.Threading.Thread.Sleep(tryCount);
            }
            
            if (wasAdded & entryWriter != null) entryWriter.WriteEntry(entry);
            return wasAdded;
        }
        
        public Int64 Count() {
            return entries.Count;
        }
        
        public Entry[] Query(params Predicate<Entry>[] Filter) {
            return (from Entry e in entries.Values
                    where Filter.Aggregate<Predicate<Entry>, bool>(true, (bool current, Predicate<Entry> f) => current && f.Invoke(e))
                    select e).ToArray();
        }
        
        
    }
    
    
}