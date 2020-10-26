using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float gravity;
    public float speed;
    public float jumpSpeed;
    public float rotateSpeed;
    public bool is_ogre = false;

    CharacterController CharacterController;
    Animator Animator;
    Vector3 moveDirection = Vector3.zero;

    //右足のコライダー
    Collider FootCollider;
    EnemyHit EnemyHit;

    // Start is called before the first frame update
    void Start()
    {
        CharacterController = GetComponent<CharacterController>();
        Animator = GetComponent<Animator>();
        //右足のコライダーを取得
        FootCollider = transform.Find("Character1_Reference").transform.Find("Character1_Hips").transform.Find("Character1_RightUpLeg").transform.Find("Character1_RightLeg").transform.Find("Character1_RightFoot").transform.Find("Character1_RightToeBase").GetComponent<SphereCollider>();
        EnemyHit = transform.Find("Character1_Reference").transform.Find("Character1_Hips").transform.Find("Character1_RightUpLeg").transform.Find("Character1_RightLeg").transform.Find("Character1_RightFoot").transform.Find("Character1_RightToeBase").GetComponent<EnemyHit>();
        FootCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (is_ogre == true)
        {
            //Sを押すとHikick
            if (Input.GetKeyDown(KeyCode.E))
            {
                Animator.SetBool("Hikick", true);
                EnemyHit.attack_collision_flag = false;
                FootCollider.enabled = true;
            }
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
        CharacterController.Move(globalDirection * Time.deltaTime);
        Animator.SetBool("Run", moveDirection.z > 0.0f);
    }
}
