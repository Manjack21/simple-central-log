/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Info
 * Datum: 22.08.2017
 * Zeit: 17:52
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */
using System;
using System.IO;
using SimpleCentralLog;
using SimpleCentralLog.Http;
using SimpleCentralLog.Persister;

namespace SimpleCentralLogConsole
{
    class Program
    {
        const string JsonFile = "D:\\Log.json";
        
        public static void Main(string[] args)
        {
            Console.WriteLine("Start HttpListener!");            
            
            var Persister = new JsonPersister(getJsonFileWriter);
            if (System.IO.File.Exists(JsonFile)) {
                using (var reader = new StreamReader(JsonFile)) Persister.ReadEntries(reader);
            }
            
            EntryRepository repo = new EntryRepository(Persister, Persister.AllEntries());
            
            LogService ls = new LogService(repo);
            ls.StartService(8080);
            
            Console.WriteLine("Service started, press key to terminate!");
            Console.ReadKey(true);
        }
        
        public static System.IO.TextWriter getJsonFileWriter() {
            return new System.IO.StreamWriter(JsonFile, false, System.Text.Encoding.UTF8);
        }
    }
}