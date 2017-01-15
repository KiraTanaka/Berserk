using System;
using Infrastructure;

namespace Domain
{
    public class Card : ICloneable<Card>
    {
        public int Id { get; set; }
        public CardTypeEnum Type { get; protected set; }
        public string Name { get; protected set; }
        public ElementEnum Element { get; protected set; }
        public string Desriprion { get; protected set; }
        public int Cost { get; protected set; }
        public int Power { get; protected set; }
        public int Health { get; protected set; }
        public Func<GameState, Result> Attack  { get; protected set; }
        public Func<GameState, Result> Feature { get; protected set; }
        public string EquipementType { get; protected set; }
        public bool Closed { get; protected set; }

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
        }

        public bool IsAlive()
        {
            return Health > 0;
        }

        public Result Action(ActionEnum actionOption, GameState state)
        {
            if (actionOption == ActionEnum.Simple) return Attack(state);
            if (actionOption == ActionEnum.Feature) return Feature(state);
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
