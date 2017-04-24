using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeckController : MonoBehaviour, IPointerClickHandler
{

    public GameObject pile;
    public enum MoveType { Time, Speed }

    GameController gameController;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameController.ListIndex < 52)
        { InstantiateCard(); }

        Debug.Log("Clicked");
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
        GameController.ListIndex++;
    }
}
