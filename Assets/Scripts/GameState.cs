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

    public bool macroEnabled;

    bool initedMessages;
    // Start is called before the first frame update
    void Start()
    {
        macroEnabled = false;
        initedMessages = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //    GameObject messageStar = GameObject.FindGameObjectsWithTag("Star")[Random.Range(0, GameObject.FindGameObjectsWithTag("Star").Length)];

        //    messageStar.GetComponent<Star>().message = Instantiate(messageStar.GetComponent<Star>().msgPrefab);

        //    messageStar.GetComponent<Star>().message.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text = "Asteroid field from Star 2 to Star!";
        //    messageStar.GetComponent<Star>().message.GetComponent<Message>().isChoice = false;
        //    messageStar.GetComponent<Star>().message.GetComponent<Message>().hyperlaneInfo = "Asteroid field";
        //    messageStar.GetComponent<Star>().message.GetComponent<Message>().relevantHyperlane = GameObject.Find("HL_Star (2)-Star");

        //    messageStar.GetComponent<Star>().message.SetActive(false);

        if (!initedMessages)
        {
            initedMessages = true;
            StartCoroutine(DeliverMessages());
        }

        if (currentLocation.GetComponent<Star>().message != null)
        {
            currentLocation.GetComponent<Star>().message.SetActive(true);
            currentLocation.GetComponent<Star>().message.transform.SetParent(messages.transform.Find("Viewport").Find("Content"));

            showMessages.GetComponent<ShowMessages>().unreadMessages.Add(currentLocation.GetComponent<Star>().message);
            currentLocation.GetComponent<Star>().message.GetComponent<Message>().isRead = false;
            currentLocation.GetComponent<Star>().message = null;
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

        if (Input.GetMouseButtonDown(2))
        {
            GenAsterField();
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
            ////TODO: Implement failure chance
            //GameObject newMessage = Instantiate(message);
            //int roll = Random.Range(0, 100);
            ////Asteroid field - causes ship to be damaged if it goes through
            //if (roll < 50)
            //{
            //    //TODO: Implement chance to clear asteroid field
                
            //    //it's info and an asteroid field
            //    newMessage.GetComponent<Message>().isChoice = false;
            //    newMessage.GetComponent<Message>().hyperlaneInfo = "Asteroid field";
                
            //    //select our stars
            //    GameObject affectedStar = GameObject.FindGameObjectsWithTag("Star")[Random.Range(0, GameObject.FindGameObjectsWithTag("Star").Length)];
            //    int selectedStar = Random.Range(0, affectedStar.GetComponent<Star>().realConnectedStars.Count);
            //    string targetStar = affectedStar.GetComponent<Star>().realConnectedStars.ElementAt(selectedStar).name;

            //    //find relevant hyperlane
            //    GameObject hyperlane = GameObject.Find("HL_" + affectedStar.name + "-" + targetStar);
            //    //swap name if it was generated the other way
            //    if (hyperlane == null)
            //    {
            //        hyperlane = GameObject.Find("HL_" + targetStar + "-" + affectedStar.name);
            //    }
            //    //prevent duplicate messages
            //    if (hyperlane.GetComponent<Hyperlane>().specialCondition != "None")
            //    {
            //        newMessage.GetComponent<Message>().relevantHyperlane = hyperlane;
            //        newMessage.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text = "Asteroid field from " + affectedStar.name + " to " + targetStar + "!";
            //        newMessage.transform.SetParent(messages.transform.Find("Viewport").Find("Content"));

            //        showMessages.GetComponent<ShowMessages>().unreadMessages.Add(newMessage);
            //        newMessage.GetComponent<Message>().isRead = false;
            //    }
            //    else
            //    {
            //        Destroy(newMessage);
            //    }
            //}
        }
    }

    void GenAsterField()
    {
        Debug.Log("Hello!");
        GameObject newMessage = Instantiate(message);
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
        if (hyperlane.GetComponent<Hyperlane>().specialCondition == "None")
        {
            newMessage.GetComponent<Message>().relevantHyperlane = hyperlane;
            newMessage.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text = "Asteroid field from " + affectedStar.name + " to " + targetStar + "!";
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
