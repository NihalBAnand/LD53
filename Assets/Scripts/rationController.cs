using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rationController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject microCanvas;

    void Start()
    {
        StartCoroutine(depleteRations());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator depleteRations()
    {
        for (;;)
        {
            microCanvas.GetComponent<microController>().rationPercent -= microCanvas.GetComponent<microController>().rationPercent * microCanvas.GetComponent<microController>().rationDepletionRate;
            yield return new WaitForSeconds(microCanvas.GetComponent<microController>().eatingRateinseconds);
        }
    }
    public void changeRationMode()
    {
        microCanvas.GetComponent<micrController>().
    }

}
