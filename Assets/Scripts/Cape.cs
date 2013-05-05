using UnityEngine;
using System.Collections;

public class Cape : MonoBehaviour {
	
	public Transform hitbox;
	
	// Use this for initialization
	void Start () {
		Physics.IgnoreCollision(hitbox.collider, this.collider);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
