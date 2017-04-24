using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; //Used to implement the Object Drag and Drop Interfaces

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Card.Seme thisSeme = Card.Seme.CUORI;
    public Card.Color thisColor = Card.Color.ROSSO;
    public int currentValue = 0;
    public List<GameObject> thisDropZoneList = new List<GameObject>();

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
                if ((thisSeme == card.thisSeme && card.value == currentValue + 1)) //just for checking reasons. change it
                {
                    thisDropZoneList.Add(card.gameObject);

                    //Here get the reference to the previous parent and if it's a tablepile update the value. if it's the discard pile update that
                    if (card.parentToReturnTo.gameObject.tag == "FrontPile")
                    {
                        //Getting references to the flipped and front list of its previous tablePile
                        TablePilesDrop oldPile = card.parentToReturnTo.gameObject.GetComponent<TablePilesDrop>();
                        FlippedTablePiles oldFlippedTablePile = oldPile.gameObject.GetComponentInParent<FlippedTablePiles>();
                        oldPile.thisPileList.Remove(card.gameObject);

                        Transform draggingItemTransform = card.gameObject.transform.parent;
                        card.parentToReturnTo = this.transform; //On drop fires before end drag so I can override Parent to return to
                        Card lastCard = card;
                        

                        currentValue = lastCard.value;
                        //thisColor = lastCard.thisColor;
                        //thisSeme = lastCard.thisSeme;

                        if (oldPile.thisPileList.Count == 0) //If the front list has 0 cards
                        {
                            //Check if the back cards have any cards left
                            if (oldFlippedTablePile.gameObject.transform.childCount > 1)
                            {
                                Card lastFlippedCard = oldFlippedTablePile.thisFlippedPileList[oldFlippedTablePile.thisFlippedPileList.Count - 1].GetComponent<Card>();
                                Debug.Log("Last Flipped Card: " + lastFlippedCard.gameObject);
                                oldPile.currentValue = lastFlippedCard.value;
                                oldPile.thisColor = lastFlippedCard.thisColor;
                                oldPile.thisSeme = lastFlippedCard.thisSeme;

                                oldFlippedTablePile.thisFlippedPileList.Remove(lastFlippedCard.gameObject);
                                oldPile.thisPileList.Add(lastFlippedCard.gameObject);
                                lastFlippedCard.gameObject.transform.SetParent(oldPile.gameObject.transform);
                                StartCoroutine(lastFlippedCard.FlippingBackCardAnimation(lastFlippedCard.gameObject.transform, new Vector3(0, -180, 0), 0.5f));
                            }

                            else
                            {
                                oldPile.currentValue = 14;
                                oldPile.thisColor = Card.Color.NEUTRAL_COLOR;
                                oldPile.thisSeme = Card.Seme.NEUTRAL_SEME;
                            }
                        }
                        //Give the same value of the next card in the list
                        else if (oldPile.thisPileList.Count != 0)
                        {
                            Card nextCardInList = oldPile.thisPileList[oldPile.thisPileList.Count - 1].GetComponent<Card>();
                            oldPile.currentValue = nextCardInList.value;
                            oldPile.thisColor = nextCardInList.thisColor; ;
                            oldPile.thisSeme = nextCardInList.thisSeme;
                        }
                    }

                    else

                    {
                        card.parentToReturnTo = this.transform;
                        currentValue = card.value;

                        //Here Update the Discard Pile
                    }



                }
            }
        }
    }

}
