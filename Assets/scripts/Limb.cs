using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Limb : MonoBehaviour
{

    public IEnumerator fadeTo( float initValue, float duration, float timeUntilStart)
    {
        yield return new WaitForSeconds(timeUntilStart);
        SpriteRenderer rdr = GetComponent<SpriteRenderer>();
        Color newColor = new Color(1,1,1,0);
        rdr.color = newColor;
        float alpha = rdr.color.a;
        for ( float t = 0.0f; t < 1.0f; t+=Time.deltaTime / duration )
        {
            newColor = new Color(1,1,1, Mathf.Lerp( initValue,alpha,1 ));
            rdr.color = newColor;
            if ( t > 0.98f)
            {
                gameObject.SetActive(false);
            }
            yield return null;
        }
    }

}
