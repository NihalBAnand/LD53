using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    public GameObject currentLocation;

    public GameObject messages;
    public GameObject message;
    public GameObject showMessages;
    public float minTimeBetweenUpdates;
    public float maxTimeBetweenUpdates;
    public float minTimeBetweenJobs;
    public float maxTimeBetweenJobs;

    public bool macroEnabled;

    bool initedMessages;
    bool generatedJob;
    // Start is called before the first frame update
    void Start()
    {
        macroEnabled = false;
        initedMessages = false;
        generatedJob = false;
    }

    // Update is called once per frame
    void Update()
    {

        

        if (!initedMessages)
        {
            initedMessages = true;
            
            StartCoroutine(DeliverMessages());
            StartCoroutine(GenerateJobs());
        }

        if (currentLocation.GetComponent<Star>().message != null)
        {
            if (!currentLocation.GetComponent<Star>().message.GetComponent<Message>().weaponsPickup || currentLocation.GetComponent<Star>().message.GetComponent<Message>().weaponsPickupActive)
            {
                currentLocation.GetComponent<Star>().message.SetActive(true);
                currentLocation.GetComponent<Star>().message.transform.SetParent(messages.transform.Find("Viewport").Find("Content"));

                showMessages.GetComponent<ShowMessages>().unreadMessages.Add(currentLocation.GetComponent<Star>().message);
                currentLocation.GetComponent<Star>().message.GetComponent<Message>().isRead = false;
                currentLocation.GetComponent<Star>().message = null;
            }
        }

        if (macroEnabled)
        {
            GameObject.Find("MacroCanvas").GetComponent<Canvas>().enabled = true;
            GameObject.Find("MicroCanvas").GetComponent<Canvas>().enabled = false;
        }
        else
        {
            GameObject.Find("MacroCanvas").GetComponent<Canvas>().enabled = false;
            GameObject.Find("MicroCanvas").GetComponent<Canvas>().enabled = true;
        }
    }

    public void ToggleMicroMacro()
    {
        macroEnabled = !macroEnabled;
    }

    IEnumerator DeliverMessages()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minTimeBetweenUpdates, maxTimeBetweenUpdates));
            //TODO: Implement failure to receive but effect still happens
            GameObject newMessage = Instantiate(message);
            int roll = Random.Range(0, 100);
            //Asteroid field - causes ship to be damaged if it goes through
            if (roll < 50)
            {
                //TODO: Implement chance to clear asteroid field

                //it's info and an asteroid field
                newMessage.GetComponent<Message>().isChoice = false;
                newMessage.GetComponent<Message>().hyperlaneInfo = "Asteroid field";

                //select our stars
                GameObject affectedStar = GameObject.FindGameObjectsWithTag("Star")[Random.Range(0, GameObject.FindGameObjectsWithTag("Star").Length)];
                int selectedStar = Random.Range(0, affectedStar.GetComponent<Star>().realConnectedStars.Count);
                string targetStar = affectedStar.GetComponent<Star>().realConnectedStars.ElementAt(selectedStar).name;

                //find relevant hyperlane
                GameObject hyperlane = GameObject.Find("HL_" + affectedStar.name + "-" + targetStar);
                //swap name if it was generated the other way
                if (hyperlane == null)
                {
                    hyperlane = GameObject.Find("HL_" + targetStar + "-" + affectedStar.name);
                }
                //prevent duplicate messages
                if (hyperlane.GetComponent<Hyperlane>().specialCondition != "None")
                {
                    newMessage.GetComponent<Message>().relevantHyperlane = hyperlane;
                    newMessage.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text = "Asteroid field from " + affectedStar.name + " to " + targetStar + "!";
                    newMessage.GetComponent<Message>().relevantHyperlane.GetComponent<Hyperlane>().specialCondition = newMessage.GetComponent<Message>().hyperlaneInfo;
                    newMessage.transform.SetParent(messages.transform.Find("Viewport").Find("Content"));

                    showMessages.GetComponent<ShowMessages>().unreadMessages.Add(newMessage);
                    newMessage.GetComponent<Message>().isRead = false;
                }
                else
                {
                    Destroy(newMessage);
                }
            }
        }
    }

    IEnumerator GenerateJobs()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minTimeBetweenJobs, maxTimeBetweenJobs));
            if (!generatedJob)
            {
                generatedJob = true;

                //determine space and value of cargo
                int cargoSpace = Random.Range(10, 50);
                int value = Random.Range(500, 2000);

                GameObject messageStar = GameObject.FindGameObjectsWithTag("Star")[Random.Range(0, GameObject.FindGameObjectsWithTag("Star").Length)];

                messageStar.GetComponent<Star>().message = Instantiate(messageStar.GetComponent<Star>().msgPrefab);

                messageStar.GetComponent<Star>().message.GetComponent<Message>().isChoice = true;
                messageStar.GetComponent<Star>().message.GetComponent<Message>().weaponsCustomerSystem = messageStar;
                GameObject dealer = messageStar;
                while (dealer == messageStar)
                    dealer = GameObject.FindGameObjectsWithTag("Star")[Random.Range(0, GameObject.FindGameObjectsWithTag("Star").Length)];
                messageStar.GetComponent<Star>().message.GetComponent<Message>().weaponsDealerSystem = dealer;
                messageStar.GetComponent<Star>().message.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text = "Transport cargo from " + dealer.name + " to " + messageStar.name + ". Cargo takes up " + cargoSpace.ToString() + " litres and is worth " + value.ToString();

                messageStar.GetComponent<Star>().message.SetActive(false);

                dealer.GetComponent<Star>().message = Instantiate(dealer.GetComponent<Star>().msgPrefab);

                dealer.GetComponent<Star>().message.GetComponent<Message>().isChoice = false;
                dealer.GetComponent<Star>().message.GetComponent<Message>().weaponsPickup = true;
                dealer.GetComponent<Star>().message.GetComponent<Message>().weaponsPickupActive = false;

                dealer.GetComponent<Star>().message.GetComponent<Message>().weaponsCustomerSystem = messageStar;
                dealer.GetComponent<Star>().message.GetComponent<Message>().weaponsDealerSystem = dealer;

                dealer.GetComponent<Star>().message.GetComponent<Message>().weaponsCargoSpace = cargoSpace;
                dealer.GetComponent<Star>().message.GetComponent<Message>().weaponsValue = value;
                dealer.GetComponent<Star>().message.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text = "Received cargo bound for " + messageStar.name + ".";

                dealer.GetComponent<Star>().message.SetActive(false);
            }
        }
    }
}
