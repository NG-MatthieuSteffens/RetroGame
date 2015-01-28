using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	[SerializeField]
	private float m_movementSpeed;

	[SerializeField]
	private float m_jumpPower;
	
	private bool m_isGrounded;
	
	/// <summary>
	/// Camera for checking is player on screen.
	/// </summary>
	private Transform m_mainCameraTransform;
	private Bounds m_mainCameraBounds;
	
	/// <summary>
	/// Cached Transform.
	/// </summary>
	private new Transform transform;

	/// <summary>
	/// Cached Rigidbody.
	/// </summary>
	private new Rigidbody2D rigidbody2D;
	
	private Vector3 BoundsCenter
	{
		get
		{
			Vector3 newPosition = m_mainCameraTransform.position;
			newPosition.z = transform.position.z;
			
			return newPosition;
		}
	}
	
	private void Awake()
	{
		transform = GetComponent<Transform>();
		rigidbody2D = GetComponent<Rigidbody2D>();
		m_mainCameraTransform = Camera.main.transform;
		
		if( m_mainCameraTransform )
		{
			// Sets bounds size to match camera size.
			Vector3 boundsSize = m_mainCameraTransform.camera.ViewportToWorldPoint( Vector3.one * 1.5f );
			boundsSize.z = 2;
			m_mainCameraBounds = new Bounds( BoundsCenter, boundsSize );
		}
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
		
		if( m_mainCameraTransform )
		{
			m_mainCameraBounds.center = BoundsCenter;
			
			if( !m_mainCameraBounds.Contains( transform.position ) )
			{
				// TODO: 'Real' GameOver!
				Debug.Log("Game Over!");
			}
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
