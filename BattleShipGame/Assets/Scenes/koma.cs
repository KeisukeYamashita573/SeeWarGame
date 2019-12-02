using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class koma : MonoBehaviour
{
	int fieldID;

    // Start is called before the first frame update
    void Start()
    {
		fieldID = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int SetID
    {
        set { fieldID = value; }
    }
	public Vector3 SetPos
	{
		set { this.transform.position = value; }
	}

	public void Return()
	{
		Debug.Log("天ちゃんprpr");
        Debug.Log("komaID:" + fieldID);
    }
}
