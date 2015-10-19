using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour 
{
    public GameObject m_PlanePrefab;

    private GameObject leftBoundary_;
    private GameObject rightBoundary_;
    private GameObject backBoundary_;
    private GameObject frontBoundary_;

    private GameObject floor_;
    private Renderer floorRenderer_;

    private GameObject background_;

	void Start () 
    {
        //Caching the reference to the floor's Mesh Renderer for use in later calculations
        floorRenderer_ = GetComponent<MeshRenderer>();
        background_ = GameObject.Find("Background");

        if (background_ != null)
        {
            gameObject.transform.localScale = new Vector3(background_.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }
        else
        {
            Debug.LogErrorFormat("Background is null at line {0}", 26);
        }

        leftBoundary_ = (GameObject)Instantiate(m_PlanePrefab, new Vector3(floorRenderer_.bounds.min.x, 0.0f, 0.0f), Quaternion.identity);
        leftBoundary_.transform.Rotate(new Vector3(0.0f, 0.0f, 270.0f));
        leftBoundary_.transform.localScale = new Vector3(leftBoundary_.transform.localScale.x, leftBoundary_.transform.localScale.y, (floorRenderer_.bounds.size.z / 10) + 0.1f);
        leftBoundary_.gameObject.name = "LeftBoundary";

        rightBoundary_ = (GameObject)Instantiate(m_PlanePrefab, new Vector3(floorRenderer_.bounds.max.x, 0.0f, 0.0f), Quaternion.identity);
        rightBoundary_.transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f));
        rightBoundary_.transform.localScale = new Vector3(rightBoundary_.transform.localScale.x, rightBoundary_.transform.localScale.y, (floorRenderer_.bounds.size.z / 10) + 0.1f);
        rightBoundary_.gameObject.name = "RightBoundary";

        frontBoundary_ = (GameObject)Instantiate(m_PlanePrefab, new Vector3(0.0f, 0.0f, floorRenderer_.bounds.min.z), Quaternion.identity);
        frontBoundary_.transform.Rotate(0.0f, 90.0f, 90.0f);
        frontBoundary_.transform.localScale = new Vector3(frontBoundary_.transform.localScale.x, frontBoundary_.transform.localScale.y, (floorRenderer_.bounds.size.x / 10) + 0.1f);
        frontBoundary_.gameObject.name = "FrontBoundary";

        backBoundary_ = (GameObject)Instantiate(m_PlanePrefab, new Vector3(0.0f, 0.0f, floorRenderer_.bounds.max.z), Quaternion.identity);
        backBoundary_.transform.Rotate(0.0f, 90.0f, 270.0f);
        backBoundary_.transform.localScale = new Vector3(backBoundary_.transform.localScale.x, backBoundary_.transform.localScale.y, (floorRenderer_.bounds.size.x / 10) + 0.1f);
        backBoundary_.gameObject.name = "BackBoundary";

        leftBoundary_.GetComponent<MeshRenderer>().enabled = false;
        rightBoundary_.GetComponent<MeshRenderer>().enabled = false;
        frontBoundary_.GetComponent<MeshRenderer>().enabled = false;
        backBoundary_.GetComponent<MeshRenderer>().enabled = false;

        
	}
}