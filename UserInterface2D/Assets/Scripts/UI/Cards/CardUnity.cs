using System.Diagnostics.CodeAnalysis;
using Assets.Scripts.UI.Transformations;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Cards
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class CardUnity : NetworkBehaviour, IActiveCard
    {
        public GameObject GameObject => gameObject;

        public bool IsClosed { get { return _activeCard.IsClosed; } }
        public string InstId { get { return _activeCard.InstId; } set { _activeCard.InstId = value; } }

        private Text _power;

        private ActiveCard _activeCard;

        private SelectionController _selectionCreature;
        
        private readonly Vector3 _selectScale = new Vector3(24, 24, 1);

        public event OnSelectCardHandler OnSelectCard;

        void Awake()
        {
            _power = transform.GetChild(1).GetComponent<Text>();
            _selectionCreature = GetComponent<SelectionController>();
            _activeCard = new ActiveCard(gameObject, _selectScale, _selectionCreature);            
        }

        public void SetCard(CardInfo cardInfo)
        {
            _activeCard.SetCard(cardInfo);
            _power.text = cardInfo.Power.ToString();
        }

        public void Close() => _activeCard.Close();

        public void Open() => _activeCard.Open();

        public void ChangeHealth(int health) => _activeCard.ChangeHealth(health);

        void OnMouseDown()
        {
            var invoke = OnSelectCard?.Invoke(InstId);
            if (invoke != null && invoke.Value)
                _selectionCreature.IsSelected = !_selectionCreature.IsSelected;
        }
        public void DestroyCard()
        {
            _selectionCreature.Border.SetActive(false);
            Destroy(gameObject);
        }
    }
}

