using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message : MonoBehaviour
{
    public bool isChoice;

    public GameObject weaponsDealer;
    public GameObject weaponsCustomer;

    public GameObject relevantHyperlane;
    public string hyperlaneInfo;

    public bool isRead;
    // Start is called before the first frame update
    void Start()
    {
        //if this is a choice, show deny button; if this is just info, hide deny button
        if (isChoice)
        {
            transform.Find("Deny").gameObject.SetActive(true);
        }
        else
        {
            transform.Find("Deny").gameObject.SetActive(false);
        }
        //set scale to identity - for some reason, it showed up at a smaller scale for no reason when instantiated
        transform.localScale = new Vector3(1, 1, 1);
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Accept()
    {
        if (isChoice)
        {
            //TODO: implement accepting a quest
            Destroy(gameObject);
        }
        else
        {
            //if the info is about a hyperlane, show that info on that hyperlane's info box
            if (relevantHyperlane != null)
            {
                relevantHyperlane.GetComponent<Hyperlane>().specialConditionKnown = true;
                relevantHyperlane.GetComponent<Hyperlane>().specialCondition = hyperlaneInfo;
            }
            //destroy this message to not clog up space
            Destroy(gameObject);
        }
    }
    public void Deny()
    {
        Destroy(gameObject);
    }
}
