using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;         
    public Vector3 offset = new Vector3(0f, 5f, -7f); 
    public float followSpeed = 5f;    

    private void LateUpdate()
    {
        if (target == null) return;
        
        Vector3 targetPosition = new Vector3(target.position.x, 122.38f, target.position.z) + offset;
        
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        
    }
}