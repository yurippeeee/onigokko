using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class EnemyHit : MonoBehaviourPunCallbacks
{
    public bool attack_collision_flag = false;

    //オブジェクトと接触した瞬間に呼び出される
    void OnCollisionEnter(Collision collision)
    {
        GetComponent<SphereCollider>().enabled = false;

        if (collision.gameObject.CompareTag("Player")) 
        {
            if(attack_collision_flag == false)
            {
                attack_collision_flag = true;
                GameObject.Find("Sync_Variable_Manager(Clone)").GetComponent<SyncVariableManager>().Damage(collision.gameObject.name);
            }   
        }
    }
}
