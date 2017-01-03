using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.GameData
{
    public class PlayerMove
    {
        public Guid PlayerId { get; set; }
        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return PlayerId.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return (obj as PlayerMove)?.PlayerId == PlayerId;
        }
    }

    public static class PlayerMoveExtension
    {
        public static bool Contains(this IEnumerable<PlayerMove> coll, Guid playerId)
        {
            return coll.Any(x => x.PlayerId == playerId);
        }
    }
}
