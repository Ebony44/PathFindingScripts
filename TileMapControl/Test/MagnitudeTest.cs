using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnitudeTest : MonoBehaviour
{
    
    public Transform[] Magnitudes;
    // Start is called before the first frame update
    void Start()
    {
        foreach(var mag in Magnitudes)
        {
            Debug.Log(mag.name + "'s magnitude is " + mag.position.sqrMagnitude);
            
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
