using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TextController : MonoBehaviour
{
    public GameObject head;
    private float jawWeightFloat;
    private string jawWeightString;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<UnityEngine.UI.Text>().text = "aaaaa";
    }

    // Update is called once per frame
    void Update()
    {
        //jawWeightFloat = head.GetComponent<BlendShapeVisualizer>().GetJawWeight;
        jawWeightString = jawWeightFloat.ToString();
        this.gameObject.GetComponent<UnityEngine.UI.Text>().text = jawWeightString;
    }
}
