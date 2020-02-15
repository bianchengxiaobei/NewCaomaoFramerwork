using System;
using System.Collections.Generic;
using UnityEngine;
namespace CaomaoFramework
{
    public class FPBaseAnimator
    {
        //private FP m_curFrameIndex = 0;
        private Animator m_animator;
        //private bool m_bPlaying = false;

        public FPBaseAnimator(Animator anim)
        {
            this.m_animator = anim;
        }

        public virtual void Play(string stateName)
        {
            if (this.m_animator != null)
            {
                this.m_animator.Play(stateName);
            }
        }


        public virtual void Simulate()
        {
            
        }
    }
}
