using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStick : MonoBehaviour
{

 
    #region Screen Properties (For Mobiles)
    [Header("Screen Properties (For Mobiles)")]
    public bool fullScreen;
    [Range(0,100)] [Tooltip("Use it for define the range in case of dont generate a fullscreen JoyStick")]public int amount;
    [Tooltip("Invert the position of the JoyStick left to right in case of using split screen")] public bool Invert;
    [Space]
    [Header("Other Properties")]
    [Tooltip("JoyStick sprites")] public GameObject stick; 
    [Tooltip("JoyStick sprites")] public GameObject panel;
    [Tooltip("Object or Character for use the JoyStick")]public GameObject characterToMove;
    [Tooltip("Test the speed of the object")][SerializeField]private float moveSpeed;
    #endregion
    private Rigidbody2D rb;
    private float screenWidth;
    private float screenHeight;

    private int leftTouch = 99;
    private Vector2 touchPosition;
    private Vector2 moveDirection;
    private float quantityToAdd;
    private bool OneJoyStick=true;
    private void Awake()
    {
        
        screenHeight = Camera.main.orthographicSize * 2f;
        screenWidth = screenHeight * ((float)Screen.width / (float)Screen.height);
    }
    void Start()
    {
        //JoyStick Sprite
        stick.SetActive(false);
        panel.SetActive(false);

        //Character or Object
        rb = characterToMove.GetComponent<Rigidbody2D>();
        quantityToAdd = characterToMove.GetComponent<CircleCollider2D>().bounds.size.x/2;


       
        
    }

    // Update is called once per frame
    void Update()
    {
        testMode();

        if(Time.timeScale != 0)
        {
            if (!fullScreen)
            {
              
                ControllerPadDividedScreen();
            }
            else
            {
                ControllerPadFullScreen();
               
            }
            ControlSlide();
            fixPosition(); 
        }
        changeVelocity();
    }

    void changeVelocity()
    {
        if (characterToMove.GetComponent<SpriteRenderer>().enabled) moveSpeed = 1.5f;
        else moveSpeed = 5f; 
    }


    private void ControllerPadDividedScreen()
    {
        int i = 0;
       
      
        while (i < Input.touchCount)
        {
            Touch t = Input.GetTouch(i);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(t.position);
            
                float percentOfScreen = (float)amount / 100f;
                float rangeOfScreen = (screenWidth * percentOfScreen) + (screenWidth / -2); 

                if(t.phase== TouchPhase.Began)
                {
                 
                    if ((touchPos.x >= rangeOfScreen &&!Invert)|| (touchPos.x< rangeOfScreen && Invert))
                    {
                    //Do something else of move 
                    //characterToMove.GetComponent<SpriteRenderer>().sharedMaterial.color = Random.ColorHSV(); 
                    }
                    else
                    {
                      
                       leftTouch = t.fingerId;
                       touchPosition = touchPos;
                       panel.SetActive(true);
                       stick.SetActive(true);
                       panel.transform.position = touchPosition;
                       stick.transform.position = touchPosition;
                      
                    }
                    
                }
                else if(t.phase == TouchPhase.Moved && leftTouch == t.fingerId)
                {
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
                    rb.velocity = moveDirection * moveSpeed;
                }
                else if((t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)&& leftTouch == t.fingerId)
                {
                    leftTouch = 99;
                    panel.SetActive(false);
                    stick.SetActive(false);
                    rb.velocity = Vector2.zero; 
                }
                i++; 
        }
    }

    private void ControllerPadFullScreen()
    {
        int i = 0;
        while (i < Input.touchCount)
        {
            Touch t = Input.GetTouch(i);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(t.position);

            float percentOfScreen = amount / 100;
            float rangeOfScreen = (screenWidth * percentOfScreen) + (screenWidth / -2);
            if (t.phase == TouchPhase.Began && OneJoyStick)
            {
              leftTouch = t.fingerId;
              touchPosition = touchPos;
              panel.SetActive(true);
              stick.SetActive(true);
              panel.transform.position = touchPosition;
              stick.transform.position = touchPosition;
                OneJoyStick = false;
            }
            else if (t.phase == TouchPhase.Moved && leftTouch == t.fingerId)
            {
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
                rb.velocity = moveDirection * moveSpeed;
            }
            else if ((t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled) && leftTouch == t.fingerId)
            {
                leftTouch = 99;
                panel.SetActive(false);
                stick.SetActive(false);
                rb.velocity = Vector2.zero;
                OneJoyStick = true; 
            }
            i++;
        }
    }

    private void ControlSlide()
    {
        
        if (characterToMove.transform.position.x + quantityToAdd>=screenWidth/2 && rb.velocity.x > 0f)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
           
        } else if(characterToMove.transform.position.x - quantityToAdd<=screenWidth/-2 && rb.velocity.x < 0f)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
        if(characterToMove.transform.position.y + quantityToAdd>=screenHeight/2 && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }else if(characterToMove.transform.position.y - quantityToAdd<=screenHeight/-2 && rb.velocity.y < 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
    }
    private void testMode()
    {


        if (Input.GetKeyDown(KeyCode.W))
        {
            rb.velocity = new Vector2(0f, 0.5f) * moveSpeed;
          

        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            rb.velocity = new Vector2(0.5f, 0f) * moveSpeed;

        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }


        if (Input.GetKeyDown(KeyCode.A))
        {
            rb.velocity = new Vector2(-0.5f, 0f) * moveSpeed;

        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }


        if (Input.GetKeyDown(KeyCode.S))
        {
            rb.velocity = new Vector2(0f, -0.5f) * moveSpeed;

        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
    }
    void fixPosition()
    {
        if (characterToMove.transform.position.x + quantityToAdd > screenWidth/2)
        {
            characterToMove.transform.position = new Vector2(screenWidth/2 - quantityToAdd, characterToMove.transform.position.y);
        }
        else if (characterToMove.transform.position.x - quantityToAdd < screenWidth / -2)
        {
            characterToMove.transform.position = new Vector2((screenWidth /-2) + quantityToAdd, characterToMove.transform.position.y);
        }
        if (characterToMove.transform.position.y + quantityToAdd > screenHeight/2)
        {
            characterToMove.transform.position = new Vector2(characterToMove.transform.position.x, screenHeight / 2 - quantityToAdd);
        }
        else if (characterToMove.transform.position.y - quantityToAdd < screenHeight / -2)
        {
            characterToMove.transform.position = new Vector2(characterToMove.transform.position.x, (screenHeight / -2) + quantityToAdd);
        }
    }

    public float get_Speed()
    {
        return moveSpeed; 
    }
    public void set_Speed(float Speed)
    {
        moveSpeed = Speed; 
    }
    public void fastSpeed()
    {
        moveSpeed = 5f; 
    }
    public void lowlySpeed()
    {
        moveSpeed = 2f; 
    }
}
