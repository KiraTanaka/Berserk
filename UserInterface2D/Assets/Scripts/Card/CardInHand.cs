using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Domain.Cards;
using UnityEngine.Networking;

public class CardInHand : NetworkBehaviour
{
    new Renderer renderer;
    public string InstId { get; set; }
    public string PlayerId { get; set; }
    float currentPositionY;
    float selectPositionY = 3;
    string currentLayer = "";
    Vector3 selectScale = new Vector3(1.2f, 1.2f, 1);
    SelectionController selectionCreature;
    #region delegates and events
    public delegate void OnSelectCardHandler(string instId);
    public event OnSelectCardHandler onSelectCard;
    #endregion
    void Awake () {
        renderer = GetComponent<Renderer>();
        currentPositionY = transform.position.y;
        Vector3 selectPosition = new Vector3(transform.position.x, transform.position.y + selectPositionY, transform.position.z);
        selectionCreature = GetComponent<SelectionController>();
        selectionCreature.SetTransformation(new Transformation(selectPosition,selectScale));
    }
    public void SetCard(string instId) => InstId = instId;
    void OnMouseEnter()
    {
        currentLayer = renderer.sortingLayerName;
        renderer.sortingLayerName = "Selected";
    }
    void OnMouseExit() => renderer.sortingLayerName = currentLayer;
    void OnMouseDown() => onSelectCard?.Invoke(InstId);  
    public void DestroyCard()
    {
        selectionCreature.border.SetActive(false);
        Destroy(gameObject);
    }


}
