using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class RoomInfo
{
    public string name;
    public int X;
    public int Y;
}

public class RoomController : MonoBehaviour
{
    public static RoomController instance;

    string currentWoldName = "Basement";

    RoomInfo currentLoadRoomData;

    Room currentRoom;

    Queue<RoomInfo> loadRoomQueue= new Queue<RoomInfo>();

    public List<Room> loadedRooms= new List<Room>();

    bool isLoadingRoom = false;

    bool spawnedBossRoom=false;

    bool spawnedShopRoom = false;

    bool updatedRooms=false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //LoadRoom("Start", 0, 0);
        //LoadRoom("Empty", 1, 0);
        //LoadRoom("Empty", -1, 0);
        //LoadRoom("Empty", 0, 1);
        //LoadRoom("Empty", 0, -1);
    }

    void Update()
    {
        UpdateRoomQueue();
    }

    void UpdateRoomQueue()
    {
        if (isLoadingRoom)
        {
            return;
        }
        if(loadRoomQueue.Count == 0)
        {
            if (!spawnedBossRoom && !spawnedShopRoom)
            {
                StartCoroutine(SpawnBossRoom());
               // StartCoroutine(SpawnShopRoom());

            }
            else if(spawnedBossRoom && spawnedShopRoom && !updatedRooms)
            {
                foreach(Room room in loadedRooms)
                {
                        room.RemoveUnconnectedDoors();
                }
                updatedRooms = true;
            }

            /* else if (!spawnedShopRoom)
            {
                StartCoroutine(SpawnShopRoom());
            }
            else if (spawnedShopRoom && !updatedRooms && spawnedBossRoom)
            {
                foreach (Room room in loadedRooms)
                {
                    room.RemoveUnconnectedDoors();
                }
                updatedRooms = true;
            }*/
            return;
        }
        currentLoadRoomData= loadRoomQueue.Dequeue();
        isLoadingRoom = true;
        StartCoroutine(loadRoomRoutine(currentLoadRoomData));
    }

    IEnumerator SpawnBossRoom()
    {
        spawnedBossRoom = true;
        yield return new WaitForSeconds(0.5f);
        if (loadRoomQueue.Count == 0)
        {
            Room bossRoom = loadedRooms[loadedRooms.Count-1];
            Vector2Int tempRoom= new Vector2Int(bossRoom.X, bossRoom.Y);
            Destroy(bossRoom.gameObject);
            var roomToRemoveB=loadedRooms.Single(rb=>rb.X== tempRoom.x && rb.Y==tempRoom.y);
            loadedRooms.Remove(roomToRemoveB);
            LoadRoom("End",tempRoom.x,tempRoom.y);
        }
        spawnedShopRoom = true;
        yield return new WaitForSeconds(0.5f);
        if (loadRoomQueue.Count == 0)
        {
            Room shopRoom = loadedRooms[loadedRooms.Count - 3];
            Vector2Int tempRoom2 = new Vector2Int(shopRoom.X, shopRoom.Y);
            Destroy(shopRoom.gameObject);
            var roomToRemoveS = loadedRooms.Single(rs => rs.X == tempRoom2.x && rs.Y == tempRoom2.y);
            loadedRooms.Remove(roomToRemoveS);
            LoadRoom("Shop", tempRoom2.x, tempRoom2.y);
        }

    }
    /*
    IEnumerator SpawnShopRoom()
    {
        spawnedShopRoom = true;
        yield return new WaitForSeconds(0.5f);
        if (loadRoomQueue.Count == 0)
        {
            Room shopRoom = loadedRooms[loadedRooms.Count - 2];
            Vector2Int tempRoom2 = new Vector2Int(shopRoom.X, shopRoom.Y);
            Destroy(shopRoom.gameObject);
            var roomToRemoveS = loadedRooms.Single(rs => rs.X == tempRoom2.x && rs.Y == tempRoom2.y);
            loadedRooms.Remove(roomToRemoveS);
            LoadRoom("Shop", tempRoom2.x, tempRoom2.y);
        }
    }*/
    public void LoadRoom(string name, int x, int y)
    {
        if(DoesRoomsExist(x, y))
        {
            return;
        }

        RoomInfo NewRoomData = new RoomInfo();
        NewRoomData.name = name;
        NewRoomData.X = x;
        NewRoomData.Y = y;

        loadRoomQueue.Enqueue(NewRoomData);
    }

    IEnumerator loadRoomRoutine(RoomInfo info)
    {
        string roomName= currentWoldName + info.name;
         AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

        while(loadRoom.isDone== false)
        {
            yield return null;
        }
    }

    public void RegisterRoom(Room room)
    {
        if (!DoesRoomsExist(currentLoadRoomData.X, currentLoadRoomData.Y))
        {
            room.transform.position = new Vector3
            (
                currentLoadRoomData.X * room.Width,
                currentLoadRoomData.Y * room.Height,
                0
            );
            room.X = currentLoadRoomData.X;
            room.Y = currentLoadRoomData.Y;
            room.name = currentWoldName + "-" + currentLoadRoomData.name + " " + room.X + "," + room.Y;
            room.transform.parent = transform;

            isLoadingRoom = false;

            if (loadedRooms.Count == 0)
            {
                cameraController.instance.currentRoom = room;
            }

            loadedRooms.Add(room);
        }
        else
        {
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }
    }

    public bool DoesRoomsExist(int x, int y)
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y) != null;
    }
    public Room FindRoom(int x, int y)
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y);
    }

    public void OnPlayerEnterRoom(Room room)
    {
        cameraController.instance.currentRoom= room;
        currentRoom = room;
    }
}
