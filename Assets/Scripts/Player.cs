using UnityEngine;

public class Player : MonoBehaviour
{
    public float thrustSpeed;
    public float turnSpeed;
    public float fireRate;
	public Missile missilePrefab;

	private Rigidbody2D _rigidBody;
    
    private bool _thrust;
    private bool _turnRight;
    private bool _turnLeft;
    private bool _fire;
	private float _nextFire;

	private void Awake()
	{
        _nextFire = 0.0f;
		_rigidBody = GetComponent<Rigidbody2D>();
	}

    // Update is called once per frame
    private void Update()
    {
        _thrust     = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        _turnRight  = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
		_turnLeft   = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        _fire       = Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0);

		if (_fire && Time.time > _nextFire)
		{
			_nextFire = Time.time + this.fireRate;
			Shoot();
		}
	}

	private void FixedUpdate()
	{
		if(_thrust) {
            _rigidBody.AddForce(transform.up * this.thrustSpeed);
		}


        if (!(_turnLeft && _turnRight))
        {
            if (_turnRight)
            {
                _rigidBody.AddTorque(-this.turnSpeed);
            }
            if (_turnLeft)
            {
				_rigidBody.AddTorque(this.turnSpeed);
            }
        }
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.CompareTag("Asteroid"))
		{
			_rigidBody.velocity = Vector2.zero;
			_rigidBody.angularVelocity = 0.0f;
			this.gameObject.SetActive(false);

			FindObjectOfType<GameManager>().PlayerDied();
		}
	}

	private void Shoot()
    {
		Missile missile = Instantiate(this.missilePrefab, transform.position, transform.rotation);
		missile.Fire(transform.up);
	}
}
