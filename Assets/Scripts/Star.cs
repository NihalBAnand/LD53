using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Star : MonoBehaviour
    , IPointerClickHandler
{
    public List<GameObject> connectedStars;
    public HashSet<GameObject> realConnectedStars;
    public GameObject hyperlane;

    public string nationality;
    public GameObject border;

    public Color color;
    bool selected;

    public GameObject msgPrefab;
    public GameObject message;
    bool msgFlashing;

    bool initedHyperlaneConns;

    public void OnPointerClick(PointerEventData eventData)
    {
        //toggle selection on click
        selected = !selected;

        if (selected)
        {
            //activate info box
            Transform infoBox = transform.Find("InfoBox");
            infoBox.gameObject.SetActive(true);

            ///write text to textbox
            //write star name to textbox
            infoBox.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text = "System name: " + name;

            //write connections to textbox
            infoBox.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text += "\nConnections:\n";
            GameObject[] hyperlanes = GameObject.FindGameObjectsWithTag("Hyperlane");
            for (int i = 0; i < hyperlanes.Length; i++)
            {
                if (Array.IndexOf(hyperlanes[i].GetComponent<Hyperlane>().connectedStars, gameObject) != -1)
                {
                    foreach (GameObject star in hyperlanes[i].GetComponent<Hyperlane>().connectedStars)
                    {
                        if (star != gameObject)
                        {
                            infoBox.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text += star.name + " ";
                            //add any found connections to the functional connections hashset
                            realConnectedStars.Add(star);
                        }
                    }
                }
            }
            //write nationality to textbox
            infoBox.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text += "\nNationality:\n" + nationality;

            //write YOU ARE HERE if you're here
            if (GameObject.Find("GameState").GetComponent<GameState>().currentLocation == gameObject)
            {
                infoBox.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text += "\n<b>You are here.</b>";
            }

            //write that there's a message if we have one
            if (message != null)
            {
                infoBox.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text += "\nA message is available!";
            }

            //if we border the star where the player is, enable "travel here" button
            if (realConnectedStars.Contains(GameObject.Find("GameState").GetComponent<GameState>().currentLocation))
            {
                infoBox.gameObject.GetComponent<StarInfoBox>().travelButton.GetComponent<Button>().interactable = true;
                infoBox.gameObject.GetComponent<StarInfoBox>().travelButton.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().color = new Color32(0, 0, 0, 255);
            }
            //otherwise, disable it
            else
            {
                infoBox.gameObject.GetComponent<StarInfoBox>().travelButton.GetComponent<Button>().interactable = false;
                infoBox.gameObject.GetComponent<StarInfoBox>().travelButton.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().color = new Color32(0, 0, 0, 0);
            }
        }
        else
        {
            //disable infobox if we're deslecting
            Transform infoBox = transform.Find("InfoBox");
            infoBox.gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        msgFlashing = false;

        selected = false;

        initedHyperlaneConns = false;

        realConnectedStars = new HashSet<GameObject>();

        //create hyperlane
        foreach (GameObject star in connectedStars)
        {
            //init hyperlane
            GameObject newHyperlane = Instantiate(hyperlane);
            newHyperlane.transform.SetParent(transform);

            //set position to center of host star
            newHyperlane.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0);

            //set length of hyperlane to distance between stars
            newHyperlane.GetComponent<RectTransform>().sizeDelta = new Vector2(3, Vector3.Distance(transform.position, star.transform.position));

            //angle hyperlane to point from one star to another
            newHyperlane.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, Mathf.Asin((transform.position.x - star.transform.position.x) / Vector3.Distance(transform.position, star.transform.position)) * Mathf.Rad2Deg);
            //reflect angle if host star is higher than target star - done because unity handles trig from 0-pi instead of 0-2pi
            if (Mathf.Sign(transform.position.y - star.transform.position.y) == 1)
            {
                newHyperlane.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 180 - newHyperlane.GetComponent<RectTransform>().eulerAngles.z);
            }

            //set connection in code
            newHyperlane.GetComponent<Hyperlane>().connectedStars[0] = gameObject;
            newHyperlane.GetComponent<Hyperlane>().connectedStars[1] = star;
            newHyperlane.name = "HL_" + gameObject.name + "-" + star.name;

            //detach hyperlane from host for rendering reasons
            newHyperlane.transform.SetParent(GameObject.Find("ScaledUI").transform);
            newHyperlane.transform.SetAsFirstSibling();
        }

        //see if we have a border for our nation, and if not, make one
        GameObject findBorder = GameObject.Find(nationality + "Border");
        if (findBorder == null)
        {
            GameObject newBorder = Instantiate(border);
            newBorder.transform.SetParent(GameObject.Find("ScaledUI").transform);
            newBorder.name = nationality + "Border";
            newBorder.GetComponent<Border>().nationality = nationality;
            newBorder.GetComponent<Border>().UpdateBorder();
        }


        
    }

    // Update is called once per frame
    void Update()
    {
        if (!initedHyperlaneConns)
        {
            initedHyperlaneConns = true;
            GameObject[] hyperlanes = GameObject.FindGameObjectsWithTag("Hyperlane");
            for (int i = 0; i < hyperlanes.Length; i++)
            {
                if (Array.IndexOf(hyperlanes[i].GetComponent<Hyperlane>().connectedStars, gameObject) != -1)
                {
                    foreach (GameObject star in hyperlanes[i].GetComponent<Hyperlane>().connectedStars)
                    {
                        if (star != gameObject)
                        {
                            //add any found connections to the functional connections hashset
                            realConnectedStars.Add(star);
                        }
                    }
                }
            }
        }
        GetComponent<Image>().color = color;

        //clear selection on right click
        if (Input.GetMouseButtonDown(1))
        {
            selected = false;
            transform.Find("InfoBox").gameObject.SetActive(false);
        }

        //set color according to status
        if (!GameObject.Find("GameState").GetComponent<GameState>().flying)
        {
            if (GameObject.Find("GameState").GetComponent<GameState>().currentLocation == gameObject) //player is here
            {
                color = new Color32(66, 135, 245, 255);
            }
            else if (!selected && !msgFlashing) //we don't have a message here and we aren't selected
            {
                color = new Color32(255, 255, 255, 255);
            }
            else if (!msgFlashing) //we don't have a message and we are selected
            {
                color = new Color32(204, 135, 8, 255);
            }
        }

        //if we have a message and we aren't flashing, start flashing
        if (message != null && !msgFlashing && !message.GetComponent<Message>().weaponsPickup || message != null && !msgFlashing && message.GetComponent<Message>().weaponsPickupActive)
        {
            StartCoroutine(FlashMessage());
        }
    }

    public void TravelHere()
    {
        StartCoroutine(Travel());
    }

    IEnumerator Travel()
    {
        string comingFrom = GameObject.Find("GameState").GetComponent<GameState>().currentLocation.name;
        string goingTo = name;

        GameObject hyplane = GameObject.Find("HL_" + comingFrom + "-" + goingTo);
        if (hyplane == null)
        {
            hyplane = GameObject.Find("HL_" + goingTo + "-" + comingFrom);
        }

        hyplane.GetComponent<Hyperlane>().selected = true;

        GameObject.Find("GameState").GetComponent<GameState>().currentLocation = null;
        GameObject.Find("GameState").GetComponent<GameState>().flying = true;

        int travelTime = 2000;
        travelTime = (int) ((double)travelTime / GameObject.Find("MicroCanvas").GetComponent<microController>().speed);
        int maxTT = travelTime;
        while (travelTime >= 0)
        {
            yield return new WaitForSeconds(0.001f);
            travelTime--;
            hyplane.GetComponent<Hyperlane>().selected = true;
            color = Color.Lerp(new Color32(66, 135, 245, 255), new Color32(255, 255, 255, 255), travelTime / (float)maxTT);
        }

        switch (hyplane.GetComponent<Hyperlane>().specialCondition)
        {
            case "Asteroid Field":
                Debug.Log("Ship damaged by asteroid field");
                break;
            default:
                break;
        }

        //set player location to here
        GameObject.Find("GameState").GetComponent<GameState>().currentLocation = gameObject;

        //deselect everything
        foreach (GameObject star in GameObject.FindGameObjectsWithTag("Star"))
        {
            star.GetComponent<Star>().selected = false;
            star.transform.Find("InfoBox").gameObject.SetActive(false);
        }
        foreach (GameObject hplane in GameObject.FindGameObjectsWithTag("Hyperlane"))
        {
            hplane.GetComponent<Hyperlane>().selected = false;
            hplane.transform.Find("InfoBox").gameObject.SetActive(false);
        }

        GameObject.Find("GameState").GetComponent<GameState>().flying = false;
        hyplane.GetComponent<Hyperlane>().selected = false;
    }

    IEnumerator FlashMessage()
    {
        //flash red, then white
        msgFlashing = true;
        while (message != null)
        {
            color = new Color32(255, 0, 0, 255);
            yield return new WaitForSeconds(.3f);
            color = new Color32(255, 255, 255, 255);
            yield return new WaitForSeconds(.3f);
        }
        msgFlashing = false;
    }
}
