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
    
    public bool flying;
    // Start is called before the first frame update
    void Start()
    {
        macroEnabled = false;
        initedMessages = false;
        flying = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            GenerateMessage();
        }
        

        if (!initedMessages)
        {
            initedMessages = true;
            
            StartCoroutine(DeliverMessages());
            StartCoroutine(GenerateJobs());
        }

        if (currentLocation != null && currentLocation.GetComponent<Star>().message != null)
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
            foreach (GameObject box in GameObject.FindGameObjectsWithTag("InfoBox"))
            {
                box.GetComponent<Canvas>().enabled = true;
            }
        }
        else
        {
            GameObject.Find("MacroCanvas").GetComponent<Canvas>().enabled = false;
            GameObject.Find("MicroCanvas").GetComponent<Canvas>().enabled = true;
            foreach (GameObject box in GameObject.FindGameObjectsWithTag("InfoBox"))
            {
                box.GetComponent<Canvas>().enabled = false;
            }
        }

        if (currentLocation != null && !currentLocation.GetComponent<Star>().hasShop && GameObject.Find("Shop") != null)
        {
            GameObject.Find("Shop").SetActive(false);
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

            GenerateMessage();
        }
    }

    void GenerateMessage()
    {
        //TODO: Implement failure to receive but effect still happens
        GameObject newMessage = Instantiate(message);
        int roll = Random.Range(0, 100);
        //receive message is random
        bool received = Mathf.RoundToInt(Random.Range(0, 100)) <= GameObject.Find("MicroCanvas").GetComponent<microController>().components["Comms"];
        //Asteroid field - causes ship to be damaged if it goes through
        if (roll < 50)
        {
            //TODO: Implement chance to clear asteroid field

            //it's info and an asteroid field
            newMessage.GetComponent<Message>().isChoice = false;
            newMessage.GetComponent<Message>().hyperlaneInfo = "Asteroid Field";

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
            if (hyperlane.GetComponent<Hyperlane>().specialCondition == "None")
            {
                hyperlane.GetComponent<Hyperlane>().specialCondition = newMessage.GetComponent<Message>().hyperlaneInfo;

                if (received)
                {
                    newMessage.GetComponent<Message>().relevantHyperlane = hyperlane;
                    newMessage.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text = "Asteroid field from " + affectedStar.name + " to " + targetStar + "!";
                    newMessage.transform.SetParent(messages.transform.Find("Viewport").Find("Content"));

                    showMessages.GetComponent<ShowMessages>().unreadMessages.Add(newMessage);
                    newMessage.GetComponent<Message>().isRead = false;
                }
                else
                {
                    Debug.Log("Dropped a message!");
                }
            }
            else
            {
                Destroy(newMessage);
            }
        }
    }

    IEnumerator GenerateJobs()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minTimeBetweenJobs, maxTimeBetweenJobs));

            bool failed = false;

            //determine space and value of cargo
            int cargoSpace = Random.Range(10, 50);
            int value = Random.Range(500, 2000);

            //determine customer system
            GameObject messageStar = GameObject.FindGameObjectsWithTag("Star")[Random.Range(0, GameObject.FindGameObjectsWithTag("Star").Length)];
            int maxTries = GameObject.FindGameObjectsWithTag("Star").Length;
            int tries = 0;
            while (messageStar.GetComponent<Star>().message != null && tries <= maxTries)
            {
                messageStar = GameObject.FindGameObjectsWithTag("Star")[Random.Range(0, GameObject.FindGameObjectsWithTag("Star").Length)];
                tries++;
                if (tries > maxTries)
                {
                    failed = true;
                }
            }
            

            //instantiate message
            messageStar.GetComponent<Star>().message = Instantiate(messageStar.GetComponent<Star>().msgPrefab);

            //set to "job" type message
            messageStar.GetComponent<Star>().message.GetComponent<Message>().isChoice = true;
            messageStar.GetComponent<Star>().message.GetComponent<Message>().weaponsCustomerSystem = messageStar;
                
            //find a random dealer that is not the customer
            GameObject dealer = messageStar;

            maxTries = GameObject.FindGameObjectsWithTag("Star").Length;
            tries = 0;
            while ((dealer == messageStar || dealer.GetComponent<Star>().message != null) && tries <= maxTries)
            {
                dealer = GameObject.FindGameObjectsWithTag("Star")[Random.Range(0, GameObject.FindGameObjectsWithTag("Star").Length)];
                tries++;
                if (tries > maxTries)
                {
                    failed = true;
                }
            }

            if (!failed)
            {
                messageStar.GetComponent<Star>().message.GetComponent<Message>().weaponsDealerSystem = dealer;

                //set text for job
                messageStar.GetComponent<Star>().message.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text = "Transport cargo from " + dealer.name + " to " + messageStar.name + ". Cargo takes up " + cargoSpace.ToString() + " litres and is worth " + value.ToString();

                messageStar.GetComponent<Star>().message.SetActive(false);


                //instantiate message for dealer
                dealer.GetComponent<Star>().message = Instantiate(dealer.GetComponent<Star>().msgPrefab);

                //set to "weapons pickup" type message
                dealer.GetComponent<Star>().message.GetComponent<Message>().isChoice = false;
                dealer.GetComponent<Star>().message.GetComponent<Message>().weaponsPickup = true;
                dealer.GetComponent<Star>().message.GetComponent<Message>().weaponsPickupActive = false;

                //set customer and dealer systems
                dealer.GetComponent<Star>().message.GetComponent<Message>().weaponsCustomerSystem = messageStar;
                dealer.GetComponent<Star>().message.GetComponent<Message>().weaponsDealerSystem = dealer;

                //set cargo value and space
                dealer.GetComponent<Star>().message.GetComponent<Message>().weaponsCargoSpace = cargoSpace;
                dealer.GetComponent<Star>().message.GetComponent<Message>().weaponsValue = value;

                //set text for job
                dealer.GetComponent<Star>().message.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text = "Received cargo bound for " + messageStar.name + ".";

                dealer.GetComponent<Star>().message.SetActive(false);

                StartCoroutine(JobAcceptTimer(messageStar.GetComponent<Star>().message));
            }
            else
            {
                Destroy(messageStar.GetComponent<Star>().message);
                messageStar.GetComponent<Star>().message = null;
            }
        }
        
    }

    IEnumerator JobAcceptTimer(GameObject job)
    {
        int acceptTime = 10;
        while (acceptTime >= 0)
        {
            yield return new WaitForSeconds(1);
            acceptTime -= 1;
        }
        if (job != null)
        {
            job.GetComponent<Message>().weaponsCustomerSystem.GetComponent<Star>().message = null;
            job.GetComponent<Message>().weaponsDealerSystem.GetComponent<Star>().message = null;
            showMessages.GetComponent<ShowMessages>().unreadMessages.Remove(job);
            Destroy(job);
        }
    }
}
