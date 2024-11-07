using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testGettingHit : MonoBehaviour
{
    float timer = 0;
    [SerializeField] Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer < 2f)
        {
            timer += Time.deltaTime;
        }
        else
        {
            animator.SetTrigger("GetHit");
            timer = 0;
        }
    }
}
