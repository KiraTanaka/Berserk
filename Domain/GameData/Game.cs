using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        private readonly IRules _rules;
        private readonly CardDeck _cardDeck;
        private readonly List<PlayerZone> _playerZones;
        private readonly BattleField _battleField;

        public Game(IRules rules, IEnumerable<IBaseCard> cards, IEnumerable<Player> players)
        {
            _rules = rules;
            _cardDeck = new CardDeck(cards);
            _playerZones = GetPlayerZones(players.ToArray(), _cardDeck);
            _battleField = new BattleField(rules.FieldRows, rules.FieldColumns);
        }
        
        private static List<PlayerZone> GetPlayerZones(Player[] players, CardDeck cardDeck)
        {
            var playerDecks = cardDeck.SplitRandom(players.Length);
            return players.Select((player, i) => new PlayerZone(player, playerDecks[i])).ToList();
        }

        public void Start()
        {
            DealCards();
            PlayGame();
        }

        private void DealCards()
        {
            for (var i = 0; i < _rules.PlayerCardsAmount; i++)
            {
                ForEachPlayer(playerSet =>
                {
                    var selectedCard = playerSet.SelectCard(GetGameInfo(), _cardDeck);
                    playerSet.DealCard(selectedCard);
                });
            }
        }
        
        private void PlayGame()
        {
            ForEachPlayer(playerSet =>
            {
                playerSet.Move(GetGameInfo());
            });
        }

        private void ForEachPlayer(Action<PlayerZone> action)
        {
            foreach (var playerSet in _playerZones)
            {
                action(playerSet);
            }
        }

        private GameInfo GetGameInfo()
        {
            PlayerSetInfo[] playerSetsInfos = _playerZones.Select(x => x.GetInfo()).ToArray();
            return new GameInfo(_battleField.GetInfo(), _cardDeck.GetSet(), playerSetsInfos);
        }
    }
}
