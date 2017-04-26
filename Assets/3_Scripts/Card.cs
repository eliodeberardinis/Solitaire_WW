using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; //Used to implement the Object Drag and Drop Interfaces

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Transform parentToReturnTo = null;
    public enum Seme { CUORI, QUADRI, FIORI, PICCHE, NEUTRAL_SEME};
    public enum Color { ROSSO, NERO, NEUTRAL_COLOR};
    public int value = 0; //from Ace (1) to King (13)
    public bool isFaceDown = false;
    public static bool isFlippingOn = false;
    public static bool isDragging = false;

    public Seme thisSeme = Seme.CUORI;
    public Color thisColor = Color.ROSSO;

    public Sprite frontImage;
    public Sprite backImage;

    GameObject draggingItem;

    GameObject Canvas;

    //Timers for clicking actions
    float timerClicks = 0.0f;
    int numOfClicks = 0;
    float timerDoubleClick = 0.0f;

    void Start()
    {
        Canvas = GameObject.FindGameObjectWithTag("Canvas");
        draggingItem = GameObject.FindGameObjectWithTag("draggingItem");
    }

    //Checking when starting to drag
    public void OnBeginDrag(PointerEventData eventData)
    {
      if (!Card.isFlippingOn && !GameController.isTranslationOn && !isDragging)
      {
            Debug.Log("BeginDrag");
            parentToReturnTo = this.transform.parent;
            isDragging = true;

            if (Canvas != null)
            {
                //this.transform.SetParent(Canvas.transform);
                draggingItem.transform.position = eventData.position;
                this.transform.SetParent(draggingItem.transform);
            }
            if (parentToReturnTo.gameObject.tag == "FrontPile")
            {
                TablePilesDrop thisCardTablePile = parentToReturnTo.gameObject.GetComponent<TablePilesDrop>();
                if (thisCardTablePile.thisPileList[thisCardTablePile.thisPileList.Count - 1] != this.gameObject)
                {
                    int i = 0;
                    while (thisCardTablePile.thisPileList[i] != this.gameObject)
                    {
                        i++;
                    }

                    for (i += 1; i < thisCardTablePile.thisPileList.Count; ++i)
                    {
                        thisCardTablePile.thisPileList[i].GetComponent<Card>().parentToReturnTo = thisCardTablePile.thisPileList[i].gameObject.transform.parent;
                        thisCardTablePile.thisPileList[i].transform.SetParent(draggingItem.transform);
                        //thisCardTablePile.thisPileList[i].GetComponent<CanvasGroup>().blocksRaycasts = false;
                    }
                }
            }

            GetComponent<CanvasGroup>().blocksRaycasts = false;

            // As you start dragging you find all the droppable zones that match (example) oe on pointer enter and exit and check if that is a valid zone or not
            // DropZone[] zones = GameObject.FindObjectsOfType<DropZone>(); 
         }
      }


    //Checking when dragging
    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            //Debug.Log("Dragging");
            draggingItem.transform.position = eventData.position;
        }     
    }

    //Checking when finish dragging
    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            Debug.Log("EndDrag");
            //this.transform.SetParent(parentToReturnTo);

            if (this.transform.parent.childCount > 1)
            {
                Transform draggingItemTransform = this.transform.parent;
                while (draggingItemTransform.childCount != 0)
                {
                    draggingItemTransform.GetChild(0).SetParent(draggingItemTransform.GetChild(0).GetComponent<Card>().parentToReturnTo);
                }
            }

            else { this.transform.SetParent(parentToReturnTo); }

            GetComponent<CanvasGroup>().blocksRaycasts = true;
            isDragging = false;         
        }
      }


    // TO DO Make Only 1 Function for these two and pass parameters
    public IEnumerator FlippingCardAnimation(Transform thisTransform, Vector3 degrees, float time)
    {
        isFlippingOn = true;
        Quaternion startRotation = thisTransform.rotation;
        Quaternion endRotation = thisTransform.rotation * Quaternion.Euler(degrees);
        float rate = 1.0f / time;
        float t = 0.0f;
        while (t < 1.0f)
        {
            if (isFlippingOn == false) { isFlippingOn = true; } // small hack to correct a bug during game initialization (Player able to move cards around)
            t += Time.deltaTime * rate;
            if (t >= 0.5f && !isFaceDown)
            {
                Image BackgroundImage = this.transform.FindChild("Background").GetComponent<Image>();
                BackgroundImage.sprite = backImage;
                this.transform.FindChild("Background").SetAsLastSibling();
                this.transform.FindChild("Background").localScale = new Vector3(-1, 1, 1);

                isFaceDown = true;
                Debug.Log("This many times called");
            }

            thisTransform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }

        isFlippingOn = false;
    }
    public IEnumerator FlippingBackCardAnimation(Transform thisTransform, Vector3 degrees, float time)
    {
        isFlippingOn = true;
        Quaternion startRotation = thisTransform.rotation;
        Quaternion endRotation = thisTransform.rotation * Quaternion.Euler(degrees);
        float rate = 1.0f / time;
        float t = 0.0f;
        while (t < 1.0f)
        {
            if (isFlippingOn == false) { isFlippingOn = true; } // small hack to correct a bug during game initialization (Player able to move cards around)
            t += Time.deltaTime * rate;
            if (t >= 0.5f && isFaceDown)
            {
                Image BackgroundImage = this.transform.FindChild("Background").GetComponent<Image>();
                BackgroundImage.sprite = frontImage;
                this.transform.FindChild("Background").SetAsFirstSibling();
                this.transform.FindChild("Background").localScale = new Vector3(1, 1, 1);

                isFaceDown = false;
                Debug.Log("This many times called");
            }

            thisTransform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }

        isFlippingOn = false;
    }

   public void FlipCard()
    {
        isFlippingOn = true;
        Image BackgroundImage = this.transform.FindChild("Background").GetComponent<Image>();
        BackgroundImage.sprite = backImage;
        this.transform.Rotate(new Vector3(0, 180, 0));
        this.transform.FindChild("Background").SetAsLastSibling();
        this.transform.FindChild("Background").localScale = new Vector3(-1, 1, 1);
        isFaceDown = true;
        isFlippingOn = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if ((Time.time > timerClicks && Time.time <= timerClicks + 0.5) || (numOfClicks == 1 && Time.time > timerDoubleClick && Time.time <= timerDoubleClick + 0.5))
        {
            Debug.Log("Card CLicked");
            numOfClicks = 0;

            //Here send Card to drop zone if it's correct
            GameObject[] dropZones = GameObject.FindGameObjectsWithTag("DropArea");
            for (int i = 0; i < dropZones.Length; ++i)
            {
                DropZone thisDropZone = dropZones[i].GetComponent<DropZone>();
                if (thisDropZone.thisSeme == thisSeme)
                {
                    Debug.Log("Automatic called");
                    thisDropZone.AutomaticCard(this.gameObject);
                    break;
                }
            }
        }

        if (numOfClicks == 0)
        {
            timerDoubleClick = Time.time;
            numOfClicks++;
        }

        if (numOfClicks == 1 && Time.time > timerDoubleClick + 0.5)
        {
            numOfClicks = 1;
            timerDoubleClick = Time.time;
        }

        Debug.Log("Number of Clicks: " + numOfClicks);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
            //Debug.Log("Enter");
            timerClicks = Time.time; //Used For Touch Devices 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        numOfClicks = 0;
       //Debug.Log("Exit");
    }
}
