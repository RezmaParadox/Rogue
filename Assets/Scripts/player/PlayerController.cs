using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rigidbody;

    //Disparos
    public GameObject bulletPrefab;
    public float bulletSpeed;
    private float lastFire;
    public float fireDelay;

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Estadisticas del pj
        speed = gameController.MoveSpeed;
        fireDelay = gameController.FireRate;

        //Movimiento
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            anim.SetFloat("Horizontal", horizontal);
            anim.SetFloat("Vertical", vertical);
        }

        if (speed != 0)
        {
            anim.SetFloat("speed", speed);
        }


        //Movimiento
        rigidbody.velocity = new Vector3(horizontal * speed, vertical * speed, 0);

        //Disparos
        float shootHorizontal= Input.GetAxis("ShootHorizontal");
        float shootVerntical= Input.GetAxis("ShootVertical");

        if((shootHorizontal!=0 || shootVerntical !=0) && Time.time> lastFire + fireDelay)
        {
            Shoot(shootHorizontal,shootVerntical);
            lastFire = Time.time;

        }


    }
    void Shoot(float x, float y)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector3(
            (x < 0) ? Mathf.Floor(x) * bulletSpeed : Mathf.Ceil(x) * bulletSpeed, //velocidad en el eje x, para mantener constante
            (y < 0) ? Mathf.Floor(y) * bulletSpeed : Mathf.Ceil(y) * bulletSpeed,
            0 //velocidad en z, es cero, porque es un juego en 2D
            );
    }
}
