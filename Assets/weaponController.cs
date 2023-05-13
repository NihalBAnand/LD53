using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject microCanvas;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void strongestFirst()
    {
        microCanvas.GetComponent<microController>().weaponMode = 0;
        

    }
    public void fastestFirst()
    {
        microCanvas.GetComponent<microController>().weaponMode = 1;

    }
    public void largestFirst()
    {
        microCanvas.GetComponent<microController>().weaponMode = 2;

    }
}
