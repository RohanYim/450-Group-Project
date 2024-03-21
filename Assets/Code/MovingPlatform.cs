using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public float moveDistance = 5.0f; // 移动平台的最大距离
    public float speed = 2.0f; // 移动平台的速度
    private Vector3 startPosition;
    private bool movingRight = true; // 平台当前的移动方向
    public Boss boss; // 对Boss组件的引用

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // 如果Boss被消灭了，平台才会移动
        if (boss == null)
        {
            MovePlatform();
        }
    }

    void MovePlatform()
    {
        // 计算平台当前位置与起始位置的距离
        float distance = transform.position.x - startPosition.x;

        // 如果到达了左侧或右侧的最大距离，改变方向
        if (distance >= moveDistance)
        {
            movingRight = false;
        }
        else if (distance <= -moveDistance)
        {
            movingRight = true;
        }

        // 根据当前方向移动平台
        if (movingRight)
        {
            transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
        }
    }

}
