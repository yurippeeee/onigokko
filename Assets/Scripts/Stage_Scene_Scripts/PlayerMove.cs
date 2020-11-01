using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerMove : MonoBehaviour
{
    public float gravity;
    public float speed;
    public float jumpSpeed;
    public float rotateSpeed;
    public bool is_ogre = false;
    float mouse_sensitivity = 2.5f;

    CharacterController CharacterController;
    Animator Animator;
    Vector3 moveDirection = Vector3.zero;

    //右足のコライダー
    Collider RightFootCollider;
    EnemyHit EnemyHit;

    void Start()
    {
        CharacterController = GetComponent<CharacterController>();
        Animator = GetComponent<Animator>();
        //右足のコライダーを取得
        RightFootCollider = transform.Find("Character1_Reference").transform.Find("Character1_Hips").transform.Find("Character1_RightUpLeg").transform.Find("Character1_RightLeg").transform.Find("Character1_RightFoot").transform.Find("Character1_RightToeBase").GetComponent<SphereCollider>();
        EnemyHit = transform.Find("Character1_Reference").transform.Find("Character1_Hips").transform.Find("Character1_RightUpLeg").transform.Find("Character1_RightLeg").transform.Find("Character1_RightFoot").transform.Find("Character1_RightToeBase").GetComponent<EnemyHit>();
        //RightFootCollider.enabled = false;
    }

    void Update()
    {
        transform.Rotate(0, Input.GetAxis("Mouse X")*mouse_sensitivity, 0); 
        if (is_ogre == true)
        {
            if (!Animator.GetCurrentAnimatorStateInfo(0).IsName("Hikick"))
            {
                //Eを押すとHikick
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Animator.SetTrigger("Hikick");
                    EnemyHit.attack_collision_flag = false;
                    RightFootCollider.enabled = true;
                }
            }
        }
        if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Hikick"))
        {

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
        {

        }

        if (Input.GetKey(KeyCode.Space))
        {
            moveDirection.y = jumpSpeed;
        }

        moveDirection.y -= gravity * Time.deltaTime;
    }

    void FixedUpdate()
    {
        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        CharacterController.Move(globalDirection * Time.deltaTime);
        Animator.SetBool("Run", moveDirection.z > 0.0f);
    }
}

