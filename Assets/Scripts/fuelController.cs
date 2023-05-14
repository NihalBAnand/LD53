using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class fuelController : MonoBehaviour
{
    
    public GameObject microCanvas;
    double depletionRate;
    public Slider speedSlider;
    double fuelPercent;
 
    
    // Start is called before the first frame update
    void Start()
    { 
        StartCoroutine(Deplete());
        fuelPercent = microCanvas.GetComponent<microController>().fuelPercent;
        depletionRate = microCanvas.GetComponent<microController>().depletionRate;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = fuelPercent.ToString();
        depletionRate = microCanvas.GetComponent<microController>().depletionRate;
    }

    IEnumerator Deplete()
    {
        for (;;)
        {
            microCanvas.GetComponent<microController>().fuelPercent -= fuelPercent * (depletionRate * (speedSlider.value));
            fuelPercent -= fuelPercent * (depletionRate * (speedSlider.value));
            yield return new WaitForSeconds(10);
        }
    }
}
