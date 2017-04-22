using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TablePilesDrop : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{

    public Card.Seme thisSeme = Card.Seme.CUORI;
    public Card.Color thisColor = Card.Color.ROSSO;
    public int currentValue = 0;

    public List<Card> thisPileList = new List<Card>();

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Exit");
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name + "Placed on: " + gameObject.name);

        Card card = eventData.pointerDrag.GetComponent<Card>();
        if (card != null)
        {
            if ((thisColor != card.thisColor && card.value == currentValue - 1)) //just for checking reasons. change it
            {
                card.parentToReturnTo = this.transform; //On drop fires before end drag so I can override Parent to return to

                currentValue = card.value;
                thisColor = card.thisColor;
            }
        }
    }
}
