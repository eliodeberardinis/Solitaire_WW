using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; //Used to implement the Object Drag and Drop Interfaces

public class DropZone : MonoBehaviour, IDropHandler
{
    //This script is assigned to the drop zones for each seme 
    public Card.Seme thisSeme = Card.Seme.CUORI;
    public Card.Color thisColor = Card.Color.ROSSO;
    public int currentValue = 0;
    public List<GameObject> thisDropZoneList = new List<GameObject>();
    public GameObject winningScreen;
    public Text winningScore;
    GameObject discardPile;
    GameObject[] allDropAreas;
    GameController gameController;

    void Start()
    {
        discardPile = GameObject.FindGameObjectWithTag("DiscardPile");
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        allDropAreas = GameObject.FindGameObjectsWithTag("DropArea");
    }

    //event triggers when a card is dropped on this drop zone
    public void OnDrop(PointerEventData eventData)
    {
        Card card = eventData.pointerDrag.GetComponent<Card>();

        if (card.gameObject.transform.parent.childCount == 1) //If it's just one card you can drop it on the dropping zones
        { 
            if (card != null)
            {
                if ((thisSeme == card.thisSeme && card.value == currentValue + 1))
                {
                    GameController.score += 10;
                    GameController.moves += 1;
                    gameController.ScoreText.text = GameController.score.ToString();
                    gameController.MovesText.text = GameController.moves.ToString();
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
                        discardPile.GetComponent<DiscardPile>().discardPileList.Remove(card.gameObject);
                    }

                    //Check if player has won
                    if (card.value == 13)
                    {
                        int completePiles = 0;
                        for (int i = 0; i < 4; ++i)
                        {
                            DropZone thisDropZone = allDropAreas[i].GetComponent<DropZone>();
                            if (thisDropZone.thisDropZoneList.Count != 13)
                            {
                                return;
                            }
                            else
                            {
                                completePiles++;

                                if (completePiles == 4)
                                {
                                    //Game is Won
                                    winningScreen.SetActive(true);
                                    winningScore.text = GameController.score.ToString();
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    //Similar to on drop but triggered when the player clicks and the card flies directly to the corresponding drop area. A bit repetitive. Can be implemented better without repetition from OnDrop
    public void AutomaticCard(GameObject cardObject)
    {
        Card card = cardObject.GetComponent<Card>();
        if (card.parentToReturnTo.GetChild(card.parentToReturnTo.childCount - 1) == cardObject.transform) //If it's the last card you can drop it if it's the right card
        {
            if (card != null)
            {
                if ((thisSeme == card.thisSeme && card.value == currentValue + 1))
                {
                    GameController.score += 10;
                    GameController.moves += 1;
                    gameController.ScoreText.text = GameController.score.ToString();
                    gameController.MovesText.text = GameController.moves.ToString();
                    thisDropZoneList.Add(card.gameObject);

                    //Here get the reference to the previous parent and if it's a tablepile update the value. if it's the discard pile update that
                    if (card.parentToReturnTo.gameObject.tag == "FrontPile")
                    {
                        TablePilesDrop oldPile = card.parentToReturnTo.gameObject.GetComponent<TablePilesDrop>();
                        FlippedTablePiles oldFlippedTablePile = oldPile.gameObject.GetComponentInParent<FlippedTablePiles>();
                        oldPile.thisPileList.Remove(card.gameObject);

                        card.parentToReturnTo = this.transform; //On drop fires before end drag so I can override Parent to return to
                        Card lastCard = card;
                        currentValue = lastCard.value;

                        card.gameObject.transform.SetParent(this.gameObject.transform.parent.parent);
                        StartCoroutine(gameController.Translation(card.gameObject.transform, card.gameObject.transform.position, new Vector3 ( this.gameObject.transform.position.x, this.gameObject.transform.position.y + 20, this.gameObject.transform.position.z), 0.2f, GameController.MoveType.Time, 0, 2));

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
                                lastFlippedCard.parentToReturnTo = oldPile.gameObject.transform;
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
                    //If it comes from the discard Pile
                    else
                    {
                        card.parentToReturnTo = this.transform;
                        currentValue = card.value;
                        discardPile.GetComponent<DiscardPile>().discardPileList.Remove(card.gameObject);
                        card.gameObject.transform.SetParent(this.gameObject.transform.parent.parent);
                        StartCoroutine(gameController.Translation(card.gameObject.transform, card.gameObject.transform.position, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 20, this.gameObject.transform.position.z), 0.2f, GameController.MoveType.Time, 0, 2));
                    }

                    //Check if player has won
                    if (card.value == 13)
                    {
                        int completePiles = 0;
                        for (int i = 0; i < 4; ++i)
                        {
                            DropZone thisDropZone = allDropAreas[i].GetComponent<DropZone>();
                            if (thisDropZone.thisDropZoneList.Count != 13)
                            {
                                return;
                            }
                            else
                            {
                                completePiles++;
                                if (completePiles == 4)
                                {
                                    //Game is Won
                                    winningScreen.SetActive(true);
                                    winningScore.text = GameController.score.ToString();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
