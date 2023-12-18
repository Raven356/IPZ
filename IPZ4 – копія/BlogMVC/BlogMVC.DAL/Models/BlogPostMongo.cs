using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogMVC.DAL.Models
{
    [BsonIgnoreExtraElements]
    public class BlogPostMongo
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; } = null!;

        [BsonElement("text")]
        public string Text { get; set; } = null!;

        [BsonElement("categoryid")]
        public string CategoryId { get; set; }

        [BsonElement("date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [BsonElement("authorid")]
        public string AuthorId { get; set; }

        [BsonElement("image")]
        public byte[] Image { get; set; }

    }
}
