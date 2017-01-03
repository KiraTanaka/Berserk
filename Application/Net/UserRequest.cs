using System;
using Domain.GameData;

namespace Application.Net
{
    public class UserRequest
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public PlayerMove Move { get; set; }
        public bool Registration { get; set; }
    }
}
