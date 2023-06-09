using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//
//Code Written by Edgar Gunther
//
[RequireComponent(typeof(Animator))]
public class ThirdPersonMovementCC : MonoBehaviour
{
    //Variables
    //--------------------------------------
    [Header("Character Controller")]
    [HideInInspector]
    public CharacterController controller;

    [Header("Movement Values")]
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    Vector3 direction = Vector3.zero;
    float gravity = 9.71f;

    [HideInInspector]
    public Animator anim;

    //--------------------------------------

    [Header("RayCast values")]
    Ray ray;
    RaycastHit hit;
    public delegate void OnUpdate();
    [HideInInspector]
    public OnUpdate onUpdate;
    [SerializeField] float sphereCastDistance = 1f;
    [SerializeField] LayerMask groundMask;



    public bool debug;

    // Update is called once per frame
    void Update()
    {

        Gravity();

        ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out hit, 200f, groundMask))
        {
            Vector3 difference = hit.point - transform.position;

            //calculates angle for player direction to move and face
            float targetAngle = Mathf.Atan2(difference.x, difference.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            //smoothes the angle transfer between current and target angle
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            //sets the rotation to the current angle
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }


        if (direction.magnitude >= 0.1f)
        {
            if (anim)
            {
                anim.SetBool("Moving", true);
            }
            controller.Move(direction.normalized * speed * Time.deltaTime);
        }
        else if(anim)
        {
            anim.SetBool("Moving", false);
        }

        if(onUpdate != null)
            onUpdate();
    }


    private void OnMove(InputValue value)
    {
        direction.x = value.Get<Vector2>().x;
        direction.z = value.Get<Vector2>().y;
    }

    private void Gravity()
    {
        if(debug)
            Debug.Log("enterGravity");

        if (!Physics.SphereCast(transform.position, .5f, Vector3.down, out hit, sphereCastDistance, groundMask))
        {
            if (debug)
                Debug.Log("enterGravity SphereCast");
            
            transform.position += Vector3.down * gravity * Time.deltaTime;
        }
    }

    public RaycastHit GetHit()
    {
        return hit;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + (Vector3.down * sphereCastDistance), .5f);
    }

}
