using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropDestructible : MonoBehaviour
{
    public GameObject destoryedVersion;

    private void Start()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<DamageDealer>() != null)
        {
            Instantiate(destoryedVersion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("collider doesn't have any script to calculate damage dealing.");
            Instantiate(destoryedVersion, transform.position, transform.rotation);
            
            Destroy(gameObject);
            
        }
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("will be destroyed");
        Destroy(destoryedVersion);
    }
}
