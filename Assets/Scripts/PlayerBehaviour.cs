using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour {

	private Animator animator;
	public Image healthBar;
	private Notification message;

	private bool gameOver;

	private bool attackWindowActive;
	private bool faceLeft = false;
	public float horizontalValue;

	public float speed = 1f;
	public int maxHealth = 100;
	public float health = 100;
	private int healing = 15;
	public int buffAttacks = 0;
	public bool hasFlower;
	public bool hasApple;

	private bool hasBuffPotion;
	private bool hasHealingPotion;
	// Use this for initialization
	void Start () {
		gameOver = false;
		animator = GetComponent<Animator>();
		healthBar = GameObject.Find("HealthBarInner").GetComponent<Image>();
		message = GameObject.Find("Notification").GetComponent<Notification>();

	}
	// Update is called once per frame
	 void Update()
	{
		PlayerControls();
		UpdateInterface();
		GameOver();
	}

	public void GameOver(){
		if (health <= 0) {
			gameOver = true;
			animator.SetBool("IsRunning", false);
			animator.SetTrigger("PlayerDie");
			AudioListener.volume = 0f;
			GameObject.Find("GameOver").GetComponent<Text>().enabled = true;
			animator.SetTrigger("PlayerDead");
			if (Input.GetKey(KeyCode.R))
			{		AudioListener.volume = 0f;
				GameReload();
			}
		}
	}
	public void GameReload()
	{
		AudioListener.volume = 1f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

	// synchronize all script properties and flags with UI elements
	void UpdateInterface(){
	
		foreach (GameObject flower in GameObject.FindGameObjectsWithTag("Flower")){
			flower.GetComponent<Image>().color = hasFlower ? Color.gray : Color.black;
		}
		foreach (GameObject apple in GameObject.FindGameObjectsWithTag("Apple")){
			apple.GetComponent<Image>().color = hasApple ? Color.gray : Color.black;
		}
		GameObject.Find("BuffAttacksRemain").GetComponent<Text>().text = buffAttacks.ToString(); 
		GameObject.Find("HealingPotion").GetComponent<Image>().color = 
			hasHealingPotion ? Color.white : Color.black;
		GameObject.Find("BuffPotion").GetComponent<Image>().color =
			hasBuffPotion ? Color.white : Color.black;
	}

	private void move(float movSpeed)
    {
		float inputX = Input.GetAxis("Horizontal");
		Vector3 movement = new Vector3(movSpeed * inputX, 0, 0);
		movement *= Time.deltaTime;
		transform.Translate(movement);
		animator.SetBool("IsRunning", true);
	}
	// key presses and actions
	void PlayerControls() {


		if (Input.GetKey(KeyCode.D) && !gameOver && !Input.GetKey(KeyCode.A))
		{
			if (faceLeft)
            {
				transform.Rotate(new Vector3(0, 180, 0));
				faceLeft = false;
			}
			move(speed);
		}
		if (Input.GetKey(KeyCode.A) && !gameOver && !Input.GetKey(KeyCode.D))
		{
            if (!faceLeft)
            {
				transform.Rotate(new Vector3(0, 180, 0));
				faceLeft = true;
			}
			move(-speed);
		}
		
		// setting animator properties to cause animation transitions!!
		if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !gameOver)
		{
			animator.SetBool("IsRunning", false);
		}
		if (Input.GetKeyDown(KeyCode.Space) && !gameOver)
		{
			applyBuff();
			animator.SetTrigger("Attack");

		}
		

		// crafting logic
		if (Input.GetKeyDown(KeyCode.Z) && !gameOver)
		{
				TryCraftBuffPotion();
		}
		if (Input.GetKeyDown(KeyCode.X) && !gameOver)
		{
			
			if (hasHealingPotion)
			{
				UseHealingPotion();
			}
			else
			{
				TryCraftHealingPotion();
			}
		}
		
	}


	private void TryCraftBuffPotion()
	{
		if (hasFlower)
		{
			hasBuffPotion = true;
			hasFlower = false;
			buffAttacks += 10;
			message.Notify("Crafted buff damage potion");
		}
	}
	private void UseHealingPotion()
	{
		if (health <= maxHealth - healing) {
			health += healing;
			UpdateHealth();
			hasHealingPotion = false;
		}
		
	}

	private void TryCraftHealingPotion()
	{
		if (hasApple)
		{
			hasHealingPotion = true;
			hasApple = false;
			message.Notify("Crafted healing potion");
		}
	}
	private void applyBuff()
    {
		if (buffAttacks > 0)
		{
			buffAttacks -= 1;
		}
		if (buffAttacks == 0)
			hasBuffPotion = false;
	}


	public void Damaged()
	{
		health -= healing;
		UpdateHealth();
	}
	public void UpdateHealth()
    {
		healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1f);
	}
	private void OnTriggerStay2D(Collider2D collision) {
		
		if (collision.tag == "Enemy" && Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !gameOver)
			{
			collision.GetComponent<EnemyBehaviour>().SoundHit();
			collision.GetComponent<EnemyBehaviour>().Damaged(buffAttacks);
		}

		if (collision.tag == "FlowerRes" && Input.GetKeyDown(KeyCode.E)) {
			hasFlower = true;
			Destroy(collision.gameObject);
			message.Notify("Flower has been picked up");
		}
		if (collision.tag == "AppleRes" && Input.GetKeyDown(KeyCode.E))
		{
			hasApple = true;
			Destroy(collision.gameObject);
			message.Notify("Apple has been picked up");
		}
	
	}


	
}
