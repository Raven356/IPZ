using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogMVC.DAL.Models
{
    [BsonIgnoreExtraElements]
    public class TagToBlogPostMongo
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("blogpostid")]
        public string BlogPostId { get; set; }

        [BsonElement("tagid")]
        public string TagId { get; set; }
    }
}
