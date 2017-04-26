using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeckController : MonoBehaviour, IPointerClickHandler
{

    public GameObject pile;
    public enum MoveType { Time, Speed }
    public List<GameObject> deckList = new List<GameObject>();
    public Image deckImage;

    //Script to control the deck
    GameController gameController;
    DiscardPile discardPile;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        discardPile = pile.GetComponent<DiscardPile>();      
    }

    //When clicking the deck you instantate a new card or flip the discard pile back
    public void OnPointerClick(PointerEventData eventData)
    {
      if (deckList.Count == 1)
      {
            Debug.Log("Color change");
            deckImage.color = new Color(deckImage.color.r, deckImage.color.g, deckImage.color.b, 0.5f);
      }

      if (!Card.isFlippingOn && !GameController.isTranslationOn && !Card.isDragging)
      { 
        //TO DO Instantiate all cards at the beginning and assign them to deck instead of doing this
        if (GameController.ListIndex < 52)
        {
          InstantiateCard();
          GameController.moves += 1;
          gameController.MovesText.text = GameController.moves.ToString();
          if (GameController.ListIndex == 52)
          {
             Debug.Log("Color change");
             deckImage.color = new Color(deckImage.color.r, deckImage.color.g, deckImage.color.b, 0.5f);
          }
       }
       //Flip the discard pile back into the deck
       else if (deckList.Count == 0 && discardPile.discardPileList.Count != 0)
       {
         for (int i = discardPile.discardPileList.Count - 1; i >= 0; --i)
         {
           deckList.Add(discardPile.discardPileList[i]);
           discardPile.discardPileList[i].GetComponent<Card>().FlipCard();
           discardPile.discardPileList[i].transform.SetParent(this.gameObject.transform);
           discardPile.discardPileList.RemoveAt(i);
         }

          deckImage.color = new Color(deckImage.color.r, deckImage.color.g, deckImage.color.b, 1.0f);
          this.gameObject.transform.GetChild(0).SetAsLastSibling();
          GameController.score = (int)GameController.score/2;
          GameController.moves += 1;
          gameController.ScoreText.text = GameController.score.ToString();
          gameController.MovesText.text = GameController.moves.ToString();
       }

       //All cards instantiated, just flip each one at a time back into the discard pile
       else if (deckList.Count != 0)
       {
          StartCoroutine(gameController.Translation(deckList[deckList.Count - 1].transform, this.transform.position, pile.transform.position, 0.2f, GameController.MoveType.Time, 0, 1));
          discardPile.discardPileList.Add(deckList[deckList.Count - 1]);
          deckList.Remove(deckList[deckList.Count - 1]);
          GameController.moves += 1;
          gameController.MovesText.text = GameController.moves.ToString();
       }
      }
    }

    //Change this not to instantiate but just to flip next card on deck and put on discard pile (Repetition of gamecontroller function, redundant after refactoring)
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

        //Initialize the card and trigger correct animations
        GameObject newCard = (GameObject)Instantiate(Resources.Load(name), this.transform, false);
        newCard.GetComponent<Card>().isFaceDown = true;
        newCard.name = numberImageCorrection + "_di_" + semeCarta;
        Image cardNumberImage = newCard.transform.FindChild("Number").GetComponent<Image>();
        Card cardScript = newCard.GetComponent<Card>();
        cardScript.value = numberImageCorrection;
        cardNumberImage.sprite = gameController.cardValueImageList[numberImageCorrection - 1];
        gameController.mazzo.Add(newCard);
        StartCoroutine(gameController.Translation(newCard.transform, this.transform.position, pile.transform.position, 0.2f, GameController.MoveType.Time, 0, 1));
        discardPile.discardPileList.Add(newCard);
        GameController.ListIndex++;
    }
}
