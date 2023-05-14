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
    public double fuelPercent = 100;
    public double depletionRate = .1;
    public double maxSpeed = 1;//(It's a proportional thingy)

    //MISC FOR NOW
    public bool hull = true;//You die if hull is gone
    public double commHealth = 1;//less likely to recieve messages
     

    //Cargo
    public int cargoArea = 100;
    public GameObject payLoad;
    public List<GameObject> cargo;
    int spaceAvail;

    //Weapons
    public int weaponMode = 3;//0=StrongestFirst, 1= fastestFirst, 2 = LargestFirst, else = off

    //Money
    public int moneyValue = 0;

    //Rations
    public double rationPercent = 100;
    public float eatingRateinseconds = 10;//FOR INTERNAL PURPOSES FOR HOW MANY SECONDS DO WE DEPLETE
    public double rationDepletionRate = .1;
    public double rationSlowDepletionRate = .05;

    //Ship Efficiency
    public double morale = 1;//I assume it causes mutiny(death)
    public double stability;
  


    void Start()
    {

        spaceAvail = cargoArea;
        cargo = new List<GameObject>();
        stability = 1;
               
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

    /*
    IEnumerator breakCheck()
    {
        for(; ; )
        {
            if (instability<1)
            {
                
            }
        }
    }*/

    public void removeCargo(int space, int value)
    {
        foreach (GameObject cargoItem in cargo)
        {
            if (cargoItem.GetComponent<payLoadController>().space == space && cargoItem.GetComponent<payLoadController>().value == value)
            {
                cargo.Remove(cargoItem);
                Destroy(cargoItem);
                break;
            }
        }
    }
}
