﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Table : MonoBehaviour, IPointerClickHandler {

    public List<GameObject> mazzo = new List<GameObject>();
    public List<int> valoriMazzo = new List<int>();

    public int ListIndex = 0;

    public GameObject pile;

    public List<Sprite> cardValueImageList = new List<Sprite>();

    // Use this for initialization
    void Start ()
    {
        for (int i = 1; i <= 52; ++i)
        {
            valoriMazzo.Add(i);
        }

        ShuffleMazzo();
        
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


}
