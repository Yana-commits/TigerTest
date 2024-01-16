
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Boundary
{ 
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private Boundary boundary;
    [SerializeField]
    private float speed = 1;

    private float horizontalInput;
    private float verticalInput;

    private void Start()
    {
        Init();
    }
    private void FixedUpdate()
    {
        //float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertiacal = Input.GetAxis("Vertical");

        rb.velocity = new Vector3(horizontalInput, verticalInput, 0f)*speed;

        rb.position = new Vector3
            (
             Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
             Mathf.Clamp(rb.position.y, boundary.zMin, boundary.zMax),
             0.0f
            );
    }
    private void GetDirection(Vector2 direction)
    {
        horizontalInput = direction.x;
        verticalInput = direction.y;
    }
    public void Init()
    {
        SwipeDetection.SwipeEvent += GetDirection;
        
    }
    private void OnDestroy()
    {
        SwipeDetection.SwipeEvent -= GetDirection;
    }


}
