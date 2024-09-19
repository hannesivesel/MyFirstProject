using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpSpeed = 7f;

    [Header("Health")]
    public int playerHealth = 3;
    [Tooltip("This sets the amount of time to count down")]
    public float damageCooldown = 0.3f;
    private float _damageCooldownTimer;
    
    [Header("Audio")]
    
    [Header("GroundCheck")]
    public bool playerIsGrounded;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public Vector2 groundBoxSize = new Vector2(0.8f, 0.2f);
    
    [Header("Components")]
    private InputActions _input;
    private Rigidbody2D _rigidbody2D;
    
    private Animator _animator;

    private void Start()
    {
       
        _input = GetComponent<InputActions>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
            
    }

    private void Update()
    {
        playerIsGrounded = Physics2D.OverlapBox(groundCheck.position, groundBoxSize, 0f, whatIsGround);
        
        
        
        if (_input.Jump && playerIsGrounded)
        {
            _rigidbody2D.linearVelocityY = jumpSpeed;
        }

        UpdateAnimation();
        Attack();
    }

    private void FixedUpdate()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D.linearVelocityX = _input.Horizontal * moveSpeed;

        if (_rigidbody2D.linearVelocity.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (_rigidbody2D.linearVelocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void Attack()
    {
        if (!Physics2D.OverlapCircle(groundCheck.position, 0.2f, LayerMask.GetMask("Enemy"))) return;
        
        var enemyColliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f, LayerMask.GetMask("Enemy"));

        foreach (var enemy in enemyColliders)
        {
            Destroy(enemy.gameObject);
        }

        _rigidbody2D.linearVelocityY = jumpSpeed / 1.3f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(groundCheck.position, groundBoxSize);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void TakeDamage()
    {
        if (Time.time > _damageCooldownTimer)
        {
            playerHealth -= 1;
            _damageCooldownTimer = Time.time + damageCooldown;
        }

        if (playerHealth <= 0)
        {
            RestartScene();
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Death"))
        {
            RestartScene();
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            TakeDamage();
        }
    }
    
    private void UpdateAnimation()
    {
        if (playerIsGrounded)
        {
            if (_input.Horizontal != 0)
            {
                _animator.Play("Player_Walk");
            }
            else
            {
                _animator.Play("Player_Idle");
            }
        }
        else
        {
            if (_rigidbody2D.linearVelocityY > 0)
            {
                _animator.Play("Player_Jump");
            }
            else
            {
                _animator.Play("Player_Fall");
            }
        }
    }
}
