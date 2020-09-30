using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float jumpForce = 500f;
    [SerializeField] float doubleJumpForce = 500f;
    bool doubleJumpAllowed = false;
    bool onGround = false;
    [SerializeField] private LayerMask groundLayerMask;
    public bool isDashing = false;
    private float dashTime = 0.5f;

    public Rigidbody2D rigidbody2d;
    public BoxCollider2D boxCollider2d;

    private TimeManager timeManager;
    [SerializeField] private GameObject moveOptions;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
        timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Movement controls
        if (IsGrounded() && Input.GetKeyDown(KeyCode.UpArrow))
        {
            StopShowingOptions();
            rigidbody2d.AddForce(new Vector2(0, jumpForce));
            doubleJumpAllowed = true;
        }
        else if (doubleJumpAllowed && Input.GetKeyDown(KeyCode.UpArrow))
        {
            rigidbody2d.AddForce(new Vector2(0, jumpForce));
            doubleJumpAllowed = false;
        }
        else if (!IsGrounded() && Input.GetKeyDown(KeyCode.DownArrow))
        {
            StopShowingOptions();
            rigidbody2d.AddForce(new Vector2(0, -jumpForce));
            rigidbody2d.AddForce(new Vector2(0, -jumpForce));
        }


        // A "dash" simulated by speeding up time briefly
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StopShowingOptions();
            timeManager.ChangeTimescale(3f, dashTime);
            isDashing = true;
            StartCoroutine(Dash());
        }

        if (isDashing == true)
        {
            transform.Rotate(0, 0, 500 * Time.deltaTime);
        }

        // keep MoveOptions transform position updated
        moveOptions.transform.position = transform.position;
    }

    private bool IsGrounded()
    {
        float extraHeightText = .05f; 
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, extraHeightText, groundLayerMask);
        //Color rayColor;
        //if (raycastHit.collider != null)
        //{
        //    rayColor = Color.green;
        //}
        //else
        //{
        //    rayColor = Color.red;
        //}
        //Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
        //Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
        //Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, boxCollider2d.bounds.extents.y), Vector2.right * (boxCollider2d.bounds.extents.x), rayColor);
        //Debug.Log(raycastHit.collider);
        return raycastHit.collider != null;
    }

    public void ShowOptions()
    {
        timeManager.ChangeTimescale(0f, 0f);
        moveOptions.SetActive(true);
        if (IsGrounded())
        {
            moveOptions.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            moveOptions.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void StopShowingOptions()
    {
        timeManager.ResetTimescale();
        moveOptions.SetActive(false);
    }

    IEnumerator Dash()
    {
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
    }
}
