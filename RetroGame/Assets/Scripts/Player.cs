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
	
	private bool m_isGrounded;
	
	/// <summary>
	/// Cached Transform.
	/// </summary>
	private new Transform transform;

	/// <summary>
	/// Cached Rigidbody.
	/// </summary>
	private new Rigidbody2D rigidbody2D;
	
	public void GameOver()
	{
		// TODO: 'Real' GameOver!
		Debug.Log("Game Over!");
	}
	
	private void Awake()
	{
		CurrentPlayer = this;
		
		transform = GetComponent<Transform>();
		rigidbody2D = GetComponent<Rigidbody2D>();
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
		
		if( !CameraController.CameraBounds.Contains( transform.position ) )
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
