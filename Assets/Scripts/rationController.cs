using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class rationController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject microCanvas;
    public GameObject rationDisp;
    int rationing = -1;

    void Start()
    {
        StartCoroutine(depleteRations());
    }

    // Update is called once per frame
    void Update()
    {
        rationDisp.GetComponent<TextMeshProUGUI>().text = microCanvas.GetComponent<microController>().rationPercent.ToString();
    }
    IEnumerator depleteRations()
    {
        for (;;)
        {
            if (rationing < 0)
            {
                microCanvas.GetComponent<microController>().rationPercent -= microCanvas.GetComponent<microController>().rationPercent * microCanvas.GetComponent<microController>().rationDepletionRate;
            }
            else
            {
                microCanvas.GetComponent<microController>().rationPercent -= microCanvas.GetComponent<microController>().rationPercent * microCanvas.GetComponent<microController>().rationSlowDepletionRate;
            }
            yield return new WaitForSeconds(microCanvas.GetComponent<microController>().eatingRateinseconds);
        }
    }
    public void changeRationMode()
    {
        rationing *=-1;
        if(rationing < 0) { microCanvas.GetComponent<microController>().morale / .9};
        else { microCanvas.GetComponent<microController>().morale *= .9 };
    }

}
