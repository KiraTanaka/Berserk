using System;
using UnityEngine;

namespace Assets.Scripts.UI.Cards
{
    public delegate bool OnSelectCardHandler(string instId);
    public interface IActiveCard : ICard
    {
        string InstId { get; set; }

        GameObject GameObject { get; }

        void ChangeHealth(int health);

        void Close();

        void Open();

        bool IsClosed { get; }

        void DestroyCard();

        event OnSelectCardHandler OnSelectCard;
    }
}

