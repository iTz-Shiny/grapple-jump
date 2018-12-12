using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HookSystem : MonoBehaviour {

    // 1
    public GameObject redRopeHingeAnchor;
    public GameObject blueRopeHingeAnchor;
    public DistanceJoint2D ropeJoint;
    public Transform crosshair;
    public SpriteRenderer crosshairSprite;
    public PlayerMovement playerMovement;
    private bool ropeAttached;
    private Vector2 playerPosition;
    private Rigidbody2D redRopeHingeAnchorRb;
    private Rigidbody2D blueRopeHingeAnchorRb;
    private SpriteRenderer redRopeHingeAnchorSprite;
    private SpriteRenderer blueRopeHingeAnchorSprite;
    public LineRenderer ropeRenderer;
    public LayerMask ropeLayerMask;
    private float ropeMaxCastDistance = 6f;
    private List<Vector2> ropePositions = new List<Vector2>();
    private bool distanceSet;
    private float angle;

void Awake()
    {
        // 2
        ropeJoint.enabled = false;
        playerPosition = transform.position;
        redRopeHingeAnchorRb = redRopeHingeAnchor.GetComponent<Rigidbody2D>();
        blueRopeHingeAnchorRb = blueRopeHingeAnchor.GetComponent<Rigidbody2D>();
        redRopeHingeAnchorSprite = redRopeHingeAnchor.GetComponent<SpriteRenderer>();
        blueRopeHingeAnchorSprite = blueRopeHingeAnchor.GetComponent<SpriteRenderer>();
        ropeRenderer.enabled = false;

    }

    void Update()
    {
        // 3
        var worldMousePosition =
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        var facingDirection = worldMousePosition - transform.position;
        var aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
        if (aimAngle < 0f)
        {
            aimAngle = Mathf.PI * 2 + aimAngle;
        }

        angle = aimAngle;

        // 4
        var aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;
        // 5
        playerPosition = transform.position;

        // 6
        if (!ropeAttached)
        {
            SetCrosshairPosition(aimAngle);
        }
        else
        {
            crosshairSprite.enabled = false;
        }
        HandleInput(aimDirection);
        UpdateRopePositions();

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0))
        {

            Vector3 startPos = transform.position;
            Vector3 endPos = mousePos;
            ropeRenderer.startColor = Color.blue;
            ropeRenderer.endColor = Color.blue;
            ropeRenderer.SetPosition(0, startPos);
            ropeRenderer.SetPosition(1, endPos);
            ropeRenderer.enabled = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            ropeRenderer.enabled = false;
        }

        if (Input.GetMouseButton(1))
        {

            Vector3 startPos = playerPosition;
            Vector3 endPos = mousePos;
            ropeRenderer.startColor = Color.red;
            ropeRenderer.endColor = Color.red;
            ropeRenderer.SetPosition(0, startPos);
            ropeRenderer.SetPosition(1, endPos);
            ropeRenderer.enabled = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            ropeRenderer.enabled = false;
        }

    }

    private void SetCrosshairPosition(float aimAngle)
    {
        if (!crosshairSprite.enabled)
        {
            crosshairSprite.enabled = true;
        }

        var x = transform.position.x + 6f * Mathf.Cos(aimAngle);
        var y = transform.position.y + 6f * Mathf.Sin(aimAngle);

        var crossHairPosition = new Vector3(x, y, 0);
        crosshair.transform.position = crossHairPosition;
    }

    // 1
    private void HandleInput(Vector2 aimDirection)
    {
        bool leftHeld = false;
        bool rightHeld = false;

        if (Input.GetMouseButton(0))
        {
            leftHeld = true;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            leftHeld = false;
        }

        if (Input.GetMouseButton(1))
        {
            rightHeld = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            rightHeld = false;
        }

        //Red Platform
        if (rightHeld)
        {
            // 2
            if (ropeAttached) return;
            ropeRenderer.startColor = Color.red;
            ropeRenderer.endColor = Color.red;
            ropeRenderer.enabled = true;

            var hit = Physics2D.Raycast(playerPosition, aimDirection, ropeMaxCastDistance, ropeLayerMask);

            // If hit Red platform
            if (hit.collider.CompareTag("Red") & hit.collider.GetType() == typeof(CircleCollider2D))
            {
                Debug.Log("Rope Hit Something");
                ropeAttached = true;
                if (!ropePositions.Contains(hit.point))
                {
                    // 4
                    // Jump slightly to distance the player a little from the ground after grappling to something.
                    transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(1f * Mathf.Cos(angle), 1.2f * Mathf.Sin(angle)), ForceMode2D.Impulse);
                    ropePositions.Add(hit.point);
                    ropeJoint.distance = Vector2.Distance(playerPosition, hit.point);
                    redRopeHingeAnchorSprite.enabled = true;
                }
            }
            // 5
            else
            {
                ropeRenderer.enabled = false;
                ropeAttached = false;
                ropeJoint.enabled = false;
            }
        }
        else
        {
            ResetRope();
        }

        //Blue Platform
        if (leftHeld)
        {
            // 2
            if (ropeAttached) return;
            ropeRenderer.startColor = Color.blue;
            ropeRenderer.endColor = Color.blue;
            ropeRenderer.enabled = true;

            var hit = Physics2D.Raycast(playerPosition, aimDirection, ropeMaxCastDistance, ropeLayerMask);

            // If hit Red platform
            if (hit.collider.CompareTag("Blue") & hit.collider.GetType() == typeof(CircleCollider2D))
            {
                Debug.Log("Rope Hit Something");
                ropeAttached = true;
                if (!ropePositions.Contains(hit.point))
                {
                    // 4
                    // Jump slightly to distance the player a little from the ground after grappling to something.
                    transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(1f * Mathf.Cos(angle), 1.2f * Mathf.Sin(angle)), ForceMode2D.Impulse);
                    ropePositions.Add(hit.point);
                    ropeJoint.distance = Vector2.Distance(playerPosition, hit.point);
                    blueRopeHingeAnchorSprite.enabled = true;
                }
            }
            // 5
            else
            {
                ropeRenderer.enabled = false;
                ropeAttached = false;
                ropeJoint.enabled = false;
                ResetRope();
            }
        }
        else
        {
            ResetRope();
        }

    }

