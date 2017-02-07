using System;
using System.Collections.Generic;
using UnityEngine;
using Domain.Cards;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CardUnity : NetworkBehaviour, IActiveCard
{
    public string InstId { get; set; }
    public string PlayerId { get; set; }
    private bool _closed = false;
    private Text _power;
    private Text _health;
    private GameScript game;
    Vector3 selectScale = new Vector3(24, 24, 1);
    SelectionCreature selectionCreature;
    private Color colorOrigin;
    private Color colorClosing;
    Renderer renderer;
    #region delegates and events
    public delegate bool OnSelectCardHandler(string instId, string playerId);
    public event OnSelectCardHandler onSelectCard;
    #endregion
    void Awake()
    {
        renderer = GetComponent<Renderer>();
        _power = transform.GetChild(0).GetComponent<Text>();
        _health = transform.GetChild(1).GetComponent<Text>();
        selectionCreature = GetComponent<SelectionCreature>();
        selectionCreature.SetTransformation(new Transformation(null, null, selectScale));
        colorOrigin = renderer.material.color;
        colorClosing = new Color32(116,116,116,255);
    }
    void OnMouseDown()
    {
        if ((onSelectCard?.Invoke(InstId,PlayerId)).Value)
            selectionCreature.IsSelected = (selectionCreature.IsSelected) ? false : true;
        
    }
    public void SetCard(CardInfo cardInfo)
    {
        InstId = cardInfo._instId;
        _power.text = cardInfo._power.ToString();
        _health.text = cardInfo._health.ToString();
    }
    public void Close() => SetClose(true);
    public void Open() => SetClose(false);
    public void SetClose(bool value)
    {
        renderer.material.SetColor("_Color", (value) ? colorClosing : colorOrigin);
        _closed = value;
    }
    public bool IsClosed()
    {
        return _closed;
    }
    public void ChangeHealth(int health)
    {
        if (health > 0)
        {
            _health.text = health.ToString();
            selectionCreature.IsSelected = false;
        }
        else
            IsDead();
    }
    private void IsDead()
    {
        GameObject.FindWithTag("BorderActiveCard").SetActive(false);
        Destroy(gameObject);        
    }
}

