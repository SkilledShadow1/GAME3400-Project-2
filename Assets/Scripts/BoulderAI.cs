using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.Splines;

public class BoulderAI : MonoBehaviour
{
    public enum BoulderState
    {
        FallingToPath,
        OnPath,
        FreeFromPath
    }

    private BoulderState currentState;

    private SplineContainer splinePath;
    private Rigidbody rb;

    private Vector3 lastPosition;
    private Vector3 lastVelocity;
    private Vector3 lastRollAxis;
    
    private float currentElementLocation;
    private float splineLength;
    private float lastRollAngle;

    [SerializeField] private float boulderLifetime = 30f;
    [SerializeField] private float waitTime = 2f;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float boulderRadius;
    [SerializeField] private float rollSpeed;

    private bool waiting;

    private void Start()
    {
        Destroy(gameObject, boulderLifetime);
        rb = GetComponent<Rigidbody>();

        currentState = BoulderState.FallingToPath;

        lastPosition = transform.position;
        lastVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case BoulderState.FallingToPath:
                FallingToPath();
                break;

            case BoulderState.OnPath:
                OnPath();
                break;

            case BoulderState.FreeFromPath:
                FreeFromPath();
                break;
        }
    }

    private void FallingToPath()
    {
        rb.freezeRotation = true;
        rb.useGravity = true;
    }

    private void OnPath()
    {
        rb.freezeRotation = true;
        rb.useGravity = false;

        Vector3 nextLocation = CalculateNextLocation();
        MoveObject(nextLocation);
    }

    private void FreeFromPath()
    {
        transform.Rotate(lastRollAxis, lastRollAngle, Space.World);
        rb.useGravity = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (waiting) return;

        if (other.gameObject.layer == LayerMask.NameToLayer("BoulderTrigger")) {
            splinePath = other.gameObject.GetComponent<SplineContainer>();
            splineLength = splinePath.CalculateLength();
            waiting = true;
            
            StartCoroutine(BoulderWait());
        }
    }

    private IEnumerator BoulderWait()
    {
        yield return new WaitForSeconds(waitTime);

        currentElementLocation = 0f;
        lastVelocity = Vector3.zero;

        transform.position = splinePath.EvaluatePosition(0f);
        lastPosition = transform.position;

        rb.linearVelocity = Vector3.zero;
        
        currentState = BoulderState.OnPath;
    }

    private Vector3 CalculateNextLocation()
    {
        float distanceToMove = moveSpeed * Time.fixedDeltaTime;
        float normalizedDistance = distanceToMove / splineLength;

        currentElementLocation = Mathf.Clamp01(
            currentElementLocation + normalizedDistance
        );

        if (currentElementLocation >= 1f)
        {
            currentElementLocation = 1f;
            currentState = BoulderState.FreeFromPath;
        }

        return splinePath.EvaluatePosition(currentElementLocation);
    }

    private void MoveObject(Vector3 nextLocation)
    {
        Vector3 diffInPositions = nextLocation - lastPosition;
        Vector3 velocity = diffInPositions / Time.fixedDeltaTime;

        lastPosition = nextLocation;

        Vector3 velChange = velocity - lastVelocity;
        rb.linearVelocity += velChange;
        lastVelocity = velocity;
        
        float distanceMoved = diffInPositions.magnitude;
        Vector3 movementDir = velocity.normalized;
        Vector3 rollAxis = Vector3.Cross(Vector3.up, movementDir).normalized;
        float rollAngle = rollSpeed * distanceMoved / boulderRadius * Mathf.Rad2Deg;
        
        lastRollAxis = rollAxis;
        lastRollAngle = rollAngle;

        transform.Rotate(rollAxis, rollAngle, Space.World);
    }
}