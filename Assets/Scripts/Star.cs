using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Star : MonoBehaviour
    , IPointerClickHandler
{
    public List<GameObject> connectedStars;
    public GameObject hyperlane;

    public string nationality;
    public GameObject border;

    Color color;
    bool selected;

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

            //detach hyperlane from host for rendering reasons
            newHyperlane.transform.SetParent(GameObject.Find("MacroCanvas").transform);
            newHyperlane.transform.SetAsFirstSibling();
        }
        GameObject findBorder = GameObject.Find(nationality + "Border");
        if (findBorder == null)
        {
            GameObject newBorder = Instantiate(border);
            newBorder.transform.SetParent(GameObject.Find("MacroCanvas").transform);
            newBorder.name = nationality + "Border";
            newBorder.GetComponent<Border>().nationality = nationality;
            newBorder.GetComponent<Border>().UpdateBorder();
        }
        
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
