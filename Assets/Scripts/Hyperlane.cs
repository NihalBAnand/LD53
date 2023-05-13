using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Hyperlane : MonoBehaviour
    , IPointerClickHandler
{
    Color color;
    bool selected;

    public GameObject[] connectedStars = new GameObject[2];
    public void OnPointerClick(PointerEventData eventData)
    {
        selected = !selected;
        if (selected)
        {
            color = new Color32(204, 135, 8, 255);
            Debug.Log(color);
        }
        else
        {
            color = new Color32(255, 255, 255, 255);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        color = new Color32(255, 255, 255, 255);
        selected = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Image>().color = color;

        if (selected && Input.GetMouseButtonDown(0))
        {
            selected = false;
            color = new Color32(255, 255, 255, 255);
        }
    }
}
