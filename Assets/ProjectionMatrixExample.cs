using UnityEngine;

public class ProjectionMatrixExample : MonoBehaviour
{
    void Start()
    {
        Camera cam = GetComponent<Camera>();
        Matrix4x4 projectionMatrix = cam.projectionMatrix;

        // Puoi utilizzare la matrice di proiezione come desideri
        Debug.Log("Matrice di proiezione della camera:\n" + projectionMatrix);
    }
}
