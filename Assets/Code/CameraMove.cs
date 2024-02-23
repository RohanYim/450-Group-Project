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
            // detect the change in vertical way
            int currentX = Mathf.RoundToInt(_target.position.x);
            faceLeft = currentX < lastX;
            lastX = currentX;

            // Calculate target position in the horizontal way, including the offset
            Vector3 horizontalTargetPosition = faceLeft ? new Vector3(_target.position.x - offset.x, transform.position.y, transform.position.z) : new Vector3(_target.position.x + offset.x, transform.position.y, transform.position.z);

            // In horizontal way, the camera only moves when the target moves beyond the dead zone
            if (Mathf.Abs(transform.position.x - _target.position.x) > deadZone)
            {
                transform.position = Vector3.SmoothDamp(transform.position, horizontalTargetPosition, ref velocity, smoothTime);
            }

            // In vertical way, no dead zone
            Vector3 verticalTargetPosition = new Vector3(transform.position.x, _target.position.y + offset.y, transform.position.z);
            transform.position = new Vector3(transform.position.x, Vector3.SmoothDamp(transform.position, verticalTargetPosition, ref velocity, smoothTime).y, transform.position.z);
        }
    }





}
