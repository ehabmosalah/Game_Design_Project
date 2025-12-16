using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEffects : MonoBehaviour
{
    public ParticleSystem attackParticle;

    public void PlayAttackParticle()
    {
        if (attackParticle != null)
            attackParticle.Play();
    }
}
