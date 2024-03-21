using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public float moveDistance = 5.0f; // �ƶ�ƽ̨��������
    public float speed = 2.0f; // �ƶ�ƽ̨���ٶ�
    private Vector3 startPosition;
    private bool movingRight = true; // ƽ̨��ǰ���ƶ�����
    public Boss boss; // ��Boss���������

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // ���Boss�������ˣ�ƽ̨�Ż��ƶ�
        if (boss == null)
        {
            MovePlatform();
        }
    }

    void MovePlatform()
    {
        // ����ƽ̨��ǰλ������ʼλ�õľ���
        float distance = transform.position.x - startPosition.x;

        // ��������������Ҳ�������룬�ı䷽��
        if (distance >= moveDistance)
        {
            movingRight = false;
        }
        else if (distance <= -moveDistance)
        {
            movingRight = true;
        }

        // ���ݵ�ǰ�����ƶ�ƽ̨
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
