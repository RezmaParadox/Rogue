using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BossState
{
    WanderB,

    FollowB,

    DieB,

    AttackB
};
public enum BossType
{
    meleeBoss
};
public class boss : MonoBehaviour
    
{
    public static boss instance;
    GameObject player; //Se hace referencia al jugador

    static BossState currState = BossState.WanderB;
    public BossType BossType;

    public float range;
    public float speed;
    public float attackRange;

    public float cd;
    private bool cdAttack = false;


    private Transform target;
    protected Vector2 direction;
    private Animator animation2;
    public Vector3 dir;
    //variables de estado
    private bool chooseDirection = false;
    private bool dead = false;
    private Vector3 randomDirection;
    public GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;
        animation2 = GetComponent<Animator>();
    }

    private void GetInput()
    {
        direction = Vector2.zero;
    }
    // Update is called once per frame
    void Update()
    {
        switch (currState)
        {
            case BossState.WanderB:
                animation2.SetInteger("cambio", 0);
                WanderB();
                break;
            case BossState.FollowB:
                animation2.SetInteger("cambio", 1);
                FollowB();
                
                break;
            case BossState.DieB:
                DeathBoss();
                break;
            case BossState.AttackB:
                //animation.SetInteger("cambio", 2);
                AttackB();
                break;

        }

        //Cambio de estado del enemigo
        if (playerInRange(range) && currState != BossState.DieB) //Jugador en rango 
        {
            currState = BossState.FollowB;

        }
        else if (!playerInRange(range) && currState != BossState.DieB) //Jugador fuera de rango
        {
            currState = BossState.WanderB;
            animation2.SetInteger("cambio", 0);
        }

        //Condicion para realizar daño al jugador
        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
        {
            currState = BossState.AttackB;
        }


        dir = target.position - transform.position;
        if (currState != BossState.DieB)
        {
            animation2.SetFloat("x", dir.x);
            animation2.SetFloat("y", dir.y);
        }


    }
    private bool playerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    void WanderB()
    {
        if (playerInRange(range))
        {
            currState = BossState.FollowB;
        }

    }

    void FollowB()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        

    }

    void AttackB()
    {
        if (!cdAttack)
        {
            switch (BossType)
            {
                case BossType.meleeBoss:
                    gameController.DamagePlayer(1);
                    StartCoroutine(CD());
                    break;
            }

        }

    }
    private IEnumerator CD()
    {
        cdAttack = true;
        if (cdAttack == true)
        {
            animation2.SetInteger("cambio", 2);
        }
        yield return new WaitForSeconds(cd);
        cdAttack = false;
    }

    public void DeathBoss()
    {
        SceneManager.LoadScene(6);
        healthB = 6;
        moveSpeedB = 5f;
        fireRateB = 1f;
        gameController.KillPlayer();
        Debug.Log("salio del juego");
    }
    //---------------------------------------------------------------------------------------------------------------------------
    private static float healthB = 6;
    private static int maxHelthB = 6;

    private static float moveSpeedB = 5f;

    private static float fireRateB = 1f;

    public static float HealthBoss { get => healthB; set => healthB = value; }
    public static int MaxHelthBoss { get => maxHelthB; set => maxHelthB = value; }
    public static float MoveSpeedBoss { get => moveSpeedB; set => moveSpeedB = value; }
    public static float FireRateBoss { get => fireRateB; set => fireRateB = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static void DamageBoss(int damage)
    {
        healthB -= damage;
        if (HealthBoss <= 0)
        {
            currState = BossState.DieB;
        }
    }
}

//-----------------------------------------------------------------------------------------------------------------
