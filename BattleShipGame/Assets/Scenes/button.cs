using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour
{
	int id;
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
	}

	private void OnCollisionEnter(Collision collision)
	{
	}

	public void Return()
	{
		Debug.Log("ｮｩｼﾞｮ" + id);
	}

	public int SetID
	{
		set{id = value;}
	}

	public Vector3 GetPos
	{
		get { return this.transform.position; }
	}
}
