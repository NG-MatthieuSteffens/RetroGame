using UnityEngine;
using System.Collections;

public class LevelEnd : MonoBehaviour {

	// "Flag Pole"
	
	[SerializeField]
	private Transform m_flag;
	
	[SerializeField]
	private float m_flagSlidingSpeed = 2f;
	private bool m_isSlidingDone;
	
	[SerializeField]
	private int m_scorePerSecond = 100;
	
	[SerializeField]
	private int m_maxJumpScore = 2000;
	
	private Player m_player;
	private float m_offsetY;
	
	private new Transform transform;
	
	private void CalculateScore()
	{
		float playerPosition = (m_player.transform.position.y - m_player.transform.localScale.y / 2) + transform.position.y - m_offsetY;
		float multiplier = Mathf.Abs( Mathf.Min( 1, m_flag.position.y / playerPosition ) );
		
		GameManager.AddScore( (int)(m_maxJumpScore * Mathf.Round(multiplier * 10) / 10) );
	}

	private void Awake()
	{
		transform = GetComponent<Transform>();
		m_offsetY = transform.localScale.y / 2;
	}
	
	private void Update()
	{
		if( GameManager.IsWorldEnded )
		{
			if( !m_isSlidingDone)
			{
				if( m_player.transform.position.y - m_player.transform.localScale.y / 2 >= transform.position.y - m_offsetY )
				{
					Vector3 speed = Vector3.down * Time.deltaTime * m_flagSlidingSpeed;
					m_player.transform.Translate( speed, Space.World );
					m_flag.Translate( speed, Space.World ); 
				}
				else
				{
					m_isSlidingDone = true;
					m_player.rigidbody2D.isKinematic = false;
				}
			}
			else if( GameManager.GameTime > 1 )
			{
				GameManager.AddScore( m_scorePerSecond );
				GameManager.GameTime -= 1;
			}
			else
			{
				// For Sure...
				GameManager.GameTime = 0;
			}
		}
	}
	
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if( collision.transform .CompareTag("Player") && !GameManager.IsWorldEnded)
		{
			GameManager.IsWorldEnded = true;
			m_player = GameManager.currentPlayer;
			m_player.rigidbody2D.isKinematic = true;
			
			CalculateScore();	
		}
	}
}
