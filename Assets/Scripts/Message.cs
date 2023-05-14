using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message : MonoBehaviour
{
    public bool isChoice;

    public GameObject weaponsDealerSystem;
    public GameObject weaponsCustomerSystem;

    public bool weaponsPickup;
    public bool weaponsPickupActive;
    public int weaponsCargoSpace;
    public int weaponsValue;

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
            weaponsDealerSystem.GetComponent<Star>().message.GetComponent<Message>().weaponsPickupActive = true;
            //TODO: implement accepting a quest
            Destroy(gameObject);
        }
        else
        {
            //if the info is about a hyperlane, show that info on that hyperlane's info box
            if (relevantHyperlane != null)
            {
                relevantHyperlane.GetComponent<Hyperlane>().specialConditionKnown = true;
            }

            if (weaponsPickup)
            {
                GameObject.Find("MicroCanvas").GetComponent<microController>().addCargo(weaponsCargoSpace, weaponsValue);
            }
            //destroy this message to not clog up space
            Destroy(gameObject);
        }
    }
    public void Deny()
    {
        if (isChoice)
        {
            Destroy(weaponsDealerSystem.GetComponent<Star>().message);
        }
        Destroy(gameObject);
    }
}
