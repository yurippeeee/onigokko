using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class ItemHit : MonoBehaviourPunCallbacks
{
    //オブジェクトと接触した瞬間に呼び出される
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Item") && (GameObject.Find("Item(Clone)").GetComponent<ItemCatch>().through))
        {
            if(GameObject.Find("Item(Clone)").GetComponent<ItemCatch>().item_hit_flag == false)
            {
                GameObject.Find("Item(Clone)").GetComponent<ItemCatch>().item_hit_flag = true;
                GameObject.Find("Sync_Variable_Manager(Clone)").GetComponent<SyncVariableManager>().Damage(gameObject.name);
            }
        }
    }
}
