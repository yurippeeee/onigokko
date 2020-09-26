using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHit : MonoBehaviour
{
    float damage = 0.34f;
    public bool attack_collision_flag = false;
    public GameObject text;
    
    //public GameObject hpbar;

    //オブジェクトと接触した瞬間に呼び出される
    void OnCollisionEnter(Collision other)
    {
        //攻撃した相手がEnemyの場合
        if (other.gameObject.CompareTag("Player"))
        {
            if(attack_collision_flag == false)
            {
                other.transform.Find("HPUI").transform.Find("HPBar").GetComponent<Slider>().value -= damage;
                if (other.transform.Find("HPUI").transform.Find("HPBar").GetComponent<Slider>().value <= 0)
                {
                    text.GetComponent<Timer>().player_exit_num -= 1;
                    other.gameObject.GetComponent<PlayerMove>().player_exist = false;
                    
                    Destroy(other.gameObject);
                }
                attack_collision_flag = true;
            }
            
            
        }
        
    }
}
