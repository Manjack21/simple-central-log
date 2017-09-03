/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Info
 * Datum: 31.08.2017
 * Zeit: 09:02
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */
using System;
using NUnit.Framework;
using SimpleCentralLog;
using SimpleCentralLog.Http;

namespace SimpleCentralLogUnitTest.Http
{
    /// <summary>
    /// Description of HttpActionsTest.
    /// </summary>
    [TestFixture]
    public class HttpActionsTest
    {
        EntryRepository repo;
        
        [SetUp]
        public void Start() {
            repo = new EntryRepository();
        }
        
        [Test]
        public void Route_Request_with_unknown_URI_returns_Error404() {
            IRequest Request = new Request("GET", "/uri/unknown");
            IResponse Response = HttpActions.Route(Request, repo).Invoke(Request);
            
            Assert.AreEqual(404, Response.Statuscode);
            Assert.AreEqual("page not found", Response.Status);
        }
        
        /*LOG*/
        
        [Test]
        public void Route_POST_log_with_JSONentry_returns_OK200() {
            IRequest Request = new Request("POST", "/log", "{\"Message\":\"Test\"}");
            IResponse Response = HttpActions.Route(Request, repo).Invoke(Request);
            
            Assert.AreEqual(200, Response.Statuscode);
            Assert.AreEqual("OK", Response.Status);
            Assert.AreEqual(1, repo.Count());
        }
        
        [Test]
        public void Route_POST_log_without_data_returns_Error400() {
            IRequest Request = new Request("POST", "/log", " ");
            IResponse Response = HttpActions.Route(Request, repo).Invoke(Request);
            
            Assert.AreEqual(400, Response.Statuscode);
            Assert.AreEqual("no data", Response.Status);
        }
        
        [Test]
        public void Route_POST_log_with_invalid_data_returns_Error400() {
            IRequest Request = new Request("POST", "/log", "[{\"Message\":\"Test\"}]");
            IResponse Response = HttpActions.Route(Request, repo).Invoke(Request);
            
            Assert.AreEqual(400, Response.Statuscode);
            Assert.AreEqual("invalid input", Response.Status);
        }
        
        /*QUERY*/
        
        [Test]
        public void Route_POST_query_with_invalid_data_returns_OK200() {
            IRequest Request = new Request("POST", "/log", "[{\"Message\":\"Test\"}]");
            IResponse Response = HttpActions.Route(Request, repo).Invoke(Request);
            
            Assert.AreEqual(400, Response.Statuscode);
            Assert.AreEqual("invalid input", Response.Status);
        }
    }
}
