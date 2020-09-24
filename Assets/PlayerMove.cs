using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float gravity;
    public float speed;
    public float jumpSpeed;
    public float rotateSpeed;

    private CharacterController characterController;
    private Animator animator;
    private Vector3 moveDirection = Vector3.zero;
    //右足のコライダー
    private Collider footCollider;
    public GameObject enemyhit;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        //右足のコライダーを取得
        footCollider = GameObject.Find("Character1_RightToeBase").GetComponent<SphereCollider>();
        footCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Sを押すとHikick
        if (Input.GetKeyDown(KeyCode.E))
        {
            footCollider = GameObject.Find("Character1_RightToeBase").GetComponent<SphereCollider>();
            animator.SetBool("Hikick", true);
            enemyhit.GetComponent<EnemyHit>().attack_collision_flag = false;
            //右足コライダーをオンにする
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

    public bool CheckGrounded()
    {
        var ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        var tolerance = 0.3f;
        return Physics.Raycast(ray, tolerance);
    }
}
