using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 tetherPosition;
    public bool isTetherd;
    public GameObject tether;

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();

        
    }

    // Update is called once per frame
    void Update()
    {
        rb.transform.up = new Vector2(rb.velocity.y, -rb.velocity.x).normalized;
        if (isTetherd)
        {
            tether.transform.up = (tetherPosition - rb.position).normalized;
            float distance = (tetherPosition - rb.position).magnitude;
            tether.transform.localScale = new Vector3(0.1f, distance, 1f);
        }


        if (Input.anyKey)
        {
            
            if (!isTetherd)
            {
                isTetherd = true;
                tetherPosition = rb.position + 10 * new Vector2(rb.transform.up.x, rb.transform.up.y);
                Debug.Log(tetherPosition);
            }
            
        }
        else
        {
            isTetherd = false;
        }


        if(isTetherd)
        {
            rb.velocity = rb.velocity.magnitude * rb.transform.right;
        }
    }
}
