using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Serialize
    [SerializeField] Rigidbody physicEngine;
    [SerializeField] [Range(0.5f, 10f)] float runSpeed;
    [SerializeField] Transform movingReferenceObj = null;
    [SerializeField] [Range(0f, 10f)] float jumpingVal;
    [SerializeField] ColliderCheck floorDetector = null;
    [SerializeField] Animator AnimationSystem = null;
    [SerializeField] float AccelerateDoubled = 9f;
    [SerializeField] FollowParentObjRotate MyCharacterRotator = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    float WS_Value = 0f;
    float AD_Value = 0f;
    void Update()
    {
        // Approximatly, using 1 sec to gradually change from original to input
        // Lerp(A, B, Percentage of each frame changed)
        // WS_Value = Mathf.Lerp(WS_Value, Input.GetAxisRaw("Vertical"), Time.deltaTime * AccelerateDoubled);
        // AD_Value = Mathf.Lerp(AD_Value, Input.GetAxisRaw("Horizontal"), Time.deltaTime * AccelerateDoubled);

        // MoveTowards(A, B, Value of each frame changed) 

        // During input
        float originalInputWS = Input.GetAxisRaw("Vertical");
        float originalInputAD = Input.GetAxisRaw("Horizontal");

        float animationInputWS = AnimationSystem.GetFloat("animWS");
        float animationInputAD = AnimationSystem.GetFloat("animAD");
        float animationCtrlPer = AnimationSystem.GetFloat("animP");

        // According animation control percentage to decide what number should be used
        originalInputWS = Mathf.Lerp(originalInputWS, animationInputWS, animationCtrlPer);
        originalInputAD = Mathf.Lerp(originalInputAD, animationInputAD, animationCtrlPer);

        // Buffering WSAD
        WS_Value = Mathf.MoveTowards(WS_Value, originalInputWS, Time.deltaTime * AccelerateDoubled);
        AD_Value = Mathf.MoveTowards(AD_Value, originalInputAD, Time.deltaTime * AccelerateDoubled);

        // Writing data into animation system while receiving WSAD_Value
        AnimationSystem.SetFloat("ws", WS_Value);
        AnimationSystem.SetFloat("ad", AD_Value);

        Vector3 movingValue;
        movingValue.x = AD_Value;
        movingValue.z = WS_Value;
        movingValue.y = 0f;

        // Transfer global coordinate to local coordinate
        movingValue = movingReferenceObj.TransformDirection(movingValue);

        // Limited vector length become 1 for maximum
        movingValue = Vector3.ClampMagnitude(movingValue, 1f);


        movingValue = movingValue * runSpeed;

        // this.transform.position = this.transform.position + movingValue;

        // Before change physic engine value that set Y axis to be main character first
        movingValue.y = physicEngine.velocity.y;

        // Using movingValue become accelation(Global coordinate)
        physicEngine.velocity = movingValue; 

        // If press space button then jumping
        bool pressSpace = Input.GetKeyDown(KeyCode.Space);


        // If press space button and floorDetecot is detected
        if (pressSpace && floorDetector.detected)
        {
            Vector3 currentState = physicEngine.velocity;
            // Changing Y axis to be jumping
            currentState.y = jumpingVal;
            // Returning current state while being changed
            physicEngine.velocity = currentState;
        }

        if(Mathf.Abs(WS_Value) + Mathf.Abs(AD_Value) > 0.1f || floorDetector.detected == false)
        {
            MyCharacterRotator.NeedFollow = true;
        }
        else
        {
            MyCharacterRotator.NeedFollow = false;
        }
    }
}
