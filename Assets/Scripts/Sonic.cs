using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Sonic : MonoBehaviour {
	public int moveSpeed;
	public int jumpSpeed;
	private Rigidbody2D rigid;
	private Animator animator;
	private SpriteRenderer spriteRender;
	public LayerMask grunt;
	public Transform testGrunt;
	private int score = 0;
	public Text textScore;
	public Text msg;
	public Text timeElapsed;
	public GameObject end;
	public GameObject bowser;
	private float seconds = 15f;
	private bool gameEnd = false;
	private AudioSource coin, jump, dead;
	private AudioSource[] allSounds;

	// Use this for initialization
	void Start () {
		rigid = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		spriteRender = GetComponent<SpriteRenderer>();
		moveSpeed = 2;
		jumpSpeed = 5;
		end.SetActive(false);
		allSounds = GetComponents<AudioSource>();
		jump = allSounds[0];
		coin = allSounds[1];
	}

	// Update is called once per frame
	void Update () {
		if(!gameEnd) {
			seconds -= Time.deltaTime;
			timeElapsed.text = "Tiempo Restante: "+((int)seconds).ToString()+" Sec.";
		}

		if(seconds <= 0) {
			msg.text = "Perdiste!!";
			gameObject.SetActive(false);
			end.SetActive(true);
			dead.Play();
		}

		if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A)) {
			animator.SetBool("walk",true);
		}

		if(Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A)) {
			animator.SetBool("walk",false);
		}

		if(Input.GetKey(KeyCode.D)) {
			spriteRender.flipX = false;
			transform.Translate(Vector2.right*moveSpeed*Time.deltaTime);
		}

		if(Input.GetKey(KeyCode.A)) {
			spriteRender.flipX = true;
			transform.Translate(Vector2.left*moveSpeed*Time.deltaTime);
		}

		if(Input.GetKeyDown(KeyCode.W)) {
			if(Physics2D.OverlapCircle(testGrunt.position, 0.01f, grunt.value)) {
				jump.Play();
				rigid.velocity = new Vector2(rigid.velocity.x, jumpSpeed);
				rigid.AddForce(Vector2.up * jumpSpeed);
			}
		}
	}

	void FixedUpdate(){
		bool isNotJump = Physics2D.OverlapCircle(testGrunt.position, 0.01f, grunt.value);
		animator.SetBool("jump", !isNotJump);
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if(collision.gameObject.tag == "coin") {
			coin.Play();
			score++;
			textScore.text = "Puntaje: "+score.ToString();
			Destroy(collision.gameObject);
			if(score == 10){
				msg.text = "Ganaste!!";
				end.SetActive(true);
				Destroy(bowser.gameObject);
				gameEnd = true;
			}
		}

		if(collision.gameObject.name == "bowser") {
			msg.text = "Perdiste!!";
			gameObject.SetActive(false);
			end.SetActive(true);
			gameEnd = true;
		}
	}

	public void onClickRestart(){
		SceneManager.LoadScene("main");
	}
}
