using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class koma : MonoBehaviour
{
	[SerializeField]
	int fieldID;	// 自身のいる場所

    // Start is called before the first frame update
    void Start()
    {
		fieldID = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	// フィールドの位置へ移動する
    public int SetFieldID
    {
        set { fieldID = value; }
    }
	// フィールドの位置へ移動する
	public Vector3 SetPos
	{
		set { this.transform.position = value; }
	}

	public void Return()
	{
		Debug.Log("天ちゃんprpr");
        Debug.Log("どこにいる:" + fieldID);
    }
}
