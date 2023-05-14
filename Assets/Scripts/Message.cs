using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Message : MonoBehaviour
{
    public bool isChoice;
    public bool isPersistent;

    public GameObject weaponsDealerSystem;
    public GameObject weaponsCustomerSystem;

    public bool weaponsPickup;
    public bool weaponsPickupActive;

    public bool weaponsDropoff;

    public int weaponsCargoSpace;
    public int weaponsValue;

    public GameObject relevantHyperlane;
    public string hyperlaneInfo;

    public bool isRead;

    public GameObject messages;
    public GameObject showMessages;
    // Start is called before the first frame update
    void Start()
    {
        showMessages = GameObject.Find("ShowMessages");
        messages = GameObject.Find("Messages");
        //if this is a choice, show deny button; if this is just info, hide deny button
        if (isChoice)
        {
            transform.Find("Deny").gameObject.SetActive(true);
        }
        else if (isPersistent)
        {
            transform.Find("Accept").gameObject.SetActive(false);
            transform.Find("Deny").gameObject.SetActive(false);
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

            GameObject newMessage = Instantiate(gameObject);
            newMessage.GetComponent<Message>().isPersistent = true;
            newMessage.GetComponent<Message>().isChoice = false;
            newMessage.transform.SetParent(messages.transform.Find("Viewport").Find("Content"));

            newMessage.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text = "ACTIVE: Cargo delivery from " + weaponsDealerSystem.name + " to " + weaponsCustomerSystem.name;

            newMessage.name = "DL_" + weaponsDealerSystem.name + "-" + weaponsCustomerSystem.name;

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
                GameObject.Find("MicroCanvas").GetComponent<microController>().addCargo(weaponsCargoSpace, weaponsValue, 10000);

                weaponsCustomerSystem.GetComponent<Star>().message = Instantiate(weaponsCustomerSystem.GetComponent<Star>().msgPrefab);

                weaponsCustomerSystem.GetComponent<Star>().message.GetComponent<Message>().isChoice = false;
                weaponsCustomerSystem.GetComponent<Star>().message.GetComponent<Message>().weaponsPickup = false;
                weaponsCustomerSystem.GetComponent<Star>().message.GetComponent<Message>().weaponsDropoff = true;
                weaponsCustomerSystem.GetComponent<Star>().message.GetComponent<Message>().weaponsPickupActive = false;

                weaponsCustomerSystem.GetComponent<Star>().message.GetComponent<Message>().weaponsDealerSystem = weaponsDealerSystem;
                weaponsCustomerSystem.GetComponent<Star>().message.GetComponent<Message>().weaponsCustomerSystem = weaponsCustomerSystem;

                weaponsCustomerSystem.GetComponent<Star>().message.GetComponent<Message>().weaponsCargoSpace = weaponsCargoSpace;
                weaponsCustomerSystem.GetComponent<Star>().message.GetComponent<Message>().weaponsValue = weaponsValue;
                weaponsCustomerSystem.GetComponent<Star>().message.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text = "Dropped off cargo from " + weaponsDealerSystem.name + ".";

                weaponsCustomerSystem.GetComponent<Star>().message.SetActive(false);
            }

            if (weaponsDropoff)
            {
                
                GameObject.Find("MicroCanvas").GetComponent<microController>().removeCargo(weaponsCargoSpace, weaponsValue, true);

                Destroy(GameObject.Find("DL_" + weaponsDealerSystem.name + "-" + weaponsCustomerSystem.name));
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
