using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 2f;
    [SerializeField] float turnSpeed = 5f;
    [SerializeField] Thruster[] thruster;

    [Tooltip("A multiplier to the input. Describes the maximum speed in degrees / second. To flip vertical rotation, set Y to a negative value")]
    [SerializeField] private Vector2 sensitivity = new Vector2(1000, 1000);
    [Tooltip("The rotation acceleration, in degrees / second")]
    [SerializeField] private Vector2 acceleration = new Vector2(1000, 1000);
    [Tooltip("The maximum angle from the horizon the player can rotate, in degrees")]
    [SerializeField] private float maxVerticalAngleFromHorizon;
    [Tooltip("The period to wait until resetting the input value. Set this as low as possible, without encountering stuttering")]
    [SerializeField] private float inputLagPeriod = 1f;
    [SerializeField] private float TEST = 0.5f;

    Transform myTransform;
    private Vector2 movementVector; // The current rotation velocity, in degrees
    private Vector2 thisFrameRotation; // The current rotation, in degrees
    private Vector2 lastInputEvent; // The last received non-zero input value
    private float inputLagTimer; // The time since the last received non-zero input value
    private float yRotationalVelocity;
    private Vector2 magnifiedMovementVector;

    private Vector3 continuousRotation;
    private void Awake()
    {
        myTransform = transform;

        continuousRotation.x = transform.rotation.x + TEST;
        continuousRotation.y = transform.rotation.y;
        continuousRotation.z = transform.rotation.z;
    }

    void Update()
    {
        Thrust();
        Rotations();
    }

    //private void OnEnable()
    //{
    //    movementVector = Vector2.zero;
    //    inputLagTimer = 0;
    //    lastInputEvent = Vector2.zero;

    //    // Calculate the current rotation by getting the gameObject's local euler angles
    //    Vector3 euler = transform.localEulerAngles;
    //    // Euler angles range from [0, 360), but we want [-180, 180)
    //    if (euler.x >= 180)
    //    {
    //        euler.x -= 360;
    //    }

    //    transform.localEulerAngles = euler;
    //    // Rotation is stored as (horizontal, vertical), which corresponds to the euler angles
    //    // around the y (up) axis and the x (right) axis
    //    thisFrameRotation = new Vector2(euler.y, euler.x);
    //}

    void Rotations()
    {
        // get input 
        // scale inputs to a max value
        // enact the rotation

        //var wantedVelocity = GetInput() * sensitivity;
        UpdateRotationalAccelleration();
        var wantedVelocity = lastInputEvent * TEST;


        //Debug.Log($"lastInputEvent: {lastInputEvent}");
        //Debug.Log($"wantedVelocity: {wantedVelocity}");
        //movementVector = wantedVelocity;

        //Debug.Log($"wanted velocity: {wantedVelocity}");

        // I dont think we want to use MoveTowards, we want to rotate and keep rotating. 
        //movementVector = new Vector2(
        //    Mathf.MoveTowards(movementVector.x, wantedVelocity.x, acceleration.x * Time.deltaTime),
        //    Mathf.MoveTowards(movementVector.y, wantedVelocity.y, acceleration.y * Time.deltaTime));


        //thisFrameRotation += movementVector * Time.deltaTime;

        transform.Rotate(new Vector3(magnifiedMovementVector.y *-1f, 0, 0) * Time.deltaTime);
        // works ok but need a dead zone for not making changes to the movement.
    }
    void Thrust()
    {
        // Activate and deactivate trail rendereer. 
        if (Input.GetAxis("Vertical") > 0)
        {
            myTransform.position += myTransform.forward * movementSpeed * Time.deltaTime * Input.GetAxis("Vertical");
        }
    }

    public static float Remap(float value)
    {
        var boundValue = value < 0
            ? value < -10
                ? -10
                : value
            : value > 10
                ? 10
                : value;

        var result = (boundValue / 0.4f);

        return result * 0.4f;
    }


    private void UpdateRotationalAccelleration()
    {// TODO extend to include roll rotation

        // rotate based on where position of mouse is on the screen. Do not do it additive, abosolute based on the mouse poisition. We can consider doing the addative approach later. 

        float screenX = Screen.width;
        float screenY = Screen.height;
        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;


        if (mouseX < 0 || mouseX > screenX || mouseY < 0 || mouseY > screenY) return;

        var mouseLocation = new Vector2(mouseX, mouseY);
        var screenCenter = new Vector2(screenX/2, screenY/2);
        var movementVector = mouseLocation- screenCenter;

        Debug.Log($"movementVector : {movementVector}");




        magnifiedMovementVector = movementVector * TEST;
        //var boundY = Remap(maginfied.y);


    }
}
