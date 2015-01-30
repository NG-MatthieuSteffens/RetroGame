using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	public static Player CurrentPlayer
	{
		get;
		private set;
	}
	
	[SerializeField]
	private float m_movementSpeed;

	[SerializeField]
	private float m_jumpPower;
	
	[SerializeField]
	private Vector3 m_grownSize = new Vector3( 1, 2, 1 );
	
	[SerializeField]
	private Vector3 m_shinkSize = new Vector3( 1, 1, 1);
	
	private bool m_isGrown;
	private bool m_isGrounded;
	
	[SerializeField]
	private float m_invulnerabilityTime = 1f;
	private bool m_isInvulnerability;
	
	/// <summary>
	/// Cached Transform.
	/// </summary>
	private new Transform transform;

	/// <summary>
	/// Cached Rigidbody.
	/// </summary>
	private new Rigidbody2D rigidbody2D;
	
	/// <summary>
	/// Cached Collider.
	/// </summary>
	private new Collider2D collider2D;
	
	public void Grow(bool isGrown = true)
	{ 
		m_isGrown = isGrown;
		
		if( m_isGrown )
		{
			transform.localScale = m_grownSize;
		}
		else
		{
			transform.localScale = m_shinkSize;
			
			// Moves player down so it won't be over any enemies.
			transform.Translate( 0, -m_grownSize.y / 2, 0 );
		}
	}
	
	public void TakeDamage()
	{
		if( m_isInvulnerability )
		{
			return;
		}
		
		if( m_isGrown )
		{
			StartCoroutine( WaitInvulnerabilityTime() );
			Grow( false );
			return;
		}
		
		GameOver();
	}
	
	private void GameOver()
	{
		// TODO: 'Real' GameOver!
		Debug.Log("Game Over!");
	}
	
	private IEnumerator WaitInvulnerabilityTime()
	{
		m_isInvulnerability = true;
		yield return new WaitForSeconds( m_invulnerabilityTime );
		m_isInvulnerability = false;
	}
	
	private void Awake()
	{
		CurrentPlayer = this;
		
		transform = GetComponent<Transform>();
		rigidbody2D = GetComponent<Rigidbody2D>();
		collider2D = GetComponent<Collider2D>();
	}
	
	private void Update()
	{
		float horizontalAxis = Input.GetAxis("Horizontal");
		transform.Translate( Vector3.right * horizontalAxis * m_movementSpeed * Time.deltaTime );
	}
	
	private void FixedUpdate()
	{ 
		if( m_isGrounded && Input.GetAxis("Vertical") > 0 )
		{
			rigidbody2D.velocity = Vector2.zero;
			rigidbody2D.AddForce( Vector2.up * m_jumpPower, ForceMode2D.Impulse );
		}
		
		if( !CameraController.CameraBounds.Intersects( collider2D.bounds ) )
		{
			GameOver();
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if( collision.contacts[0].point.y < transform.position.y ) 
		{
			m_isGrounded = true;
		}
	}
	
	private void OnCollisionExit2D(Collision2D collision)
	{
		m_isGrounded = false;
	}
}
