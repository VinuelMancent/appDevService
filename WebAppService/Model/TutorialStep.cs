using MongoDB.Bson.Serialization.Attributes;

namespace WebAppService.Model
{
    public class TutorialStep
    {
        [BsonElement("id")]
        public int id { get; set; }
        [BsonElement("content")]
        public string content { get; set; }

        
        public TutorialStep(int id, string content)
        {
            this.id = id;
            this.content = content;
        }
    }
}