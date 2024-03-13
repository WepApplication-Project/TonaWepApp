using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

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

        private bool _isActive;

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        private DateTime _startTime;

        public DateTime StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;
                IsActive = DateTime.Now >= value;
            }
        }

        private DateTime _endTime;

        public DateTime EndTime
        {
            get { return _endTime; }
            set
            {
                _endTime = value;
                IsActive = DateTime.Now < value;
            }
        }

        public DateTime MeetingTime { get; set; }

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
