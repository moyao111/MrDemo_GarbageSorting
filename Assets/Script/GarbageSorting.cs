using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;
//using static Zinnia.Cast.PointsCast;
/// <summary>
/// 垃圾分类检测，动画
/// </summary>
public class GarbageSorting : MonoBehaviour
{
    int score;
    public GameObject player;
    private Animator animator;
   
    void Start()
    {
      //  player = GameObject.FindGameObjectWithTag("RightController");
        animator = GetComponent<Animator>();
     //   Invoke(nameof(Open), 4);
    }

    void Update()
    {
        //Find();
    }

    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.tag);
       if (other.gameObject.tag==gameObject.tag)
        {
            Destroy(other.gameObject);
            Manager.Instance.score +=10;
        }
        else
        {
            Destroy(other.gameObject);
            Manager.Instance.score -= 10;
        }
    }
   
    #region 没用的
    public void Entered(/*Event data*//*SurfaceData data*/)
    {
        //  Debug.Log("Entered");
        // Debug.Log(data.id);

        //if (data.id)
        //{

        animator.SetBool("open1", true);
        animator.SetBool("close1", false);
        //}
    }
    public void Exited()
    {
      //  Debug.Log("Exited");
        Invoke("close", 2f);
    }


    private void Find()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance < 2)
        {
            //  animator.SetTrigger("open");

            animator.SetBool("open1", true);
            animator.SetBool("close1", false);
        }
        if (distance > 2.5)
        {
            // animator.SetTrigger("close");
            // Invoke("close", 2f);
            animator.SetBool("open1", false);
            animator.SetBool("close1", true);
        }

    }
    private void close()
    {
     //   Debug.Log("close");
        // animator.SetTrigger("close");
        animator.SetBool("open1", false);
        animator.SetBool("close1", true);
    }
    #endregion

    public void Open()
    {
        animator.SetBool("open1", true);
    }
    
    

}

