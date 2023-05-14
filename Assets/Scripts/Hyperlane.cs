using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Hyperlane : MonoBehaviour
    , IPointerClickHandler
{
    public Color color;
    public bool selected;

    public GameObject[] connectedStars = new GameObject[2];
    public string specialCondition;
    public bool specialConditionKnown;
    public void OnPointerClick(PointerEventData eventData)
    {
        //toggle selection on click
        selected = !selected;
        if (selected)
        {
            //enable textbox
            transform.Find("InfoBox").gameObject.SetActive(true);
            
            //undo turning of the hyperlane for the textbox, so it appears stright up
            transform.Find("InfoBox").localEulerAngles = -transform.localEulerAngles;

            //display connections and any special condition on this hyperlane
            transform.Find("InfoBox").Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text = "Connections:\n" + connectedStars[0].name + ", " + connectedStars[1].name;
            
            //if we have the knowledge about this special condition from a message, then show it; otherwise, say "none"
            if (specialConditionKnown)
            {
                transform.Find("InfoBox").Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text += "\nSpecial Condition:\n" + specialCondition;
            }
            else
            {
                transform.Find("InfoBox").Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text += "\nSpecial Condition:\nNone";
            }
        }
        else
        {
            //hide info box on deselect
            transform.Find("InfoBox").gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //default selected value
        selected = false;

        //default condition values
        specialCondition = "None";
        specialConditionKnown = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Image>().color = color;

        //deselect on right click
        if (selected && Input.GetMouseButtonDown(1))
        {
            selected = false;
            color = new Color32(255, 255, 255, 255);
            transform.Find("InfoBox").gameObject.SetActive(false);
        }

        //set colors according to selection status
        if (!selected)
        {
            color = new Color32(255, 255, 255, 255);
        }
        else
        {
            color = new Color32(204, 135, 8, 255);
        }
    }
}
