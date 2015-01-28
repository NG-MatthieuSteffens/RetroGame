using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
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
	
	private void Awake()
	{
		transform = GetComponent<Transform>();
		m_cameraPosition = transform.position;
	}
	
	private void Update()
	{
		if( m_target )
		{
			m_cameraPosition.x = m_target.position.x;
		}

		if( m_movementMode == CameraMovementMode.Follow || ( m_movementMode == CameraMovementMode.MoveForward &&  m_cameraPosition.x > transform.position.x ) )
		{
			transform.position = Vector3.SmoothDamp( transform.position, m_cameraPosition, ref m_cameraMovementSmooth, m_smooth );
		}

	}
}
