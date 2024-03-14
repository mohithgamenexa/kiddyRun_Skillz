
using UnityEngine;

public class raysrotate : MonoBehaviour
{
    public bool xaxis,yaxis;
    public float speed;
    void Update()
    {
        if (xaxis)
        {
            transform.Rotate(Vector3.right * Time.deltaTime * speed);

        }
        else if(yaxis)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * speed);

        }
        else
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * speed);
        }
    }
}
