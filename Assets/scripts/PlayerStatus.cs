using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Range(0, 2)]       private int __HP;


    void Start()
    {

    }

    void Update()
    {

    }

    public void damage( int iAmount )
    {
        __HP -= iAmount;
        if ( __HP <= 0 )
            kill();
        else
            updateLimbs();
    }

    public void kill()
    {
        // Invoke GameOver & restart level ?
        Debug.Log("DEAD : GAMEOVER TODO..");
    }

    public void updateLimbs()
    {

    }

}