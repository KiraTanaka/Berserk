using Assets.Scripts.UI.Transformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Infrastructure;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Cards
{
    public class ActiveCard
    {
        public GameObject _gameObject;
        public string InstId { get; set; }

        public string PlayerId { get; set; }

        public bool IsClosed { get; private set; }
        private Text _health;

        private Color _colorOrigin;

        private Color _colorClosing;

        private Renderer _renderer;

        private SelectionController _selectionCreature;
        public ActiveCard(GameObject gameObject, Vector3 selectScale, SelectionController selectionCreature)
        {
            _gameObject = gameObject;
            _renderer = _gameObject.GetComponent<Renderer>();           
            _selectionCreature = selectionCreature;
            _colorOrigin = _renderer.material.color;
            _colorClosing = new Color32(116, 116, 116, 255);           
            _health = _gameObject.transform.GetChild(0).GetComponent<Text>();            
            _selectionCreature.SetTransformation(new Transformation(null, null, selectScale));            
        }

        public void SetCard(CardInfo cardInfo)
        {
            InstId = cardInfo.InstId;
            _health.text = cardInfo.Health.ToString();
        }

        public void ChangeHealth(int health)
        {
            _health.text = health.ToString();
            _selectionCreature.IsSelected = false;
        }

        public void Close() => SetClose(true);

        public void Open() => SetClose(false);

        public void SetClose(bool value)
        {
            _renderer.material.SetColor("_Color", value ? _colorClosing : _colorOrigin);
            IsClosed = value;
        }
    }
}
