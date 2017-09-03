/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Info
 * Datum: 31.08.2017
 * Zeit: 07:08
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */
using System;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace SimpleCentralLog
{
    [ServiceContract]
    public interface ILogService
    {
       [OperationContract]
       [WebInvoke(UriTemplate = "log", Method = "POST")]
       string LogAdd(string name);
    }
    
    public class LogService : ILogService
    {
        private EntryRepository repo;
        
        public LogService(){
            repo = new EntryRepository(new Persister.JsonPersister(() => new System.IO.StreamWriter("D:\\log.json", false, System.Text.Encoding.UTF8)));
        }
        
        public string LogAdd(string name)
        {
          // implement the operation
          return string.Format("Operation name: {0}", name);
}
    } 
}
