using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CustomCamera : MonoBehaviour
{
    public Vector2 boundsOffset;
    public float targetOffset;
    public float cameraSpeed;

    public GameObject target;
    public Collider2D cameraBounds;

    private Vector2 xLimits;
    private Vector2 yLimits;

    private Vector2 maxBound;
    private Vector2 minBound;

    private Camera _camera;
    private float cameraHalfWidth;
    private float cameraTimer = 0.0f;
    private Vector3 lastPos = Vector3.zero;

    private bool locked = false;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        maxBound = cameraBounds.bounds.max;
        minBound = cameraBounds.bounds.min;
    }

    private void FixedUpdate()
    {
        Vector3 currentTargetPos = target.transform.position;
        Vector3 currentPos = transform.position;

        transform.position = new Vector3(Mathf.Lerp(transform.position.x, target.transform.position.x, cameraTimer * cameraSpeed),
                                         Mathf.Lerp(transform.position.y, target.transform.position.y, cameraTimer * cameraSpeed),
                                         transform.position.z);

        CheckBounds(currentPos);

        cameraTimer += Time.deltaTime;
        if ((currentTargetPos - lastPos).magnitude >= targetOffset)
            cameraTimer = 0.0f;

        lastPos = target.transform.position;
    }
    
    private void CheckBounds(Vector3 currentPos)
    {
        cameraHalfWidth = _camera.aspect * _camera.orthographicSize;

        xLimits = new Vector2(transform.position.x + boundsOffset.x + cameraHalfWidth, transform.position.x + boundsOffset.x - cameraHalfWidth);
        yLimits = new Vector2(transform.position.y + boundsOffset.y + _camera.orthographicSize, transform.position.y + boundsOffset.y - _camera.orthographicSize);

        if (xLimits.x > maxBound.x || xLimits.y < minBound.x)
        {
            transform.position = new Vector3(currentPos.x, transform.position.y, transform.position.z);

            if (yLimits.x > maxBound.y || yLimits.y < minBound.y)
            {
                transform.position = currentPos;
            }
        }
        else if (yLimits.x > maxBound.y || yLimits.y < minBound.y)
        {
            transform.position = new Vector3(transform.position.x, currentPos.y, transform.position.z);

            if (xLimits.x > maxBound.x || xLimits.y < minBound.x)
            {
                transform.position = currentPos;
            }
        }
    }
}
