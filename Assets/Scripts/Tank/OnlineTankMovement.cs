﻿using UnityEngine;
using UnityEngine.Networking;

//----------------------------------------
//This is just normal TankMovement with if(isLocalPlayer) added
//----------------------------------------

public class OnlineTankMovement : NetworkBehaviour
{
	public int m_PlayerNumber = 1;              // Used to identify which tank belongs to which player.  This is set by this tank's manager.
	//public float m_Speed = 12f;                 // How fast the tank moves forward and back.
	//public float m_TurnSpeed = 180f;            // How fast the tank turns in degrees per second.
	public AudioSource m_MovementAudio;         // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
	public AudioClip m_EngineIdling;            // Audio to play when the tank isn't moving.
	public AudioClip m_EngineDriving;           // Audio to play when the tank is moving.
	public float m_PitchRange = 0.2f;           // The amount by which the pitch of the engine noises can vary.


	//private string m_MovementAxisName;          // The name of the input axis for moving forward and back.
	//private string m_TurnAxisName;              // The name of the input axis for turning.
	private Rigidbody m_Rigidbody;              // Reference used to move the tank.
	//private float m_MovementInputValue;         // The current value of the movement input.
	//private float m_TurnInputValue;             // The current value of the turn input.
	private float m_OriginalPitch;              // The pitch of the audio source at the start of the scene.


	private void Awake()
	{
		m_Rigidbody = GetComponent<Rigidbody>();
	}


	private void OnEnable()
	{
		// When the tank is turned on, make sure it's not kinematic.
		m_Rigidbody.isKinematic = false;

		// Also reset the input values.
		//m_MovementInputValue = 0f;
		//m_TurnInputValue = 0f;
	}


	private void OnDisable()
	{
		// When the tank is turned off, set it to kinematic so it stops moving.
		m_Rigidbody.isKinematic = true;
	}


	private void Start()
	{
		// The axes names are based on player number.
		//m_MovementAxisName = "Vertical" + m_PlayerNumber;
		//m_TurnAxisName = "Horizontal" + m_PlayerNumber;

		// Store the original pitch of the audio source.
		m_OriginalPitch = m_MovementAudio.pitch;
	}

	public override void OnStartLocalPlayer()
	{
		this.gameObject.transform.GetChild(0).gameObject.transform.GetChild(3).GetComponent<MeshRenderer>().material.color = Color.blue;
	
	}


	private void Update()
	{
		if (isLocalPlayer) {
			EngineAudio ();
		}
	}


	private void EngineAudio()
	{
		// If there is no input (the tank is stationary)...
		if (Mathf.Abs(MoveSpeed) < 0.3f && Mathf.Abs(TurnSpeed) < 0.3f)
		{
			// ... and if the audio source is currently playing the driving clip...
			if (m_MovementAudio.clip == m_EngineDriving)
			{
				// ... change the clip to idling and play it.
				m_MovementAudio.clip = m_EngineIdling;
				m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
				m_MovementAudio.Play();
			}
		}
		else
		{
			// Otherwise if the tank is moving and if the idling clip is currently playing...
			if (m_MovementAudio.clip == m_EngineIdling)
			{
				// ... change the clip to driving and play.
				m_MovementAudio.clip = m_EngineDriving;
				m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
				m_MovementAudio.Play();
			}
		}
	}


	private void FixedUpdate()
	{	
		if (isLocalPlayer) {
			// Adjust the rigidbodies position and orientation in FixedUpdate.
			Move ();
			Turn ();
		}
	}


	private float MoveSpeed = 0;
	private float MoveSpeedScale = 12f;
	private void Move()
	{
		Vector3 camUp = GvrViewer.Instance.HeadPose.Orientation * Vector3.up;
		MoveSpeed = camUp.z * MoveSpeedScale;

		Vector3 movement = transform.forward * MoveSpeed * Time.deltaTime;

		m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
	}


	private float TurnSpeed = 0;
	private float TurnSpeedScale = 180f;
	private void Turn()
	{
		Vector3 camUp = GvrViewer.Instance.HeadPose.Orientation * Vector3.up;
		TurnSpeed = camUp.x * TurnSpeedScale;

		Quaternion turnRotation = Quaternion.Euler(0f, TurnSpeed * Time.deltaTime, 0f);

		m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
	}
}
