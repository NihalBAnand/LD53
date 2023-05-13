using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowMessages : MonoBehaviour
{
    public List<GameObject> unreadMessages;
    // Start is called before the first frame update
    void Start()
    {
        unreadMessages = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject msg in unreadMessages)
        {
            if (msg.GetComponent<Message>().isRead)
            {
                unreadMessages.Remove(msg);
            }
        }
        if (unreadMessages.Count > 0)
        {
            transform.Find("Unread").gameObject.SetActive(true);
            transform.Find("Unread").Find("Text").GetComponent<TextMeshProUGUI>().text = unreadMessages.Count.ToString();
        }
        else
        {
            transform.Find("Unread").gameObject.SetActive(false);
        }
    }
}
