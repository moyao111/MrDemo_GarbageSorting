using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 丢,挂载到垃圾上
/// </summary>
public class diu : MonoBehaviour
{
    Rigidbody mRb;
    bool isDiscard;//标志是否放手
    private  GameObject camera;
    // Start is called before the first frame update
    void Start()
    {
        mRb = GetComponent<Rigidbody>();
        camera = GameObject.Find("SDKSystem");
    }

    // 不能用update,因为用fixupdate刚体刚激活
    private void FixedUpdate()
    {
        //拿起后被放下
        if (isDiscard)
        {
            isDiscard = false;
            //Vector3 dis = mRb.position - Camera.main.transform.position;
            Vector3 dis = mRb.position - camera.transform.position;
            mRb.AddForce((dis + transform.up) * 90);
          //  Debug.Log(dis);
        }
    }
    //放下物体时调用
    public void Ondiscard()
    {
        isDiscard = true;
    }
}
