using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ItemCatch : MonoBehaviourPunCallbacks
{
    bool grab = false;
    bool col = false;
    public bool through = false;
    public bool item_hit_flag = false;
    float through_time = 0.7f;
    GameObject grabbing_player;
    SyncVariableManager svm;
    bool push_f = false;
    bool push_v = false;
    public bool timer_end = false;

    void Start()
    {
        GetComponent<Animator>().SetBool("Turn Head", true);
        svm = GameObject.Find("Sync_Variable_Manager(Clone)").GetComponent<SyncVariableManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            push_f = true;
        }
        if(Input.GetKeyDown(KeyCode.V))
        {
            push_v = true;
        }
    }


    void FixedUpdate()
    {
        if (through)
        {
            if (through_time >= 0)
            {
                through_time -= Time.deltaTime; //スタートしてからの秒数を格納
                
            }
            else
            {
                through = false;
                through_time = 0.7f;
            }
        }
            
        if (push_f && col && !(svm.is_item_grab))
        {
            grab = true;
            svm.Grabbed_Item();
            push_f = false;
        }
        if (grab && grabbing_player.GetComponent<NetworkPlayerCheck>().is_mine)
        {
            this.photonView.RequestOwnership();
            Vector3 pos = grabbing_player.transform.position;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            pos.y += 2.0f;
            gameObject.GetComponent<Rigidbody>().position = pos;
        }
        if (push_v && grab == true)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            Vector3 vec = grabbing_player.transform.forward;
            gameObject.GetComponent<Rigidbody>().AddForce(vec * 400.0f, ForceMode.Force);
            grab = false;
            col = false;
            through = true;
            item_hit_flag = false;
            svm.Threw_Item();
            push_v = false;
        }
        if(timer_end ==true)
        {
            if (grabbing_player)
            {
                grab = false;
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                Vector3 vec = grabbing_player.transform.forward;
                gameObject.GetComponent<Rigidbody>().AddForce(vec * 100.0f, ForceMode.Force);
                through = false;
                col = false;
                item_hit_flag = false;
                svm.Threw_Item();
                push_v = false;
                push_f = false;
                timer_end = false;
            }
        }
    }

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && (grab == false) && (other.gameObject.GetComponent<PlayerMove>().enabled == true))
        {
            col = true;
            grabbing_player = other.gameObject;
        }
    }
}
