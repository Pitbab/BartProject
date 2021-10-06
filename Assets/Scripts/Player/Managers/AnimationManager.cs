using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator PlayerAnimator;
    private const string Slide = "Slide";
    private const string Jumping = "Jumping";
    private const string Falling = "Falling";
    private const string Climbing = "Climbing";
    private const string WallEnd = "WallEnd";
    private const string Trapped = "Trapped";

    private void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        //Debug.Log(GetAnimName());
    }

    public void PlaySlidingAnim()
    {
        PlayerAnimator.SetTrigger(Slide);
    }

    public void PlayjumpAnim()
    {
        PlayerAnimator.SetTrigger(Jumping);
    }



    public string GetAnimName()
    {
        string playingAnim = " ";
        if (this != null)
        {
            playingAnim = PlayerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            return playingAnim;
        }

        return null;
    }

    public void PlayClimbingAnim()
    {
        PlayerAnimator.SetBool(Climbing, true);
    }

    public void StopClimbingAnim()
    {
        PlayerAnimator.SetBool(Climbing, false);
    }

    public void PlayFallingAnim()
    {
        PlayerAnimator.SetBool(Falling, true);
        PlayerAnimator.SetBool(WallEnd, false);
    }

    public void StopFallingAnim()
    {
        PlayerAnimator.SetBool(Falling, false);
    }

    public void SetMovingBlend(float HorAxis, float VerAxis)
    {
        PlayerAnimator.SetFloat("Horizontal", HorAxis);
        PlayerAnimator.SetFloat("Vertical", VerAxis);
    }
    public void PlayClimbingUp()
    {
        PlayerAnimator.SetBool(WallEnd, true);
    }

    public void StopClimbingUp()
    {
        PlayerAnimator.SetBool(WallEnd, false);
    }

    public void PlayTrapped()
    {
        PlayerAnimator.SetBool(Trapped, true);
    }

    public void StopTrapped()
    {
        PlayerAnimator.SetBool(Trapped, false);
    }



}
