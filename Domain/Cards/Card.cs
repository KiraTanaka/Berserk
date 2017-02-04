using System;
using Domain.Process;
using Infrastructure;
using Infrastructure.Cloneable;

namespace Domain.Cards
{
    public class Card : ICloneable<Card>
    {
        public int Id { get; set; }
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
        #region delegates and events
        public delegate void OnChangeHealthHandler(int cardId, int health);
        public delegate void OnChangeClosedHandler(int cardId, bool closed);
        public event OnChangeHealthHandler onChangeHealth;
        public event OnChangeClosedHandler onChangeClosed;
        #endregion
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
            onChangeHealth?.Invoke(Id, Health);
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
            onChangeClosed?.Invoke(Id, Closed);
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

        public Card Clone()
        {
            return new Card
            {
                Id = Id,
                Name = Name,
                Health = Health,
                Attack = Attack,
                Type = Type,
                Desriprion = Desriprion,
                Feature = Feature,
                Closed = Closed,
                Power = Power,
                Cost = Cost,
                Element = Element,
                EquipementType = EquipementType
            };
        }
    }
}