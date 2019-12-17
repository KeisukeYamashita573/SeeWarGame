using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class user : MonoBehaviour
{
    [SerializeField]
    string userName;
    [SerializeField]
    int userID;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetUserName()
    {
        return userName;
    }

    public void SetUserName(string name)
    {
        userName = name;
    }

    public int GetUserID()
    {
        return userID;
    }

    public void SetUserID(int id)
    {
        userID = id;
    }
}
