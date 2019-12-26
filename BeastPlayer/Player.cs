using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void Deathdelegate();
    public event Deathdelegate deathEvent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Die()
    {
        if (deathEvent != null)
        {
            deathEvent();
        }
    }

}
