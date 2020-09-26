using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float gravity;
    public float speed;
    public float jumpSpeed;
    public float rotateSpeed;
    public bool ogre = false;
    public bool player_exist = true;

    private CharacterController characterController;
    private Animator animator;
    private Vector3 moveDirection = Vector3.zero;

    //右足のコライダー
    private Collider footCollider;
    public EnemyHit enemyhit;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        //右足のコライダーを取得
        footCollider = transform.Find("Character1_Reference").transform.Find("Character1_Hips").transform.Find("Character1_RightUpLeg").transform.Find("Character1_RightLeg").transform.Find("Character1_RightFoot").transform.Find("Character1_RightToeBase").GetComponent<SphereCollider>();
        enemyhit = transform.Find("Character1_Reference").transform.Find("Character1_Hips").transform.Find("Character1_RightUpLeg").transform.Find("Character1_RightLeg").transform.Find("Character1_RightFoot").transform.Find("Character1_RightToeBase").GetComponent<EnemyHit>();
        footCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (ogre == true)
        {
            //Sを押すとHikick
            if (Input.GetKeyDown(KeyCode.E))
            {
                animator.SetBool("Hikick", true);
                enemyhit.attack_collision_flag = false;
                footCollider.enabled = true;
            }
            if (Input.GetKey(KeyCode.W))
            {
                moveDirection.z = speed;
            }
            else
            {
                moveDirection.z = 0;
            }

            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
            { }
            else if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(0, rotateSpeed * -1, 0);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(0, rotateSpeed, 0);
            }

            if (Input.GetKey(KeyCode.Space))
            {
                moveDirection.y = jumpSpeed;
            }

            moveDirection.y -= gravity * Time.deltaTime;
            Vector3 globalDirection = transform.TransformDirection(moveDirection);
            characterController.Move(globalDirection * Time.deltaTime);
            animator.SetBool("Run", moveDirection.z > 0.0f);
        }
    }
}
