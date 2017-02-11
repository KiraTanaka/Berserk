using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Cards
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class Hero : MonoBehaviour, IActiveCard
    {
        public string InstId { get; set; }

        public string PlayerId { get; set; }

        public GameObject GameObject => gameObject;

        public bool IsClosed { get; private set; }


        private Color _colorOrigin;

        private Color _colorClosing;

        private Renderer _renderer;

        private Text _health;
        

        public void SetCard(CardInfo heroInfo)
        {
            InstId = heroInfo.InstId;
            _health.text = heroInfo.Health.ToString();
        }

        public void ChangeHealth(int health)
        {
            if (health > 0) _health.text = health.ToString();
            else Destroy(gameObject);
        }

        public void Close() => SetClose(true);

        public void Open() => SetClose(false);

        public void SetClose(bool value)
        {
            _renderer.material.SetColor("_Color", value ? _colorClosing : _colorOrigin);
            IsClosed = value;
        }


        void Awake()
        {
            _health = transform.GetChild(0).GetComponent<Text>();
            _renderer = GetComponent<Renderer>();
            _colorOrigin = _renderer.material.color;
            _colorClosing = new Color32(116, 116, 116, 255);
        }

        void OnMouseDown()
        {
            OnSelectCard?.Invoke(InstId);
        }

        #region delegates and events
        public delegate bool OnSelectCardHandler(string instId);
        public event OnSelectCardHandler OnSelectCard;
        #endregion
    }
}
