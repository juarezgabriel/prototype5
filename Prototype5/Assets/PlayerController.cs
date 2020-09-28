using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float jumpForce = 500f;
    [SerializeField] float doubleJumpForce = 250f;
    bool doubleJump = false;
    [SerializeField] private LayerMask groundLayerMask;

    public Rigidbody2D rigidbody2d;
    public BoxCollider2D boxCollider2d;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (IsGrounded() && Input.GetKeyDown(KeyCode.UpArrow))
        {
            rigidbody2d.AddForce(new Vector2(0, jumpForce));
        }

        if (!IsGrounded() && Input.GetKeyDown(KeyCode.DownArrow))
        {
            rigidbody2d.AddForce(new Vector2(0, -jumpForce));
            rigidbody2d.AddForce(new Vector2(0, -jumpForce));
        }
        else if (!IsGrounded() && Input.GetKeyDown(KeyCode.UpArrow))
        {
            rigidbody2d.AddForce(new Vector2(0, doubleJumpForce));
            doubleJump = false;
        }

    }

    private bool IsGrounded()
    {
        doubleJump = true;
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

}
