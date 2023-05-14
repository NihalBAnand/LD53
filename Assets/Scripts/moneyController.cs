using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class moneyController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject microCanvas;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = microCanvas.GetComponent<microController>().moneyValue.ToString();
    }
}
