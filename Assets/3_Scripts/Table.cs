﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Table : MonoBehaviour, IPointerClickHandler {

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
        distributeCards();
        //InitializeCardsTable();


    }

    public void distributeCards()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < i + 1; j++)
            {
                InitializeCardsTable(i);
            }
            // turn last card of pile 
            //tablePile[i].lastCard().flip();
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
        newCard.transform.SetParent(this.transform.parent);
        newCard.name = numberImageCorrection + "_di_" + semeCarta;

        //Image cardNumberImage = newCard.transform.GetChild(2).GetComponent<Image>();
        Image cardNumberImage = newCard.transform.FindChild("Number").GetComponent<Image>();

        Card cardScript = newCard.GetComponent<Card>();
        cardScript.value = numberImageCorrection;

        cardNumberImage.sprite = cardValueImageList[numberImageCorrection - 1];
        mazzo.Add(newCard);
        ListIndex++;

        StartCoroutine(Translation(newCard.transform, newCard.transform.position, TablePiles[tableNumber].transform.position, 100.0f, MoveType.Speed, tableNumber));
       
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

        thisTransform.SetParent(TablePiles[tableNumber].transform);
    }



}
