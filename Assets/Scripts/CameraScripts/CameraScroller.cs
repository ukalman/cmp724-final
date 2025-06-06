using UnityEngine;

public class CameraScroller : MonoBehaviour
{
    [Header("Scroll Settings")]
    public float scrollSpeed = 5f;
    public float edgeSize = 20f;

    [Header("Movement Bounds")]
    public Vector2 minBounds = new Vector2(-10f, -10f); 
    public Vector2 maxBounds = new Vector2(10f, 10f);

    private bool _cameraReset = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetCamera();
        }

        HandleMouseEdgeScroll();
    }

    private void HandleMouseEdgeScroll()
    {
        if (_cameraReset)
        {
            if (!IsMouseOnEdge())
            {
                _cameraReset = false;
            }
            else return;
        }

        Vector2 input = GetEdgeScrollDirection();
        Vector3 movement = CalculateIsometricMovement(input);
        MoveCamera(movement);
    }

    private Vector2 GetEdgeScrollDirection()
    {
        Vector2 direction = Vector2.zero;
        Vector2 mousePos = Input.mousePosition;

        if (mousePos.x < edgeSize) direction.x -= 1;
        else if (mousePos.x > Screen.width - edgeSize) direction.x += 1;

        if (mousePos.y < edgeSize) direction.y -= 1;
        else if (mousePos.y > Screen.height - edgeSize) direction.y += 1;

        return direction.normalized;
    }

    private Vector3 CalculateIsometricMovement(Vector2 input)
    {
        Vector3 right = transform.right;
        Vector3 forward = transform.forward;
        
        right.y = 0f;
        forward.y = 0f;

        right.Normalize();
        forward.Normalize();

        Vector3 move = (right * input.x + forward * input.y).normalized;
        return move * (scrollSpeed * Time.deltaTime);
    }

    private void MoveCamera(Vector3 movement)
    {
        Vector3 currentPos = transform.position;
        Vector3 tentativePos = currentPos + movement;
        
        if (tentativePos.x < minBounds.x || tentativePos.x > maxBounds.x)
        {
            movement.x = 0f;
        }
        
        if (tentativePos.z < minBounds.y || tentativePos.z > maxBounds.y)
        {
            movement.z = 0f;
        }

        transform.position += movement;
    }

    private void ResetCamera()
    {
        transform.position = Vector3.zero;
        _cameraReset = true;
    }

    private bool IsMouseOnEdge()
    {
        Vector2 mousePos = Input.mousePosition;
        return mousePos.x < edgeSize || mousePos.x > Screen.width - edgeSize ||
               mousePos.y < edgeSize || mousePos.y > Screen.height - edgeSize;
    }
}
