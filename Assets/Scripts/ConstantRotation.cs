using UnityEngine;

public class ConstantRotation : MonoBehaviour {

	public float speed = 22.5f;
	public int axis = 1;

	void Update() {
		Vector3 rot = Vector3.zero;
		rot[axis] = Time.time * speed;
		transform.localRotation = Quaternion.Euler(rot);
	}
}
