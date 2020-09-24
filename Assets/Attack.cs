using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    //PlayerのAnimatorコンポーネント保存用
    private Animator animator;
    //右足のコライダー
    private Collider footCollider;

    // Use this for initialization
    void Start()
    {
        //PlayerのAnimatorコンポーネントを取得する
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
            //右足コライダーをオンにする
            footCollider.enabled = true;
            //footCollider.enabled = false;
        }
    }
    
}
