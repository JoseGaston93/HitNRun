using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player obj;

    public int lives = 3;

    public bool isGrounded = false;
    public bool isMoving = false;  
    public bool isImmune = false;

    public float speed = 5f;
    public float jumpForce = 3f;
    public float movHor;

    public float immuneTimeCnt = 0f;
    public float immumeTime = 0.5f;

    public LayerMask groundLayer;
    public float radius = 0.3f;
    public float groundRayDist = 0.5f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spr;

    void Awake()
    {
        obj = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();//Asignamos componentes a nuestras variables
        anim= GetComponent<Animator>();//Asignamos componentes a nuestras variables
        spr = GetComponent<SpriteRenderer>();//Asignamos componentes a nuestras variables
    }

    // Update is called once per frame
    void Update()
    {
        if (Game.obj.gamePaused) 
        {
            movHor = 0f;
            return;
        }
        movHor = Input.GetAxisRaw("Horizontal");//Movimiento horizontal

        isMoving = (movHor != 0f);//Marcar que se esta moviendo

        isGrounded = Physics2D.CircleCast(transform.position, radius, Vector3.down, groundRayDist, groundLayer);//Marcar si toca suelo

        if (Input.GetKeyDown(KeyCode.Space))//Si pulsa espacio
            jump();//salta

        if (isImmune)
        {
            spr.enabled = !spr.enabled;

            immuneTimeCnt -= Time.deltaTime;

            if (immuneTimeCnt <= 0)
            {
                isImmune = false;
                spr.enabled=true;
            }

        }

        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);



        flip(movHor);


    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(movHor * speed, rb.velocity.y);

    }


    private void goImmune()
    {
        isImmune= true;
        immuneTimeCnt = immumeTime;


    }


    public void jump()
    {
        if (!isGrounded) return;
        
            rb.velocity = Vector2.up * jumpForce;
            AudioManager.obj.playJump();

    }

    private void flip(float _xValue)
    {
        Vector3 theScale = transform.localScale;

        if (_xValue < 0)
            theScale.x = Mathf.Abs(theScale.x) * -1;
        else
        if (_xValue > 0)
            theScale.x = Mathf.Abs(theScale.x);

        transform.localScale = theScale;
    }

    public void getDamage()
    {
        lives--;//Cuando te dañan pierdes vida
        AudioManager.obj.playHit();

        UiManager.obj.updateLives();

        goImmune();

        if (lives <= 0)
        {
            FXManager.obj.showPop(transform.position);
            Game.obj.gameOver();
        }
          

    }

    public void addLive()
    {
        lives++;
        if (lives > Game.obj.maxLives)
        {
            lives = Game.obj.maxLives;
        }
    }

    void OnDestroy()
    {
        obj= null;
    }
}
