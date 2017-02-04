namespace Domain.Process
{
    public class MoveResult
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public static MoveResult GetSuccess()
        {
            return new MoveResult { Success = true };
        }

        public static MoveResult GetError(string message)
        {
            return new MoveResult { Success = false, Message = message };
        }
    }
            
}