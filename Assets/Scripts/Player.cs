#region Old
//using System.Collections;
//using UnityEngine;
//using TMPro;
//using static Unity.Collections.Unicode;

//public class Player : MonoBehaviour
//{
//    public int health = 100;
//    public int score=0;
//    public float moveSpeed = 5f;
//    public float runSpeed = 10f; 
//    public float jumpForce = 10f;
//    public float pushForce = 10f;
//    public Transform groundCheck;
//    public float groundCheckRadius = 0.2f;
//    public LayerMask groundLayer;
//    public int extraJumpValue = 1;
//    public static bool isDead;
//    public TextMeshProUGUI scoreText;
//    public GameObject gameOverText;
//    public GameObject restartText;

//    private Rigidbody2D rb;
//    private bool isGrounded;
//    private bool isRunning;
//    private Animator animator;
//    private int extraJumps;
//    private SpriteRenderer spriteRenderer;
//    private float lockedXPosition; 

//    private const string IDLE = "Player_Idle";
//    private const string WALK = "Player_Walk";
//    private const string RUN = "Player_Run";
//    private const string JUMP = "Player_Jump";
//    private const string FALL = "Player_Fall";
//    private const string Dead = "Player_Dead";
//    void Start()
//    {
//        AudioManager.Instance.StopAllSounds();
//        rb = GetComponent<Rigidbody2D>();
//        animator = GetComponent<Animator>();
//        spriteRenderer = GetComponent<SpriteRenderer>();
//        extraJumps = extraJumpValue;
//        lockedXPosition = transform.position.x;
//        isDead = false;
//    }

//    void Update()
//    {
//        if(isDead)
//        {
//            if(Input.GetKeyDown(KeyCode.D)|| Input.GetKeyDown(KeyCode.A))
//            {
//                isDead= false;
//                UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
//            }

//        }
//        else
//        {
//            float moveInput = Input.GetAxis("Horizontal");
//            isRunning = Input.GetKey(KeyCode.LeftShift);
//            float currentSpeed = isRunning ? runSpeed : moveSpeed;
//            //rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);
//            // --- ÊÚÏíá ---
//            // ÇááÇÚÈ áã íÚÏ íÊÍÑß ÃÝÞíÇð¡ ÝÞØ äÍÇÝÙ Úáì ÓÑÚÊå ÇáÑÃÓíÉ (ááÞÝÒ æÇáÓÞæØ)
//            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
//            // --- äåÇíÉ ÇáÊÚÏíá ---

//            if (isGrounded)
//            {
//                extraJumps = extraJumpValue;
//            }

//            if (Input.GetKeyDown(KeyCode.Space))
//            {
//                if (isGrounded)
//                {
//                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
//                    AudioManager.Instance?.PlayJump();
//                }
//                else if (extraJumps > 0)
//                {
//                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
//                    AudioManager.Instance?.PlayJump();
//                    extraJumps -= 1;
//                }
//            }

//            if (moveInput > 0)
//            {
//                transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
//            }
//            else if (moveInput < 0)
//            {
//                transform.localScale = new Vector3(-1.5f, 1.5f, 1.5f);
//            }
//            SetAnimationAndAudio(moveInput);
//        }

//    }

//    private void FixedUpdate()
//    {
//        if (isDead) return; // <-- ÅÖÇÝÉ
//        isGrounded = Physics2D.OverlapCircle(groundCheck.position,groundCheckRadius,groundLayer);
//    }

//    void LateUpdate()
//    {
//        if (isDead) return; // <-- ÅÖÇÝÉ
//        transform.position = new Vector3(lockedXPosition, transform.position.y, transform.position.z);
//    }

//    private void SetAnimationAndAudio(float moveInput)
//    {
//        float speed = Mathf.Abs(moveInput);

//        if (!isGrounded && rb.linearVelocity.y > 0.1f)
//        {
//            PlayAnimation(JUMP);
//            AudioManager.Instance?.StopFootsteps();
//        }
//        else if (!isGrounded && rb.linearVelocity.y < -0.1f)
//        {
//            PlayAnimation(FALL);
//            AudioManager.Instance?.StopFootsteps();
//        }
//        else if (isGrounded && speed > 0.1f && isRunning)
//        {
//            PlayAnimation(RUN);
//            AudioManager.Instance?.PlayRun();
//        }
//        else if (isGrounded && speed > 0.1f)
//        {
//            PlayAnimation(WALK);
//            AudioManager.Instance?.PlayWalk();
//        }
//        else if (isGrounded)
//        {
//            PlayAnimation(IDLE);
//            AudioManager.Instance?.StopFootsteps();
//        }
//    }

//    private void PlayAnimation(string animationName)
//    {
//        animator.Play(animationName);
//    }

//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.gameObject.tag == "Damage")
//        {
//            health -= 25;
//            rb.linearVelocity = new Vector2(rb.linearVelocity.x, pushForce);
//            StartCoroutine(BlinkRed());

//            if(health <= 0)
//            {
//                Die();
//            }
//        }
//        else if (collision.gameObject.tag == "Deadly")
//        {
//            Die();
//        }
//        else if (collision.gameObject.tag == "UpperSky")
//        {
//            rb.linearVelocity = Vector2.zero;
//        }

//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if(collision.gameObject.tag=="Coin")
//        {
//            AudioManager.Instance?.PlayCoin();
//            Score(1);
//            Destroy(collision.gameObject);
//        }
//    }
//    private IEnumerator BlinkRed()
//    {
//        spriteRenderer.color = Color.red;
//        yield return new WaitForSeconds(0.1f);
//        spriteRenderer.color = Color.white;
//    }

