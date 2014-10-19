using UnityEngine;
using System.Collections;

[System.Serializable]
public enum ControlScheme
{
		WASD,
		Arrows
}

public class Tank : MonoBehaviour
{
		public float Health;
		public float Damage;
		public float Speed;
		public float RotationSpeed;
		public ControlScheme ActiveControlScheme;
		public GameObject Bullet;
		public GameObject FireAnchor;
		private GameObject BulletsParent;
		private ControlSchemeKeys ActiveKeys;

		public struct ControlSchemeData
		{
				public Vector3 position;
				public Vector3 rotation;
				public float speed;
		}

		public struct ControlSchemeKeys
		{
				public KeyCode Up;
				public KeyCode Down;
				public KeyCode Right;
				public KeyCode Left;
				public KeyCode Fire1;
		}

		public void Start ()
		{
				BulletsParent = GameObject.FindGameObjectWithTag ("Bullets");

				switch (ActiveControlScheme) {
				default:
				case ControlScheme.WASD:
						ActiveKeys = new ControlSchemeKeys (){
				Up = KeyCode.W,
				Down = KeyCode.S,
				Right = KeyCode.D,
				Left = KeyCode.A,
				Fire1 = KeyCode.F
			};
						break;
			
				case ControlScheme.Arrows:
						ActiveKeys = new ControlSchemeKeys (){
				Up = KeyCode.UpArrow,
				Down = KeyCode.DownArrow,
				Right = KeyCode.RightArrow,
				Left = KeyCode.LeftArrow,
				Fire1 = KeyCode.M
			};
						break;
				}
		}

		public void Update ()
		{
				Vector3 newPosition = transform.position;
				float speedCoeficient = Speed * Time.deltaTime;
				Vector3 newRotation = transform.rotation.eulerAngles;

				ControlSchemeData inputData = new ControlSchemeData (){
			position = newPosition,
			rotation = newRotation,
			speed = speedCoeficient
		};

				inputData = TankUpdate (inputData, ActiveKeys);

				transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.Euler (inputData.rotation), speedCoeficient * RotationSpeed);
				transform.position = inputData.position;
		}

		private ControlSchemeData TankUpdate (ControlSchemeData controlData, ControlSchemeKeys keys)
		{
				Vector3 newPosition = controlData.position;
				Vector3 newRotation = controlData.rotation;

				if (Input.GetKey (keys.Down)) {
						newPosition.y -= controlData.speed;
						newRotation.z = 270;
				}
		
				if (Input.GetKey (keys.Up)) {
						newPosition.y += controlData.speed;
						newRotation.z = 90;
				}
		
				if (Input.GetKey (keys.Left)) {
						newPosition.x -= controlData.speed;

						if (newRotation.z == 90) {
								newRotation.z = 180 - newRotation.z / 2;
						} else if (newRotation.z == 270) {
								newRotation.z = newRotation.z - 45;
						} else {
								newRotation.z = 180;
						}
				}
		
				if (Input.GetKey (keys.Right)) {
						newPosition.x += controlData.speed;
			
						if (newRotation.z == 90) {
								newRotation.z = newRotation.z / 2;
						} else if (newRotation.z == 270) {
								newRotation.z = newRotation.z + 45;
						} else {
								newRotation = Vector3.zero;
						}
				}

				if (Input.GetKeyUp (keys.Fire1)) {
						GameObject bullet = (GameObject)Instantiate (Bullet, FireAnchor.transform.position, Quaternion.identity);
						bullet.transform.parent = BulletsParent.transform;

						Bullet bulletEntity = bullet.GetComponent<Bullet> ();

						bulletEntity.Damage = Damage;
						bulletEntity.Shoot (newRotation.z, controlData.speed);

						AudioManager.GetInstance ().PlayShoot ();
				}

				return new ControlSchemeData (){
			position = newPosition,
			rotation = newRotation,
			speed = controlData.speed
		};
		}

		public void OnCollisionEnter2D (Collision2D col)
		{
				if (col.gameObject.tag == "Bullet") {
						Health -= col.gameObject.GetComponent<Bullet> ().Damage;

						if (Health <= 0) {
								Debug.Log (name + " died");

								AudioManager.GetInstance ().PlayExplosion ();

								Destroy (gameObject);
						} else {
								rigidbody2D.velocity = Vector2.zero;
								rigidbody2D.angularVelocity = 0;
						}
			
						Destroy (col.gameObject);
				}
		}
}