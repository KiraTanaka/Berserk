namespace Domain
{
    public class Result
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public static Result GetSuccess()
            => new Result { Success = true };

        public static Result GetError(string message)
            => new Result {Success = false, Message = message};
    }
}