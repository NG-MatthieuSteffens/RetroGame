using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public static Player currentPlayer;
	public static Bounds cameraBounds;
	
	private static GameManager m_currentManager;
	
	public static bool IsWorldEnded
	{
		get
		{
			return m_currentManager.m_isEnded;
		}
		
		set
		{
			m_currentManager.m_isEnded = value;
		}
	}
	
	public static bool IsGameOver
	{
		get
		{
			return m_currentManager.m_isGameOver;
		}
		
		set
		{
			
			if( value )
			{
				m_currentManager.m_gameoverUI.SetActive( true );
			}
		
			m_currentManager.m_isGameOver = value;
		}
	}
	
	public static float GameTime
	{
		get
		{
			return m_currentManager.m_time;
		}
		
		set
		{
			m_currentManager.m_time = value;
		}
	}

	public static void AddCoin()
	{
		m_currentManager.m_coins++;
	}
	
	public static void AddScore(int score)
	{
		m_currentManager.m_score += score;
	}
	
	[SerializeField]
	private GameObject m_gameoverUI;
	
	[SerializeField]
	private Text m_gameoverText;
	
	[SerializeField]
	private float m_timeSpeed = 1.5f;
	
	[SerializeField]
	private float m_time = 400;
	
	[SerializeField]
	private Text m_scoreText;
	
	[SerializeField]
	private string m_scoreFormat = "00000";
	
	[SerializeField]
	private Text m_coinsText;
	
	[SerializeField]
	private string m_coinsFormat = "$x00";
	
	[SerializeField]
	private Text m_timeText;
	
	private int m_score;
	private int m_coins;
	
	private bool m_isEnded;
	private bool m_isGameOver;
	
	private void Awake()
	{
		if( m_currentManager == null )
		{
			m_currentManager = this;
		}
	
		m_score = 0;
		m_coins = 0;
		
		if( !m_scoreText || !m_coinsText || !m_timeText )
		{
			enabled = false;
			Debug.LogError("Missing Text Components!");
		} 
	}
	
	private void Update()
	{
		if( Input.GetButtonDown("Quit") )
		{
			Application.Quit();
		}
	}
	
	private void FixedUpdate()
	{
		m_timeText.text = m_time.ToString("000");
		m_scoreText.text = m_score.ToString( m_scoreFormat );
		m_coinsText.text = m_coins.ToString( m_coinsFormat );
		
		if( m_isEnded && m_time == 0 )
		{
			m_gameoverText.text = "World Cleared!";
			m_gameoverUI.SetActive(true);
		}
		
		if ( !m_isEnded && !m_isGameOver)
		{
			m_time -= Time.fixedDeltaTime * m_timeSpeed;
			
			 if( m_time <= 0 )
			 {
			 	currentPlayer.GameOver();
			 } 
		 }
	}
}
