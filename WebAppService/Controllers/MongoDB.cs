using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using WebAppService.Model;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebAppService.Controllers
{
    [ApiController]
    [Produces("text/json")]
    [Route("/mongo")]
    public class MongoDB
    {
        private MongoClient dbClient;
        [HttpGet]
        [Route("/mongo/getAll")]
        public async Task<string> Get()
        {
            init();
            var database = dbClient.GetDatabase ("sample_training");
            var tutorialDB = dbClient.GetDatabase("sample_training");
            var tutorialCollection = tutorialDB.GetCollection<Tutorial>("Tutorials");
            var allTutorials = await tutorialCollection.Find(_ => true).ToListAsync();
            Console.WriteLine(JsonSerializer.Serialize(allTutorials));
            
            #region getallDB
            /*
            var dbList = dbClient.ListDatabases().ToList();
            dbClient.GetDatabase("Tutorials");
            Console.WriteLine("The list of databases on this server is: ");
            foreach (var db in dbList)
            {
                Console.WriteLine(db);
            }
            */
            #endregion
            string result = JsonSerializer.Serialize(allTutorials);
            return result;
        }

        [HttpGet]
        [Route("/test")]
        public int GetTest()
        {
            init();
            return 2;
        }

        [HttpGet]
        [Route("/mongo/create")]
        public int CreateTutorial()
        {
            init();
            var database = dbClient.GetDatabase ("sample_training");
            Tutorial t = new Tutorial();
            t.id = 1;
            t.steps = new List<TutorialStep>();
            t.steps.Add(new TutorialStep(1, "content 1 step 1"));
            t.steps.Add(new TutorialStep(2, "content 2 step 1"));
            t.steps.Add(new TutorialStep(3, "content 3 step 1"));
            t.steps.Add(new TutorialStep(4, "content 4 step 1"));
            var step5 = new TutorialStep(5, "content 5 step 1");
            t.title = "test2";
            var tutCollection = database.GetCollection<Tutorial>("Tutorials");
            tutCollection.InsertOne(t);
            return 0;
        }

        [HttpGet]
        [Route("/mongo/deleteAll")]
        public int DeleteAllTutorials()
        {
            init();
            Console.WriteLine("löschen");
            dbClient.GetDatabase("sample_training").GetCollection<Tutorial>("Tutorials").DeleteMany(_ => true);
            return 3;
        }

        private void init()
        {
            string atlasConnString = "mongodb://localhost:27017";
            if (dbClient == null)
            {
                dbClient = new MongoClient(atlasConnString);

            }
        }
    }
}