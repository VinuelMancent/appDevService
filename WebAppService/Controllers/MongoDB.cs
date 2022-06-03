using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using WebAppService.Model;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;

namespace WebAppService.Controllers
{
    

    
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("/mongo")]
    public class MongoDB
    {
        
        string databaseName = "Tutorials";
        private string collectionName = "Tutorials";
        
        
        private MongoClient dbClient;
        [HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("/mongo/getAll")]
        public async Task<string> Get()
        {
            init();
            var tutorialDB = dbClient.GetDatabase(databaseName);
            var tutorialCollection = tutorialDB.GetCollection<Tutorial>(collectionName);
            var allTutorials = await tutorialCollection.Find(_ => true).ToListAsync();
            Console.WriteLine(JsonSerializer.Serialize(allTutorials));
            
            #region getallDB
            
            var dbList = dbClient.ListDatabases().ToList();
            dbClient.GetDatabase(databaseName);
            Console.WriteLine("The list of databases on this server is: ");
            foreach (var db in dbList)
            {
                Console.WriteLine(db);
            }
            
            #endregion
            string result = JsonSerializer.Serialize(allTutorials);
            return result;
        }

        [HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("/test")]
        public int GetTest()
        {
            init();
            return 2;
        }

        [HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("/mongo/create")]
        public int CreateTutorial()
        {
            init();
            var database = dbClient.GetDatabase (databaseName);
            Tutorial t = new Tutorial();
            t.id = 1;
            t.steps = new List<TutorialStep>();
            t.steps.Add(new TutorialStep(1, "content 1 step 1"));
            t.steps.Add(new TutorialStep(2, "content 2 step 1"));
            t.steps.Add(new TutorialStep(3, "content 3 step 1"));
            t.steps.Add(new TutorialStep(4, "content 4 step 1"));
            var step5 = new TutorialStep(5, "content 5 step 1");
            t.title = "test2";
            var tutCollection = database.GetCollection<Tutorial>(collectionName);
            tutCollection.InsertOne(t);
            return 0;
        }

        [HttpPost]
        [Consumes("application/json")]
        [Microsoft.AspNetCore.Mvc.Route("/mongo/add")]
        public int Add([FromBody] string json)
        {
            //deserialize json to object
            byte[] newBytes = Convert.FromBase64String(json);
            var decodedJson = BitConverter.ToString(newBytes);
            var tutorial = JsonSerializer.Deserialize<Tutorial>(decodedJson);
            Console.WriteLine($"Adding tutorial {tutorial}");
            //send object to database
            var database = dbClient.GetDatabase (databaseName);
            var tutCollection = database.GetCollection<Tutorial>(collectionName);
            tutCollection.InsertOne(tutorial);
            return 5;
        }
        [HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("/mongo/deleteAll")]
        public int DeleteAllTutorials()
        {
            init();
            Console.WriteLine("löschen");
            dbClient.GetDatabase(databaseName).GetCollection<Tutorial>(collectionName).DeleteMany(_ => true);
            return 3;
        }

        private void createNewTutorial()
        {
            Tutorial tut = new Tutorial();
            tut.id = 0;
            tut.title = "example";
            tut.steps = new List<TutorialStep>();
            tut.steps.Add(new TutorialStep(1, "content 1"));
            tut.steps.Add(new TutorialStep(2, "content 2"));

            var tutorialDB = dbClient.GetDatabase(databaseName);
            var tutorialCollection = tutorialDB.GetCollection<Tutorial>(collectionName);
            tutorialCollection.InsertOne(tut);
        }
        private void init()
        {
            try
            {
                createNewTutorial();
            }
            catch (Exception)
            {
                
            }
            string username = "AppDev";
            string passwd = "LfD9XvAMDWJRPqwE";
            var settings = MongoClientSettings.FromConnectionString(
                $"mongodb+srv://{username}:{passwd}@tutorialapp.bvdap.mongodb.net/?retryWrites=true&w=majority");
            if (dbClient == null)
            {
                dbClient = new MongoClient(settings);

            }
        }
    }
}