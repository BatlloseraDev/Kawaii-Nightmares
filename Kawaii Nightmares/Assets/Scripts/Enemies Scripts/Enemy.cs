using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public BulletEnemy[] bullets;
   // public GameObject bullet;
    [SerializeField]
    private float fireRate;
    [SerializeField]
    private float _coldown;
    private Transform playerpos; 
    public AudioClip fireSound;
    public float FireVolume;
    private bool startFire = false;
    private wavesController waveControl;
    private float plusTimefire= 0;


    [Header("Boss Properties")]
    #region BossVariables
    public DialogueTrigger dialogue;
    private GameObject textBox; 
    private bool FireBoss = false;
    [SerializeField]
    private float _coldownBoss= 0;
    [SerializeField]
    private int routineShoot;
    private float circularShootH;
    private float circularShootAntiH;
    private int lifesBoss; 

    #endregion



    private void Awake()
    {
        waveControl = GameObject.FindGameObjectWithTag("GameManager").GetComponent<wavesController>();
        playerpos = GameObject.FindGameObjectWithTag("Player").transform;
      
    }
    private void Start()
    {
        if (gameObject.name == "boss")
        {
            textBox = GameObject.FindGameObjectWithTag("dialoguebox");
            lifesBoss = 2 + waveControl.get_numState(); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_coldown > 0) _coldown -= Time.deltaTime;
        if (_coldownBoss > 0) _coldownBoss -= Time.deltaTime; 
        
        if(gameObject.name == "boss" && !FireBoss)checkBossShoot(); 
        Fire();
    }
    public void Fire()
    {
        if (bullets.Length == 0) return;
        if (_coldown > 0) return;
        
        TypeOfShoot(gameObject.name);
    }

    void TypeOfShoot(string name)
    {
        if (waveControl.get_numWave() == 4)
        {

            plusTimefire = 2f + (4/ (waveControl.get_numState()+1)); 
        }
        else if(waveControl.get_numWave() < 4)
        {
            plusTimefire = (2 / (waveControl.get_numState() + 1)); 
        }

        if (name == "enemy1")
        {
            instatiateBullet("bullet1", 2f, 0f, -5f);
            _coldown = fireRate + plusTimefire ;
        }
        else if (name == "enemy2")
        {
            instatiateBullet("bullet2", 3f, playerpos.position.x, playerpos.position.y);
            _coldown = fireRate + plusTimefire; 
        }
        else if (name == "enemy3" && startFire)
        {
            instatiateBullet("bullet1", 5f, 0f, -1f);
            instatiateBullet("bullet2", 5f, playerpos.position.x, playerpos.position.y);
            _coldown = fireRate + plusTimefire;

        }
        else if(name =="boss" && FireBoss)
        {
            TypeOfShootBoss();
        }
    }

    private void instatiateBullet(string nameBullet, float lifeTime,float dirx, float diry)
    {
        GameObject newBullet = Instantiate(bullets[0].gameObject, transform.position, gameObject.transform.rotation);
        newBullet.SetActive(true);
        newBullet.name = nameBullet;
        newBullet.GetComponent<BulletEnemy>().ChangeVelocityAndDirection(dirx, diry);
        Destroy(newBullet, lifeTime);
    }

    public void Dissapear()
    {
        waveControl.enemyDie(); 
        Destroy(gameObject);
    }

    public void StartToFire()
    {
        startFire = true; 
    }

    public void DMG(float dmg)
    {
        HP -= dmg; 
        if(HP<= 0 && !isDead && gameObject.name!="boss")
        {
            isDead = true; 
            waveControl.enemyDie();
            Die(); 
        }
        else if(HP<=0 && !isDead && gameObject.name == "boss")
        {
            if(lifesBoss > 0)
            {
                lifesBoss--;
                ChangePositionBoss();
            }
        }
    }




#region Boss
    public void ReadyToFight()
    {
        startFire = true;   
    }
    private void checkBossShoot()
    {
        bool txtDone = textBox.GetComponent<Animator>().GetBool("IsOpen"); 
        if(!txtDone && startFire)
        {
            FireBoss = true; 
        }
    }
   public void testChangeText()
    {
        Dialogue d = dialogue.dialogue;
        string s = "Second_Phrase";
        d.sentences[0] = s; 

    }

    private void TypeOfShootBoss()
    {
       if(_coldownBoss<= 0)
       {    
            routineShoot=  Random.Range((int)1, (int)6);
            _coldownBoss = Random.Range(30f, 60f);
            circularShootH = 0;
            circularShootAntiH = 0;
       }

        switch (routineShoot)
        {
            case 1: // shoot 1
          
                fireRate = 0.8f;
                angleShoot(90,270,5);
                _coldown = fireRate; 
                return;
            case 2: //shoot 2
          
                fireRate = 0.2f;
                angleShoot(0, 360, 4);
                _coldown = fireRate;
                return;
            case 3: //shoot 3
                
                fireRate = 1.5f;
                angleShoot(90, 270, 5);
                _coldown = fireRate;
                return;
            case 4: //shoot 4
                
                fireRate = 1f;
                angleShoot(135, 225, 3);
                _coldown = fireRate;
                return;
            case 5: // shoot5
               
                fireRate = 0.2f;
                instatiateBullet("bullet2", 5f, -30f, -30f);
                instatiateBullet("bullet2", 5f, 30f, -30f);
                instatiateBullet("bullet2", 5f, playerpos.position.x, playerpos.position.y);
                _coldown = fireRate;
                return;
            case 6: // shoot6
             
                fireRate = 0.2f;
                circularH();
                circularAntiH(); 
                _coldown = fireRate;
               
                return;
            default:
                Debug.Log("Estoy en default del boss");
                _coldown = fireRate;
                return; 
        }

    }

   private void angleShoot(float startAngle, float endAngle, int nShoots)  
   {
        float angleStep = (endAngle - startAngle) / nShoots;
        float angle = startAngle;
        for (int i= 0; i<=nShoots; i++)
        {
            float dirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float dirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);
            instatiateBullet("bullet2", 5f, dirX,dirY);
            angle += angleStep;
        }
   }
   
    private void ChangePositionBoss()
    {
        //cambiar la posicion, animacion;
    }

    private void circularH()
    {
        float dirX = transform.position.x + Mathf.Sin((circularShootH * Mathf.PI) / 180f);
        float dirY = transform.position.y + Mathf.Cos((circularShootH * Mathf.PI) / 180f);
        instatiateBullet("bullet2", 5f, dirX, dirY);
        circularShootH += 10; 
    }
    private void circularAntiH()
    {
        float dirX = transform.position.x + Mathf.Sin((circularShootAntiH * Mathf.PI) / 180f);
        float dirY = transform.position.y + Mathf.Cos((circularShootAntiH * Mathf.PI) / 180f);
        instatiateBullet("bullet2", 5f, dirX, dirY);
        circularShootAntiH -= 10; 
    }
#endregion
}
