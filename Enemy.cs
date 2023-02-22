using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;

    public float movHor = 1f;
    public float speed = 3f;

    public bool isGroundFloor = true;
    public bool isGroundFront = false;

    public LayerMask groundLayer;
    public float frontGrndRayDist = 0.25f;
    public float floorCheckY = 0.52f;
    public float frontCheck = 0.51f;
    public float frontDist = 0.001f;

    public int scoreGive = 50;

    private RaycastHit2D hit;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        //Evitar caer en precipicios
        isGroundFloor = (Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - floorCheckY, transform.position.z),
            new Vector3(movHor, 0, 0), frontGrndRayDist, groundLayer));// Hace raycast a la direccion que nos movemos para ver si hay suelo

        if (isGroundFloor)
        
            movHor = movHor * -1;//Cambiamos direccion
        

        //Cuando choca con pared
        if(Physics2D.Raycast(transform.position,new Vector3(movHor,0,0), frontCheck,groundLayer))//Si raycast detecta choque hacia nos movemos
            movHor = movHor * -1;//Cambiamos direccion


        //Cuando choca con otro enemigo
        hit = Physics2D.Raycast(new Vector3(transform.position.x + movHor*frontCheck, transform.position.y, transform.position.z),
            new Vector3(movHor, 0,0), frontDist);

        if (hit != null)
            if (hit.transform != null)
                if (hit.transform.CompareTag("Enemy"))
                    movHor = movHor * -1;//Cambiamos direccion







    }
    void FixedUpdate()
    {
        rb.velocity = new Vector2(movHor * speed, rb.velocity.y);


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Dañar player

        if (collision.gameObject.CompareTag("Player"))
        {
            //Dañar player
            Player.obj.getDamage();//Activamos funcion de tocado en script del player

        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Destruir enemigo
        if (collision.gameObject.CompareTag("Player"))

        {
            //Destruir enemigo
            AudioManager.obj.playEnemyHit();
            getKilled();

        }


    }



    private void getKilled()
    {
        FXManager.obj.showPop(transform.position);
        gameObject.SetActive(false);


    }


}