//    private void Die()
//    {
//        gameOverText.SetActive(true);
//        restartText.SetActive(true);
//        isDead = true;

//        AudioManager.Instance?.PlayDeath();


//        PlayAnimation(Dead);

//        rb.linearVelocity =Vector2.zero;

//    }

//    private void Score( int coinValue)
//    {
//        score+= coinValue;
//        scoreText.text = $"Score : {score}";
//    }
//}
#endregion

using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth = 100;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float runSpeed = 10f;

    [Header("Jump Settings")]
    public float jumpForce = 10f;
    public int extraJumpValue = 1;
    public float pushForce = 10f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool isGrounded;
    private bool isRunning;
    private int extraJumps;
    private float lockedXPosition;
    private bool isDead;
    private bool wasGrounded;

    private float currentMoveInput;
    private bool hasMovementInput;

    private const string IDLE = "Player_Idle";
    private const string WALK = "Player_Walk";
    private const string RUN = "Player_Run";
    private const string JUMP = "Player_Jump";
    private const string FALL = "Player_Fall";
    private const string DEAD = "Player_Dead";

    private void Start()
    {
        InitializeComponents();
        InitializePlayer();
    }

    private void Update()
    {
        if (isDead) return;

        HandleInput();
        HandleJump();
        UpdateAnimationState();
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        CheckGrounded();
        HandleMovement();
    }

    private void LateUpdate()
    {
        if (isDead) return;

        LockXPosition();
    }

    #region Initialization

    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void InitializePlayer()
    {
        extraJumps = extraJumpValue;
        lockedXPosition = transform.position.x;
        currentHealth = maxHealth;
        isDead = false;
        wasGrounded = true;
        hasMovementInput = false;
    }

    #endregion

    #region Input & Movement

    private void HandleInput()
    {
        currentMoveInput = Input.GetAxis("Horizontal");
        isRunning = Input.GetKey(KeyCode.LeftShift);

        hasMovementInput = Mathf.Abs(currentMoveInput) > 0.1f;

        if (currentMoveInput > 0.1f)
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
        else if (currentMoveInput < -0.1f)
        {
            transform.localScale = new Vector3(-1.5f, 1.5f, 1.5f);
        }
    }

    private void HandleMovement()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                PerformJump();
            }
            else if (extraJumps > 0)
            {
                PerformJump();
                extraJumps--;
            }
        }

        if (isGrounded)
        {
            extraJumps = extraJumpValue;
        }
    }

    private void PerformJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        AudioManager.Instance?.PlayJump();
        AudioManager.Instance?.StopFootsteps(); 
    }

    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void LockXPosition()
    {
        transform.position = new Vector3(lockedXPosition, transform.position.y, transform.position.z);
    }

    #endregion

    #region Animation & Audio

    private void UpdateAnimationState()
    {
        bool justLanded = !wasGrounded && isGrounded;

        if (!isGrounded)
        {
            if (rb.linearVelocity.y > 0.1f)
            {
                PlayAnimation(JUMP);
            }
            else if (rb.linearVelocity.y < -0.1f)
            {
                PlayAnimation(FALL);
            }

            if (!AudioManager.Instance.IsFootstepSourceStopped())
            {
                AudioManager.Instance?.StopFootsteps();
            }
        }
        else
        {
            if (justLanded)
            {
                OnLand();
            }

            if (hasMovementInput)
            {
                if (isRunning)
                {
                    PlayAnimation(RUN);
                    PlayRunSound();
                }
                else
                {
                    PlayAnimation(WALK);
                    PlayWalkSound();
                }
            }
            else
            {
                PlayAnimation(IDLE);
                AudioManager.Instance?.StopFootsteps();
            }
        }

        wasGrounded = isGrounded;
    }

    private void OnLand()
    {
        if (hasMovementInput)
        {
            if (isRunning)
            {
                PlayRunSound();
            }
            else
            {
                PlayWalkSound();
            }
        }
        else
        {
            AudioManager.Instance?.StopFootsteps();
        }
    }

    private void PlayWalkSound()
    {
        if (isGrounded && hasMovementInput && !isRunning)
        {
            AudioManager.Instance?.PlayWalk();
        }
    }

    private void PlayRunSound()
    {
        if (isGrounded && hasMovementInput && isRunning)
        {
            AudioManager.Instance?.PlayRun();
        }
    }

    private void PlayAnimation(string animationName)
    {
        animator.Play(animationName);
    }

    #endregion

    #region Damage & Death

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, pushForce);
        StartCoroutine(BlinkRed());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator BlinkRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        PlayAnimation(DEAD);
        rb.linearVelocity = Vector2.zero;
        AudioManager.Instance?.StopFootsteps();

        GameManager.Instance?.GameOver();
    }

    #endregion

    #region Collisions

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        switch (collision.gameObject.tag)
        {
            case "Damage":
                TakeDamage(25);
                break;

            case "Deadly":
                Die();
                break;

            case "UpperSky":
                rb.linearVelocity = Vector2.zero;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Coin"))
        {
            CollectCoin(collision.gameObject);
        }
    }

    private void CollectCoin(GameObject coin)
    {
        AudioManager.Instance?.PlayCoin();
        GameManager.Instance?.AddScore(1);
        Destroy(coin);
    }

    #endregion

    #region Getters

    public bool IsDead => isDead;
    public bool IsGrounded => isGrounded;
    public bool IsRunning => isRunning;
    public bool HasMovementInput => hasMovementInput;

    #endregion
}