using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField]
    private float rotationSpeed = 100;
    [SerializeField]
    private float moveSpeed = 10;
    [SerializeField]
    private float maxAngle = 30;

    private bool leftWallColliding = false;
    private bool rightWallColliding = false;
    private List<Collider> colliders = new List<Collider>();

    public Rigidbody rigidBody;

    void Start()
    {
        rigidBody = this.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other) {
        colliders.Add(other);
        if (other.tag == "WallLeft") leftWallColliding = true;
        if (other.tag == "WallRight") rightWallColliding = true;
    }
    
    private void OnTriggerExit(Collider other) {
        colliders.Remove(other);
        if (other.tag == "WallLeft") leftWallColliding = false;
        if (other.tag == "WallRight") rightWallColliding = false;
    }

    private void moveHorizontal (float speed) {
        if (speed != 0) {

        leftWallColliding = false;
        rightWallColliding = false;
        foreach (Collider collider in colliders) {
            if (collider.tag == "WallLeft") leftWallColliding = true;
            if (collider.tag == "WallRight") rightWallColliding = true;
        }
        
            if ((leftWallColliding && speed < 0) 
                || (rightWallColliding && speed > 0)) 
            {
                speed = 0;
            }
        } 
        transform.position += new Vector3(speed, 0, 0);
    }

    private void rotateHorizontal (float speed) {
        float currentAngle = transform.eulerAngles.y;
        if (currentAngle > 180) currentAngle = -360 + currentAngle;

        if (speed != 0) 
        {
            if ((currentAngle >= maxAngle && speed > 0) 
                || (currentAngle <= -maxAngle && speed < 0))
            {
                speed = 0;
            } 
            
            transform.Rotate(0, speed, 0);
        } 
        else if (currentAngle != 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        } 
    }

    void FixedUpdate()
    {
        if (GameManager.current.gameEnded) return;
        float rotationInput = Input.GetAxisRaw("Horizontal");
        float horizontalMoveSpeed = Input.GetAxisRaw("Horizontal") * Time.deltaTime * moveSpeed;
        float horizontalRotationSpeed = Input.GetAxisRaw("Horizontal") * Time.deltaTime * rotationSpeed;
        
        rotateHorizontal(horizontalRotationSpeed); 
        moveHorizontal(horizontalMoveSpeed);
    }

    void Update()
    {
        
    }
}
