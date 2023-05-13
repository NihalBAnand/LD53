using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public List<GameObject> connectedStars;
    public GameObject hyperlane;

    public string nationality;
    public GameObject border;
    // Start is called before the first frame update
    void Start()
    {
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

        if (GameObject.Find(nationality + "Border") == null)
        {
            GameObject newBorder = Instantiate(border);
            newBorder.transform.SetParent(GameObject.Find("MacroCanvas").transform);
            newBorder.name = nationality + "Border";
            newBorder.GetComponent<Border>().nationality = nationality;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
