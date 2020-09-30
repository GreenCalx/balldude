using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    [SerializeField] private float jump_factor = 400f;
    [Range(0, .5f)] [SerializeField] private float moveSmooth = .05f;
    [Range(0, .5f)] [SerializeField] private float airMoveSmooth = .05f;
    [Range(0f, 2f)] [SerializeField] private float slope_epsilon = 05f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform feet;
    [SerializeField] private Transform head;
    
    private Rigidbody2D rigidbody2D;
    private bool facingRight = true;
    private bool hasRightZRotation = false;
    private bool grounded;
    private Vector3 __velocity = Vector3.zero;
    private int groundLayerIndex = 0;

    private const float GROUNDED_RADIUS = .2f;
    private const float SPEED_FACTOR = 10f;
    private const float AIR_SPEED_FACTOR = SPEED_FACTOR / 1.5f;

    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        // As LayerMasks values in unity are 2^layerIndex, we find the right one using a log2..
        groundLayerIndex = (int)Mathf.Log(groundLayer.value, 2);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll( feet.position, GROUNDED_RADIUS, groundLayer);
        foreach ( Collider2D collider in colliders)
        {
            if ( collider.gameObject != this.gameObject)
            {
                grounded = true;
                //Debug.Log("Grounded : " + collider.gameObject.name );
            }
        }
    }

    protected void OnCollisionStay2D(Collision2D collision)
    {
        if (!hasRightZRotation)
            updatePlayerRotToSlope(collision);
        Debug.DrawRay( transform.position, transform.up * 10f, Color.white);
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        updatePlayerRotToSlope(collision);

    }

    private void updatePlayerRotToSlope(Collision2D collision)
    {
        // exit if not ground
        if (collision.gameObject.layer !=  groundLayerIndex )
            return;
        Debug.Log(" is ground " );

        //ContactPoint2D contact = new ContactPoint2D();
        ContactPoint2D[] contacts = {};
        contacts = collision.contacts;
        if (contacts.Length == 0)
            return;
        // todo : use the normal of segment linking both contact points
        ContactPoint2D contact = contacts[0];
        Debug.Log(" ground point found : n_contacts is " + contacts.Length );

        Vector2 ground_N = contact.normal;
        Vector2 up       = transform.up;

        Vector3 rayOrigin = this.transform.position;
        float rayLength = 10f;
        Debug.DrawRay( rayOrigin, new Vector3( ground_N.x, ground_N.y, 0) * rayLength, Color.red);

        // Nvector dot UPvector / Nnorm * UPnorm
        //float acos_val = Vector3.Dot( ground_N, up ) / (ground_N.magnitude * up.magnitude );
        //float angle = Mathf.Acos(acos_val);
  
        // TODO : 1 bug with 180d angle
        float angle2 = Vector2.Angle(ground_N, up);
        if ( angle2 == 0f )
        {
            Debug.Log("transfo ok");
            hasRightZRotation = true;
            return;
        }

        hasRightZRotation = false;
        transform.Rotate( 0f, 0f, angle2, Space.Self );
        Debug.Log("angle2 is " + angle2);

    }
 
    public void move( float move, bool doJump)
    {
        // flip if needed
        bool changingDirection =  
            ( ( move > 0 && !facingRight ) ||
            ( move < 0 && facingRight ) );
        
        if (changingDirection)
            flip();

        if ( !!grounded ) // ground movement
        {
            Vector3 velocity = new Vector2( move * SPEED_FACTOR, rigidbody2D.velocity.y );
            rigidbody2D.velocity = Vector3.SmoothDamp( rigidbody2D.velocity, velocity, ref __velocity, moveSmooth );

        } else if ( !grounded && move == 0f) { // STATIC AIR CONTROL
            Vector3 velocity = new Vector2( move * AIR_SPEED_FACTOR, rigidbody2D.velocity.y );
            rigidbody2D.velocity = Vector3.SmoothDamp( rigidbody2D.velocity, velocity, ref __velocity, airMoveSmooth );
        } else { // Jumping with some initial speed
            Vector3 velocity = new Vector2( move * AIR_SPEED_FACTOR, rigidbody2D.velocity.y );
            rigidbody2D.velocity = Vector3.SmoothDamp( rigidbody2D.velocity, velocity, ref __velocity, airMoveSmooth );
        }

        if ( grounded && doJump )
        {
            grounded = false;
            rigidbody2D.AddForce(new Vector2(0f, jump_factor));
        }



    }

    private void flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

}
