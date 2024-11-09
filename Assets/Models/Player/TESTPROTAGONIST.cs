using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TESTPROTAGONIST : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Slider sliderX;
    [SerializeField] Slider sliderZ;

    public void ChangeMovementX()
    {
        anim.SetFloat("velocity_x", (sliderX.value * 2) - 1);
    }
    public void ChangeMovementZ()
    {
        anim.SetFloat("velocity_z", (sliderZ.value * 2) - 1);
    }
    public void Hit()
    {
        anim.SetBool("Attack",anim.GetBool("Attack"));
    }
    public void GetHit()
    {
        anim.SetTrigger("GetHit");
    }
    public void Death()
    {
        anim.SetTrigger("Death");
    }
    public void Victory()
    {
        anim.SetTrigger("Victory");
    }
}
