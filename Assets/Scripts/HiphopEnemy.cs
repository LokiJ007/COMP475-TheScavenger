using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiphopEnemy : Enemy
{
    public AudioClip enemyHide;
    private int a=0;


    protected override void OnGUI()
    {
       
        if (i < 15)
        {
            a++;

            /*for (var ren : Renderer in GetComponentsInChildren<Renderer>())
            {
                ren.enabled = !ren.enabled;
            }
            */
            
            GetComponent<Renderer>().enabled = true;
        }
        else
        {
            a++;
            if (i == 30)
            {
                i = 0;
                a = 0;
            }

            /*for (var ren : Renderer in GetComponentsInChildren<Renderer>())
            {
                ren.enabled = !ren.enabled;
            }
            */
            //GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
            GetComponent<Renderer>().enabled = false;
        }

    }  
        
    
}


