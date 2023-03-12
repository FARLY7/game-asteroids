using UnityEngine;

public class Missile : MonoBehaviour
{
	public float maxLifetime;
    public float speed;

	private Rigidbody2D _rigidbody;

	private void Awake()
	{
        _rigidbody = GetComponent<Rigidbody2D>();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Destroy(this.gameObject);
	}

	public void Fire(Vector2 direction)
	{
		_rigidbody.velocity = direction * speed;
		Destroy(this.gameObject, maxLifetime);
	}
}
