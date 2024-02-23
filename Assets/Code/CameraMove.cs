using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {
    public float damping = 1.5f;
    public Transform _target;
    public Vector2 offset = new Vector2(2f, 1f);
    public float deadZone = 6f; 

    private bool faceLeft;
    private int lastX;
    private float dynamicSpeed;
    private Camera _cam;
    private Vector3 targetPosition;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.3F; 

    void Start()
    {
        offset = new Vector2(Mathf.Abs(offset.x), offset.y);
        FindPlayer();
        _cam = gameObject.GetComponent<Camera>();
        targetPosition = transform.position; 
    }

    public void FindPlayer()
    {
        lastX = Mathf.RoundToInt(_target.position.x);
        targetPosition = new Vector3(_target.position.x + offset.x, _target.position.y + offset.y, transform.position.z);
        transform.position = targetPosition;
    }


    void Update()
    {
        if (_target)
        {
            // 检测水平方向的变化
            int currentX = Mathf.RoundToInt(_target.position.x);
            faceLeft = currentX < lastX;
            lastX = currentX;

            // 计算水平方向的目标位置，包括水平偏移
            Vector3 horizontalTargetPosition = faceLeft ? new Vector3(_target.position.x - offset.x, transform.position.y, transform.position.z) : new Vector3(_target.position.x + offset.x, transform.position.y, transform.position.z);

            // 在水平方向上，仅当目标移动超出死区时才更新摄像机位置
            if (Mathf.Abs(transform.position.x - _target.position.x) > deadZone)
            {
                transform.position = Vector3.SmoothDamp(transform.position, horizontalTargetPosition, ref velocity, smoothTime);
            }

            // 在竖直方向上，摄像机始终跟随玩家，无死区
            Vector3 verticalTargetPosition = new Vector3(transform.position.x, _target.position.y + offset.y, transform.position.z);
            transform.position = new Vector3(transform.position.x, Vector3.SmoothDamp(transform.position, verticalTargetPosition, ref velocity, smoothTime).y, transform.position.z);
        }
    }





}
