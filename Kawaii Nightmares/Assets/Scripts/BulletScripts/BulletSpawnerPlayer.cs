using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 

public class BulletSpawnerPlayer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Transform position1;
    public Transform position2;
    public Transform position3;
    public Rigidbody2D bullet;
    [SerializeField]
    private int NumberShoot;
    
    private float nextFire = 0.5f;
    [Space]
    public float fireDelta = 0.5f;
    private float myTime = 0.0f;
    private bool buttonDown;

   
    void Awake()
    {
        buttonDown = false;
    }
    void Start()
    {
        NumberShoot = 1;
    }

    // Update is called once per frame
    void Update()
    {
        myTime += Time.deltaTime;
        if(buttonDown)
        {
            if(nextFire<= 0)
            {
              shootButton();
              nextFire = myTime + fireDelta;
            }
            nextFire -= myTime;
            myTime = 0.0f; 
        }
    }


    public void shootButton()
    {
        if (NumberShoot == 1)
        {
            Rigidbody2D bulletsInstatiated;
            bulletsInstatiated = Instantiate(bullet, position2.position, position2.rotation);
            bulletsInstatiated.velocity = transform.TransformDirection(Vector2.up * 10);
            bulletsInstatiated.name = bullet.name;

        }
        else if (NumberShoot == 2) 
        {
            Rigidbody2D bulletsInstatiated;
            bulletsInstatiated = Instantiate(bullet, position1.position, position1.rotation);
            bulletsInstatiated.velocity = transform.TransformDirection(Vector2.up * 10);
            bulletsInstatiated.name = bullet.name;
            bulletsInstatiated = Instantiate(bullet, position3.position, position3.rotation);
            bulletsInstatiated.velocity = transform.TransformDirection(Vector2.up * 10);
            bulletsInstatiated.name = bullet.name;

        }
        else if (NumberShoot == 3)
        {
            Rigidbody2D bulletsInstatiated;
            bulletsInstatiated = Instantiate(bullet, position1.position, position1.rotation);
            bulletsInstatiated.velocity = transform.TransformDirection(Vector2.up * 10);
            bulletsInstatiated.name = bullet.name;
            bulletsInstatiated = Instantiate(bullet, position2.position, position2.rotation);
            bulletsInstatiated.velocity = transform.TransformDirection(Vector2.up * 10);
            bulletsInstatiated.name = bullet.name;
            bulletsInstatiated = Instantiate(bullet, position3.position, position3.rotation);
            bulletsInstatiated.velocity = transform.TransformDirection(Vector2.up * 10);
            bulletsInstatiated.name = bullet.name;
        }
        //Rigidbody p = Instantiate(bullet, position2.position, position2.rotation );
    }

    public void incrementShoot(int number)
    {
        if((number==-1 && NumberShoot>1)||(number==1 && NumberShoot<3))NumberShoot += number; 
       

    }



    public void OnPointerDown(PointerEventData pointerEventData)
    {
        buttonDown = true;   
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        buttonDown = false;
    }
}
