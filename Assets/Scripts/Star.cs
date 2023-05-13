using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public List<GameObject> connectedStars;
    public GameObject hyperlane;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject star in connectedStars)
        {
            GameObject newHyperlane = Instantiate(hyperlane);
            newHyperlane.transform.SetParent(transform);

            newHyperlane.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0);
            newHyperlane.GetComponent<RectTransform>().sizeDelta = new Vector2(1, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
