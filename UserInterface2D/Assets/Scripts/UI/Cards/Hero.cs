using Assets.Scripts.UI.Transformations;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Cards
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class Hero : MonoBehaviour, IActiveCard
    {
        public GameObject GameObject => gameObject;

        public bool IsClosed { get { return _activeCard.IsClosed; } }
        public string InstId { get { return _activeCard.InstId; } set { _activeCard.InstId = value; } }

        private ActiveCard _activeCard;

        private SelectionController _selectionCreature;

        private readonly Vector3 _selectScale = new Vector3(36, 36, 1);

        public event OnSelectCardHandler OnSelectCard;

        void Awake()
        {
            _selectionCreature = GetComponent<SelectionController>();
            _activeCard = new ActiveCard(gameObject, _selectScale, _selectionCreature);
        }
        public void SetCard(CardInfo heroInfo) => _activeCard.SetCard(heroInfo);

        public void ChangeHealth(int health) => _activeCard.ChangeHealth(health);

        public void Close() => _activeCard.Close();

        public void Open() => _activeCard.Open();

        public void OnMouseDown()
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
