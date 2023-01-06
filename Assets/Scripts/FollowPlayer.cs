using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public SpriteRenderer mapBounds;
    public Vector2 offset = new(0, 2.5f);
    public Vector2 movementSpeedCameraMultiplier;


    private Rigidbody2D playerRigidBody;
    private float xMin, xMax, yMin, yMax;
    private float camY, camX;
    private float camHorizontalOrthsize;
    private Camera mainCam;
    private float camOrthsize;
    private Vector2 movementOffset;


    void Start()
    {
        playerRigidBody = player.GetComponent<Rigidbody2D>();

        xMin = mapBounds.bounds.min.x;
        xMax = mapBounds.bounds.max.x;
        yMin = mapBounds.bounds.min.y;
        yMax = mapBounds.bounds.max.y;

        mainCam = GetComponent<Camera>();
        camOrthsize = mainCam.orthographicSize;
        camHorizontalOrthsize = mainCam.aspect * camOrthsize;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        movementOffset = playerRigidBody.velocity * movementSpeedCameraMultiplier;

        camY = Mathf.Clamp(player.transform.position.y + offset.y + movementOffset.y, yMin + camOrthsize, yMax - camOrthsize);
        camX = Mathf.Clamp(player.transform.position.x + offset.x + movementOffset.x, xMin + camHorizontalOrthsize, xMax - camHorizontalOrthsize);

        this.transform.position = new Vector3(camX, camY, -10);
    }
}
