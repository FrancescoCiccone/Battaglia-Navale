using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MovementsCamera : MonoBehaviour
{
    public int bol;
    //public Transform target;
    public int grades;
    //public int i=-50;
    // Start is called before the first frame update
    public float speed;

    bool up,rotating, camerainput = false;

    [SerializeField] private Vector3 _rotation;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Rotating" + rotating);
        if (Input.GetKeyDown(KeyCode.UpArrow)|| Input.GetKeyDown(KeyCode.DownArrow))
        {

            rotating = true;
            camerainput = true;
         }

        if (camerainput == true)
        {
            CameraRotation();
        }  
    }
    private void CameraRotation()
    {
        if (up)
        {
            if (rotating == true)
            {
                transform.Rotate(_rotation * Time.deltaTime);
            }
            //rotating = true;
            if (transform.rotation.eulerAngles.x >72)
            {
                up = false;
                rotating = false;
                camerainput = false;
            }
        } else
        {
            if (rotating == true)
            {
                transform.Rotate(-_rotation * Time.deltaTime);
            }
            //rotating = true;
            if (transform.rotation.eulerAngles.x<15)
            {
                up = true;
                rotating = false;
                camerainput = false;
            }
        }
    }
}