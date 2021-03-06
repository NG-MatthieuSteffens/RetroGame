﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	[SerializeField]
	private LayerMask m_enemyLayer;
	
	[SerializeField]
	private float m_movementSpeed;

	[SerializeField]
	private float m_jumpPower;
	
	[SerializeField]
	private Vector3 m_grownSize = new Vector3( 1, 2, 1 );
	
	[SerializeField]
	private Vector3 m_shinkSize = new Vector3( 1, 1, 1);
	
	private bool m_isGrounded;
	
	[SerializeField]
	private float m_invulnerabilityTime = 1f;
	private bool m_isInvulnerability;

	[SerializeField]
	private PowerupBase m_currentPowerup;
	
	/// <summary>
	/// Cached Transform.
	/// </summary>
	public new Transform transform;

	/// <summary>
	/// Cached Rigidbody.
	/// </summary>
	public new Rigidbody2D rigidbody2D;
	
	/// <summary>
	/// Cached Collider.
	/// </summary>
	public new Collider2D collider2D;
	
	/// <summary>
	/// Cached Renderer.
	/// </summary>
	public new Renderer renderer;
	
	public int PlayerDirection
	{
		get;
		private set;
	}
	
	public PowerupBase CurrentPowerup
	{
		get
		{
			return m_currentPowerup;
		}
	}
	
	public void SetPowerUp(PowerupBase power, System.Type type = null)
	{
		PowerupBase oldPowerup = m_currentPowerup;
		
		if( type != null )
		{
			// Script call. (Like Fireflower when it's deactive.
			m_currentPowerup = PowerupBase.AddPowerUp( type, this );
		}
		else
		{
			// Power up object calls.
			m_currentPowerup = power ? power.SetToPlayer( this ) : null;
		}
		
		if( oldPowerup )
		{
			oldPowerup.OnDeActivate( this );
			Destroy( oldPowerup );
		}
		
		if( m_currentPowerup)
		{
			m_currentPowerup.OnActivate( this );
		}
	}
	
	public void Grow(bool isGrown = true)
	{ 
		if( isGrown )
		{
			transform.localScale = m_grownSize;
		}
		else
		{
			transform.localScale = m_shinkSize;
		}
	}
	
	public void TakeDamage()
	{
		if( m_isInvulnerability )
		{
			return;
		}
		
		if( m_currentPowerup )
		{
			StartCoroutine( WaitInvulnerabilityTime(m_invulnerabilityTime) );
			SetPowerUp( null );
			return;
		}
		
		GameOver();
	}
	
	public void GameOver()
	{
		GameManager.IsGameOver = true;
		gameObject.SetActive( false );
	}
	
	public IEnumerator WaitInvulnerabilityTime(float time)
	{
		m_isInvulnerability = true;
		yield return new WaitForSeconds( time );
		m_isInvulnerability = false;
	}
	
	private void Awake()
	{
		GameManager.currentPlayer = this;
		
		transform = GetComponent<Transform>();
		rigidbody2D = GetComponent<Rigidbody2D>();
		collider2D = GetComponent<Collider2D>(); 
		renderer = GetComponent<Renderer>();
	}
	
	private void Update()
	{
		if( GameManager.IsWorldEnded )
		{
			return;
		}
	
		float horizontalAxis = Input.GetAxis("Horizontal");
		transform.Translate( Vector3.right * horizontalAxis * m_movementSpeed * Time.deltaTime );
		
		if( horizontalAxis > 0 )
		{
			PlayerDirection = 1;
		}
		else if( horizontalAxis < 0 )
		{
			PlayerDirection = -1;
		}
		
		if( m_currentPowerup && Input.GetButtonDown("Action2") )
		{
			m_currentPowerup.DoPowerUp( this );
		}
	}
	
	private void FixedUpdate()
	{ 
		if( GameManager.IsWorldEnded )
		{
			return;
		}
	
		if( m_isGrounded && Input.GetAxis("Action1") > 0 )
		{
			rigidbody2D.velocity = Vector2.zero;
			rigidbody2D.AddForce( Vector2.up * m_jumpPower, ForceMode2D.Impulse );
		}
		
		if( !GameManager.cameraBounds.Intersects( collider2D.bounds ))
		{
			Vector2 position = transform.position;
			
			// Ignore Camera Bounds if
			if( GameManager.cameraBounds.max.x > position.x && GameManager.cameraBounds.min.x < position.x && GameManager.cameraBounds.min.y < position.y )
			{
				return;
			}
			
			GameOver();
		}
	}
	
	private void OnCollisionEnter2D(Collision2D collision)
	{
		Vector2 normal = collision.contacts[0].normal;
		
		if( collision.transform.CompareTag("Block") && normal.y < 0 && normal.x == 0)
		{
			ItemBlock block = collision.transform.GetComponent<ItemBlock>();
			
			// If enemy is over block and block got hitted, kill enemy.
			RaycastHit2D hit = Physics2D.Raycast( collision.transform.position + Vector3.up, Vector2.up, 1, m_enemyLayer);
			
			if( hit )
			{
				EnemyBase enemy = hit.collider.GetComponent<EnemyBase>();
				
				if( enemy )
				{
					enemy.Die();
				}
			}
			
			if( block )
			{
				block.OnHit(); 
			}
			else
			{
				GameObject.Destroy(collision.gameObject);
			}
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if( collision.contacts[0].normal.y > 0 ) 
		{
			m_isGrounded = true;
		}
	}
	
	private void OnCollisionExit2D(Collision2D collision)
	{
		m_isGrounded = false;
	}
}
