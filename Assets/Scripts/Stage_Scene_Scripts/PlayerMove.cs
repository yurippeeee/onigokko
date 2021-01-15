using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMove : MonoBehaviourPunCallbacks, IPunObservable
{
    float gravity = 11.0f;
    float speed = 3.0f;
    float jumpSpeed = 6.0f;
    public bool is_ogre = false;
    float mouse_sensitivity = 2.5f;

    GameObject player;
    CharacterController character_controller;
    Animator animator;
    Vector3 move_direction = Vector3.zero;

    //右足のコライダー
    Collider right_foot_collider;
    EnemyHit enemy_hit;

    void Start()
    {
        character_controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        //右足のコライダーを取得
        right_foot_collider = transform.Find("Character1_Reference").transform.Find("Character1_Hips").transform.Find("Character1_RightUpLeg").transform.Find("Character1_RightLeg").transform.Find("Character1_RightFoot").transform.Find("Character1_RightToeBase").GetComponent<SphereCollider>();
        right_foot_collider.enabled = false;
        //右足の衝突処理をするスクリプト
        enemy_hit = transform.Find("Character1_Reference").transform.Find("Character1_Hips").transform.Find("Character1_RightUpLeg").transform.Find("Character1_RightLeg").transform.Find("Character1_RightFoot").transform.Find("Character1_RightToeBase").GetComponent<EnemyHit>();
        
    }

    void Update()
    {
        transform.Rotate(0, Input.GetAxis("Mouse X") * mouse_sensitivity, 0);
        speed = 4.0f;
        if (is_ogre == true)
        {
            speed = 5.0f;
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Hikick"))
            {
                //Eを押すとHikick
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Character_Move_Angle(0);
                    animator.SetTrigger("Hikick");
                    GameObject.Find("Sync_Variable_Manager(Clone)").GetComponent<SyncVariableManager>().Play_Attack_Sound();
                    enemy_hit.attack_collision_flag = false;
                    right_foot_collider.enabled = true;
                }
            }
        }


        //左前に動く
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            Character_Move_Angle(-45);
            Character_Move_Direction(-speed, speed);
        }

        //右前に動く
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            Character_Move_Angle(45);
            Character_Move_Direction(speed, speed);
        }

        //左後ろに動く
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            Character_Move_Angle(-135);
            Character_Move_Direction(-speed, -speed);
        }

        //右後ろに動く
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            Character_Move_Angle(135);
            Character_Move_Direction(speed, -speed);
        }

        //動かない
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            Character_Move_Direction(0, 0);
        }

        //動かない
        else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            Character_Move_Direction(0, 0);
        }

        //前に動く
        else if (Input.GetKey(KeyCode.W))
        {
            Character_Move_Angle(0);
            Character_Move_Direction(0, speed);
        }

        //後ろに動く
        else if (Input.GetKey(KeyCode.S))
        {
            Character_Move_Angle(180);
            Character_Move_Direction(0, -speed);
        }

        //左に動く
        else if (Input.GetKey(KeyCode.A))
        {
            Character_Move_Angle(-90);
            Character_Move_Direction(-speed, 0);
        }

        //右に動く
        else if (Input.GetKey(KeyCode.D))
        {
            Character_Move_Angle(90);
            Character_Move_Direction(speed, 0);
        }

        else
        {
            Character_Move_Direction(0, 0);
        }

        if (character_controller.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                move_direction.y = jumpSpeed;
            }
        }

        move_direction.y -= gravity * Time.deltaTime;
    }

    //Trigger型のAnimationの同期はFixedUpdateの方がいいらしいため
    void FixedUpdate()
    {
        Vector3 global_direction = transform.TransformDirection(move_direction);
        character_controller.Move(global_direction * Time.deltaTime);
        animator.SetBool("Run", (move_direction.z != 0.0f) || (move_direction.x != 0.0f));
    }

    void LateUpdate()
    {
        for (int player_number = 1; player_number <= 4; player_number++)
        {
            if(GameObject.Find("Player" + player_number.ToString() + "(Clone)") == true)
                //プレイヤーの頭上の表示がカメラに向くようにする
                GameObject.Find("Player" + player_number.ToString() +"(Clone)").transform.Find("HPUI").GetComponent<Canvas>().transform.rotation = transform.Find("Player_Camera").GetComponent<Camera>().transform.rotation;
        }
    }
    
    void Character_Move_Direction(float x_speed,float z_speed)
    {
        move_direction.z = z_speed;
        move_direction.x = x_speed;
    }
    
    void Character_Move_Angle(float angle)
    {
        transform.Find("Character1_Reference").transform.localEulerAngles = new Vector3(0, angle, 0);
    }

    //"Character1_Reference"の向きを同期(photon_transform_viewでは同期できないため)
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.Find("Character1_Reference").transform.localEulerAngles);
        }
        else
        {
            transform.Find("Character1_Reference").transform.localEulerAngles = (Vector3)stream.ReceiveNext();
        }
            
    }
}

