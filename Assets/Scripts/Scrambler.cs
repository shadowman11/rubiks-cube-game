using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RubiksCubeScrambler: applies a series of valid random face turns to shuffle the cube in a solvable state.
public class Scrambler : MonoBehaviour {
    [Tooltip("Degrees per second for face rotation animation.")]
    public float rotationSpeed = 200f;  // Speed of individual face rotations

    [Header("Face Pivots")]
    public Transform upFacePivot;       // Transform pivot for the Up face
    public Transform downFacePivot;     // Transform pivot for the Down face
    public Transform leftFacePivot;     // Transform pivot for the Left face
    public Transform rightFacePivot;    // Transform pivot for the Right face
    public Transform frontFacePivot;    // Transform pivot for the Front face
    public Transform backFacePivot;     // Transform pivot for the Back face

    [Header("Scramble Settings")]
    [Tooltip("Number of random moves for scrambling.")]
    public int scrambleMoves = 20;      // How many turns in the scramble sequence
    [Tooltip("Delay between scramble moves in seconds.")]
    public float scrambleDelay = 0.1f;  // Pause between turns for clarity

    // Enum listing each face to rotate
    private enum FaceName { Up, Down, Left, Right, Front, Back }

    // Mappings from face names to their pivot transforms and axes
    private Dictionary<FaceName, Transform> facePivots;
    private Dictionary<FaceName, Vector3> faceAxes;
    private bool isRotating = false;    // Prevent overlapping rotations

    void Awake() {
        // Initialize pivot lookup for rotations
        facePivots = new Dictionary<FaceName, Transform> {
            { FaceName.Up, upFacePivot },
            { FaceName.Down, downFacePivot },
            { FaceName.Left, leftFacePivot },
            { FaceName.Right, rightFacePivot },
            { FaceName.Front, frontFacePivot },
            { FaceName.Back, backFacePivot }
        };

        // Define local axis for each face rotation
        faceAxes = new Dictionary<FaceName, Vector3> {
            { FaceName.Up, Vector3.up },
            { FaceName.Down, Vector3.up },
            { FaceName.Left, Vector3.right },
            { FaceName.Right, Vector3.right },
            { FaceName.Front, Vector3.forward },
            { FaceName.Back, Vector3.forward }
        };
    }

    // Public entry point: starts the scramble process.
    public void Scramble() {
        if (!isRotating) {
            // Begin coroutine that runs multiple random face turns
            StartCoroutine(ScrambleCoroutine(scrambleMoves));
        }
    }

    // Coroutine: performs a sequence of random, valid 90° turns.
    private IEnumerator ScrambleCoroutine(int moves) {
        isRotating = true;  // Lock out other commands during scramble

        // Get array of all faces to choose from
        FaceName[] faces = (FaceName[]) System.Enum.GetValues(typeof(FaceName));

        for (int i = 0; i < moves; i++) {
            // Pick a random face
            FaceName face = faces[Random.Range(0, faces.Length)];
            // Choose clockwise or counter-clockwise 90°
            float angle = Random.value > 0.5f ? 90f : -90f;

            // Execute the single turn and wait for completion
            yield return StartCoroutine(RotateFaceCoroutine(face, angle));

            // Optional delay between moves for visual clarity
            if (scrambleDelay > 0f) {
                yield return new WaitForSeconds(scrambleDelay);
            }
        }

        isRotating = false; // Unlock when done
    }

    // Coroutine: rotates a given face by the specified angle smoothly.
    private IEnumerator RotateFaceCoroutine(FaceName face, float angle) {
        Transform pivot = facePivots[face];          // Find pivot for this face
        Vector3 axis = faceAxes[face];               // Determine local rotation axis
        float rotated = 0f;                          // Track accumulated rotation

        // Continue rotating in small steps until target reached
        while (Mathf.Abs(rotated) < Mathf.Abs(angle)) {
            // Calculate step for this frame
            float step = rotationSpeed * Time.deltaTime * Mathf.Sign(angle);
            // Clamp final step to not overshoot
            if (Mathf.Abs(rotated + step) > Mathf.Abs(angle)) {
                step = angle - rotated;
            }

            // Apply rotation around pivot's local axis
            pivot.Rotate(axis, step, Space.Self);
            rotated += step;
            yield return null;  // Wait next frame
        }
    }
}
