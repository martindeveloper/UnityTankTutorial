using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public float Damage;
	public float LifeTime = 4;

	public void Shoot(float zAngle, float speed)
	{
		Vector3 shootDir = Quaternion.Euler(0, 0, zAngle) * Vector3.right;
		rigidbody2D.velocity = shootDir * speed * 80;

		StartCoroutine (DestroyAfterSeconds ());
	}

	private IEnumerator DestroyAfterSeconds()
	{
		yield return new WaitForSeconds(LifeTime);

		Destroy (gameObject);
	}
}