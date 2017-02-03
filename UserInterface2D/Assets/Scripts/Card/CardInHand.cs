using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Domain.Cards;
using UnityEngine.Networking;

public class CardInHand : NetworkBehaviour
{
    new Renderer renderer;
    public int CardId;
    float currentPositionY;
    float selectPositionY = 3;
    string currentLayer = "";
    Vector3 selectScale = new Vector3(1.2f, 1.2f, 1);
    SelectionCreature selectionCreature;
    #region delegates and events
    public delegate void OnSelectCardHandler(int _cardId);
    public event OnSelectCardHandler onSelectCard;
    #endregion
    // Use this for initialization
    void Awake () {
        renderer = GetComponent<Renderer>();
        currentPositionY = transform.position.y;
        Vector3 selectPosition = new Vector3(transform.position.x, transform.position.y + selectPositionY, transform.position.z);
        selectionCreature = GetComponent<SelectionCreature>();
        selectionCreature.SetTransformation(new Transformation(selectPosition,selectScale));
    }
    public void SetCard(int cardId) => CardId = cardId;
    void OnMouseEnter()
    {
        currentLayer = renderer.sortingLayerName;
        renderer.sortingLayerName = "Selected";
    }
    void OnMouseExit() =>renderer.sortingLayerName = currentLayer;
    void OnMouseDown() => onSelectCard?.Invoke(CardId);         
    public void DestroyCard()
    {
        selectionCreature.border.SetActive(false);
        Destroy(gameObject);
    }


}
