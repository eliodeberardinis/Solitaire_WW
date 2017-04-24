using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour, IPointerClickHandler {

    public List<GameObject> mazzo = new List<GameObject>();
    public List<int> valoriMazzo = new List<int>();

    public List<GameObject> TablePiles = new List<GameObject>();

    public int ListIndex = 0;

    public GameObject pile;

    public List<Sprite> cardValueImageList = new List<Sprite>();

    public enum MoveType { Time, Speed }

    // Use this for initialization
    void Start ()
    {
        for (int i = 1; i <= 52; ++i)
        {
            valoriMazzo.Add(i);
        }

        ShuffleMazzo();

        //For Debug Purposes
        valoriMazzo[0] = 1;
        valoriMazzo[2] = 15 + 13;
        valoriMazzo[5] = 3;
        valoriMazzo[9] = 17 + 13;
        valoriMazzo[14] = 5;
        valoriMazzo[20] = 19 + 13;
        valoriMazzo[27] = 7;

        distributeCards();
    }

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

    // Update is called once per frame
    void Update ()
    {
		
	}

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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (ListIndex < 52)
        { InstantiateCard(); }


        Debug.Log("Clicked");
    }

    //Change this not to instantiate but just to flip next card on deck and put on discard pile
    void InstantiateCard()
    {
        string name = "4-Prefabs/CardCuori";
        string semeCarta = "Cuori";

        int numberImageCorrection = valoriMazzo[ListIndex];

        if (valoriMazzo[ListIndex] <= 13)
        {
            name = "4-Prefabs/CardCuori";
            semeCarta = "Cuori";
        }

        else if (valoriMazzo[ListIndex] > 13 && valoriMazzo[ListIndex] <= 26)
        {
            name = "4-Prefabs/CardQuadri";
            numberImageCorrection -= 13;
            semeCarta = "Quadri";
        }

        else if (valoriMazzo[ListIndex] > 26 && valoriMazzo[ListIndex] <= 39)
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

        cardNumberImage.sprite = cardValueImageList[numberImageCorrection - 1];
        mazzo.Add(newCard);
        ListIndex++;
    }

    void InitializeCardsTable(int tableNumber)
    {
        string name = "4-Prefabs/CardCuori";
        string semeCarta = "Cuori";

        int numberImageCorrection = valoriMazzo[ListIndex];

        if (valoriMazzo[ListIndex] <= 13)
        {
            name = "4-Prefabs/CardCuori";
            semeCarta = "Cuori";
        }

        else if (valoriMazzo[ListIndex] > 13 && valoriMazzo[ListIndex] <= 26)
        {
            name = "4-Prefabs/CardQuadri";
            numberImageCorrection -= 13;
            semeCarta = "Quadri";
        }

        else if (valoriMazzo[ListIndex] > 26 && valoriMazzo[ListIndex] <= 39)
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

        GameObject newCard = (GameObject)Instantiate(Resources.Load(name), this.transform.position, Quaternion.Euler(new Vector3(0,180,0)));

        newCard.GetComponent<Card>().isFaceDown = true;
        newCard.transform.SetParent(this.transform.parent);
        newCard.name = numberImageCorrection + "_di_" + semeCarta;
        this.transform.parent.FindChild(newCard.name).SetAsFirstSibling();

        //Image cardNumberImage = newCard.transform.GetChild(2).GetComponent<Image>();
        Image cardNumberImage = newCard.transform.FindChild("Number").GetComponent<Image>();

        Card cardScript = newCard.GetComponent<Card>();
        cardScript.value = numberImageCorrection;

        cardNumberImage.sprite = cardValueImageList[numberImageCorrection - 1];
        mazzo.Add(newCard);
        ListIndex++;

        StartCoroutine(Translation(newCard.transform, newCard.transform.position, new Vector3(TablePiles[tableNumber].transform.position.x, TablePiles[tableNumber].transform.position.y + 20, TablePiles[tableNumber].transform.position.z) , 100.0f, MoveType.Speed, tableNumber));
       
    }


    public IEnumerator Translation(Transform thisTransform, Vector3 startPos, Vector3 endPos, float value, MoveType moveType, int tableNumber)
    {
        float rate = (moveType == MoveType.Time) ? 1.0f / value : 1.0f / Vector3.Distance(startPos, endPos) * value;
        float t = 0.0f;
        while (t < 1.0)
        {
            t += Time.deltaTime * rate;
            thisTransform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0.0f, 1.0f, t));
            yield return null;
        }

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




}
