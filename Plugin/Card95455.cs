﻿using System.Linq;
using Domain;

namespace Plugin
{
    // http://berserk.ru/?route=card/card&card_id=95455
    public class Card95455 : Card
    {
        public Card95455()
        {
            Id = 95455;
            Type = CardTypeEnum.Creature;
            Name = "Авгур";
            Element = ElementEnum.Neutral;
            Desriprion =
                "Найм: Заплатите 2 Монеты, при этом замешайте 2 выбранные карты с любых кладбищ в колоды владельцев.";
            Cost = 2;
            Power = 2;
            Health = 2;
            Attack = Helper.StandartAttack;
            Feature = state => Result.GetSuccess();
        }
    }
}
