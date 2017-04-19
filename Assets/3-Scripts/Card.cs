using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; //Used to implement the Object Drag and Drop Interfaces

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentToReturnTo = null;
    public enum Seme { CUORI, QUADRI, FIORI, PICCHE};
    public enum Color { ROSSO, NERO};

    public Seme thisSeme = Seme.CUORI;
    public Color thisColor = Color.ROSSO;

    //Checking when starting to drag
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("BeginDrag");

        parentToReturnTo = this.transform.parent;
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
