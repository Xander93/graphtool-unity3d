using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{

    public List<GameObject> goList = new List<GameObject>();

    void Start()
    {
        foreach (Transform child in transform)
        {
            goList.Add(child.gameObject);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        if (eventData.pointerDrag == null)
            return;

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null)
        {
            d.placeHolderParent = this.transform;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        if (eventData.pointerDrag == null)
            return;

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null && d.placeHolderParent == this.transform)
        {
            d.placeHolderParent = d.parentToReturnTo;
        }
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null)
        {
            goList.Add(d.gameObject);
            d.parentToReturnTo.GetComponent<DropZone>().goList.Remove(d.gameObject);
            d.parentToReturnTo = this.transform;
        }
    }
}
