using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; //Used to implement the Object Drag and Drop Interfaces

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //Checking when starting to drag
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("BeginDrag");
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
    }

}
