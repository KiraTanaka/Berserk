using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Domain.Cards;

public class CardInHand : MonoBehaviour {
    Renderer renderer;
    private Card _card;
    float currentPositionY;
    float selectPositionY = 3;
    string currentLayer = "";
    Vector3 selectScale = new Vector3(1.2f, 1.2f, 1);
    #region delegates and events
    public delegate bool OnSelectCardHandler(Card card);
    public event OnSelectCardHandler onSelectCard;
    #endregion
    // Use this for initialization
    void Awake () {
        renderer = GetComponent<Renderer>();
        currentPositionY = transform.position.y;
        Vector3 selectPosition = new Vector3(transform.position.x, transform.position.y + selectPositionY, transform.position.z);
        GetComponent<SelectionCreature>().SetTransformation(new Transformation(selectPosition,selectScale));
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnMouseEnter()
    {
        currentLayer = renderer.sortingLayerName;
        renderer.sortingLayerName = "Selected";
    }
    void OnMouseExit()
    {
        renderer.sortingLayerName = currentLayer;
    }
    void OnMouseDown()
    {
        if ((onSelectCard?.Invoke(_card)).Value)
            Destroy(gameObject);
    }
    public void SetCard(Card card)
    {
        _card = card;
    }
}
