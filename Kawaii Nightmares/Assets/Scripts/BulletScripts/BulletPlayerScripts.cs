using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlayerScripts : MonoBehaviour
{
    public float dmg=0.5f; 
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 1.5f); 
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "enemy")
        {
            col.gameObject.GetComponent<Enemy>().DMG(dmg);
            Destroy(gameObject);
        }

    }
    
}
