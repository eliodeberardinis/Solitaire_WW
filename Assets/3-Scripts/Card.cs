using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; //Used to implement the Object Drag and Drop Interfaces

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentToReturnTo = null;
    public enum Seme { CUORI, QUADRI, FIORI, PICCHE};
    public enum Color { ROSSO, NERO};
    public int value = 0; //from Ace (1) to King (13)
    public bool isFaceDown = false;

    public Seme thisSeme = Seme.CUORI;
    public Color thisColor = Color.ROSSO;

    public Sprite frontImage;
    public Sprite backImage;

    void Start()
    {
        //Refactor and add to a "Flipping" function
        if (isFaceDown)
        {
            Image BackgroundImage = this.transform.FindChild("Background").GetComponent<Image>();
            BackgroundImage.sprite = backImage;
            this.transform.Rotate(new Vector3(0,180,0));
            this.transform.FindChild("Background").SetAsLastSibling();
            this.transform.FindChild("Background").localScale = new Vector3(-1,1,1);

        }
    }

    //Checking when starting to drag
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("BeginDrag");

        parentToReturnTo = this.transform.parent;

        if (this.transform.parent.parent != null)
        { this.transform.SetParent(this.transform.parent.parent); }

        GetComponent<CanvasGroup>().blocksRaycasts = false;

        // As you start dragging you find all the droppable zones that match (example) oe on pointer enter and exit and check if that is a valid zone or not
       // DropZone[] zones = GameObject.FindObjectsOfType<DropZone>(); 
    }

    //Checking when dragging
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");

        this.transform.position = eventData.position; //Position where the mouse/finger is
    }

    //Checking when finish dragging
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("EndDrag");

        this.transform.SetParent(parentToReturnTo);
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        //EventSystem.current.RaycastAll(eventData, LIST)  // eventData is the position where it is now the mouse, and then it wants a list of all the objects that will hit so I can use this to check the card that I hit and check if it's a valid place
    }

}
