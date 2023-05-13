using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class fuelController : MonoBehaviour
{
    double fuelPercent = 100;
    double depletionRate = .1;
    public Slider speedSlider;
 
    
    // Start is called before the first frame update
    void Start()
    { 
        StartCoroutine(Deplete());
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = fuelPercent.ToString();
    }

    IEnumerator Deplete()
    {
        for (;;)
        {
            fuelPercent -= fuelPercent * (depletionRate * (speedSlider.value));
            yield return new WaitForSeconds(10);
        }
    }
}
