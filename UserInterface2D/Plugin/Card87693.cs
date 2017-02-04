using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Cards;

namespace Plugin
{
    public class Card87693 : Card
    {
        public Card87693()
        {
            Id = 87693;
            Name = "Дейла";
            Type = CardTypeEnum.Hero;
            Desriprion = "Посмотрите верхнюю карту любой колоды, вы можете положить её под низ этой колоды.";
            Cost = 0;
            Power = 0;
            Health = 27;
        }
    }
}
