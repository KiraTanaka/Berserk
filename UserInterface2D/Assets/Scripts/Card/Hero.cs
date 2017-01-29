using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Domain.Cards;

public class Hero : MonoBehaviour {
    private Card _card;
    Text _health;
    #region delegates and events
    public delegate void OnSelectCardHandler(Card card);
    public event OnSelectCardHandler onSelectCard;
    #endregion
    void Awake() {
        _health = transform.GetChild(0).GetComponent<Text>();
    }
    void Update()
    {
    }
    public void SetCard(Card card)
    {
        _card = card;
        _card.onChangeClosed += onChangeClosed;
        _card.onChangeHealth += onChangeHealth;
        _health.text = _card.Health.ToString();
    }
    void OnMouseDown()
    {
        onSelectCard?.Invoke(_card);
    }
    void onChangeHealth()
    {
        if (_card.IsAlive())
            _health.text = _card.Health.ToString();
        else
            IsDead();
    }
    void onChangeClosed()
    {

    }
    private void IsDead()
    {
        Destroy(gameObject);
    }
}
