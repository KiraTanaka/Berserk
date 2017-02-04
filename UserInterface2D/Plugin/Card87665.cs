using Domain.Cards;

namespace Plugin
{
    public class Card87665 : Card
    {
        public Card87665()
        {
            Id = 87665;
            Name = "Азатот";
            Type = CardTypeEnum.Hero;
            Desriprion = "Ранить выбранное ваше существо на 2, излечиться на 3. " +
                         "Поворот, получить 2 раны: Выбранное существо не наносит ран в свой " +
                         "следующий ход.";
            Cost = 0;
            Power = 0;
            Health = 28;
        }
    }
}
