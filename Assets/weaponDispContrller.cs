using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class weaponDispContrller : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject microCanvas;
    int mode;
    void Start()
    {
        mode = microCanvas.GetComponent<microController>().weaponMode;
        Debug.Log(mode);
    }

    // Update is called once per frame
    void Update()
    {

        mode = microCanvas.GetComponent<microController>().weaponMode;
        switch (mode)
        {
            case 0:
                gameObject.GetComponent<TextMeshProUGUI>().text = "Strongest First";
                break;
            case 1:
                gameObject.GetComponent<TextMeshProUGUI>().text = "Fastest First";
                break;
            case 2:
                gameObject.GetComponent<TextMeshProUGUI>().text = "Largest First";
                break;
            default:
                gameObject.GetComponent<TextMeshProUGUI>().text = "WEAPONS OFF";
                break;
        }
    }
}
