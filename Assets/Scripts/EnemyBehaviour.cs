using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour
{

    private Animator animator;
    private AudioSource source;
    private GameObject Player;

    public int health ;
    private float speed = 0.5f;
    private bool isCollision;



    void Start()
    {
        source = GetComponent<AudioSource>();
        Player = GameObject.Find("Player");
        animator = gameObject.GetComponent<Animator>();

    }

    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
            GameObject.Find("Score").GetComponent<Score>().UpdateScore();
        }
        VerifyPosition();
        transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, speed * Time.deltaTime);
        if (isCollision)
        {
            animator.SetTrigger("Attack");
        }
  
    }
    void FixedUpdate()
    {
        isCollision = false;

    }
    public void TakeDamage(int damage)
    {
        health -= damage;
    }
    public void Damaged(int buffAttacks)
    {
       
        if (buffAttacks > 0)
        {
            health -= 25;
        }
        else { 
            health -= 10; 
        }
       

    }
    private void VerifyPosition()
    {
        if (transform.position.x > Player.transform.position.x)
        {
            transform.localScale = new Vector2(-3, 3);
        }
        else if (transform.position.x < Player.transform.position.x)
        {
            transform.localScale = new Vector2(3, 3);
        }
    }
    public void SoundHit()
    {
        source.Play();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerBehaviour>().Damaged();
            isCollision = true;
           
        }
    }


}
