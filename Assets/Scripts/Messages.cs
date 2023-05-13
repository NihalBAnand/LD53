using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Messages : MonoBehaviour
{
    public GameObject message;

    // Start is called before the first frame update
    void Start()
    {
        //Demo - remove
        for (int i = 0; i < 10; i++)
        {
            GameObject newMessage = Instantiate(message);
            newMessage.transform.SetParent(transform.Find("Viewport").Find("Content"));
            newMessage.GetComponent<Message>().showMessages = GameObject.Find("ShowMessages");
            newMessage.GetComponent<Message>().isRead = false;
            newMessage.GetComponent<Message>().showMessages.GetComponent<ShowMessages>().unreadMessages.Add(newMessage);
        }

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.SetAsLastSibling();
    }

    public void ToggleShown()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        
    }
}
