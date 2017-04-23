﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TablePilesDrop : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{

    public Card.Seme thisSeme = Card.Seme.CUORI;
    public Card.Color thisColor = Card.Color.ROSSO;
    public int currentValue = 0;

    public List<GameObject> thisPileList = new List<GameObject>();

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
                //Here get the reference to the previous parent and if it's a tablepile update the value. if it's the discard pile update that
                if (card.parentToReturnTo.gameObject.tag == "FrontPile")
                {
                    TablePilesDrop oldPile = card.parentToReturnTo.gameObject.GetComponent<TablePilesDrop>();
                    if (oldPile.thisPileList.Count == 1) //If the list only has 1 card (the currently dragged one)
                    {
                        //Check if the back cards have any cards left (MODIFY THIS)
                        if (false)
                        {

                        }

                        else
                        {
                            oldPile.currentValue = 14;
                            oldPile.thisColor = Card.Color.NEUTRAL_COLOR;
                            oldPile.thisSeme = Card.Seme.NEUTRAL_SEME;
                        }
                    }
                    //Give the same value of the next card in the list
                    else
                    {
                        Card nextCardInList = oldPile.thisPileList[oldPile.thisPileList.Count - 2].GetComponent<Card>();
                        oldPile.currentValue = nextCardInList.value;
                        oldPile.thisColor = nextCardInList.thisColor; ;
                        oldPile.thisSeme = nextCardInList.thisSeme;
                    }

                    oldPile.thisPileList.Remove(card.gameObject);
                }

                card.parentToReturnTo = this.transform; //On drop fires before end drag so I can override Parent to return to

                currentValue = card.value;
                thisColor = card.thisColor;
                thisSeme = card.thisSeme;
                thisPileList.Add(card.gameObject);
            }
        }
    }
}
