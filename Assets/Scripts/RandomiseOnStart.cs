
using UnityEngine;

public class RandomiseOnStart : MonoBehaviour
{
    void Start()
    {
        foreach(Transform t in transform)
        {
            t.gameObject.SetActive(false);
        }
        transform.GetChild(Random.Range(0, transform.childCount)).gameObject.SetActive(true);
    }

}
