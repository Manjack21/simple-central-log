/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Info
 * Datum: 22.08.2017
 * Zeit: 17:52
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */
using System;
using NUnit.Framework;
using SimpleCentralLog;

namespace SimpleCentralLogUnitTest
{
    [TestFixture]
    public class EntryRepositoryTest
    {
        private EntryRepository repo;
        
        [NUnit.Framework.SetUp]
        public void Startup() {
            repo = new EntryRepository();
        } 

        [NUnit.Framework.TearDown]
        public void Teardown() {
            repo = null;
        }          
            
        [Test]
        public void Add_new_item_to_Repository_return_true() {
            repo.Add(Helper.makeEntry());
            
            Assert.AreEqual(1, repo.Count());
        }
                
        [Test]
        public void query_via_messageclass_and_return_entry() {
            repo.Add(Helper.makeEntry(Messageclass.Warning));
            
            repo.Add(Helper.makeEntry());
            
            Entry[] actual = repo.Query(EntryFilter.MessageclassFilter(Messageclass.Error));
            
            Assert.AreEqual(1, actual.Length);
        }
        
        [Test]
        public void query_via_messageclass_without_entries_and_return_no_entry() {
            repo.Add(Helper.makeEntry(Messageclass.Warning));
            
            repo.Add(Helper.makeEntry());
            
            Entry[] actual = repo.Query(EntryFilter.MessageclassFilter(Messageclass.Information));
            
            Assert.AreEqual(0, actual.Length);
        }
        
        [Test]
        public void query_via_message_and_return_two_entries() {
            repo.Add(Helper.makeEntry(Messageclass.Warning, "Test"));
                  
            repo.Add(Helper.makeEntry(Messageclass.Warning, "Text"));
            
            repo.Add(Helper.makeEntry(Messageclass.Warning, "Warning"));
            
            Entry[] actual = repo.Query(EntryFilter.MessageFilter("^Te[sx]{1,1}t$"));
            
            Assert.AreEqual(2, actual.Length);
        }
        
        [Test]
        public void query_via_logdate_and_message_should_return_one_entry() {
            repo.Add(Helper.makeEntry(Message:"Test", Logdate:"2017-07-01Z00:00:00"));
                  
            repo.Add(Helper.makeEntry(Message:"Text", Logdate:"2017-08-20Z00:00:00"));
            
            repo.Add(Helper.makeEntry(Message:"Message", Logdate:"2017-07-20Z00:00:00"));
            
            Entry[] actual = repo.Query(EntryFilter.MessageFilter("^Te[sx]{1,1}t$"), 
                                        EntryFilter.LogdateFilter(new DateTime(2017, 7, 1), new DateTime(2017, 7, 31)));
            
            Assert.AreEqual(3, repo.Count());
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual("Test", actual[0].Message);
        }
        
        [Test]
        public void query_via_application_and_return_entry() {
            repo.Add(Helper.makeEntry(Application:"App1"));
            repo.Add(Helper.makeEntry(Application:"App2"));
                        
            Assert.AreEqual(2, repo.Count());
            
            Entry[] actual = repo.Query(EntryFilter.ApplicationFilter("App1"));
            
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual("App1", actual[0].Application);
        }
        
        
        
    }
}
