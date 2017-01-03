using Domain.GameData;
using Newtonsoft.Json;

namespace Application.Net
{
    public class GameResponce
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public GameInfo GameInfo { get; set; }

        public static GameResponce Error(string message)
            => new GameResponce {IsValid = false, Message = message};

        public static GameResponce Success(GameInfo gameInfo)
            => new GameResponce {IsValid = true, GameInfo = gameInfo };

        public static GameResponce Success(string message)
            => new GameResponce { IsValid = true, Message = message };

        public string ToJson() => JsonConvert.SerializeObject(this);
    }
}
