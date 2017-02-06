namespace Domain.Process
{
    public class UserLimitedSet : LimitedSet<User>
    {
        public UserLimitedSet() : base(2) { }
    }
}
