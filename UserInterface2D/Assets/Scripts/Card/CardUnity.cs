using System;
using System.Collections.Generic;
using UnityEngine;
using Domain.Cards;
using UnityEngine.UI;

public class CardUnity : MonoBehaviour
{
    private Card _card;
    private Text _power;
    private Text _health;
    private GameScript game;
    Vector3 selectScale = new Vector3(24, 24, 1);
    SelectionCreature selectionCreature;
    Shader shaderOrigin;
    Shader shaderClosing;
    Renderer renderer;
    #region delegates and events
    public delegate void OnSelectCardHandler(Card card);
    public event OnSelectCardHandler onSelectCard;
    #endregion
    void Awake()
    {
        renderer = GetComponent<Renderer>();
        _power = transform.GetChild(0).GetComponent<Text>();
        _health = transform.GetChild(1).GetComponent<Text>();
        game = GameObject.FindWithTag("Scripts").GetComponent<GameScript>();
        selectionCreature = GetComponent<SelectionCreature>();
        selectionCreature.SetTransformation(new Transformation(null, null, selectScale));
        shaderOrigin = renderer.material.shader;
        shaderClosing = Shader.Find("Mobile/Particles/Multiply");
    }
    void OnMouseDown()
    {
        selectionCreature.IsSelected = (selectionCreature.IsSelected) ? false : true;
        onSelectCard?.Invoke(_card);
    }
    public void SetCard(Card card)
    {
        _card = card;
        _card.onChangeClosed += onChangeClosed;
        _card.onChangeHealth += onChangeHealth;
        _power.text = card.Power.ToString();
        _health.text = card.Health.ToString();
    }
    void onChangeClosed()
    {
        renderer.material.shader = (_card.Closed) ? shaderClosing : shaderOrigin;
    }
    void onChangeHealth()
    {
        if (_card.IsAlive())
            _health.text = _card.Health.ToString();
        else
            IsDead();
    }
    private void IsDead()
    {
        GameObject.FindWithTag("BorderActiveCard").SetActive(false);
        Destroy(gameObject);        
    }
    public void onAfterMove()
    {
        selectionCreature.IsSelected =  false;
    }
}

