using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class payLoadController : MonoBehaviour
{
    public int initvalue;
    public int space;
    public int time;
    public int valueLost;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator timePenalty()
    {
        yield return new WaitForSeconds(time);
        for(; ; )
        {
            valueLost += 1;
            yield return new WaitForSeconds(10);
            
        }

    }
    public int roi()
    {
        return initvalue - valueLost;
    }
}
