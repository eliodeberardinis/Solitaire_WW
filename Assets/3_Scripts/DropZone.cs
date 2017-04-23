using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; //Used to implement the Object Drag and Drop Interfaces

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Card.Seme thisSeme = Card.Seme.CUORI;
    public Card.Color thisColor = Card.Color.ROSSO;
    public int currentValue = 0;

    public bool isTestZone = false;

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

        if (card.gameObject.transform.parent.childCount == 1) //If it's just one card you can drop it on the dropping zones
        { 
            if (card != null)
            {
                if ((thisSeme == card.thisSeme && card.value == currentValue + 1) || isTestZone) //just for checking reasons. change it
                {
                    card.parentToReturnTo = this.transform; //On drop fires before end drag so I can override Parent to return to

                    if (!isTestZone)
                    { currentValue = card.value; }
                }
            }
        }
    }

}
