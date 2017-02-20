﻿using System.Diagnostics.CodeAnalysis;
using Assets.Scripts.UI.Transformations;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.UI.Cards
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class CardInHand : NetworkBehaviour
    {
        public string InstId { get; set; }

        public string PlayerId { get; set; }


        private Renderer _renderer = new Renderer();

        private float _currentPositionY;

        private readonly float _selectPositionY = 3;

        private string _currentLayer = "";

        private readonly Vector3 _selectScale = new Vector3(1.2f, 1.2f, 1);

        private SelectionController _selectionCreature;


        public void SetCard(string instId)
        {
            InstId = instId;
        }

        public void DestroyCard()
        {
            _selectionCreature.Border.SetActive(false);
            Destroy(gameObject);
        }
        
        void Awake ()
        {
            _renderer = GetComponent<Renderer>();
            _currentPositionY = transform.position.y;
            var selectPosition = new Vector3(transform.position.x, transform.position.y + _selectPositionY, transform.position.z);
            _selectionCreature = GetComponent<SelectionController>();
            _selectionCreature.SetTransformation(new Transformation(selectPosition,_selectScale));
        }
        
        void OnMouseEnter()
        {
            _currentLayer = _renderer.sortingLayerName;
            _renderer.sortingLayerName = "Selected";
        }

        void OnMouseExit()
        {
            _renderer.sortingLayerName = _currentLayer;
        }

        void OnMouseDown()
        {
            OnSelectCard?.Invoke(InstId);
        }

        #region delegates and events
        public delegate void OnSelectCardHandler(string instId);
        public event OnSelectCardHandler OnSelectCard;
        #endregion
    }
}