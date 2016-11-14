using UnityEngine;
using System.Collections;
using System;

public class Bowser : MonoBehaviour {
	public Transform targetTransform;
	private Transform bowserTransform;
	public LayerMask grunt;
	public Transform testGrunt;
	public int moveSpeed;
	private Animator animator;
	private SpriteRenderer spriteRender;
	private Vector2 direction;
	private Rigidbody2D rigid;
	public int jumpSpeed;
	private AudioSource deadSonic, worldSound;
	private AudioSource[] allSounds;
	private string[] referencesNamesLeft = { "reference_three_left", "reference_two_left" };
	private string[] referencesNamesRight = { "reference_two_right", "reference_one_right"  };
	// Use this for initialization
	void Start () {
		rigid = GetComponent<Rigidbody2D>();
		bowserTransform = transform;
		animator = GetComponent<Animator>();
		spriteRender = GetComponent<SpriteRenderer>();
		moveSpeed = 2;
		jumpSpeed = 5;
		GameObject[] coins = GameObject.FindGameObjectsWithTag("coin");
		direction = Vector2.left;
		allSounds = GetComponents<AudioSource>();
		deadSonic = allSounds[0];
		worldSound = allSounds[1];

		foreach (GameObject coin in coins) {
        Physics2D.IgnoreCollision(coin.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }
	}

	// Update is called once per frame
	void Update () {
		transform.Translate(direction*moveSpeed*Time.deltaTime);
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if(collision.gameObject.name == "sonic") {
			worldSound.Stop();
			deadSonic.Play();
		}

		if((Array.IndexOf(referencesNamesLeft, collision.gameObject.name) != -1) && direction == Vector2.left) {
			rigid.velocity = new Vector2(rigid.velocity.x, jumpSpeed);
			rigid.AddForce(new Vector2(0, jumpSpeed));
		}

		if((Array.IndexOf(referencesNamesRight, collision.gameObject.name) != -1) && direction == Vector2.right) {
			rigid.velocity = new Vector2(rigid.velocity.x, jumpSpeed);
			rigid.AddForce(new Vector2(0, jumpSpeed));
		}

		if(collision.gameObject.name == "reference_one_left" && direction == Vector2.left) {
			direction = Vector2.right;
			spriteRender.flipX = true;
		}

		if(collision.gameObject.name == "reference_three_right" && direction == Vector2.right) {
			direction = Vector2.left;
			spriteRender.flipX = false;
		}
	}

	void FixedUpdate(){
		bool isNotJump = Physics2D.OverlapCircle(testGrunt.position, 0.01f, grunt.value);
		animator.SetBool("jump_bowser", !isNotJump);
	}
}
