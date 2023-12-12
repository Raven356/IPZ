using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogMVC.DAL.Models
{
    [BsonIgnoreExtraElements]
    public class CommentMongo
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("text")]
        public string Text { get; set; } = null!;

        [BsonElement("userid")]
        public string UserId { get; set; } = null!;

        [BsonElement("blogpostid")]
        public string BlogPostId { get; set; }
    }
}
