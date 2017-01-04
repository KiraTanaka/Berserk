using Domain.BoardData;
using Domain.CardData;

namespace Domain.GameData
{
    public struct GameInfo
    {
        public BattleFieldInfo BattleFieldInfo { get; }
        public IBaseCard[] CardSetInfo { get; }
        public PlayerSetInfo[] PlayerSetInfos { get; }

        public GameInfo(
            BattleFieldInfo battleFieldInfo,
            IBaseCard[] cardSetInfo, 
            PlayerSetInfo[] playerSetInfos)
        {
            BattleFieldInfo = battleFieldInfo;
            CardSetInfo = cardSetInfo;
            PlayerSetInfos = playerSetInfos;
        }
    }
}
