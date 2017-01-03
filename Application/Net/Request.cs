using System;
using Domain.GameData;

namespace Application.Net
{
    public class Request
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public PlayerMove Move { get; set; }
        public string Card { get; set; }
        public bool Registration { get; set; }
    }
}
