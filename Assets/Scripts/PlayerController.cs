using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    public Vector2 tetherPosition;
    public bool isTethered;
    public GameObject tether;
    public LayerMask wallMask;
    public float changeUpVelocitySensativity = 3.0f;
    public float velocityReleaseMultiplier = 1.05f;
    public float minimumVelocityForNoStuck = 1f;
    public float extraUpMultiplier = 0.1f;
    public GameController gameController;

    [Header("Sprites")]
    public Sprite grabSprite;
    public Sprite freeSprite;
    private SpriteRenderer sr;

    private Collider2D col;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip deathSound;
    public AudioClip tetherSound;
    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(10, 10);
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PointUp();

        DrawTether();

        HandleInput();

        FixDirection();

        SetSprite();

        if (!gameController.isRunning) {
            col.enabled = false;
        } else
        {
            col.enabled = true;
        }

    }

    private void SetSprite()
    {
        if (isTethered)
        {
            sr.sprite = grabSprite;
        } else
        {
            sr.sprite= freeSprite;
        }
    }

    private void FixDirection()
    {
        if (isTethered)
        {
            if (rb.velocity.magnitude > minimumVelocityForNoStuck)
            {
                if (Vector2.Dot(rb.velocity, rb.transform.right) > 0)
                {
                    rb.velocity = rb.velocity.magnitude * rb.transform.right;
                }
                else
                {
                    rb.velocity = rb.velocity.magnitude * -rb.transform.right;
                }
            }

        }
    }

    private void PointUp()
    {
        if (isTethered)
        {
            rb.transform.up = (tetherPosition - rb.position).normalized;
        }
        else
        {
            if (rb.velocity.magnitude > changeUpVelocitySensativity)
            {
                rb.transform.up = Vector2.Perpendicular(rb.velocity);
                if (rb.transform.up.y < 0)
                {
                    rb.transform.up = -rb.transform.up;
                }
                rb.transform.up = new Vector2(rb.transform.up.x, rb.transform.up.y) + (Vector2.up * extraUpMultiplier) / (rb.velocity.magnitude + 1);
            }
        }
    }

    private void DrawTether()
    {
        if (isTethered)
        {
            tether.SetActive(true);
            tether.transform.localScale = new Vector3(0.1f, (tetherPosition - rb.position).magnitude, 1f);
        }
        else
        {
            tether.SetActive(false);
        }
    }

    private void HandleInput()
    {
        if (Input.anyKey)
        {
            
            if (!isTethered)
            {

                RaycastHit2D hit = Physics2D.Raycast(rb.transform.position, rb.transform.up, float.MaxValue, wallMask);
                if (hit)
                {
                    if (hit.collider != null)
                    {
                        isTethered = true;
                        tetherPosition = hit.point;
                        gameController.AddSwing();
                    }
                }
            }
        }
        else
        {
            if (isTethered)
            {
                rb.velocity *= velocityReleaseMultiplier;
            }
            isTethered = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Wall")
        {
            gameController.SetDeathCanvas();
            StartCoroutine(Die());
        }
    }

    public IEnumerator Die()
    {
        audioSource.clip = deathSound;
        audioSource.Play();
        gameController.isRunning = false;
        yield return new WaitForSeconds(0.3f);
        col.enabled = false;
    }
    
}
