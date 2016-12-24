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
        private readonly IRules _rules;
        private readonly List<CardPlace<Card>> _cardSet;
        private readonly BattleField<Card> _battleField;
        private readonly List<PlayerCardPlace<Card>> _desks;
        private readonly List<PlayerCardPlace<Card>> _utilityAreas;
        private readonly List<PlayerCardPlace<Card>> _cemeteries;
        private readonly List<Player> _players;

        public Game(List<CardPlace<Card>> cardSet, IRules rules)
        {
            _rules = rules;
            _cardSet = cardSet;
            _battleField = new BattleField<Card>(_rules.FieldRows, _rules.FieldColumns);
            _desks = new List<PlayerCardPlace<Card>>();
            _utilityAreas = new List<PlayerCardPlace<Card>>();
            _cemeteries = new List<PlayerCardPlace<Card>>();
            _players = new List<Player>();
        }

        public void Attach(Player p)
        {
            _players.Add(p);
        }

        public void Start()
        {
            FillPlayerCardPlaces();
            DealCards();
        }

        private void FillPlayerCardPlaces()
        {
            _players.ForEach(player =>
            {
                _desks.AddEmpty(player.Id);
                _utilityAreas.AddEmpty(player.Id);
                _cemeteries.AddEmpty(player.Id);
            });
        }

        private void DealCards()
        {
            for (var i = 0; i < _rules.StartCardsAmount; i++)
            {
                LetPlayersSelectCards();
            }
        }

        private void LetPlayersSelectCards()
        {
            foreach (var player in _players)
            {
                var selectedCard = player.SelectCard(_cardSet);
                _desks.PushFirst(player.Id, selectedCard);
            }
        }
    }
}
