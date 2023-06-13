using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject speedSlider;
    public double speed;

    //MISC FOR NOW
    public bool hull = true;//You die if hull is gone
    public int commHealth = 100;//less likely to recieve messages
     

    //Cargo
    public int cargoArea = 100;
    public GameObject payLoad;
    public List<GameObject> cargo;
    int spaceAvail;

    //Weapons
    public int weaponMode = 3;//0=StrongestFirst, 1= fastestFirst, 2 = LargestFirst, else = off

    //Money
    public int moneyValue = 0;//call roi for finished jobs

    //Rations
    public double rationPercent = 100;
    public float eatingRateinseconds = 10;//FOR INTERNAL PURPOSES FOR HOW MANY SECONDS DO WE DEPLETE
    public double rationDepletionRate = .1;
    public double rationSlowDepletionRate = .05;

    //Ship Efficiency
    public double morale = 1;//I assume it causes mutiny(death)
    public double stability;

    //Components
    public Dictionary<string, int> components;
  
    

    void Start()
    {
        components = new Dictionary<string, int>();
        components.Add("Engine Health", 100);
        components.Add("Hull", 100);
        components.Add("Comms", 100);
        components.Add("Weapons", 100);

        spaceAvail = cargoArea;
        cargo = new List<GameObject>();
        stability = 1;
               
    }

    // Update is called once per frame
    void Update()
    {
        speed = speedSlider.GetComponent<Slider>().value;

        components["Comms"] = commHealth;
    }


    public void addCargo(int space, int initvalue, int time)
    {
       
        if (spaceAvail >= space)
        {
            //create the cargo
            cargo.Add(Instantiate(payLoad, new Vector3(0, 0, 0), Quaternion.identity));
            cargo[cargo.Count - 1].GetComponent<payLoadController>().space = space;
            cargo[cargo.Count - 1].GetComponent<payLoadController>().initvalue = initvalue;
            cargo[cargo.Count - 1].GetComponent<payLoadController>().time = time;
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

    public void removeCargo(int space, int value, bool complete)
    {
        foreach (GameObject cargoItem in cargo)
        {
            if (complete)
            {
                moneyValue += cargoItem.GetComponent<payLoadController>().roi();
            }
            if (cargoItem.GetComponent<payLoadController>().space == space && cargoItem.GetComponent<payLoadController>().initvalue == value)
            {
                cargo.Remove(cargoItem);
                Destroy(cargoItem);
                break;
            }
        }
    }
    public string repair(string c, int cost)
    {
        if (components[c] < 100 && moneyValue>cost)
        {
            components[c] = 100;
            moneyValue -= cost;
            return "repaired" + c;
        }
        else
        {
            return "no money get rekt";
        }
    }
}
