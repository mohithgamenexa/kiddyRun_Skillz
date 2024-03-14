
using UnityEngine;

public class WayPointMover : MonoBehaviour
{
    public Transform[] WayPoints;
    int _index = 0;
    Transform _player;
    private void Start()
    {
        _player = TrackManager._instance.playerScript.transform;
    }
    private void Update()
    {
        if(Vector3.Distance(transform.position,_player.position) < 70)
        {
            Transform tartget = WayPoints[_index];
            if (Vector3.Distance(transform.position, tartget.position) < 0.1f)
            {
                _index += 1;
                tartget = WayPoints[_index];
            }
           
        }
    }
}
