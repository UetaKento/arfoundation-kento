using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePrefabManager : MonoBehaviour
{
    private bool isCollision = false;

    public bool IsCollision
    {
        get { return isCollision; }
        set { isCollision = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        isCollision = true;
    }
}
