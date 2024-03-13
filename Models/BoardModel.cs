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

        public User? Auther { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int MaxMember { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public DateTime MeetingTime {get; set;}

        public string Tag { get; set; } = null!;

        public List<User> MemberList { get; set; } = [];

        public string Place { get; set; } = null!;

        public void AddMember(User user)
        {
            MemberList.Add(user);
        }

        public void RemoveMember(User user)
        {
            MemberList.Remove(user);
        }
    }
}