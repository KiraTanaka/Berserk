using System;
using System.Collections.Generic;
using Domain.Cards;
using Domain.GameProcess;
using System.Linq;
using UnityEngine.Networking;

namespace Assets.Scripts.UI.Game
{
    public class GameUnity : Domain.GameProcess.Game
    {

        public Guid? ActionCardId
        {
            get { return State.ActionCard?.InstId; }
            set
            {
                if (value != null)
                {
                    Card card = SearchCardAtPlayers(value);
                    State.ActionCard = card.Closed ? null : card;
                }
                else
                {
                    State.ActionCard = null;
                }
            }
        }

        public Guid? TargetCardId
        {
            get { return State.TargetCards?.FirstOrDefault()?.InstId; }
            set
            {
                State.TargetCards = value == null
                    ? null
                    : new List<Card> { SearchCardAtPlayers(value) };
            }
        }

        public GameUnity(IRules rules, IEnumerable<Card> cards, UserLimitedSet users) 
            : base(rules, cards, users)
        { }
    
        public void SaveSelectActiveCards(Guid playerId, Guid instId, NetworkInstanceId networkId)
        {
            if (playerId != State.MovingPlayer.Id) return;

            if (ActionCardId == null)
                ActionCardId = instId;
            else if (ActionCardId != instId)
            {
                TargetCardId = instId;
                onSelectionAttackWay?.Invoke(networkId);
            }
            else
                SetToZeroCards();
        }
        private Card SearchCardAtPlayers(Guid? instId)
            => FindCard(instId, State.MovingPlayer) ?? FindCard(instId, State.WaitingPlayer);

        private static Card FindCard(Guid? instId, Player player)
        {
            Card card = player.CardOnField.FirstOrDefault(x => x.InstId == instId);
            return card ?? (player.Hero.InstId == instId ? player.Hero : null);
        }
        public void SetAttackWay(CardActionEnum attackWay)
        {
            Move(attackWay);
            SetToZeroCards();
        }
        #region delegates and events

        public delegate void OnSelectionAttackWay(NetworkInstanceId networkId);
        public event OnSelectionAttackWay onSelectionAttackWay;

        #endregion
    }
}