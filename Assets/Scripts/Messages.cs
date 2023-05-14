using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Messages : MonoBehaviour
{
    public GameObject message;
    bool inited;

    // Start is called before the first frame update
    void Start()
    {
        inited = false;
    }

    private void OnEnable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //disable object after all start functions have run
        if (!inited)
        {
            inited = true;
            gameObject.SetActive(false);
        }
        //render on top of everything
        transform.SetAsLastSibling();

        //clear on right click
        if (Input.GetMouseButtonDown(1))
        {
            gameObject.SetActive(false);
        }
    }

    public void ToggleShown()
    {
        //toggle whether the box is shown when the show messages button is clicked
        gameObject.SetActive(!gameObject.activeSelf);

        //set all messages to read
        foreach (GameObject msg in GameObject.FindGameObjectsWithTag("Message"))
        {
            if (!msg.GetComponent<Message>().isRead)
            {
                msg.GetComponent<Message>().isRead = true;
            }
        }
    }

    
}
