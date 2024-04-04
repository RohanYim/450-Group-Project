using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 1f;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // shoot left
        rb.velocity = transform.right * -speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            player.TakeDamage(damage);

            Vector2 knockbackDirection = collision.collider.transform.position - transform.position;
            knockbackDirection = knockbackDirection.normalized; 


            float knockbackForce = 20f; 
            Vector2 force = knockbackDirection * knockbackForce;


            player.AddKnockback(force);
        }
        Destroy(gameObject);
    }
}