private void UpdateRopePositions()
{
    // 1
    if (!ropeAttached)
    {
        return;
    }

    // 2
    ropeRenderer.positionCount = ropePositions.Count + 1;

    // 3
    for (var i = ropeRenderer.positionCount - 1; i >= 0; i--)
    {
        if (i != ropeRenderer.positionCount - 1) // if not the Last point of line renderer
        {
            ropeRenderer.SetPosition(i, ropePositions[i]);

            // 4
            if (i == ropePositions.Count - 1 || ropePositions.Count == 1)
            {
                var ropePosition = ropePositions[ropePositions.Count - 1];
                if (ropePositions.Count == 1)
                {
                        redRopeHingeAnchorRb.transform.position = ropePosition;
                        blueRopeHingeAnchorRb.transform.position = ropePosition;
                        if (!distanceSet)
                    {
                        ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
                        distanceSet = true;
                    }
                }
                else
                {
                    redRopeHingeAnchorRb.transform.position = ropePosition;
                    blueRopeHingeAnchorRb.transform.position = ropePosition;
                    if (!distanceSet)
                    {
                        ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
                        distanceSet = true;
                    }
                }
            }
            // 5
            else if (i - 1 == ropePositions.IndexOf(ropePositions.Last()))
            {
                var ropePosition = ropePositions.Last();
                    redRopeHingeAnchorRb.transform.position = ropePosition;
                    blueRopeHingeAnchorRb.transform.position = ropePosition;
                    if (!distanceSet)
                {
                    ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
                    distanceSet = true;
                }
            }
        }
        else
        {
            // 6
            ropeRenderer.SetPosition(i, transform.position);
        }
    }
}

// 6
private void ResetRope()
    {
        ropeJoint.enabled = false;
        ropeAttached = false;
        playerMovement.onGrapple = false;
        ropeRenderer.enabled = false;
        ropeRenderer.positionCount = 2;
        ropeRenderer.SetPosition(0, transform.position);
        ropeRenderer.SetPosition(1, transform.position);
        ropePositions.Clear();
        redRopeHingeAnchorSprite.enabled = false;
        blueRopeHingeAnchorSprite.enabled = false;
    }



}
