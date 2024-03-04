using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TonaWebApp.Models
{
    public class Board
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Auther { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int MaxMember { get; set; } = 0;

        public int AmountMember { get; set; } = 0;

        public bool IsActive { get; set; } = false;

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Tag { get; set; } = null!;

        public List<User> MemberList { get; set; } = [];
    }
}