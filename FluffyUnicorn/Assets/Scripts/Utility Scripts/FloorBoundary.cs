using UnityEngine;
using System.Collections;

public class FloorBoundary : MonoBehaviour 
{
    public GameObject m_PlanePrefab;

    private GameObject leftBoundary_;
    private GameObject rightBoundary_;
    private GameObject backBoundary_;
    private GameObject frontBoundary_;

    private Renderer floorRenderer_;

	void Start () 
    {
        //Caching the reference to the floor's Mesh Renderer for use in later calculations
        floorRenderer_ = GetComponent<MeshRenderer>();
        Debug.Log("floorRenderer X size " + floorRenderer_.bounds.size.x + "    Y Size " + floorRenderer_.bounds.size.y + "    Z Size " + floorRenderer_.bounds.size.z);

        leftBoundary_ = (GameObject)Instantiate(m_PlanePrefab, new Vector3(floorRenderer_.bounds.min.x, 0.0f, 0.0f), Quaternion.identity);
        leftBoundary_.transform.Rotate(new Vector3(0.0f, 0.0f, 270.0f));
        leftBoundary_.gameObject.name = "LeftBoundary";

        rightBoundary_ = (GameObject)Instantiate(m_PlanePrefab, new Vector3(floorRenderer_.bounds.max.x, 0.0f, 0.0f), Quaternion.identity);
        rightBoundary_.transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f));
        rightBoundary_.gameObject.name = "RightBoundary";

        frontBoundary_ = (GameObject)Instantiate(m_PlanePrefab, new Vector3(0.0f, 0.0f, floorRenderer_.bounds.min.z), Quaternion.identity);
        frontBoundary_.transform.Rotate(0.0f, 90.0f, 90.0f);
        
        frontBoundary_.gameObject.name = "FrontBoundary";

        backBoundary_ = (GameObject)Instantiate(m_PlanePrefab, new Vector3(0.0f, 0.0f, floorRenderer_.bounds.max.z), Quaternion.identity);
        backBoundary_.transform.Rotate(0.0f, 90.0f, 270.0f);
        backBoundary_.transform.localScale = new Vector3(backBoundary_.transform.localScale.x, backBoundary_.transform.localScale.y, floorRenderer_.bounds.max.y);
        backBoundary_.gameObject.name = "BackBoundary";

        /*leftBoundary_.GetComponent<MeshRenderer>().enabled = false;
        rightBoundary_.GetComponent<MeshRenderer>().enabled = false;
        frontBoundary_.GetComponent<MeshRenderer>().enabled = false;
        backBoundary_.GetComponent<MeshRenderer>().enabled = false;*/


	}
	
	void Update () 
    {

	}
}