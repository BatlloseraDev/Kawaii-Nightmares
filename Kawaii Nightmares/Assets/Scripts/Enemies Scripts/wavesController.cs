using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wavesController : MonoBehaviour
{
   
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject boss;
    
    //Crear la clase Enemy
    [SerializeField]
    private int NumEnemies;
    [SerializeField]
    private int NumResEnemies;
    private int NumTotalEnemies;
    [SerializeField]
    private int NumWave= 0;
    private int NumState;
    [SerializeField]
    private bool generatingEnemies = false;

    void Start()
    {
        NumEnemies = 0;
        NumResEnemies = 0; 
    }

    // Update is called once per frame
    void Update()
    {
        ControllerWave();
       


        if (generatingEnemies)
        {
            if (NumTotalEnemies == NumEnemies*2 && NumWave<4) 
            {
                generatingEnemies = false;
                StopAllCoroutines(); 
            }
            else if(NumTotalEnemies == 19 && NumWave == 4)
            {
                generatingEnemies = false;
                StopAllCoroutines();
            }
            
        }

    }

    private void calculateEnemies()
    {
        if (NumWave == 1) NumEnemies = 5 + (NumState * 2);
        else if (NumWave == 2) NumEnemies = 4 + NumState;
        else if (NumWave == 3) NumEnemies = 1;
        else if (NumWave == 4) NumEnemies = 19;
        
    }
    private void ControllerWave()
    {
        if (NumResEnemies == 0 && !generatingEnemies)
        {
            
            if (NumWave == 5)
            {
                NumTotalEnemies = 0; 
                NumState++;
                NumWave = 1;
                generateEnemies();
                generatingEnemies = true; 

            }else
            {
                NumTotalEnemies = 0; 
                StopAllCoroutines(); 
                NumWave++;
                generateEnemies();
                generatingEnemies = true; 
            }
   
        }
    }

    private void generateEnemies()
    {
        calculateEnemies();
        if (NumWave == 1)
        {
            Debug.Log(NumEnemies); 
            //añadir enemigos
            StartCoroutine(spawn(enemy1, 0.8f, NumEnemies, "Animation_1"));
            StartCoroutine(spawn(enemy1, 0.8f, NumEnemies, "Animation_2")); 
        }
        else if(NumWave == 2)
        {
            StartCoroutine(spawn(enemy2, 1.6f, NumEnemies, "Animation_1"));
            StartCoroutine(spawn(enemy2, 1.6f, NumEnemies, "Animation_2"));
        }
        else if (NumWave == 3)
        {
            StartCoroutine(spawn(enemy3, 0.1f, NumEnemies, "Animation_1"));
            StartCoroutine(spawn(enemy3, 0.1f, NumEnemies, "Animation_2"));
        }
        else if (NumWave == 4)
        {
            StartCoroutine(spawn(enemy1, 1f, 5, "Animation_3")); 
            StartCoroutine(spawn(enemy1, 1f, 5, "Animation_4"));
            StartCoroutine(spawn(enemy2, 2f, 3, "Animation_3"));
            StartCoroutine(spawn(enemy2, 2f, 3, "Animation_4"));
            StartCoroutine(spawn(enemy3, 0.1f, 1, "Animation_1"));
            StartCoroutine(spawn(enemy3, 0.1f, 1, "Animation_2"));
            StartCoroutine(spawn(enemy3, 0.1f, 1, "Animation_3"));
        }
        else if (NumWave == 5)
        {
            StartCoroutine(spawn(boss, 0.1f, 1, "appear"));
        }
    }

    public int get_numWave()
    {
        return NumWave;
    }
    public int get_numState()
    {
        return NumState;
    }
    public void enemyDie()
    {
        NumResEnemies -= 1; 
    }

    IEnumerator spawn(GameObject enem, float wait, int amount, string animation)
    {
        for(int i= 0; i<amount; i++)
        {
            ;
            yield return new WaitForSeconds(wait);
            GameObject enemyCopy =  Instantiate(enem );
            enemyCopy.SetActive(true);
            enemyCopy.GetComponent<Animator>().SetTrigger(animation);
            enemyCopy.name = enem.name;  
            NumTotalEnemies++;
            NumResEnemies++; 
        }
        
    }
}
