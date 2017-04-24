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

        GameObject newCard = (GameObject)Instantiate(Resources.Load(name), pile.transform);
        newCard.GetComponent<Card>().isFaceDown = true;
        newCard.name = numberImageCorrection + "_di_" + semeCarta;

        //Image cardNumberImage = newCard.transform.GetChild(2).GetComponent<Image>();
        Image cardNumberImage = newCard.transform.FindChild("Number").GetComponent<Image>();

        Card cardScript = newCard.GetComponent<Card>();
        cardScript.value = numberImageCorrection;

        cardNumberImage.sprite = gameController.cardValueImageList[numberImageCorrection - 1];
        gameController.mazzo.Add(newCard);
        GameController.ListIndex++;
    }

    //public IEnumerator TranslationToPile(Transform thisTransform, Vector3 startPos, Vector3 endPos, float value, MoveType moveType, int tableNumber)
    //{
    //    float rate = (moveType == MoveType.Time) ? 1.0f / value : 1.0f / Vector3.Distance(startPos, endPos) * value;
    //    float t = 0.0f;
    //    while (t < 1.0)
    //    {
    //        t += Time.deltaTime * rate;
    //        thisTransform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0.0f, 1.0f, t));
    //        yield return null;
    //    }

    //    //Getting a reference to the flipped card object and script
    //    FlippedTablePiles flippedTablePile = TablePiles[tableNumber].gameObject.GetComponent<FlippedTablePiles>();

    //    //Setting the parent to the flipped table object
    //    thisTransform.SetParent(flippedTablePile.gameObject.transform);

    //    //Adding it to the flipped table list structure
    //    flippedTablePile.thisFlippedPileList.Add(thisTransform.gameObject);

    //    //Correcting the order in the children for correct rendering
    //    flippedTablePile.gameObject.transform.GetChild(flippedTablePile.gameObject.transform.childCount - 1).SetSiblingIndex(flippedTablePile.gameObject.transform.childCount - 2);

    //    //Get the last card of that pile and flip it
    //    if (flippedTablePile.gameObject.transform.childCount == tableNumber + 2)
    //    {
    //        //Get reference to last card
    //        Card lastCard = flippedTablePile.thisFlippedPileList[flippedTablePile.thisFlippedPileList.Count - 1].GetComponent<Card>();

    //        //flip it
    //        StartCoroutine(lastCard.FlippingBackCardAnimation(lastCard.gameObject.transform, new Vector3(0, -180, 0), 0.5f));

    //        //Re parent the last card to the front pile
    //        Transform frontPileTransform = TablePiles[tableNumber].transform.GetChild(TablePiles[tableNumber].transform.childCount - 1);
    //        lastCard.gameObject.transform.SetParent(frontPileTransform);

    //        //Update the front and back pile lists and the front pile value
    //        GameObject frontCards = TablePiles[tableNumber].transform.GetChild(TablePiles[tableNumber].transform.childCount - 1).gameObject;
    //        TablePilesDrop thisTablePileDrop = frontCards.GetComponent<TablePilesDrop>();
    //        thisTablePileDrop.thisColor = lastCard.thisColor;
    //        thisTablePileDrop.currentValue = lastCard.value;
    //        thisTablePileDrop.thisSeme = lastCard.thisSeme;
    //        flippedTablePile.thisFlippedPileList.Remove(lastCard.gameObject);
    //        thisTablePileDrop.thisPileList.Add(lastCard.gameObject);
    //    }
    //}
}
