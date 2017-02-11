using System;
using Domain.GameProcess;

namespace Domain.Cards
{
    public class Card
    {
        public Guid InstId { get; set; }

        public int CardId { get; set; }

        public CardTypeEnum Type { get; protected set; }

        public string Name { get; protected set; }

        public CardElementEnum Element { get; protected set; }

        public string Desriprion { get; protected set; }

        public int Cost { get; protected set; }

        public int Power { get; protected set; }

        public int Health { get; protected set; }

        public Func<GameState, MoveResult> Attack  { get; protected set; }

        public Func<GameState, MoveResult> Feature { get; protected set; }

        public string EquipementType { get; protected set; }

        public bool Closed { get; protected set; }

        public Card()
        {
            InstId = Guid.NewGuid();
        }

        public void Hurt(int value)
        {
            AddHealth(-value);
        }

        public void Heal(int value)
        {
            AddHealth(value);
        }

        private void AddHealth(int value)
        {
            Health = Health + value;
            OnChangeHealth?.Invoke(InstId, Health);
        }

        public void Open()
        {
            SetClose(false);
        }

        public void Close()
        {
            SetClose(true);
        }

        private void SetClose(bool value)
        {
            Closed = value;
            OnChangeClosed?.Invoke(InstId, Closed);
        }

        public bool IsAlive()
        {
            return Health > 0;
        }

        public MoveResult Action(CardActionEnum actionOption, GameState state)
        {
            if (actionOption == CardActionEnum.Simple) return Attack(state);
            if (actionOption == CardActionEnum.Feature) return Feature(state);
            throw new NotImplementedException($"State {state} is not implemented");
        }

        public static Card CreateInstance(Card prototype)
        {
            return new Card
            {
                InstId = Guid.NewGuid(),
                CardId = prototype.CardId,
                Name = prototype.Name,
                Health = prototype.Health,
                Attack = prototype.Attack,
                Type = prototype.Type,
                Desriprion = string.Copy(prototype.Desriprion),
                Feature = prototype.Feature,
                Closed = prototype.Closed,
                Power = prototype.Power,
                Cost = prototype.Cost,
                Element = prototype.Element,
                EquipementType = prototype.EquipementType
            };
        }

        #region delegates and events

        public delegate void OnChangeHealthHandler(Guid instId, int health);

        public delegate void OnChangeClosedHandler(Guid instId, bool closed);

        public event OnChangeHealthHandler OnChangeHealth;

        public event OnChangeClosedHandler OnChangeClosed;

        #endregion
    }
}
