using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 1f;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
