using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    
    [SerializeField] private GameObject ballPrefabs;
    [SerializeField] private Rigidbody2D pivot;
    [SerializeField] private float detachDelay;
    [SerializeField] private float respawnDelay;


    private Rigidbody2D currentBallRigidbody;
    private SpringJoint2D currentBallSpringJoint;
    private Camera mainCamera;
    private bool isDragging;

    void Start()
    {
        mainCamera = Camera.main;
        RespawnNewBall();
    }

    void Update()
    {
        if(currentBallRigidbody == null){
            return;
        }

        if (!Touchscreen.current.primaryTouch.press.isPressed){
            if(isDragging){
                LaunchBall();
            }
            isDragging = false;
            return;
        }

        isDragging = true;
        currentBallRigidbody.bodyType = RigidbodyType2D.Kinematic;
        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector2 worldPosition = mainCamera.ScreenToWorldPoint(new Vector2(touchPosition.x, touchPosition.y));
        // Debug.Log(worldPosition);
        // worldPosition.z = 0f; // Ensure Z is set to 0 for 2D
        currentBallRigidbody.position = worldPosition;
        
    }

    private void RespawnNewBall(){
        GameObject ballInstance = Instantiate(ballPrefabs, pivot.position, quaternion.identity);
        currentBallRigidbody = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();
        currentBallSpringJoint.connectedBody = pivot;
    }

    private void LaunchBall(){
        currentBallRigidbody.bodyType = RigidbodyType2D.Dynamic;
        currentBallRigidbody = null;
        Invoke(nameof(DetachBall),detachDelay);
        
    }


    private void DetachBall(){
        currentBallSpringJoint.enabled = false;
        currentBallSpringJoint = null;
        Invoke(nameof(RespawnNewBall), respawnDelay);
    }
}
