/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Info
 * Datum: 31.08.2017
 * Zeit: 07:52
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SimpleCentralLog.Http
{
    /// <summary>
    /// Description of LogService.
    /// </summary>
    public class LogService
    {
        EntryRepository repository;
        HttpListener listener;
        Task contextTask;
        List<IAsyncResult> runningContexts;
        
        public LogService(EntryRepository Repository)        {
            repository = Repository;
        }
        
        /// <summary>
        /// Start a httplistener and creates helper-objects to get http-contexts in a seperate thread.
        /// </summary>
        /// <param name="Prefix">prefix to listen to; mind the tailing slash; format http[s]://[domain|*]:[port]/</param>
        public void StartService(string Prefix) {
            this.StopService();
            
            listener = new HttpListener();
            listener.Prefixes.Add(Prefix);
            listener.Start();
            
            runningContexts = new List<IAsyncResult>();
            contextTask = Task.Factory.StartNew( () => {
                while (listener != null && listener.IsListening) {
                    runningContexts.RemoveAll((item) => item.IsCompleted);
                    if (runningContexts.Count < 10) {
                        IAsyncResult result = listener.BeginGetContext(new AsyncCallback(this.Actionhandler), null);
                        runningContexts.Add(result);
                    } 
                    else {
                        System.Threading.Thread.Sleep(200);
                    }
                }
            });
        }
        
        /// <summary>
        /// Async callback for incoming requests. calls routing function from HttpActions and 
        /// writes response
        /// </summary>
        /// <param name="Result"></param>
        private void Actionhandler(IAsyncResult Result) {
            HttpListenerContext Context = listener.EndGetContext(Result);
            
            Request Request = new Request(Context.Request);
            IResponse Response = HttpActions.Route(new Request(Context.Request), repository).Invoke(Request);
            
            Context.Response.StatusDescription = Response.Status;
            Context.Response.StatusCode = Response.Statuscode;
            
            Context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            Context.Response.Headers.Add("Content-type", "application/json");
            foreach(string key in Response.Header.Keys) Context.Response.Headers.Add(key, Response.Header[key]);
            
            byte[] output = System.Text.Encoding.UTF8.GetBytes(Response.Body);
            Context.Response.ContentLength64 = output.Length;
            Context.Response.OutputStream.Write(output, 0, output.Length);
            Context.Response.OutputStream.Close();
        }
        
        public void StopService() {
            if (listener == null) return;
            
            if (listener.IsListening) listener.Stop();            
            listener = null;
            
            contextTask.Wait();
            contextTask = null;
            
            runningContexts = null;
        }
    }
}
