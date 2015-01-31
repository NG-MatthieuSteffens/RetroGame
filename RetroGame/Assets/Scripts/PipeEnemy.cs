using UnityEngine;
using System.Collections;

public class PipeEnemy : EnemyBase 
{
	// TODO: Waiting!
	
	[SerializeField]
	private Collider2D m_pipeCollider;

	[SerializeField]
	protected float m_lerpSpeed;
	protected float m_lerpTime;
	
	[SerializeField]
	protected float m_lerpOffset = 0.5f;
	private bool m_isLerpIncreasing;
	
	/// <summary>
	/// LocalPosition from.
	/// </summary>
	[SerializeField]
	protected Vector3 m_fromPosition;
	
	/// <summary>
	/// LocalPostion to.
	/// </summary>
	[SerializeField]
	protected Vector3 m_toPosition;

	protected override void Start ()
	{
		Physics2D.IgnoreCollision( collider2D, m_pipeCollider, true );
		m_isLerpIncreasing = true;	
	}
	
	protected virtual void FixedUpdate()
	{
		HandleLerp();
		transform.localPosition = Vector3.Lerp( m_fromPosition, m_toPosition, m_lerpTime );
	}
	
	private void HandleLerp()
	{
		if( m_isLerpIncreasing )
		{
			m_lerpTime += Time.fixedDeltaTime * m_lerpSpeed;
		}
		else
		{
			m_lerpTime -= Time.fixedDeltaTime * m_lerpSpeed;
		}
		
		if( m_lerpTime - m_lerpOffset > 1 || m_lerpTime + m_lerpOffset < 0 )
		{
			m_isLerpIncreasing = !m_isLerpIncreasing;
		} 
	}
}
