using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class SyncVariableManager : MonoBehaviourPunCallbacks
{
    public int player_exist_num = 13; //プレイヤー全員が減算命令を実行するため、4人×3人死亡+1人(勝者)=13
    float damage = 0.34f;

    void PlayerDead()
    {
        photonView.RPC(nameof(Num_of_Player_Decrement), RpcTarget.All, new object[] { });
    }

    public void Damage(string PlayerObjectName)
    {
        photonView.RPC(nameof(DamageMethod), RpcTarget.All, new object[] { PlayerObjectName });
    }

    [PunRPC]
    public void Num_of_Player_Decrement(PhotonMessageInfo info)
    {
        --player_exist_num;
    }

    [PunRPC]
    void DamageMethod(string PlayerObjectName, PhotonMessageInfo info)
    {
        GameObject.Find(PlayerObjectName).transform.Find("HPUI").transform.Find("HPBar").GetComponent<Slider>().value -= damage;
        if (GameObject.Find(PlayerObjectName).transform.Find("HPUI").transform.Find("HPBar").GetComponent<Slider>().value <= 0.31f)
        {
            //photonView.RequestOwnership();
            Destroy(GameObject.Find(PlayerObjectName).gameObject);
            PlayerDead();
        }
    }
}
