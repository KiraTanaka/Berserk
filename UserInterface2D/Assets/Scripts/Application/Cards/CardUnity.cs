using System.Diagnostics.CodeAnalysis;
using Assets.Scripts.Application.Transformations;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Assets.Scripts.Application.Cards
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class CardUnity : NetworkBehaviour, IActiveCard
    {
        public string InstId { get; set; }

        public string PlayerId { get; set; }
        
        public GameObject GameObject => gameObject;

        public bool IsClosed { get; private set; }


        private Text _power;

        private Text _health;
        
        private SelectionController _selectionCreature;

        private Color _colorOrigin;

        private Color _colorClosing;

        private Renderer _renderer;
        
        private readonly Vector3 _selectScale = new Vector3(24, 24, 1);


        public void SetCard(CardInfo cardInfo)
        {
            InstId = cardInfo.InstId;
            _power.text = cardInfo.Power.ToString();
            _health.text = cardInfo.Health.ToString();
        }

        public void Close() => SetClose(true);

        public void Open() => SetClose(false);

        public void SetClose(bool value)
        {
            _renderer.material.SetColor("_Color", value ? _colorClosing : _colorOrigin);
            IsClosed = value;
        }

        public void ChangeHealth(int health)
        {
            if (health > 0)
            {
                _health.text = health.ToString();
                _selectionCreature.IsSelected = false;
            }
            else
            {
                GameObject.FindWithTag("BorderActiveCard").SetActive(false);
                Destroy(gameObject);
            }
        }

        
        void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _power = transform.GetChild(0).GetComponent<Text>();
            _health = transform.GetChild(1).GetComponent<Text>();
            _selectionCreature = GetComponent<SelectionController>();
            _selectionCreature.SetTransformation(new Transformation(_selectScale));
            _colorOrigin = _renderer.material.color;
            _colorClosing = new Color32(116, 116, 116, 255);
        }

        void OnMouseDown()
        {
            var invoke = OnSelectCard?.Invoke(InstId);
            if (invoke != null && invoke.Value)
                _selectionCreature.IsSelected = !_selectionCreature.IsSelected;
        }

        #region delegates and events
        public delegate bool OnSelectCardHandler(string instId);
        public event OnSelectCardHandler OnSelectCard;
        #endregion
    }
}

