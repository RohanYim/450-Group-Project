using UnityEngine;
using System.Collections;

public class Blade : MonoBehaviour
{
    public float swingSpeed = 1f;
    public Vector3 swingRotation = new Vector3(0f, 0f, -85f);
    private Quaternion originalRotation;
    private Quaternion targetRotation;
    public bool isSwinging = false;
    private bool canDealDamage = false;

    void Start()
    {
        originalRotation = transform.localRotation;
    }

    public void StartSwing()
    {
        if (!isSwinging)
        {
            StartCoroutine(SwingBlade());
        }
    }

    // swing blade to attack
    private IEnumerator SwingBlade()
    {
        isSwinging = true;  // no swinging before the previous move is done
        canDealDamage = true;  // one swing one damage
        float time = 0f;
        targetRotation = Quaternion.Euler(swingRotation) * originalRotation;

        // swinging down
        while (time < 1f)
        {
            transform.localRotation = Quaternion.Lerp(originalRotation, targetRotation, time);
            time += Time.deltaTime * swingSpeed;
            yield return null;
        }

        time = 0f;
        // swinging up
        while (time < 1f)
        {
            transform.localRotation = Quaternion.Lerp(targetRotation, originalRotation, time);
            time += Time.deltaTime * swingSpeed;
            yield return null;
        }

        transform.localRotation = originalRotation;
        isSwinging = false;
        canDealDamage = false;
    }

    // give 1 damage when the blade touches the mob
    void OnTriggerEnter2D(Collider2D other)
    {
        if (canDealDamage && other.gameObject.CompareTag("Mob"))
        {
            var mob = other.GetComponent<Mob>();
            if (mob != null)
            {
                mob.TakeDamage(1);
                canDealDamage = false;
            }
        }
    }

    // continuous collision detection during blade swing
    void OnTriggerStay2D(Collider2D other)
    {
        if (canDealDamage && isSwinging && other.gameObject.CompareTag("Mob"))
        {
            var mob = other.GetComponent<Mob>(); 
            if (mob != null)
            {
                mob.TakeDamage(1); 
                canDealDamage = false; 
            }
        }
    }

}