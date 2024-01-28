using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*
    This component is to be applied to Character in this hierarchical structure:
        Character
        |_  CamOrigin
            |_  Camera

    The choice of minCamToColliderDistance is of crucial importance. Make sure that it stays
    within the character's capsule collider's radius!
 
 */
public class CharachterControllerScript : MonoBehaviour
{

    public InputEventLayer InputEventLayerSO;

    Animator characterAnimator;

    [Range(0.1f, 10.0f)]    public float sensitivity = 6f;
    [Range(0.1f, 6.0f)]     public float maxWalkingSpeed = 5f;          // in Meters/Second
    [Range(0.1f, 2.0f)]     public float maxBackwardSpeed = 1.5f;       // in Meters/Second
    [Range(0.1f, 5.0f)]     public float maxSidewalkSpeed = .8f;        // in Meters/Second

    public float acceleration = 1f;         // how fast will the character start moving
    public float deceleration = 1f;         // in Meters/Second^2

    public bool invertMouse = false;

    [Range(10f, 60f)]   public float smallObjectCullingLayerDistance;
    [Range(3f, 20f)]    public float videoPlayerCullingLayerDistance;
    [Range(10f, 100f)]  public float bannerCullingLayerDistance;
    [Range(10f, 100f)]  public float furnitureCullingLayerDistance;

    private int mouseInvertionValue;
    private readonly float maxCamRotation = 60.00f;
    private readonly float minCamRotation = -20.00f;

    [SerializeField] private float minCamToColliderDistance;        //  this specifies how close the main camera can get to any collider
                                                                    //  with specific layer (see below)
    [SerializeField] private float defaultCamToOriginDistance;      //  default distance between the main Camera and the character

    public Transform camTransform;
    public Transform camOriginTransform;            // this is the object the camera will be rotating around
    
    CharacterController controller;
    LayerMask camCollisionRayMask;

    Camera mainCam;                         // usefull for some optimizations


    int xVelocityHash;
    int zVelocityHash;

    float maxAllowedVelocity;               // the maximum allowed velocity determined from different speed settings

    private void Awake()
    {
        InputEventLayerSO.EnableLocomotionInput();

        InputEventLayerSO.LookEvent += OnLook;

        mainCam = camTransform.GetComponent<Camera>();
    }

    private void OnEnable()
    {
        UIManager.Instance.SetMapCharacterToFollow(transform);
    }

    void Start()
    {
        // setting maximum allowed velocity
        maxAllowedVelocity = Mathf.Max(maxWalkingSpeed, maxBackwardSpeed, maxSidewalkSpeed);

        // initializing Animator reference
        characterAnimator = GetComponent<Animator>();
        xVelocityHash = Animator.StringToHash("velocityX");
        zVelocityHash = Animator.StringToHash("velocityZ");


        // initializing components
        controller = GetComponent<CharacterController>();

        // locking cursor
        Cursor.lockState = CursorLockMode.Locked;

        // initiating private attributes
        mouseInvertionValue = (invertMouse)? -1 : 1;

        // transform camera's position to defaultCamOriginDistance from CamOrigin
        camTransform.Translate(Vector3.forward * -defaultCamToOriginDistance);
        
        // Layer-masks-related initializations
        camCollisionRayMask = LayerMask.GetMask("CamCollision");

        // setting-up per-layer camera clipping plane\distance
        CameraLayerCulling();
    }

