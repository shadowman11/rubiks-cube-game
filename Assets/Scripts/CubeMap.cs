using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class CubeMap : MonoBehaviour
{
    CubeState cubeState;

    public Transform up;
    public Transform down;
    public Transform left;
    public Transform right;
    public Transform front;
    public Transform back;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    List<GameObject> FlipFace(List<GameObject> face, bool flipHorizontally)
    {
        List<GameObject> flipped = new List<GameObject>(9);

        if (flipHorizontally)
        {
            // Horizontal flip: reverse each row
            for (int row = 0; row < 3; row++)
            {
                flipped.Add(face[row * 3 + 2]);
                flipped.Add(face[row * 3 + 1]);
                flipped.Add(face[row * 3 + 0]);
            }
        }
        else
        {
            // Vertical flip: reverse row order
            for (int row = 2; row >= 0; row--)
            {
                flipped.Add(face[row * 3 + 0]);
                flipped.Add(face[row * 3 + 1]);
                flipped.Add(face[row * 3 + 2]);
            }
        }

        return flipped;
    }

    public void Set()
    {
        cubeState = FindAnyObjectByType<CubeState>();

        // Horizontal flip (left/right mirrored)
        UpdateMap(FlipFace(cubeState.front, false), front);
        UpdateMap(FlipFace(cubeState.up, false), up);
        UpdateMap(FlipFace(cubeState.down, false), down);

        // Vertical flip (top/bottom mirrored)
        UpdateMap(FlipFace(cubeState.left, true), left);
        UpdateMap(FlipFace(cubeState.right, true), right);
        UpdateMap(FlipFace(cubeState.back, true), back);
    }

    void UpdateMap(List<GameObject> face, Transform side)
    {
        int i = 0;
        foreach (Transform map in side)
        {
            if (face[i].name[0] == 'F')
            {
                map.GetComponent<Image>().color = Color.blue;
            }
            if (face[i].name[0] == 'B')
            {
                map.GetComponent<Image>().color = Color.green;
            }
            if (face[i].name[0] == 'L')
            {
                map.GetComponent<Image>().color = Color.white;
            }
            if (face[i].name[0] == 'R')
            {
                map.GetComponent<Image>().color = Color.yellow;
            }
            if (face[i].name[0] == 'U')
            {
                map.GetComponent<Image>().color = new Color(1, 0.5f, 0, 1);
            }
            if (face[i].name[0] == 'D')
            {
                map.GetComponent<Image>().color = Color.red;
            }
            i++;
        }
    }
}
