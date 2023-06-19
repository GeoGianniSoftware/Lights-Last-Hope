using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ThidPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    CombatController Combat;
    public Transform cam;
    public Animator Anim;
    public Image deathScreen;

    public float speed = 6f;
    public float runModifier;
    public float turnSmooth = 0.1f;
    public float jumpSpeed = 12f;

    //[System.NonSerialized]
    public Vector3 lastSafeSpot;

    public float gravity = -9.81f;
    Vector3 velocity;
    float coyoteTime = .55f;
    float coyoteTimeMax = .55f;

    public Vector3 currentMoveVelocity;

    float turnSmoothVelocity;

    private void Start() {
        Combat = GetComponent<CombatController>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Combat.currentHealth <= 0) {
            Death();
        }
    //Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    //Inputs
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        currentMoveVelocity = new Vector3(x, 0, y);
        float finSpeed = speed;
        float finJump = 0;

    //Direction
        Vector3 direction = new Vector3(x, -finJump, y).normalized;

        //Movement
        float lookAngle = cam.eulerAngles.y;
        float targetAngle = Mathf.Atan2(direction.x, direction.z)* Mathf.Rad2Deg + cam.eulerAngles.y;

        float lookAngleFin = Mathf.SmoothDampAngle(transform.eulerAngles.y, lookAngle, ref turnSmoothVelocity, turnSmooth);
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, lookAngleFin, ref turnSmoothVelocity, turnSmooth);
        transform.rotation = Quaternion.Euler(0, angle, 0);
        if (Input.GetKey(KeyCode.LeftShift)) {
            finSpeed *= runModifier;
        }
        if (direction.magnitude >= 0.1f) {
            
            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            Anim.SetFloat("Speed", moveDirection.magnitude * finSpeed);
            Anim.SetFloat("MoveDir", direction.x);
            currentMoveVelocity = (moveDirection * finSpeed);

            controller.Move(moveDirection.normalized * finSpeed * Time.deltaTime);
        }
        else {
            Anim.SetFloat("Speed", 0f);
        }

        //Animations

    //Jump & Gravity
        if(coyoteTime<= 0)
            velocity.y += gravity * Time.deltaTime;
        
        float jumpMod = 1;

        Anim.SetFloat("VelocityY", velocity.y);

        Anim.SetFloat("VelocityABS", Mathf.Abs(velocity.y));

        if (Input.GetButton("Jump") && coyoteTime > 0f) {


            Anim.ResetTrigger("Grounded");
            velocity.y = Mathf.Sqrt(jumpSpeed * -2 * gravity);
            coyoteTime = 0;

        }



        if (controller.isGrounded) {

            Anim.SetTrigger("Grounded");
            Anim.SetFloat("VelocityY", 0);
            coyoteTime = coyoteTimeMax;
            velocity.y = 0;
            lastSafeSpot = transform.position;
            
        }    
        else {

            coyoteTime -= Time.deltaTime;
            if(!controller.isGrounded && velocity.y < 0)
                jumpMod = 1.5f;
        }
        controller.Move(velocity * jumpMod * Time.deltaTime);
    }

    bool hasDied = false;
    void Death() {
        if (!hasDied) {
            
            speed = 0;
            jumpSpeed = 0;
            Combat.enabled = false;

            Anim.SetTrigger("Dead");
            if (Combat.torchObject.isBeingHeld)
                Combat.ToggleTorch();
            hasDied = true;
        }
        else {

            deathScreen.gameObject.SetActive(true);
            deathScreen.color = Color.Lerp(deathScreen.color, new Color(deathScreen.color.r, deathScreen.color.g, deathScreen.color.b, deathScreen.color.a + Time.deltaTime * 2f), .18f);
            if(deathScreen.color.a > 1.0f) {
                //End Game
                SceneManager.LoadScene(0);
            }
        }
    }
}
