using Domain.GameData;
using Newtonsoft.Json;

namespace Application.Net
{
    public class Responce
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public GameInfo GameInfo { get; set; }

        public static Responce Error(string message)
            => new Responce {IsValid = false, Message = message};

        public static Responce Error(string message, GameInfo gameInfo)
            => new Responce { IsValid = false, Message = message, GameInfo = gameInfo };

        public static Responce Success(GameInfo gameInfo)
            => new Responce {IsValid = true, GameInfo = gameInfo };

        public static Responce Success(string message)
            => new Responce { IsValid = true, Message = message };

        public static Responce Success(string message, GameInfo gameInfo)
            => new Responce { IsValid = true, Message = message, GameInfo = gameInfo };

        public string ToJson() => JsonConvert.SerializeObject(this);

        public override string ToString()
            => $"{nameof(Responce)}: IsValid={IsValid}, Message={Message}";
    }
}
