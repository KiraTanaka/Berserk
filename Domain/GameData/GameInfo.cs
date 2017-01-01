using Domain.BoardData;

namespace Domain.GameData
{
    public struct GameInfo
    {
        public BattleFieldInfo BattleFieldInfo { get; }
        public CardSetInfo CardSetInfo { get; }
        public PlayerSetInfo[] PlayerSetInfos { get; }

        public GameInfo(
            BattleFieldInfo battleFieldInfo, 
            CardSetInfo cardSetInfo, 
            PlayerSetInfo[] playerSetInfos)
        {
            BattleFieldInfo = battleFieldInfo;
            CardSetInfo = cardSetInfo;
            PlayerSetInfos = playerSetInfos;
        }
    }
}
