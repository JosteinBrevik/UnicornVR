using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMovement : MonoBehaviour {
	private Rigidbody m_Rigidbody;
	// Use this for initialization
	void Start () {
		
	}

	private void Awake()
	{
		m_Rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		Move();
	}
	public float MoveSpeed = 1;
	private void Move()
	{
		
		Vector3 movement = new Vector3(0f, 0f, 1f) * MoveSpeed * Time.deltaTime;

		transform.Translate(movement);
	}
}
