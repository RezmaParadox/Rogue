using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string Name;

    public string Description;

    public Sprite itemImage;

}
public class CollectionControler : MonoBehaviour
{
    //Variables para el cambio de estadisticas del pj, para los items que se recojan
    
    public Item item;

    public float healthChange;

    public float moveSpeedChange;

    public float attackSpeedChange;

    public float bulletSizeChange;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.itemImage;
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            gameController.HealthPlayer(healthChange);
            gameController.MoveSpeedChange(moveSpeedChange);
            gameController.FireRateChange(attackSpeedChange);
            gameController.BulletSizeChange(bulletSizeChange);  
            Destroy(gameObject);
        }
    }
}
