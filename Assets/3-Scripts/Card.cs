using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; //Used to implement the Object Drag and Drop Interfaces

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentToReturnTo = null;
    public Transform placeholderParent = null;

    public enum Seme { CUORI, QUADRI, FIORI, PICCHE};
    public enum Color { ROSSO, NERO};

    public Seme thisSeme = Seme.CUORI;
    public Color thisColor = Color.ROSSO;

    GameObject placeholder = null;

    //Checking when starting to drag
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("BeginDrag");

        //Creating a way to keep the position that the card had in the layout

        placeholder = new GameObject();
        placeholder.transform.SetParent(this.transform.parent);

        LayoutElement le = placeholder.AddComponent<LayoutElement>();
        le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;

        //Getting correct size for empty gameobject (placeholder)
        placeholder.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.GetComponent<LayoutElement>().preferredWidth);
        placeholder.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.GetComponent<LayoutElement>().preferredHeight);

        //To get the position of that card in the layout of the card and set the same one for the placeholder
        placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex()); 

        parentToReturnTo = this.transform.parent;
        placeholderParent = parentToReturnTo;
        this.transform.SetParent(this.transform.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;

        // As you start dragging you find all the droppable zones that match (example) oe on pointer enter and exit and check if that is a valid zone or not
       // DropZone[] zones = GameObject.FindObjectsOfType<DropZone>(); 
    }

    //Checking when dragging
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");

        this.transform.position = eventData.position; //Position where the mouse/finger is

        if (placeholder.transform.parent != placeholderParent)
        {
            placeholder.transform.SetParent(placeholderParent);
        }

        int newSiblingIndex = placeholderParent.childCount;

        for (int i = 0; i < placeholderParent.childCount; ++i)
        {
            if (this.transform.position.x < placeholderParent.GetChild(i).position.x)
            {
                newSiblingIndex = i;

                if (placeholder.transform.GetSiblingIndex() < newSiblingIndex)
                {
                    newSiblingIndex--;
                }
                
                break;
            }
        }

        placeholder.transform.SetSiblingIndex(newSiblingIndex);
    }

    //Checking when finish dragging
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("EndDrag");

        this.transform.SetParent(parentToReturnTo);
        this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());

        GetComponent<CanvasGroup>().blocksRaycasts = true;

        //EventSystem.current.RaycastAll(eventData, LIST)  // eventData is the position where it is now the mouse, and then it wants a list of all the objects that will hit so I can use this to check the card that I hit and check if it's a valid place

        Destroy(placeholder);
    }


}
