using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// RubiksCubeController: handles face rotations triggered by UI buttons.
/// Design Decisions:
/// 1. Each face's 9 cubies are grouped under a pivot Transform for rotation.
/// 2. FaceName enum defines face identifiers.
/// 3. facePivots and faceAxes dictionaries map face names to pivots and rotation axes.
/// 4. Rotation performed via coroutine for smooth animation and to prevent overlapping rotations.
/// 5. Public methods exposed for UI Buttons to call for each face clockwise or counter-clockwise rotation.
/// 6. rotationSpeed adjustable for animation timing.
/// 7. Locking boolean prevents multiple simultaneous rotations.
/// </summary>
public class RubiksCube : MonoBehaviour {
    [Tooltip("Degrees per second for face rotation animation.")]
    public float rotationSpeed = 200f;

    [Header("Face Pivots")]
    public Transform upFacePivot;
    public Transform downFacePivot;
    public Transform leftFacePivot;
    public Transform rightFacePivot;
    public Transform frontFacePivot;
    public Transform backFacePivot;

    private enum FaceName { Up, Down, Left, Right, Front, Back }

    private Dictionary<FaceName, Transform> facePivots;
    private Dictionary<FaceName, Vector3> faceAxes;
    private bool isRotating = false;

    void Awake() {
        facePivots = new Dictionary<FaceName, Transform> {
            { FaceName.Up, upFacePivot },
            { FaceName.Down, downFacePivot },
            { FaceName.Left, leftFacePivot },
            { FaceName.Right, rightFacePivot },
            { FaceName.Front, frontFacePivot },
            { FaceName.Back, backFacePivot }
        };

        faceAxes = new Dictionary<FaceName, Vector3> {
            { FaceName.Up, Vector3.up },
            { FaceName.Down, Vector3.up },
            { FaceName.Left, Vector3.right },
            { FaceName.Right, Vector3.right },
            { FaceName.Front, Vector3.forward },
            { FaceName.Back, Vector3.forward }
        };
    }

    // Public methods for UI Buttons
    public void RotateUpCW()     { TryRotateFace(FaceName.Up,  90f); }
    public void RotateUpCCW()    { TryRotateFace(FaceName.Up, -90f); }
    public void RotateDownCW()   { TryRotateFace(FaceName.Down, -90f); }
    public void RotateDownCCW()  { TryRotateFace(FaceName.Down,  90f); }
    public void RotateLeftCW()   { TryRotateFace(FaceName.Left,  90f); }
    public void RotateLeftCCW()  { TryRotateFace(FaceName.Left, -90f); }
    public void RotateRightCW()  { TryRotateFace(FaceName.Right, -90f); }
    public void RotateRightCCW() { TryRotateFace(FaceName.Right,  90f); }
    public void RotateFrontCW()  { TryRotateFace(FaceName.Front,  90f); }
    public void RotateFrontCCW() { TryRotateFace(FaceName.Front, -90f); }
    public void RotateBackCW()   { TryRotateFace(FaceName.Back,  -90f); }
    public void RotateBackCCW()  { TryRotateFace(FaceName.Back,   90f); }

    private void TryRotateFace(FaceName face, float angle) {
        if (!isRotating) StartCoroutine(RotateFaceCoroutine(face, angle));
    }

    private IEnumerator RotateFaceCoroutine(FaceName face, float angle) {
        isRotating = true;
        Transform pivot = facePivots[face];
        Vector3 axis = faceAxes[face];
        float rotated = 0f;
        while (Mathf.Abs(rotated) < Mathf.Abs(angle)) {
            float step = rotationSpeed * Time.deltaTime * Mathf.Sign(angle);
            if (Mathf.Abs(rotated + step) > Mathf.Abs(angle)) {
                step = angle - rotated;
            }
            pivot.Rotate(axis, step, Space.Self);
            rotated += step;
            yield return null;
        }
        isRotating = false;
    }
}
