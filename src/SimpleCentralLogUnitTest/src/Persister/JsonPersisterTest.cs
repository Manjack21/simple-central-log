/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Info
 * Datum: 24.08.2017
 * Zeit: 10:31
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */
using System;
using System.Linq;
using System.Text;
using System.IO;
using SimpleCentralLog;
using SimpleCentralLog.Persister;
using Newtonsoft.Json;
using NUnit.Framework;

namespace SimpleCentralLogUnitTest
{
    [TestFixture]
    public class JsonPersisterTest
    {
        JsonPersister persister;
        StringBuilder jsonOutput;
        
        [NUnit.Framework.SetUp]
        public void Startup() {
            jsonOutput = new StringBuilder("");
            persister = new JsonPersister(() => { jsonOutput.Clear(); return new System.IO.StringWriter(jsonOutput); }, 20);
        }
        
        [NUnit.Framework.TearDown]
        public void Teardown() {
            jsonOutput = null;
            persister = null;
        }
        
        [Test]
        public void persist_single_Entry_returns_JSON_Array_with_one_Entries() {
            persister.WriteEntry(Helper.makeEntry(Messageclass.Error, "Text"));
            System.Threading.Thread.Sleep(100);
            
            Entry[] actual = (Entry[])JsonSerializer.Create().Deserialize(new System.IO.StringReader(jsonOutput.ToString()), typeof(Entry).MakeArrayType());
            
            
            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual(Messageclass.Error, actual.First().Class);
            Assert.AreEqual("Text", actual.First().Message);
        }
        
        [Test]
        public void persist_two_Entries_returns_JSON_Array_with_two_Entries() {
            persister.WriteEntry(Helper.makeEntry(Messageclass.Error, "Text"));
            persister.WriteEntry(Helper.makeEntry(Messageclass.Warning, "Test"));
            
            System.Threading.Thread.Sleep(100);
            Entry[] actual = (Entry[])JsonSerializer.Create().Deserialize(new System.IO.StringReader(jsonOutput.ToString()), typeof(Entry).MakeArrayType()); 
            
            Assert.AreEqual(2, actual.Count());
            Assert.AreEqual(Messageclass.Error, actual.First().Class);
            Assert.AreEqual("Text", actual.First().Message);
            
            Assert.AreEqual(Messageclass.Warning, actual.Last().Class);
            Assert.AreEqual("Test", actual.Last().Message);
        }
        
        [Test]
        public void ReadEntries_from_empty_Textreader_returns_empty_Array() {
            persister.ReadEntries(new StringReader(""));
                
            Assert.AreEqual(0, persister.AllEntries().Count());
        }
        
        [Test]
        public void ReadEntries_Textreader_isNull_should_throw_ArgumentNullException() {
            Assert.Throws(typeof(ArgumentNullException), () => persister.ReadEntries(null));
        }
        
        [Test]
        public void ReadEntries_from_JSONstring_returns_Array_containing_1_entry() {
            persister.ReadEntries(new StringReader("[{\"Message\":\"Test\"}]"));
                
            Assert.AreEqual(1, persister.AllEntries().Count());
            Assert.AreEqual("Test", persister.AllEntries().First().Message);
        }     
    }
}
