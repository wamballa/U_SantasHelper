using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MouseCollisionZone : MonoBehaviour
{
    public List<GameObject> fallenCakes = new List<GameObject>();

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Cake"))
        {

        }
    }


}
