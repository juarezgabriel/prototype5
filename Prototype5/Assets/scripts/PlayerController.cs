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
    private float dashCoolDown = 2f;
    private float dashTimer = 0f;

    public Rigidbody2D rigidbody2d;
    public BoxCollider2D boxCollider2d;

    private TimeManager timeManager;
    private SoundManager soundManager;
    [SerializeField] private GameObject moveOptions;
    private bool isShowingOptions = false;

    [SerializeField] UnityEngine.Object impact;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
        timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        soundManager = GameObject.Find("Sounds").GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

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

        // Press space to skip choosing movement
        if (isShowingOptions && Input.GetKeyDown(KeyCode.Space))
        {
            StopShowingOptions();
        }

        // A "dash" simulated by speeding up time briefly
        if (dashTimer > 0f)
        {
            dashTimer -= Time.unscaledDeltaTime;
        }
        if (dashTimer <= 0 && isShowingOptions)
        {
            moveOptions.transform.GetChild(2).gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && dashTimer <= 0f)
        {
            StopShowingOptions();
            timeManager.ChangeTimescale(3f, dashTime);
            soundManager.PlayDash();
            isDashing = true;
            dashTimer = dashCoolDown;
            StartCoroutine(Dash());
        }

        if (isDashing == true)
        {
            transform.Rotate(0, 0, 500 * Time.deltaTime);
        }

        // keep MoveOptions transform position updated
        if (isShowingOptions)
        {
            moveOptions.transform.position = transform.position;
        }
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
        isShowingOptions = true;
        timeManager.ChangeTimescale(0f, 0f);
        moveOptions.transform.position = transform.position;
        moveOptions.SetActive(true);
        if (IsGrounded())
        {
            moveOptions.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            moveOptions.transform.GetChild(1).gameObject.SetActive(true);
        }

        if (dashTimer < 0f)
        {
            moveOptions.transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            moveOptions.transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    public void StopShowingOptions()
    {
        isShowingOptions = false;
        timeManager.ResetTimescale();
        moveOptions.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ground" && collision.relativeVelocity.magnitude > 2f)
        {
            GameObject.Instantiate(impact, collision.GetContact(0).point, Quaternion.identity);
            soundManager.PlayLand();
        }
    }

    IEnumerator Dash()
    {
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
    }
}
