using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Domain.Cards;

public class Hero : MonoBehaviour,IActiveCard
{
    public int CardId { get; set; }
    private bool _closed = false;
    public string PlayerId { get; set; }
    private Color colorOrigin;
    private Color colorClosing;
    Renderer renderer;
    Text _health;
    #region delegates and events
    public delegate bool OnSelectCardHandler(int _cardId);
    public event OnSelectCardHandler onSelectCard;
    #endregion
    void Awake() {
        _health = transform.GetChild(0).GetComponent<Text>();
        renderer = GetComponent<Renderer>();
        colorOrigin = renderer.material.color;
        colorClosing = new Color32(116, 116, 116, 255);
    }
    public void SetCard(CardInfo heroInfo)
    {
        CardId = heroInfo._id;
        _health.text = heroInfo._health.ToString();
    }
    void OnMouseDown()
    {
        onSelectCard?.Invoke(CardId);
    }
    public void ChangeHealth(int health)
    {
        if (health>0)
            _health.text = health.ToString();
        else
            IsDead();
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
    private void IsDead()
    {
        Destroy(gameObject);
    }
}
