using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public static DungeonGenerator s;

    #region Attributes
    private int maxRooms;
    private int nCurrentRooms;
    private int nDeadEnds = 3;

    private Queue<DungeonRoom> _pendingRooms;
    private List<DungeonRoom> _dungeonRooms;
    private List<GameObject> _dungeonRoomInstances;
    private List<GameObject> _propInstances;

    private List<GameObject> roomPrefabs;
    public List<Enemy> enemyPrefabs;
    private List<Enemy> _enemyInstances;
    #endregion
    
    private enum ROOM_DIRECTIONS { UP = 0, RIGHT, DOWN, LEFT }
    private class DungeonRoom
    {
        public int xPosition;
        public int zPosition;
        public int NeighboursCount { get { return _neighbours.Count; } }

        private List<Tuple<ROOM_DIRECTIONS, DungeonRoom>> _neighbours;
        public List<Tuple<ROOM_DIRECTIONS, DungeonRoom>> Neighbours { get { return _neighbours;  } }

        public RoomTypes type = RoomTypes.INVALID;

        public DungeonRoom(int x, int z)
        {
            xPosition = x;
            zPosition = z;
            _neighbours = new List<Tuple<ROOM_DIRECTIONS, DungeonRoom>>();
        }

        public bool HasNeighbourInDirection(ROOM_DIRECTIONS direction)
        {
            foreach (Tuple<ROOM_DIRECTIONS, DungeonRoom> n in _neighbours)
            {
                if (n.Item1 == direction)
                    return true;
            }
            return false;
        }

        public void AddNeighbourInDirection(DungeonRoom room, ROOM_DIRECTIONS direction)
        {
            _neighbours.Add(new Tuple<ROOM_DIRECTIONS, DungeonRoom>(direction, room));
        }
    }

    private void Awake()
    {
        s = this;

        LoadRoomPrefabs();
    }

    private void LoadRoomPrefabs()
    {
        string roomsPath = "Prefabs/Rooms/";
        string[] roomPrefabNames = { "Room_Door_1", "Room_Door_2_Close", "Room_Door_2_Opposite", "Room_Door_3", "Room_Door_4" };
        roomPrefabs = new List<GameObject>();

        var sb = new System.Text.StringBuilder();
        for (int i = 0; i < roomPrefabNames.Length; ++i)
        {
            sb.Append(roomsPath).Append(roomPrefabNames[i]);
            GameObject room = Resources.Load<GameObject>(sb.ToString());
            if (!ReferenceEquals(room, null))
                roomPrefabs.Add(room);
            else
                Debug.LogError("Room prefab " + sb.ToString() + " could not be found in " + roomsPath);
            sb.Clear();
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }


    #region Dungeon Generation
    public void GenerateDungeon()
    {
        GenerateDungeonLayout();
        GenerateSpecialRooms();

        InstantiateDungeon();

        SpawnEnemies();
        SpawnSpecialRooms();
    }

    private void GenerateDungeonLayout()
    {
        _dungeonRooms = new List<DungeonRoom>();
        maxRooms = GetDungeonMaxRoomCount();
        nCurrentRooms = 0;
        _pendingRooms = new Queue<DungeonRoom>();
        DungeonRoom startRoom = new DungeonRoom(0, 0);
        _pendingRooms.Enqueue(startRoom);
        _dungeonRooms.Add(startRoom);

        while (_pendingRooms.Count > 0)
        {
            nCurrentRooms++;
            DungeonRoom currentRoom = _pendingRooms.Dequeue();
            
            int nNeighbours = (nCurrentRooms + _pendingRooms.Count < maxRooms) ? UnityEngine.Random.Range(1, 4) : 0;
            for (int i = 0; i < nNeighbours; ++i)
            {
                if(currentRoom.NeighboursCount < 4) { 
                    ROOM_DIRECTIONS newNeighbourDirection = GetRandomNeighbourDirection(currentRoom);
                    (DungeonRoom, bool) newNeighbour = GenerateNeighbour(currentRoom, newNeighbourDirection);
                    DungeonRoom newNeighbourRoom = newNeighbour.Item1;
                    bool neighbourJustCreated = newNeighbour.Item2;
                    currentRoom.AddNeighbourInDirection(newNeighbourRoom, newNeighbourDirection);
                    if (neighbourJustCreated) 
                    {
                        _pendingRooms.Enqueue(newNeighbourRoom);
                        _dungeonRooms.Add(newNeighbourRoom);
                    }
                }
            }
        }

        Debug.Log(" === DUNGEON HAS BEEN GENERATED === ");
    }

    private bool IsThereRoomInPosition(int x, int z)
    {
        bool result = false;

        for (int i = 0; i < _dungeonRooms.Count; ++i)
        {
            if (_dungeonRooms[i].xPosition == x && _dungeonRooms[i].zPosition == z)
            {
                result = true;
                break;
            }
        }

        return result;
    }

    private DungeonRoom GetRoomInPosition(int x, int z)
    {
        for (int i = 0; i < _dungeonRooms.Count; ++i)
        {
            if (_dungeonRooms[i].xPosition == x && _dungeonRooms[i].zPosition == z)
            {
                return _dungeonRooms[i];
            }
        }
        return null;
    }

    private void InstantiateDungeon()
    {
        GameObject environmentParent = GameObject.Find("===== ENVIRONMENT =====");

        _dungeonRoomInstances = new List<GameObject>();
        foreach (DungeonRoom room in _dungeonRooms)
        {
            GameObject roomPrefab = null;
            Quaternion roomRotation = Quaternion.identity;
            switch (room.NeighboursCount)
            {
                case 1:
                    roomPrefab = roomPrefabs[0];
                    roomRotation = Get1DoorRoomRotation(room);
                    break;
                case 2:
                    roomPrefab = HasOppositeNeighbours(room) ? roomPrefabs[2] : roomPrefabs[1];
                    roomRotation = Get2DoorRoomRotation(room);
                    break;
                case 3:
                    roomPrefab = roomPrefabs[3];
                    roomRotation = Get3DoorRoomRotation(room);
                    break;
                case 4:
                    roomPrefab = roomPrefabs[4];
                    break;
                default: break;
            }
            GameObject roomInstance = Instantiate(roomPrefab, new Vector3(room.xPosition * 24, 0, room.zPosition * 24), roomRotation);
            if (!ReferenceEquals(environmentParent, null))
                roomInstance.transform.parent = environmentParent.transform;
            _dungeonRoomInstances.Add(roomInstance);
        }
    }

    private Quaternion Get1DoorRoomRotation(DungeonRoom room)
    {
        Quaternion result = Quaternion.identity;

        if (room.NeighboursCount == 1)
        {
            if (room.HasNeighbourInDirection(ROOM_DIRECTIONS.RIGHT))
            {
                result = Quaternion.Euler(0, 90, 0);
            }
            else if (room.HasNeighbourInDirection(ROOM_DIRECTIONS.DOWN))
            {
                result = Quaternion.Euler(0, 180, 0);
            }
            else if (room.HasNeighbourInDirection(ROOM_DIRECTIONS.LEFT))
            {
                result = Quaternion.Euler(0, 270, 0);
            }

        }
        return result;
    }

    private Quaternion Get2DoorRoomRotation(DungeonRoom room)
    {
        Quaternion result = Quaternion.identity;
        
        if (room.NeighboursCount == 2) 
        {
            if (HasOppositeNeighbours(room))
            {
                if (room.HasNeighbourInDirection(ROOM_DIRECTIONS.UP))
                    result = Quaternion.Euler(0, 90, 0);
            }
            else 
            {
                if (room.HasNeighbourInDirection(ROOM_DIRECTIONS.UP) && 
                    room.HasNeighbourInDirection(ROOM_DIRECTIONS.RIGHT))
                {
                    result = Quaternion.Euler(0, 90, 0);
                }
                else if (room.HasNeighbourInDirection(ROOM_DIRECTIONS.RIGHT) &&
                    room.HasNeighbourInDirection(ROOM_DIRECTIONS.DOWN))
                {
                    result = Quaternion.Euler(0, 180, 0);
                }
                else if (room.HasNeighbourInDirection(ROOM_DIRECTIONS.LEFT) &&
                    room.HasNeighbourInDirection(ROOM_DIRECTIONS.DOWN))
                {
                    result = Quaternion.Euler(0, 270, 0);
                }
            }
        }
        return result;
    }

    private Quaternion Get3DoorRoomRotation(DungeonRoom room)
    {
        Quaternion result = Quaternion.identity;

        if (room.NeighboursCount == 3)
        {
            if (room.HasNeighbourInDirection(ROOM_DIRECTIONS.RIGHT) &&
                !room.HasNeighbourInDirection(ROOM_DIRECTIONS.LEFT))
            {
                result = Quaternion.Euler(0, 90, 0);
            }
            else if (room.HasNeighbourInDirection(ROOM_DIRECTIONS.DOWN) &&
                !room.HasNeighbourInDirection(ROOM_DIRECTIONS.UP))
            {
                result = Quaternion.Euler(0, 180, 0);
            }
            else if (room.HasNeighbourInDirection(ROOM_DIRECTIONS.LEFT) &&
                !room.HasNeighbourInDirection(ROOM_DIRECTIONS.RIGHT))
            {
                result = Quaternion.Euler(0, 270, 0);
            }
            
        }
        return result;
    }

    private bool HasOppositeNeighbours(DungeonRoom room)
    {
        if (room.NeighboursCount == 2)
        {
            switch (room.Neighbours[0].Item1) 
            {
                case ROOM_DIRECTIONS.UP: return room.Neighbours[1].Item1 == ROOM_DIRECTIONS.DOWN;
                case ROOM_DIRECTIONS.RIGHT: return room.Neighbours[1].Item1 == ROOM_DIRECTIONS.LEFT;
                case ROOM_DIRECTIONS.DOWN: return room.Neighbours[1].Item1 == ROOM_DIRECTIONS.UP;
                case ROOM_DIRECTIONS.LEFT: return room.Neighbours[1].Item1 == ROOM_DIRECTIONS.RIGHT;
                default: return false;
            }
            
        }
        else return false;
    }

    private ROOM_DIRECTIONS GetRandomNeighbourDirection(DungeonRoom currentRoom)
    {
        bool found = false;
        ROOM_DIRECTIONS direction = ROOM_DIRECTIONS.UP;
        while (!found)
        {
            direction = GetRandomDirection();
            if (!currentRoom.HasNeighbourInDirection(direction))
                found = true;
        }
        return direction;
    }
    
    private ROOM_DIRECTIONS GetRandomDirection()
    {
        return (ROOM_DIRECTIONS)UnityEngine.Random.Range(0, 4);
    }
    
    private (DungeonRoom, bool) GenerateNeighbour(DungeonRoom currentRoom, ROOM_DIRECTIONS direction)
    {
        (DungeonRoom, bool) resultTuple;
        DungeonRoom result;
        bool roomCreated = false;
        (int, int)[] newRoomPositions = 
            {
                (currentRoom.xPosition, currentRoom.zPosition + 1),
                (currentRoom.xPosition + 1, currentRoom.zPosition),
                (currentRoom.xPosition, currentRoom.zPosition - 1),
                (currentRoom.xPosition - 1, currentRoom.zPosition)
            };

        (int, int) newPosition = newRoomPositions[(int)direction];
        if (IsThereRoomInPosition(newPosition.Item1, newPosition.Item2))
            result = GetRoomInPosition(newPosition.Item1, newPosition.Item2);
        else
        { 
            result = new DungeonRoom(newPosition.Item1, newPosition.Item2);
            roomCreated = true;
        }
        ROOM_DIRECTIONS oppositeDirection = (ROOM_DIRECTIONS)(((int)direction + 2) % 4);
        
        result.AddNeighbourInDirection(currentRoom, oppositeDirection);

        resultTuple.Item1 = result;
        resultTuple.Item2 = roomCreated;
        return resultTuple;
    }

    private int GetDungeonMaxRoomCount()
    {
        return Mathf.RoundToInt(3.33f + 1 + UnityEngine.Random.Range(5, 6));
    }

    private void GenerateSpecialRooms()
    {
        bool bossGenerated = false;
        _dungeonRooms[0].type = RoomTypes.START;

        for (int i = _dungeonRooms.Count -1; i >= 0 ; --i)
        {
            DungeonRoom room = _dungeonRooms[i];
            if (room.NeighboursCount == 1)
            {
                if (!bossGenerated)
                {
                    room.type = RoomTypes.BOSS;
                    bossGenerated = true;
                }
                else 
                {
                    RoomTypes roomType = GetRandomSpecialRoomType();
                    room.type = roomType;
                }
            }
        }
    }

    private RoomTypes GetRandomSpecialRoomType()
    {
        float rng = UnityEngine.Random.Range(0f, 1f);
        if (rng < 0.5f)
            return RoomTypes.TREASURE;
        else if (rng < 0.9f)
            return RoomTypes.ENEMIES;
        else
            return RoomTypes.EMPTY;
    }

    private void SpawnSpecialRooms()
    {
        _propInstances = new List<GameObject>();

        for (int i = 0; i < _dungeonRooms.Count; ++i)
        {
            DungeonRoom room = _dungeonRooms[i];
            if (room.type == RoomTypes.TREASURE)
            {
                _propInstances.Add(SpawnProp(PROPS_ID.TREASURECHEST, _dungeonRoomInstances[i].transform.position));
            }
            else if (room.type == RoomTypes.START)
            {
                _propInstances.Add(SpawnProp(PROPS_ID.BONFIRE, _dungeonRoomInstances[0].transform.position + Vector3.up*0.1f));
                if (_dungeonRooms[0].NeighboursCount != 2)
                    _propInstances.Add(SpawnProp(PROPS_ID.STARTROOMPROPS, _dungeonRoomInstances[0].transform.position));
            }
            else if (room.type == RoomTypes.BOSS)
            {
                // Spawn Boss Door
                Transform roomTransform = _dungeonRoomInstances[i].transform;
                Vector3 doorPosition = roomTransform.position + roomTransform.forward * 12;
                float doorRotationY = roomTransform.eulerAngles.y;
                
                GameObject doorGo = SpawnProp(PROPS_ID.BOSSDOOR, doorPosition, Quaternion.Euler(0, doorRotationY, 0));
                _propInstances.Add(doorGo);
                // Spawn Boss Enemy
                GameObject bossGo = SpawnEnemy(BOSS_ID.BOSS_BARBARIANGIANT, _dungeonRoomInstances[i].transform.position);
                bossGo.GetComponent<Enemy>()?.SetType(ENEMY_TYPE.BOSS);
                // Link door to enemy
                doorGo.GetComponent<BossDoor>().LinkToEnemy(bossGo.GetComponent<Enemy>());
            }
        }
    }

    #endregion

    #region Enemies
    private void SpawnEnemies()
    {
        _enemyInstances = new List<Enemy>();
        for (int i = 1; i < _dungeonRoomInstances.Count; ++i)
        {
            if (_dungeonRooms[i].NeighboursCount > 1) 
            {
                GameObject room = _dungeonRoomInstances[i];
                GameObject enemiesParentObject = new GameObject("Enemy Instances");
                enemiesParentObject.transform.parent = room.transform;
                enemiesParentObject.transform.localPosition = Vector3.zero;
                Transform enemySpawnsParent = room.transform.Find("EnemySpawnPoints");
                if (!ReferenceEquals(enemySpawnsParent, null))
                {
                    List<Transform> enemySpawns = new List<Transform>(enemySpawnsParent.GetComponentsInChildren<Transform>());
                    enemySpawns.RemoveAt(0);

                    foreach (Transform spawn in enemySpawns)
                    {
                        if (UnityEngine.Random.Range(0f, 1f) <= 0.75f) 
                        {
                            Enemy e = Instantiate(GetRandomEnemyPrefab(), spawn.position, Quaternion.identity, enemiesParentObject.transform);
                            e.SetType(ENEMY_TYPE.NORMAL);
                            _enemyInstances.Add(e);
                        }
                    }
                }
            }
        }
    }

    private Enemy GetRandomEnemyPrefab()
    {
        int enemyCount = enemyPrefabs.Count;
        return enemyPrefabs[UnityEngine.Random.Range(0, enemyCount)];
    }
    
    private GameObject SpawnEnemy(BOSS_ID bossId, Vector3 position)
    {
        // TODO: Spawn Enemies
        string bossPath = "Prefabs/Enemies/Bosses/";

        var sb = new System.Text.StringBuilder();
        sb.Append(bossPath).Append(bossId.ToString());

        GameObject boss = Resources.Load<GameObject>(sb.ToString());
        if (!ReferenceEquals(boss, null))
            return Instantiate(boss, position, Quaternion.identity);
        else
            Debug.LogError("Boss prefab " + sb.ToString() + " could not be found in " + bossPath);
        sb.Clear();
        return null;
    }

    public Enemy GetClosestEnemyToPlayer()
    {
        Vector3 playerPosition = ThirdPersonControllerMovement.s.transform.position;
        Enemy result = _enemyInstances[0];
        float minDistance = Vector3.Distance(playerPosition, result.transform.position);

        for (int i = 1; i < _enemyInstances.Count; ++i)
        {
            Enemy currentEnemy = _enemyInstances[i];
            float distance = Vector3.Distance(playerPosition, currentEnemy.transform.position);
            if (distance < minDistance) { 
                result = currentEnemy;
                minDistance = distance;
            }
        }

        return result;
    }
    #endregion

    #region Special Rooms

    private GameObject SpawnProp(PROPS_ID propId, Vector3 position)
    {
        return SpawnProp(propId, position, Quaternion.identity);
    }

    private GameObject SpawnProp(PROPS_ID propId, Vector3 position, Quaternion rotation)
    {
        string propsPath = "Prefabs/Props/";
        
        var sb = new System.Text.StringBuilder();
        sb.Append(propsPath).Append(propId.ToString());
        
        GameObject prop = Resources.Load<GameObject>(sb.ToString());
        if (!ReferenceEquals(prop, null)) 
        {
            return Instantiate(prop, position, rotation); 
        }
        else
            Debug.LogError("Prop prefab " + sb.ToString() + " could not be found in " + propsPath);
        sb.Clear();

        return null;
    }

    #endregion

    public void DeleteDungeon()
    {
        try
        {
            foreach (GameObject go in _propInstances)
                Destroy(go);

            foreach (GameObject room in _dungeonRoomInstances)
                Destroy(room);

            foreach (Enemy enemy in _enemyInstances)
                Destroy(enemy.gameObject);
        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning("There is no dungeon to delete.");
        }
    }
}
