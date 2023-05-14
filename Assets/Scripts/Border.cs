using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Border : MonoBehaviour
{
    public string nationality;

    Color32 color;
    // Start is called before the first frame update
    void Start()
    {
        //new canvas group so we can click on things below these borders
        CanvasGroup cg = gameObject.AddComponent<CanvasGroup>();
        cg.blocksRaycasts = false;

        //set position to 0,0 for ease of use
        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        
        //set color according to nationality
        switch (nationality)
        {
            case "Kingdom":
                color = new Color32(150, 0, 0, 125);
                break;
            case "Republic":
                color = new Color32(0, 150, 0, 125);
                break;
            case "Raiders":
                color = new Color32(0, 0, 150, 125);
                break;
            default:
                color = new Color32((byte)Random.Range(0, 256), (byte)Random.Range(0, 256), (byte)Random.Range(0, 256), 125);
                break;
        }
        GetComponent<Image>().color = color;

        UpdateBorder();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateBorder()
    {
        float furthestLeft = 999999;
        float furthestRight = -9999999999;
        float furthestUp = -999999999;
        float furthestDown = 9999999999;

        //iterate through each star, find the ones with our nationality, and find the bounds of our border
        foreach (GameObject star in GameObject.FindGameObjectsWithTag("Star"))
        {
            if (star.GetComponent<Star>().nationality == nationality)
            {
                if (star.transform.position.x < furthestLeft)
                {
                    furthestLeft = star.transform.position.x;
                }
                if (star.transform.position.x > furthestRight)
                {
                    furthestRight = star.transform.position.x;
                }
                if (star.transform.position.y > furthestUp)
                {
                    furthestUp = star.transform.position.y;
                }
                if (star.transform.position.y < furthestDown)
                {
                    furthestDown = star.transform.position.y;
                }
            }
        }

        //set position to middle of bounds
        transform.position = new Vector2((furthestLeft + furthestRight) / 2, (furthestUp + furthestDown) / 2);
        //set size to encompass the bounds with some padding (50px on all sides)
        GetComponent<RectTransform>().sizeDelta = new Vector2(furthestRight + 100 - furthestLeft, Mathf.Abs(furthestDown - 100 - furthestUp));
    }
}
