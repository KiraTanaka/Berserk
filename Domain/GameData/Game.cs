using System;
using System.Collections.Generic;
using System.Linq;
using Domain.BoardData;

namespace Domain.GameData
{
    /// <summary>
    /// Управляет процессом игры.
    /// </summary>
    public class Game
    {
        private readonly IRules _rules;
        private readonly CardSet _cardSet;
        private readonly List<PlayerSet> _playerSets;
        private readonly BattleField _battleField;

        public Game(IRules rules, CardSet cardSet, List<PlayerSet> playerSets)
        {
            _rules = rules;
            _cardSet = cardSet;
            _playerSets = playerSets;
            _battleField = new BattleField(rules.FieldRows, rules.FieldColumns);
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
                    var selectedCard = playerSet.SelectCard(_cardSet);
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

        private void ForEachPlayer(Action<PlayerSet> action)
        {
            foreach (var playerSet in _playerSets)
            {
                action(playerSet);
            }
        }

        private GameInfo GetGameInfo()
        {
            PlayerSetInfo[] playerSetsInfos = _playerSets.Select(x => x.GetInfo()).ToArray();
            return new GameInfo(_battleField.GetInfo(), _cardSet.GetInfo(), playerSetsInfos);
        }
    }
}
