using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIScaler : MonoBehaviour
    , IDragHandler
{
    public float scrollSpeed;

    Vector2 lastMousePos;
    
    public void OnDrag(PointerEventData eventData)
    {
        if (Vector3.Magnitude(eventData.position - lastMousePos) < 100)
        {
            transform.position += new Vector3(eventData.position.x - lastMousePos.x, eventData.position.y - lastMousePos.y);
        }
        lastMousePos = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        lastMousePos = eventData.position;
    }

    private void OnGUI()
    {
        transform.localScale += new Vector3(Input.mouseScrollDelta.y * scrollSpeed, Input.mouseScrollDelta.y * scrollSpeed, 0);
        if (transform.localScale.x < 0.1)
            transform.localScale = new Vector3(0.1f, 0.1f);
    }
}
