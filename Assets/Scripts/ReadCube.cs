using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReadCube : MonoBehaviour
{
    public Transform tUp;
    public Transform tDown;
    public Transform tLeft;
    public Transform tRight;
    public Transform tBack;
    public Transform tFront;

    private List<GameObject> frontRays = new List<GameObject>();
    private List<GameObject> backRays = new List<GameObject>();
    private List<GameObject> leftRays = new List<GameObject>();
    private List<GameObject> rightRays = new List<GameObject>();
    private List<GameObject> upRays = new List<GameObject>();
    private List<GameObject> downRays = new List<GameObject>();

    private int layerMask = 1 << 8; // layerMask of the Cube
    CubeState cubeState;
    CubeMap cubeMap;
    public GameObject emptyGO;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // get mode of game
        if (ModeIndicator.isTimed)
        {
            // scramble cube
            // start timer (or give player a few seconds to look at the cube before starting timer)
        } else
        {
            // don't scramble cube
            // no timer
        }

            SetRayTransforms();

        cubeState = FindAnyObjectByType<CubeState>();
        cubeMap = FindAnyObjectByType<CubeMap>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReadState()
    {
        if (PauseMenu.isPaused) return;
        cubeState = FindAnyObjectByType<CubeState>();
        cubeMap = FindAnyObjectByType<CubeMap>();

        cubeState.up = ReadFace(upRays, tUp);
        cubeState.down = ReadFace(downRays, tDown);
        cubeState.left = ReadFace(leftRays, tLeft);
        cubeState.right = ReadFace(rightRays, tRight);
        cubeState.front = ReadFace(frontRays, tFront);
        cubeState.back = ReadFace(backRays, tBack);

        cubeMap.Set();
    }

    public void SetRayTransforms()
    {
        upRays = BuildRays(tUp, new Vector3(90, 90, 0));
        downRays = BuildRays(tDown, new Vector3(270, 270, 0));
        leftRays = BuildRays(tLeft, new Vector3(0, 90, 90));
        rightRays = BuildRays(tRight, new Vector3(0, -90, 90));
        frontRays = BuildRays(tFront, new Vector3(0, 0, 0));
        backRays = BuildRays(tBack, new Vector3(0, 180, 0));
    }

    List<GameObject> BuildRays(Transform rayTransform, Vector3 direction)
    {
        int rayCount = 0;
        List<GameObject> rays = new List<GameObject>();

        for (int y = 1; y > -2; y--)
        {
            for (int x = -1; x < 2; x++)
            {
                Vector3 startPos = new Vector3(rayTransform.localPosition.x + x,
                                                rayTransform.localPosition.y + y,
                                                rayTransform.localPosition.z);
                GameObject rayStart = Instantiate(emptyGO, startPos, Quaternion.identity, rayTransform);
                rays.Add(rayStart);
                rayCount++;
            }
        }
        rayTransform.localRotation = Quaternion.Euler(direction);
        return rays;
    }

    public List<GameObject> ReadFace(List<GameObject> rayStarts, Transform rayTransform)
    {
        List<GameObject> facesHit = new List<GameObject>();

        foreach (GameObject rayStart in rayStarts)
        {
            Vector3 ray = rayStart.transform.position;
            RaycastHit hit;

            if (Physics.Raycast(ray, rayTransform.forward, out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(ray, rayTransform.forward * hit.distance, Color.yellow);
                facesHit.Add(hit.collider.gameObject);
                // print(hit.collider.gameObject.name);
            }
            else
            {
                Debug.DrawRay(ray, rayTransform.forward * 1000, Color.green);
            }
        }

        return facesHit;
    }
}
