using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    public AudioClip fireSound;
    public float FireVolume;
    private AudioSource audioSourceFire;
    private Rigidbody2D rb;
    [SerializeField]
    private float Vel;
    //esto es la bala enemiga todo lo que tengo hecho aqui deberia de ir a enemy
    //bullet 1 = direccion manual
    //bullet 2 = AIM player 
    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
   

    private void PlaySound()
    {
        if (fireSound == null) return;
        audioSourceFire.enabled = true;
        audioSourceFire.PlayOneShot(fireSound); 
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public void ChangeVelocityAndDirection(float directionX, float directionY)
   {
        if (gameObject.name == "bullet1")
        {
            Vector3 direction = new Vector3(directionX, directionY, 0f);
            Vector2 buldir = (direction).normalized;
            rb.velocity = buldir * Vel;
        }
        else if(gameObject.name == "bullet2")
        {
            Vector3 direction = new Vector3(directionX, directionY, 0f);
            Vector2 buldir = (direction - gameObject.transform.position).normalized;
            rb.velocity = buldir * Vel;
            var angle = Mathf.Atan2(buldir.y, buldir.x) * Mathf.Rad2Deg;
            angle += 90; 
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);  
        }
   }
}
