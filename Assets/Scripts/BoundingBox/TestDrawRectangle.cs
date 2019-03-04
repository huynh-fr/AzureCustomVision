using UnityEngine;

public class TestDrawRectangle : MonoBehaviour {
   

    private GameObject quad;

	// Use this for initialization
	void Start () {
        quad = DrawInSpace.Instance.DrawRectangle(transform.position, 0.2f, 0.3f);
        //DrawInSpace.Instance.ChooseMaterial(quad, "BoundingBoxTransparent");
        DrawInSpace.Instance.ChooseMaterial(quad, "BoundingBoxLines");

        Debug.Log(quad.GetComponent<Renderer>().bounds.size);
    }
	
	// Update is called once per frame
	void Update () {
        
    }
}
