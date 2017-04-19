﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; //Used to implement the Object Drag and Drop Interfaces

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Card.Seme thisSeme = Card.Seme.CUORI;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Enter");

        if (eventData.pointerDrag == null)
            return;

        Card card = eventData.pointerDrag.GetComponent<Card>();
        if (card != null)
        {
            if (thisSeme == card.thisSeme) //just for checking reasons. change it
            {
                card.placeholderParent = this.transform; //On drop fires before end drag so I can override Parent to return to
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Exit");
        if (eventData.pointerDrag == null)
            return;

        Card card = eventData.pointerDrag.GetComponent<Card>();
        if (card != null && card.placeholderParent == this.transform)
        {
            if (thisSeme == card.thisSeme) //just for checking reasons. change it
            {
                card.placeholderParent = card.parentToReturnTo; //On drop fires before end drag so I can override Parent to return to
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name + "Placed on: " + gameObject.name);

        Card card = eventData.pointerDrag.GetComponent<Card>();
        if (card != null)
        {
            if (thisSeme == card.thisSeme) //just for checking reasons. change it
            {
                card.parentToReturnTo = this.transform; //On drop fires before end drag so I can override Parent to return to
            }            
        }
    }

}
