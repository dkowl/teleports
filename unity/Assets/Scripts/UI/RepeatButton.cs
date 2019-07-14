using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RepeatButton : Button, IPointerDownHandler, IPointerUpHandler {

    private bool isHeld;
    PointerEventData eventData;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        isHeld = true;
        this.eventData = eventData;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        isHeld = false;
    }

    void Update()
    {
        if (isHeld)
        {
            OnPointerClick(eventData);
        }
    }

    public bool IsHeld
    {
        get { return isHeld; }
    }

}
