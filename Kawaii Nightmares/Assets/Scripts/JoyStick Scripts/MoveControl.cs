using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveControl : MonoBehaviour
{
    
        [SerializeField]
        private GameObject stick, panel;
        private Rigidbody2D rb;
        private float movespeed;
        private Touch oneTouch;
        private Vector2 touchPosition;
        private Vector2 moveDirection;
        private GameObject ship;
        private Vector2 testPosition;

        //calculo de pantalla
        float MaxX;
        float MaxY;
        float sideGOX;
        float sideGOY;
        private GameObject gameManager; 


        private int leftTouch = 99;

        // test
        public Text texto;

        void Start()
        {
            ship = GameObject.FindGameObjectWithTag("playerShip");
            rb = ship.GetComponent<Rigidbody2D>();
            gameManager = GameObject.FindGameObjectWithTag("GameController");
            stick.SetActive(false);
            panel.SetActive(false);
            movespeed = 3f;
            
                    var unitsInY = Camera.main.orthographicSize *2f;
                    var unitsInX = unitsInY * ((float)Screen.width / (float)Screen.height);

                    MaxX = unitsInX / 2;
                    MaxY = unitsInY / 2;
                    print("units in Y: " + MaxY +" units in X: "+ MaxX);


            //sideGOX = ship.transform.localScale.x;
            //sideGOY = ship.transform.localScale.y;
           // MaxX = gameManager.GetComponent<GameManager>().GetSizeX();
           // MaxY = gameManager.GetComponent<GameManager>().GetSizeY();
            sideGOX = ship.GetComponent<BoxCollider2D>().size.x;
            sideGOY = ship.GetComponent<BoxCollider2D>().size.y;

        }

        // Update is called once per frame
        void Update()
        {

            if (Time.timeScale != 0)
            {
               // testMode();
                ControllerPad();
                ControlSide();
                fixPosition();
            }


            //test mode



        }//fin update

        void fixPosition()
        {
            if (ship.transform.position.x + (sideGOX / 2) > (MaxX +0.01f) )
            {
                ship.transform.position = new Vector3(MaxX - (sideGOX / 2), ship.transform.position.y, 0);
            }
            else if (ship.transform.position.x - (sideGOX / 2) < -(MaxX +0.01))
            {
                ship.transform.position = new Vector3(-MaxX + (sideGOX / 2), ship.transform.position.y, 0);
            }
            if (ship.transform.position.y + (sideGOY / 2) > (MaxY+0.01))
            {
                ship.transform.position = new Vector3(ship.transform.position.x, MaxY - (sideGOY / 2), 0);
            }
            else if (ship.transform.position.y - (sideGOY / 2) < -(MaxY+0.01))
            {
                ship.transform.position = new Vector3(ship.transform.position.x, -MaxY + (sideGOY / 2) , 0);
            }

        }
        private void ControlSide()
        {
            if (ship.transform.position.x +(sideGOX/2) >= MaxX && rb.velocity.x >0f)
            {
                rb.velocity = new Vector2(0f, rb.velocity.y);
            }
            else if(ship.transform.position.x - (sideGOX / 2) <= -MaxX && rb.velocity.x < 0f)
            {
                rb.velocity = new Vector2(0f, rb.velocity.y);
            }

            if(ship.transform.position.y + (sideGOY / 2) >= MaxY && rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
            }
            else if(ship.transform.position.y - (sideGOY / 2) <= -MaxY && rb.velocity.y < 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
            }


        }


        private void ControllerPad()
        {
            int i = 0;

            while (i < Input.touchCount)
            {

                Touch t = Input.GetTouch(i);                    
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(t.position);

                 if (t.phase == TouchPhase.Began)
                 {
                     if(t.position.x>= Screen.width / 2)
                     {

                        //gameObject.GetComponent<hpShootNave>().shootShip();

                     }
                     else
                     {
                         leftTouch = t.fingerId;
                         touchPosition = touchPos;
                         panel.SetActive(true);
                         stick.SetActive(true);
                         panel.transform.position = touchPosition;
                         stick.transform.position = touchPosition;
                        texto.text = "estoy quieto";
                    }
                }
                else if(t.phase == TouchPhase.Moved && leftTouch == t.fingerId)
                {
                    texto.text = "me he movido";
                    stick.transform.position = touchPos;
                    stick.transform.position = new Vector2(
                        Mathf.Clamp(stick.transform.position.x,
                        panel.transform.position.x - 0.5f,
                        panel.transform.position.x + 0.5f),
                        Mathf.Clamp(stick.transform.position.y,
                        panel.transform.position.y - 0.5f,
                        panel.transform.position.y + 0.5f)
                        );

                    moveDirection = (stick.transform.position - panel.transform.position).normalized;
                    rb.velocity = moveDirection * movespeed;

                    // MoveNave();
                }
                else if(t.phase == TouchPhase.Ended && leftTouch== t.fingerId)
                {
                     leftTouch = 99;
                     panel.SetActive(false);
                     stick.SetActive(false);
                     rb.velocity = Vector2.zero;

                }
                i++;
            }





        }



        private void MoveNave()
        {
            texto.text = "me he movido";


        }
    /*
        private void testMode()
        {


            if (Input.GetKeyDown(KeyCode.W))
            {
                rb.velocity = new Vector2(0f, 0.5f) * movespeed;

            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                rb.velocity = new Vector2(0.5f, 0f) * movespeed;

            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                rb.velocity = new Vector2(0f, rb.velocity.y);
            }


            if (Input.GetKeyDown(KeyCode.A))
            {
                rb.velocity = new Vector2(-0.5f, 0f) * movespeed;

            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                rb.velocity = new Vector2(0f, rb.velocity.y);
            }


            if (Input.GetKeyDown(KeyCode.S))
            {
                rb.velocity = new Vector2(0f, -0.5f) * movespeed;

            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                gameObject.GetComponent<hpShootNave>().shootShip();
            }

        }
    */
          
}
