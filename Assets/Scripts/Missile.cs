using UnityEngine;

public class Missile : MonoBehaviour
{
	private Rigidbody2D _rigidbody;
	private float maxLifetime = 2.0f;

    public float speed;

	private void Awake()
	{
        _rigidbody = GetComponent<Rigidbody2D>();
	}

	public void Fire(Vector2 direction)
	{
		_rigidbody.velocity = direction * speed;
		Destroy(this.gameObject, maxLifetime);
	}
}