    Vector3 deltaVelocity = new Vector3(0.0f, 0.0f, 0.0f);              // describes the direction and the velocity at which the player will move
    private void LateUpdate()
    {

        if (InputEventLayerSO.IsWalking())               // if (VERTICAL arrow keys are pressed)
        {
            // accelerate forward or backward while staying whithin the speed range
            deltaVelocity.z = Mathf.Clamp(deltaVelocity.z + InputEventLayerSO.GetWalkValue() * (acceleration * Time.deltaTime), -maxBackwardSpeed, maxWalkingSpeed);

        } else if (deltaVelocity.z != 0.0f)
        {
            // deceleration to idle speed = 0.00
            deltaVelocity.z = Mathf.Sign(deltaVelocity.z) * Mathf.Clamp(Mathf.Abs(deltaVelocity.z) - (deceleration * Time.deltaTime), 0.0f, Mathf.Infinity);
        }


        if (InputEventLayerSO.IsSidewalking())           // if (HORIZONTAL arrow keys are pressed)
        {
            // accleration to maxSidewalkSpeed in positive direction
            deltaVelocity.x = Mathf.Clamp(deltaVelocity.x + InputEventLayerSO.GetSidewalkValue() * (acceleration * Time.deltaTime), -maxSidewalkSpeed, maxSidewalkSpeed);

        } else if (deltaVelocity.x != 0.0f)
        {
            // deceleration to idle speed = 0.00
            deltaVelocity.x = Mathf.Sign(deltaVelocity.x) * Mathf.Clamp(Mathf.Abs(deltaVelocity.x) - (deceleration * Time.deltaTime), 0.0f, Mathf.Infinity);
        }

        // if velocity is greater than maximum permitted velocity then scale down the delta velocity vector
        float magnitude = deltaVelocity.magnitude;
        if (magnitude > maxAllowedVelocity)
        {
            float lambda = maxAllowedVelocity / magnitude;
            deltaVelocity *= lambda;
        }

        controller.Move(transform.TransformVector(deltaVelocity) * Time.deltaTime);

        // INFORMING ANIMATOR
        characterAnimator.SetFloat(zVelocityHash, deltaVelocity.z);
        characterAnimator.SetFloat(xVelocityHash, deltaVelocity.x);

        // Camera positioning to avoid collision
        CameraCollisionVerification();
    }

    private void OnDisable()
    {
        InputEventLayerSO.LookEvent -= OnLook;

        InputEventLayerSO.DisableLocomotionInput();
    }

    // input event handlers
    private void OnLook(Vector2 deltaVect)
    {
        /* ----------------------- Response to user input -----------------------*/
        // vertical-axis rotation
        transform.Rotate(0f, deltaVect.x * sensitivity * Time.fixedDeltaTime * mouseInvertionValue, 0f);

        // horizontal-axis rotation
        // Time.deltaTime could get really large (especially during expensive operations such as material creation)
        // thus horizontalRotationValue can get larger that 360 degrees which makes the first if statement true
        // even though it shouldn't be.
        //
        // one solution is to use the fixedDletaTime update
        // a better solution is get better at dealing with rotations
        //
        float horizontalRotationValue = -deltaVect.y * sensitivity * Time.fixedDeltaTime * mouseInvertionValue;
        float angle = camTransform.localRotation.eulerAngles.x % 360.00f;

        if (angle > 180.00f) angle -= 360.00f;

        float willBeRotationAroundX = angle + horizontalRotationValue;

        if ((willBeRotationAroundX >= minCamRotation) && (willBeRotationAroundX <= maxCamRotation))
            camTransform.RotateAround(camOriginTransform.position, camOriginTransform.right, horizontalRotationValue);
        /*-----------------------------------------------------------------------*/
    }

    RaycastHit hitInfo;
    private void CameraCollisionVerification()
    {
        
        if (Physics.Raycast(camOriginTransform.position, -camTransform.forward, out hitInfo,
                            minCamToColliderDistance + defaultCamToOriginDistance, camCollisionRayMask))
        {
            float camToOriginDistance = (camTransform.position - camOriginTransform.position).magnitude;
            float camToWallDistance = hitInfo.distance - camToOriginDistance;

            if (camToWallDistance <= 0f)
            {
                camTransform.Translate(Vector3.forward * (-camToWallDistance + 0.05f));
            }
            else if (camToWallDistance <= minCamToColliderDistance)
            {
                camTransform.Translate(Vector3.forward * (minCamToColliderDistance - camToWallDistance));
            }
            else if (camToOriginDistance < defaultCamToOriginDistance)
            {
                float valueToAdd = Mathf.Clamp((camToWallDistance - minCamToColliderDistance), 0f, defaultCamToOriginDistance - camToOriginDistance);
                camTransform.Translate(Vector3.back * valueToAdd);
            }
        }
    }

    private void CameraLayerCulling()
    {
        float[] distances = new float[32];

        mainCam.layerCullSpherical = true;

        distances[6] = bannerCullingLayerDistance;
        distances[7] = furnitureCullingLayerDistance;
        distances[8] = smallObjectCullingLayerDistance;
        distances[10] = videoPlayerCullingLayerDistance;

        mainCam.layerCullDistances = distances;
    }
}
