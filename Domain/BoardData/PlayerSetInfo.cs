using System;
namespace Domain.BoardData
{
    public struct PlayerSetInfo // TODO нужен ли этот fluent?
    {
        public Guid PlayerId { get; }
        public PlayerCardSetInfo DeskInfo { get; }
        public PlayerCardSetInfo UtilityAreaInfo { get; }
        public PlayerCardSetInfo CementeryInfo { get; }

        private PlayerSetInfo(
            Guid playerId, 
            PlayerCardSetInfo deskInfo, 
            PlayerCardSetInfo utilityAreaInfo, 
            PlayerCardSetInfo cementeryInfo)
        {
            PlayerId = playerId;
            DeskInfo = deskInfo;
            UtilityAreaInfo = utilityAreaInfo;
            CementeryInfo = cementeryInfo;
        }

        public static DeckInfoData SetDeskInfo(PlayerCardSetInfo deskInfo)
        {
            return new DeckInfoData(deskInfo);
        }

        public struct DeckInfoData
        {
            public PlayerCardSetInfo DeskInfo { get; }

            public DeckInfoData(PlayerCardSetInfo deskInfo)
            {
                DeskInfo = deskInfo;
            }

            public UtilityAreaInfoData SetUtilityAreaInfo(PlayerCardSetInfo utilityAreaInfo)
                => new UtilityAreaInfoData(utilityAreaInfo, this);
        }

        public struct UtilityAreaInfoData
        {
            public PlayerCardSetInfo UtilityAreaInfo { get; }
            public DeckInfoData DeckInfoData { get; }

            public UtilityAreaInfoData(PlayerCardSetInfo utilityAreaInfo, DeckInfoData deckInfoData)
            {
                UtilityAreaInfo = utilityAreaInfo;
                DeckInfoData = deckInfoData;
            }

            public CementeryInfoData SetCementeryInfo(PlayerCardSetInfo cementeryInfo)
                =>  new CementeryInfoData(cementeryInfo, this);
        }

        public struct CementeryInfoData
        {
            public PlayerCardSetInfo CementeryInfo { get; }
            public UtilityAreaInfoData UtilityAreaInfoData { get; }

            public CementeryInfoData(PlayerCardSetInfo cementeryInfo, UtilityAreaInfoData utilityAreaInfoData)
            {
                CementeryInfo = cementeryInfo;
                UtilityAreaInfoData = utilityAreaInfoData;
            }

            public PlayerInfoData SetPlayerId(Guid playerId)
                => new PlayerInfoData(playerId, this);
        }

        public struct PlayerInfoData
        {
            public Guid PlayerId { get; }
            public CementeryInfoData CementeryInfoData { get; }

            public PlayerInfoData(Guid playerId, CementeryInfoData cementeryInfoData)
            {
                PlayerId = playerId;
                CementeryInfoData = cementeryInfoData;
            }

            public PlayerSetInfo GetPlayerSetInfo()
                => new PlayerSetInfo(
                    PlayerId,
                    CementeryInfoData.UtilityAreaInfoData.DeckInfoData.DeskInfo,
                    CementeryInfoData.UtilityAreaInfoData.UtilityAreaInfo,
                    CementeryInfoData.CementeryInfo
                );
        }
    }
}
