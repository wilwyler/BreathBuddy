using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;

public class ClickToMove : MonoBehaviour
{

	JellySprite m_JellySprite;
	public float m_MinJumpForce = 4000.0f;
	public float m_MaxJumpForce = 4000.0f;
	public Vector2 m_MinJumpVector = new Vector2(-0.1f, 1.0f);
	public Vector2 m_MaxJumpVector = new Vector2(0.1f, 1.0f);
	public LayerMask m_GroundLayer;
	// float speed = 5f;

	void Start () {
		m_JellySprite = GetComponent<JellySprite>();
	}

	void Update() {
		if(Input.GetMouseButtonDown(0)) {
			Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			UnityEngine.Debug.Log("entered");
			// GetComponent<JellySprite>().SetPosition(target, false);
		    Vector2 jumpVector = Vector2.zero;
			jumpVector.x = target.x;
			jumpVector.y = target.y;
			jumpVector.Normalize();
			m_JellySprite.AddForce(jumpVector * UnityEngine.Random.Range(m_MinJumpForce, m_MaxJumpForce));
			// GetComponent<JellySprite>().Rotate(180f);
		}	
	}



	// float angle = ((Mathf.PI * 2.0f)/32.0f) * loop;
	// float x = Mathf.Cos(angle) * referencePoint.Radius;
	// float y = Mathf.Sin(angle) * referencePoint.Radius;
}
