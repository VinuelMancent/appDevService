using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebAppService.Model
{
    public class Tutorial
    {
        [BsonId]
        public int  id { get; set; }

        [BsonElement("title")]
        public string title { get; set; } = null!;
        
        [BsonElement("steps")]
        public List<TutorialStep> steps { get; set; }

    }
}