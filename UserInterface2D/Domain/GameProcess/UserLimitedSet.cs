using Domain.Tools;

namespace Domain.GameProcess
{
    public class UserLimitedSet : LimitedSet<User>
    {
        public UserLimitedSet() : base(2) { }
    }
}
