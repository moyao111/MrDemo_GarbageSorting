using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    private Transform RightHand;
    private SkinnedMeshRenderer RightHandmesh;
    private bool handVisbility = true;
    private Animator Animator;
    void Start()
    {
      // RightHand =/* GameObject.Find("RightHand(zz)").*/GetComponent<Transform>();
        RightHandmesh = /*GameObject.Find("RightHand(zz)").*/GetComponent<SkinnedMeshRenderer>();
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void ToggleHandVisbility()
    {
       // handVisbility = !handVisbility;
    // gameObject.SetActive(handVisbility);

        // RightHandmesh.enabled = !RightHandmesh.enabled;
    }
   /* private void Update()
    {
        if (Input.GetMouseButton(0))
        {
           
        }
        else
        {
            Animator.SetBool("close", false);

        }
    }
    */
    
    public void Grab()
    {
       // Debug.Log("1");
        Animator.SetBool("close", true);
    }
    public void Release()
    {
       // Debug.Log("q1");
        Animator.SetBool("close", false);
    }
}
