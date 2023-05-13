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
        CanvasGroup cg = gameObject.AddComponent<CanvasGroup>();
        cg.blocksRaycasts = false;

        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
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

        transform.position = new Vector2((furthestLeft + furthestRight) / 2, (furthestUp + furthestDown) / 2);
        GetComponent<RectTransform>().sizeDelta = new Vector2(furthestRight + 100 - furthestLeft, Mathf.Abs(furthestDown - 100 - furthestUp));
    }
}
