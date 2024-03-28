using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirWall : MonoBehaviour
{

    public GameObject bossObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (bossObject == null) {
        
            Destroy(gameObject);
        }
    }
}
