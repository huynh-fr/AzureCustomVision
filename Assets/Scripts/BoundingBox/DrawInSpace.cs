using UnityEngine;

public class DrawInSpace : MonoBehaviour {

    public static DrawInSpace Instance;

    [SerializeField]
    /// <summary>
    /// Used as a list of materials that can be used on holograms
    /// IMPORTANT : needs to be set in the Unity Inspector
    /// </summary>
    private Material[] materials;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        //For testing
        //DrawStaticRectangle(new Vector3(0, 0, 0), 1, 0.2f);
        //DrawCube(new Vector3(0, 0, 0), 1, 0.5f, 0.1f);
    }

    /// <summary>
    /// Draw a 2D rectangle in 3D space 
    /// </summary>
    public GameObject DrawRectangle(Vector3 position, float width, float height)
    {
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.transform.position = position;

        //Set size of RectTransform
        RectTransform rectTrans = quad.AddComponent<RectTransform>();
        rectTrans.sizeDelta = new Vector2(width, height);

        //Change scale depending on RectTransform width and height
        Resize(quad);

        //Debug.Log($"Rect : Size={rectTrans.rect.width};{rectTrans.rect.height}, Scale={quad.transform.localScale}, Bounds={quad.GetComponent<Renderer>().bounds.size}");

        //Set the first material as default
        quad.GetComponent<Renderer>().material = materials[0];

        quad.layer = LayerMask.NameToLayer("Holograms");

        return quad;
    }

    /// <summary>
    /// Draw a cube in 3D space 
    /// </summary>
    public GameObject DrawCube(float width, float height, float depth = 0, Vector3 position = default(Vector3))
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        cube.transform.position = position;
        if (depth == 0)
            cube.transform.localScale = new Vector3(width, height, width);
        else
            cube.transform.localScale = new Vector3(width, height, depth);

        //Set the first material as default
        cube.GetComponent<Renderer>().material = materials[0];

        cube.layer = LayerMask.NameToLayer("Holograms");

        return cube;
    }

    public void ChooseMaterial(GameObject hologram, string materialName)
    {
        foreach (Material mat in materials)
        {
            if(mat.name == materialName)
            {
                // assign the material to the renderer
                hologram.GetComponent<Renderer>().material = mat;
            }
        }
    }

    private void Resize(GameObject obj)
    {
        //Initially a GameObject is 1x1 Unity units and Scale (1,1,1)
        float ExpectedWidth = obj.GetComponent<RectTransform>().rect.width;
        float ExpectedHeight = obj.GetComponent<RectTransform>().rect.height;

        if (ExpectedWidth != obj.transform.localScale.x || ExpectedHeight != obj.transform.localScale.y)
        {
            //Set the scale of the gameObject depending of the RectTransform values
            obj.transform.localScale = new Vector3(ExpectedWidth, ExpectedHeight, transform.localScale.z);
        }
    }
}
