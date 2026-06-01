using UnityEngine;
using UnityEngine.InputSystem;

public class CameraPan : MonoBehaviour
{
    [SerializeField] float panSpeed;
    [SerializeField] float halfWidth;
    [SerializeField] float halfHeight;
    float xMousePos;
    float yMousePos;
    Vector3 position;
    float camHalfHeight;
    float camHalfWidth;
    float minX;
    float maxX;
    float minY;
    float maxY;

    private void Start()
    {
        camHalfHeight = Camera.main.orthographicSize;
        camHalfWidth = camHalfHeight * Camera.main.aspect;
        minX = -halfWidth + camHalfWidth;
        maxX = halfWidth - camHalfWidth;
        minY = -halfHeight + camHalfHeight;
        maxY = halfHeight - camHalfHeight;
    }

    // Update is called once per frame
    void Update()
    {
        if (halfWidth == 0) return;
        xMousePos = Mouse.current.position.ReadValue().x / Screen.width;
        yMousePos = Mouse.current.position.ReadValue().y / Screen.height;

        position = transform.position;

        if (xMousePos < 0.1f && xMousePos > 0)
        {
            position += panSpeed * Time.deltaTime * Vector3.left;
        }
        else if (xMousePos > 0.9f && xMousePos < 1)
        {
            position += panSpeed * Time.deltaTime * Vector3.right;
        }

        if (yMousePos < 0.1f && yMousePos > 0)
        {
            position += panSpeed * Time.deltaTime * Vector3.down;
        }
        else if (yMousePos > 0.9f && yMousePos < 1)
        {
            position += panSpeed * Time.deltaTime * Vector3.up;
        }

        ClampCameraPosition(position);
    }

    void ClampCameraPosition(Vector3 newPosition)
    {
        transform.position = new Vector3
            (
                Mathf.Clamp(newPosition.x, minX, maxX),
                Mathf.Clamp(newPosition.y, minY, maxY),
                -10
            );
    }
}
