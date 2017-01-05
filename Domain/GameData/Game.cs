using System;
using System.Collections.Generic;
using System.Linq;
using Domain.BoardData;
using Domain.CardData;

namespace Domain.GameData
{
    /// <summary>
    /// Управляет процессом игры.
    /// </summary>
    public class Game
    {
        private readonly CardDeck _cardDeck;
        private readonly List<PlayerZone> _playerZones;
        private readonly BattleField _battleField;

        public Game(IRules rules, IEnumerable<IBaseCard> cards, IGameContext context)
        {
            _cardDeck = new CardDeck(cards);
            _playerZones = GetPlayerZones(rules, context.GetPlayers().ToArray(), _cardDeck);
            _battleField = new BattleField(rules.FieldRows, rules.FieldColumns);

            Console.WriteLine("Game loaded");
            //DealCards(rules.PlayerCardsAmount);
            PlayGame();
        }
        
        private static List<PlayerZone> GetPlayerZones(IRules rules, Player[] players, CardDeck cardDeck)
        {
            var playerDecks = cardDeck.SplitRandom(players.Length);
            return players.Select((player, i) =>
                new PlayerZone(rules, player, playerDecks[i])).ToList();
        }

        private void DealCards(int playerCardsAmount)
        {
            for (var i = 0; i < playerCardsAmount; i++)
                _playerZones.ForEach(x =>
                {
                    var selectedCard = x.SelectCard(GetGameInfo());
                    x.DealCard(selectedCard);
                });
        }
        
        private void PlayGame()
        {
            _playerZones.ForEach(x => x.Move(GetGameInfo()));
        }

        private GameInfo GetGameInfo()
        {
            PlayerSetInfo[] playerSetsInfos = _playerZones.Select(x => x.GetInfo()).ToArray();
            return new GameInfo(_battleField.GetInfo(), _cardDeck.GetSet(), playerSetsInfos);
        }
    }
}
