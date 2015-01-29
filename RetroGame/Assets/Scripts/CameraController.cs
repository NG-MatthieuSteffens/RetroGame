using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	private static Bounds cameraBounds;
	public static Bounds CameraBounds
	{
		get
		{
			return cameraBounds;
		}
	}
	
	private enum CameraMovementMode
	{
		/// <summary>
		/// Follows target everywhere.
		/// </summary>
		Follow,
		
		/// <summary>
		/// Moves only if player moves forward.
		/// </summary>
		MoveForward,
	}
	
	[SerializeField]
	private CameraMovementMode m_movementMode;
	
	[SerializeField]
	private Transform m_target;
	
	[SerializeField]
	private float m_smooth = 0.5f;
	
	private Vector3 m_cameraPosition;
	private Vector3 m_cameraMovementSmooth;
	private new Transform transform;
	
	private Vector3 BoundsCenter
	{
		get
		{
			Vector3 newPosition = transform.position;
			newPosition.z = m_target.position.z;
			
			return newPosition;
		}
	}
	
	private void Awake()
	{
		transform = GetComponent<Transform>();
		m_cameraPosition = transform.position;
		
		// Sets bounds size to match camera size.
		Vector3 boundsSize = camera.ViewportToWorldPoint( Vector3.one * 1.5f );
		boundsSize.z = 5;
		cameraBounds = new Bounds( BoundsCenter, boundsSize );
	}
	
	private void Update()
	{
		if( m_target )
		{
			m_cameraPosition.x = m_target.position.x;
		}

		if( m_movementMode.Equals( CameraMovementMode.Follow ) || ( m_movementMode.Equals( CameraMovementMode.MoveForward ) &&  m_cameraPosition.x > transform.position.x ) )
		{
			transform.position = Vector3.SmoothDamp( transform.position, m_cameraPosition, ref m_cameraMovementSmooth, m_smooth );
		}
	}
	
	private void FixedUpdate()
	{
		cameraBounds.center = BoundsCenter;
	}
}
