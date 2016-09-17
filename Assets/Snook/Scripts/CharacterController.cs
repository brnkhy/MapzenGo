using System.Collections;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float inputDelay = 0.1f;
    public float forwardVel = 0f;
    public float rotateVel = 100;
    private Quaternion targetRotation;
    private Rigidbody rBody;
    private float forwardInput, turnInput;
    private Animator anim;

    public Quaternion TargetRotation
    {
        get { return targetRotation; }
    }

    private void Start()
    {
        targetRotation = transform.rotation;
        if (GetComponent<Rigidbody>())
            rBody = GetComponent<Rigidbody>();
        else
            Debug.LogError("The character needs a rigidbody.");

        anim = GetComponent<Animator>();

        forwardInput = turnInput = 0;
    }

    public void GetInput()
    {
        forwardInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }

    // Update is called once per frame
    public void Update()
    {
        GetInput();
        Turn();
    }

    public void FixedUpdate()
    {
        Run();
    }

    public void Run()
    {
        if (Mathf.Abs(forwardInput) > inputDelay)
        {
            //move
            float sprint;
            if (Input.GetKeyDown(KeyCode.LeftShift))
                sprint = 10;
            else
                sprint = 1;

            rBody.velocity = transform.forward * forwardInput * forwardVel;
            //anim.speed = rBody.velocity.magnitude;

            //rBody.transform.position += Vector3.forward * forwardVel * Time.deltaTime;
            //anim.SetFloat("speed", .31f);
            anim.SetFloat("speed", forwardVel);
            Debug.Log("forwardInput = " + forwardInput);
        }
        else
        {
            rBody.velocity = Vector3.zero;
            anim.SetFloat("speed", 0f);
        }
    }

    private void Turn()
    {
        targetRotation *= Quaternion.AngleAxis(rotateVel * turnInput * Time.deltaTime, Vector3.up);
        transform.rotation = targetRotation;
    }
}