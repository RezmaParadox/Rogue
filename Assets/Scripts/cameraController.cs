using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public static cameraController instance;

    public Room currentRoom;

    public float moveSpeedWhenRoomChange;

    void Awake()
    {
        instance = this;   
    }
 
    void Update()
    {
        UpdatePosition();
    }
    void UpdatePosition()
    {
        if (currentRoom == null)
        {
            return;
        }
        Vector3 targetPos = GetcameraTargedPosition();

        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime*moveSpeedWhenRoomChange);
    }
    Vector3 GetcameraTargedPosition()
    {
        if (currentRoom == null)
        {
            return Vector3.zero;
        }
        Vector3 targetPos = currentRoom.GetRoomCentre();
        targetPos.z=transform.position.z;

        return targetPos;
    }
    public bool IsSwitchingScene()
    {
        return transform.position.Equals(GetcameraTargedPosition()) == false;

    }   
    
}
