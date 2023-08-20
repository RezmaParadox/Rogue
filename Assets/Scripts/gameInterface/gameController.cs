using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class gameController : MonoBehaviour
{
    public static gameController instance;

    //Estadisticas del jugador

    private static float health = 6;

    private static int maxHelth = 6;

    private static float moveSpeed = 5f;

    private static float fireRate = 1f;

    private static float bulletSize = 0.5f;


    public static float Health { get => health; set => health = value; }
    public static int MaxHelth { get => maxHelth; set => maxHelth = value; }
    public static float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }  
    public static float FireRate { get => fireRate; set => fireRate = value; }
    public static float BulletSize { get => bulletSize; set => bulletSize = value; }

    GameObject player;

    public Text healthText;

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance==null) 
        {
            instance = this;
        }
    }

    void Update()
    {
        healthText.text = "Health: " + health;
        //player = GameObject.FindGameObjectWithTag("Player");
    }
    public static void DamagePlayer(int damage)
    {
        health -= damage;
        if (Health <= 0)
        {
            KillPlayer();
        }
    }

    public static void HealthPlayer(float healthAmount)
    {
        health=Mathf.Min(maxHelth, health + healthAmount);
    }

    public static void MoveSpeedChange(float speed)
    {
        moveSpeed += speed;
    }

    public static void FireRateChange(float rate)
    {
        fireRate -= rate;
    }

    public static void BulletSizeChange(float size)
    {
        bulletSize += size;
    }
    public static void KillPlayer()
    {

        //Destroy(GameObject.FindGameObjectWithTag("Player"));
        SceneManager.LoadScene(6);
        health = 6;
        moveSpeed = 5f;
        fireRate = 1f;
        bulletSize = 0.5f;
        Debug.Log("salio del juego");
    }
}
