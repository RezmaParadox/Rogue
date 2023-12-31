﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int Width;
    public int Height;
    public int X;
    public int Y;

    public Door leftDoor;
    public Door rightDoor;
    public Door topDoor;
    public Door bottomDoor;

    public List<Door> doors=new List<Door>();

    private bool updateDoors=false;
    public Room(int x, int y)
    {
        X = x;
        Y = y;
    }
        
    // Start is called before the first frame update
    void Start()
    {
        if (RoomController.instance == null)
        {
            Debug.Log("You pressed play in the wrong scene!");
            return;
        }

        Door[] ds= GetComponentsInChildren<Door>();
        foreach(Door d in ds)
        {
            doors.Add(d);
            switch (d.doortype)
            {
                case Door.DoorType.right:
                    rightDoor = d;
                break;
                case Door.DoorType.left:
                    leftDoor = d;
                break;
                case Door.DoorType.top:
                    topDoor = d;
                 break;
                case Door.DoorType.bottom:
                    bottomDoor = d;
                break ;

            }
        }
        RoomController.instance.RegisterRoom(this);
    }

    private void Update()
    {
        if(name.Contains("End") && !updateDoors)
        {
            RemoveUnconnectedDoors();
            updateDoors=true;   
        }
        else if (name.Contains("Shop") && !updateDoors)
        {
            RemoveUnconnectedDoors();
            updateDoors = true;
        }
    }

    public void RemoveUnconnectedDoors()
    {
        foreach (Door door in doors)
        {
            switch (door.doortype)
            {
                case Door.DoorType.right:
                    if(GetRight()==null)
                        door.gameObject.SetActive(false);
                    break;
                case Door.DoorType.left:
                    if (GetLeft() == null)
                        door.gameObject.SetActive(false);
                    break;
                case Door.DoorType.top:
                    if (GetTop() == null)
                        door.gameObject.SetActive(false);
                    break;
                case Door.DoorType.bottom:
                    if (GetBotton() == null)
                        door.gameObject.SetActive(false);
                    break;
            }
        }
    }

    public Room GetRight()
    {
        if (RoomController.instance.DoesRoomsExist(X + 1, Y))
        {
            return RoomController.instance.FindRoom(X + 1, Y);
        }
        return null;
    }

    public Room GetLeft()
    {
        if (RoomController.instance.DoesRoomsExist(X -1 , Y))
        {
            return RoomController.instance.FindRoom(X - 1, Y);
        }
        return null;
    }

    public Room GetTop()
    {
        if (RoomController.instance.DoesRoomsExist(X , Y+1))
        {
            return RoomController.instance.FindRoom(X , Y+1);
        }
        return null;
    }

    public Room GetBotton()
    {
        if (RoomController.instance.DoesRoomsExist(X , Y-1))
        {
            return RoomController.instance.FindRoom(X , Y-1);
        }
        return null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(Width, Height, 0));
    }
    public Vector3 GetRoomCentre()
    {
        return new Vector3(X*Width, Y*Height);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            RoomController.instance.OnPlayerEnterRoom(this);
        }
    }
}
