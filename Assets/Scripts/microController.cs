using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class microController : MonoBehaviour
{
    // Start is called before the first frame update

    //Ship Components:
    
    //AI controls air qc
    public bool AI = true;

    //Engine
    public double engineHealth = 1; //fuel efficiency coeficcient 
    public double radiationLevel = 0;

    
    public bool hull = true;//You die if hull is gone
    public double commHealth = 1;//less likely to recieve messages
    public double morale = 1;//I assume it causes mutiny(death)

    //Cargo
    public int cargoArea = 100;
    public GameObject payLoad;
    public List<GameObject> cargo;
    int spaceAvail;

    //Weapons
    public int weaponMode = 3;//0=StrongestFirst, 1= fastestFirst, 2 = LargestFirst, else = off
  
  


    void Start()
    {
        spaceAvail = cargoArea;
        cargo = new List<GameObject>();
               
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void addCargo(int space, int value)
    {
       
        if (spaceAvail >= space)
        {
            //create the cargo
            cargo.Add(Instantiate(payLoad, new Vector3(0, 0, 0), Quaternion.identity));
            cargo[cargo.Count - 1].GetComponent<payLoadController>().space = space;
            cargo[cargo.Count - 1].GetComponent<payLoadController>().value = value;

            spaceAvail -= space;
        }
        else
        {
            Debug.Log("NO SPACE");
        }
    }
}
