﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeckController : MonoBehaviour, IPointerClickHandler
{

    public GameObject pile;
    public enum MoveType { Time, Speed }
    public List<GameObject> deckList = new List<GameObject>();

    GameController gameController;
    DiscardPile discardPile;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        discardPile = pile.GetComponent<DiscardPile>();
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
      if (!Card.isFlippingOn && !GameController.isTranslationOn && !Card.isDragging)
      { 
        if (GameController.ListIndex < 52)
        {
          InstantiateCard();
        }

       else if (deckList.Count == 0 && discardPile.discardPileList.Count != 0)
       {
         for (int i = discardPile.discardPileList.Count - 1; i >= 0; --i)
         {
           deckList.Add(discardPile.discardPileList[i]);
           discardPile.discardPileList[i].GetComponent<Card>().FlipCard();
           discardPile.discardPileList[i].transform.SetParent(this.gameObject.transform);
           discardPile.discardPileList.RemoveAt(i);
         }

          this.gameObject.transform.GetChild(0).SetAsLastSibling();
       }

       else if (deckList.Count != 0)
       {
          StartCoroutine(gameController.Translation(deckList[deckList.Count - 1].transform, this.transform.position, pile.transform.position, 150.0f, GameController.MoveType.Speed, 0, false));
          discardPile.discardPileList.Add(deckList[deckList.Count - 1]);
          deckList.Remove(deckList[deckList.Count - 1]);
       }

         Debug.Log("Clicked");
      }
    }

    //Change this not to instantiate but just to flip next card on deck and put on discard pile
    void InstantiateCard()
    {
        string name = "4-Prefabs/CardCuori";
        string semeCarta = "Cuori";

        int numberImageCorrection = gameController.valoriMazzo[GameController.ListIndex];

        if (gameController.valoriMazzo[GameController.ListIndex] <= 13)
        {
            name = "4-Prefabs/CardCuori";
            semeCarta = "Cuori";
        }

        else if (gameController.valoriMazzo[GameController.ListIndex] > 13 && gameController.valoriMazzo[GameController.ListIndex] <= 26)
        {
            name = "4-Prefabs/CardQuadri";
            numberImageCorrection -= 13;
            semeCarta = "Quadri";
        }

        else if (gameController.valoriMazzo[GameController.ListIndex] > 26 && gameController.valoriMazzo[GameController.ListIndex] <= 39)
        {
            name = "4-Prefabs/CardFiori";
            numberImageCorrection -= 26;
            semeCarta = "Fiori";
        }

        else
        {
            name = "4-Prefabs/CardPicche";
            numberImageCorrection -= 39;
            semeCarta = "Picche";
        }

        GameObject newCard = (GameObject)Instantiate(Resources.Load(name), this.transform);
        newCard.GetComponent<Card>().isFaceDown = true;
        newCard.name = numberImageCorrection + "_di_" + semeCarta;

        Image cardNumberImage = newCard.transform.FindChild("Number").GetComponent<Image>();

        Card cardScript = newCard.GetComponent<Card>();
        cardScript.value = numberImageCorrection;

        cardNumberImage.sprite = gameController.cardValueImageList[numberImageCorrection - 1];
        gameController.mazzo.Add(newCard);

        StartCoroutine(gameController.Translation(newCard.transform, this.transform.position, pile.transform.position, 150.0f, GameController.MoveType.Speed, 0, false));
        discardPile.discardPileList.Add(newCard);
        GameController.ListIndex++;
    }
}