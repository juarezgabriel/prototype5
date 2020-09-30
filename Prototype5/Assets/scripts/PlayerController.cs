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

    //health
    public HealthbarScript healthbar;
    public int maxHealth = 100;
    public int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
        timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        soundManager = GameObject.Find("Sounds").GetComponent<SoundManager>();

        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
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

        if ((collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "bonusEnemy") && !isDashing)
        {
            TakeDamage(10);
        }
        if (collision.gameObject.tag == "bonusEnemy" && isDashing)
        {
            Debug.Log("it should work");
            GetHealth(30);
        }


    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthbar.SetHealth(currentHealth);
    }

    public void GetHealth(int health)
    {
        currentHealth += 10;
        healthbar.SetHealth(currentHealth);
    }


    IEnumerator Dash()
    {
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
    }
}
