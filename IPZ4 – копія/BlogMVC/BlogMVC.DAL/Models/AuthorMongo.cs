using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogMVC.DAL.Models
{
    [BsonIgnoreExtraElements]
    public class AuthorMongo
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("nickname")]
        public string NickName { get; set; } = null!;

        [BsonElement("userid")]
        public string UserId { get; set; } = null!;

        public override string? ToString()
        {
            return NickName;
        }
    }
}
