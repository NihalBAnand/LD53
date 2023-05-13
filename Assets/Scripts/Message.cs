using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message : MonoBehaviour
{
    public bool isChoice;

    public GameObject weaponsDealer;
    public GameObject weaponsCustomer;

    public GameObject showMessages;

    public bool isRead;
    // Start is called before the first frame update
    void Start()
    {
        if (isChoice)
        {
            transform.Find("Deny").gameObject.SetActive(true);
        }
        else
        {
            transform.Find("Deny").gameObject.SetActive(false);
        }

        transform.localScale = new Vector3(1, 1, 1);
    }

    private void OnEnable()
    {
        if (!isRead)
        {
            isRead = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Accept()
    {
        if (isChoice)
        {
            //TODO: implement accepting a quest
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Deny()
    {
        Destroy(gameObject);
    }
}
