using UnityEngine;

namespace Assets.Scripts.UI.Cards
{
    public interface IActiveCard
    {
        string InstId { get; set; }

        GameObject GameObject { get; }

        void ChangeHealth(int health);

        void Close();

        void Open();

        void SetClose(bool value);

        bool IsClosed { get; }
        void IsDead();
    }
}

