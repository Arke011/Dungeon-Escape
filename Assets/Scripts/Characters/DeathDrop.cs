using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathDrop : MonoBehaviour
{
    private Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(startMovement());
    }

   

    private IEnumerator startMovement()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("isMoving", true);
    }
}