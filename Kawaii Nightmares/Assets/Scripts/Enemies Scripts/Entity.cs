using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public float HP = 0.5f;
    public AudioClip deathSound;
    public bool isDead = false;
    public float fxVolume; 
    public int scoreValue = 100;
    private AudioSource audioSource;
    

    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        
        //dependiendo del nombre del enemigo sumarle vida 
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
  

    public  void Die()
    {
        if (deathSound == null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            gameObject.GetComponent<Collider2D>().enabled = false;
            audioSource.enabled = true;
            audioSource.PlayOneShot(deathSound);
            Destroy(gameObject, deathSound.length + 0.1f);
        }
    
        //reproducir sonido
        //Sumar score al codigo del score
        //aplicar posible spawn 
    }
}
