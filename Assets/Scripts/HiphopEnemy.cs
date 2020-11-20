using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiphopEnemy : Enemy
{
    public AudioClip enemyHide;
    private int a=0;
    protected override void OnGUI()
    {
        if (i < 3)
        {
            a++;
            MeshRenderer[] marr = this.GetComponentsInChildren<MeshRenderer>(true);
            SoundManager.instance.RandomizeSfx(enemyHide);
            foreach (MeshRenderer m in marr)
            {
                m.enabled = false;
            }
        }
        else
        {
            a++;
            if(a == 6)
            {
                i = 0;
            }
            MeshRenderer[] marr = this.GetComponentsInChildren<MeshRenderer>(true);
            foreach (MeshRenderer m in marr)
            {
                m.enabled = true;
            }
        }
    }
}


