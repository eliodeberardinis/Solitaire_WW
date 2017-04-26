using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour {

    //Static Variables to control animations and detect global parameters for game state
    public static int ListIndex = 0;
    public static int score = 0;
    public static int moves = 0;
    public static bool isTranslationOn = false;

    //Public Objects directly referenced in the editor
    public List<GameObject> mazzo = new List<GameObject>(); //Used in Debug to check no repeats of cards (Not used throuout the game, will be removed later)
    public List<int> valoriMazzo = new List<int>();
    public List<GameObject> TablePiles = new List<GameObject>();
    public List<GameObject> DropAreas = new List<GameObject>();
    public List<Sprite> cardValueImageList = new List<Sprite>();
    public GameObject Deck;
    public GameObject discardPile;
    public Text ScoreText;
    public Text MovesText;

    public enum MoveType { Time, Speed }

    // Here I initialize the deck and the table
    void Start ()
    {
        for (int i = 1; i <= 52; ++i)
        {
            valoriMazzo.Add(i);
        }
        ShuffleMazzo();
        distributeCards();
    }

    //Here I shuffle the deck of ints
    void ShuffleMazzo()
    {
        for (int i = 0; i < valoriMazzo.Count; i++)
        {
            int temp = valoriMazzo[i];
            int randomIndex = Random.Range(i, valoriMazzo.Count);
            valoriMazzo[i] = valoriMazzo[randomIndex];
            valoriMazzo[randomIndex] = temp;
        }
    }

    //The cards are distributed on the table
    public void distributeCards()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < i + 1; j++)
            {
                InitializeCardsTable(i);
            }
        }   
    }

    //Here I create the cards during initialization and send them to the correct table
    void InitializeCardsTable(int tableNumber)
    {
        string name = "4-Prefabs/CardCuori";
        string semeCarta = "Cuori";
        int numberImageCorrection = valoriMazzo[GameController.ListIndex];

        if (valoriMazzo[GameController.ListIndex] <= 13)
        {
            name = "4-Prefabs/CardCuori";
            semeCarta = "Cuori";
        }
        else if (valoriMazzo[GameController.ListIndex] > 13 && valoriMazzo[GameController.ListIndex] <= 26)
        {
            name = "4-Prefabs/CardQuadri";
            numberImageCorrection -= 13;
            semeCarta = "Quadri";
        }
        else if (valoriMazzo[GameController.ListIndex] > 26 && valoriMazzo[GameController.ListIndex] <= 39)
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

        //Card prefab initialization and correct orientation and rotation
        GameObject newCard = (GameObject)Instantiate(Resources.Load(name), this.transform.position, Quaternion.Euler(new Vector3(0,180,0)), this.transform);
        newCard.GetComponent<Card>().isFaceDown = true;
        newCard.name = numberImageCorrection + "_di_" + semeCarta;
        Image cardNumberImage = newCard.transform.FindChild("Number").GetComponent<Image>();
        Card cardScript = newCard.GetComponent<Card>();
        cardScript.value = numberImageCorrection;
        cardNumberImage.sprite = cardValueImageList[numberImageCorrection - 1];
        mazzo.Add(newCard);
        GameController.ListIndex++;
        StartCoroutine(Translation(newCard.transform, newCard.transform.position, new Vector3(TablePiles[tableNumber].transform.position.x, TablePiles[tableNumber].transform.position.y + 20, TablePiles[tableNumber].transform.position.z) , 650.0f, MoveType.Speed, tableNumber, 0));      
    }

    //Translation coroutine for the translation animation. Can be called by 3 different translation types: from deck to table, from deck to discard pile and from tables to the drop zones 
    public IEnumerator Translation(Transform thisTransform, Vector3 startPos, Vector3 endPos, float value, MoveType moveType, int tableNumber, int translationType)
    {
        isTranslationOn = true;
        float rate = (moveType == MoveType.Time) ? 1.0f / value : 1.0f / Vector3.Distance(startPos, endPos) * value;
        float t = 0.0f;
        while (t < 1.0)
        {
            t += Time.deltaTime * rate;
            thisTransform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0.0f, 1.0f, t));
            yield return null;
        }
        //The initial translation of the cards to the tables
        if (translationType == 0)
        {
            //Getting a reference to the flipped card object and script
            FlippedTablePiles flippedTablePile = TablePiles[tableNumber].gameObject.GetComponent<FlippedTablePiles>();

            //Setting the parent to the flipped table object
            thisTransform.SetParent(flippedTablePile.gameObject.transform);

            //Adding it to the flipped table list structure
            flippedTablePile.thisFlippedPileList.Add(thisTransform.gameObject);

            //Correcting the order in the children for correct rendering
            flippedTablePile.gameObject.transform.GetChild(flippedTablePile.gameObject.transform.childCount - 1).SetSiblingIndex(flippedTablePile.gameObject.transform.childCount - 2);

            //Get the last card of that pile and flip it
            if (flippedTablePile.gameObject.transform.childCount == tableNumber + 2)
            {
                //Get reference to last card
                Card lastCard = flippedTablePile.thisFlippedPileList[flippedTablePile.thisFlippedPileList.Count - 1].GetComponent<Card>();

                //flip it
                StartCoroutine(lastCard.FlippingBackCardAnimation(lastCard.gameObject.transform, new Vector3(0, -180, 0), 0.5f));

                //Re parent the last card to the front pile
                Transform frontPileTransform = TablePiles[tableNumber].transform.GetChild(TablePiles[tableNumber].transform.childCount - 1);
                lastCard.gameObject.transform.SetParent(frontPileTransform);
                lastCard.parentToReturnTo = frontPileTransform;

                //Update the front and back pile lists and the front pile value
                GameObject frontCards = TablePiles[tableNumber].transform.GetChild(TablePiles[tableNumber].transform.childCount - 1).gameObject;
                TablePilesDrop thisTablePileDrop = frontCards.GetComponent<TablePilesDrop>();
                thisTablePileDrop.thisColor = lastCard.thisColor;
                thisTablePileDrop.currentValue = lastCard.value;
                thisTablePileDrop.thisSeme = lastCard.thisSeme;
                flippedTablePile.thisFlippedPileList.Remove(lastCard.gameObject);
                thisTablePileDrop.thisPileList.Add(lastCard.gameObject);
            }
        }
        //The translation from the deck to the discard pile
        else if (translationType == 1)
        {
            Card thisCard = thisTransform.gameObject.GetComponent<Card>();
            StartCoroutine(thisCard.FlippingBackCardAnimation(thisTransform, new Vector3(0, -180, 0), 0.5f));
            thisTransform.SetParent(discardPile.transform);
            thisCard.parentToReturnTo = discardPile.transform;
        }
        //The translation from discard pile or table to drop areas
        else if (translationType == 2)
        {
            for (int i = 0; i < DropAreas.Count; ++i)
            {
                DropZone thisDropZone = DropAreas[i].GetComponent<DropZone>();
                if (thisDropZone.thisSeme == thisTransform.gameObject.GetComponent<Card>().thisSeme)
                {
                    thisTransform.SetParent(thisDropZone.gameObject.transform);
                    break;
                }
            }
        }

        isTranslationOn = false;
     }
}
