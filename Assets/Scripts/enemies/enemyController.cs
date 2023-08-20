using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Wander,

    Follow,

    Die,

    Attack
};

public enum EnemyType
{
    melee,

    range
};
public class enemyController : MonoBehaviour
{
    GameObject player; //Se hace referencia al jugador

    public EnemyState currState=EnemyState.Wander;
    public EnemyType enemyType;

    public float range;
    public float speed;
    public float attackRange;

    public float cd;
    private bool cdAttack=false;

    
    private Transform target;
    protected Vector2 direction;
    private  Animator animation;
    public Vector3 dir;
    //variables de estado
    private bool chooseDirection=false;
    private bool dead=false;
    private Vector3 randomDirection;
    public GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;
        animation = GetComponent<Animator>();
    }
    
    private void GetInput()
    {
        direction=Vector2.zero;
    }
    // Update is called once per frame
    void Update()
    {
        switch (currState)
        {
            case EnemyState.Wander:
                Wander();
                break;
            case EnemyState.Follow:
                Follow();
                break;
            case EnemyState.Die:
                Death();
                break;
            case EnemyState.Attack:
                Attack();
                break;

        } 
        
        //Cambio de estado del enemigo
        if(playerInRange(range) && currState != EnemyState.Die) //Jugador en rango 
        {
            currState = EnemyState.Follow;

        }
        else if(!playerInRange(range) && currState != EnemyState.Die) //Jugador fuera de rango
        {
            currState=EnemyState.Wander;
        }

        //Condicion para realizar daño al jugador
        if(Vector3.Distance(transform.position, player.transform.position)<= attackRange)
        {
            currState= EnemyState.Attack;
        }


        dir= target.position - transform.position;
        if(currState != EnemyState.Die)
        {
            animation.SetFloat("x", dir.x);
            animation.SetFloat("y", dir.y);
        }
        
        
    }
    private bool playerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position)<=range;
    }

   /* private IEnumerator chooseDirection2()
    {
        chooseDirection=true;
        yield return new WaitForSeconds(Random.Range(2f, 8f));
        randomDirection = new Vector3(0,0, Random.Range(0,360));
        Quaternion nextRotation = Quaternion.Euler(randomDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(0.5f, 2.5f));
        chooseDirection=false;
    }*/
    void Wander()
    {
      /*  if (!chooseDirection) {
            StartCoroutine(chooseDirection2());
        }
        transform.position += -transform.right* speed*Time.deltaTime;*/
        if (playerInRange(range))
        {
            currState = EnemyState.Follow;
        }
        
    }

    void Follow()
    {
        transform.position=Vector2.MoveTowards(transform.position,player.transform.position,speed*Time.deltaTime);
        
    }

    void Attack()
    {
        if (!cdAttack)
        {
            switch (enemyType)
            {
                case EnemyType.melee:
                    gameController.DamagePlayer(1);
                    StartCoroutine(CD());
                    break;
                case EnemyType.range:
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity)as GameObject;
                    bullet.GetComponent<bulletController>().GetPlayer(player.transform);
                    bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
                    bullet.GetComponent<bulletController>().isEnemyBullet = true;
             
                    StartCoroutine(CD());
                    break;
            }
            
        }

    }
    private IEnumerator CD()
    {
        cdAttack = true;
        yield return new WaitForSeconds(cd);
        cdAttack = false;
    }

    public void Death()
    {
        Destroy(gameObject);
    }

}
