using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class speedSlider : MonoBehaviour
{
    double maxSpeed;
    public GameObject microCanvas;
    // Start is called before the first frame update
    void Start()
    {
        maxSpeed = microCanvas.GetComponent<microController>().maxSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        maxSpeed = microCanvas.GetComponent<microController>().maxSpeed;

        if (GameObject.Find("GameState").GetComponent<GameState>().flying)
        {
            GetComponent<Slider>().enabled = false;
        }
        else
        {
            GetComponent<Slider>().enabled = true;
        }
    }
   
}
