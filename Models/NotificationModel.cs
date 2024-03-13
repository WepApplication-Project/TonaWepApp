using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TonaWebApp.Models
{
    public class Notification
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Title { get; set; } = null!;

        public string SubTitle {get; set; } = null!;

        public Board BoardData { get; set; } = null!;

        public User User { get; set; } = null!;

        public bool IsReaded { get; set; } = false;
    }
}