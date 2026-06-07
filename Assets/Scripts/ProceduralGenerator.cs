using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class ProceduralGenerator : MonoBehaviour
{
    [SerializeField] LevelDetails levelDetails;
    [SerializeField] GameObject resourcePrefab;
    [SerializeField] GameObject enemyPrefab;
    Room[] allRooms;
    Room[] nonPortalRooms;

    void Start()
    {
        allRooms = FindObjectsByType<Room>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        nonPortalRooms = allRooms.Where(room => room.portal == null).ToArray();
        MakeRoomsUnscannable();
        OpenDoors();
        SetAltars();
        SpawnEnemies();
        GenerateResources();
    }

    void GenerateResources()
    {
        (int, int) minMaxRooms = levelDetails.rewards switch
        {
            Rewards.LOW => (2, 4),
            Rewards.MEDIUM => (4, 6),
            Rewards.HIGH => (6, 10)
        };

        int roomNum = Random.Range(minMaxRooms.Item1, minMaxRooms.Item2);
        List<Room> usableRooms = nonPortalRooms.ToList();
        for(int i = 0; i < roomNum; i++)
        {
            int roomIndex = Random.Range(0, usableRooms.Count);
            SpawnResourcesInRoom(usableRooms[roomIndex]);
            usableRooms.Remove(usableRooms[roomIndex]);
        }
    }

    void SpawnResourcesInRoom(Room room)
    {
        (int, int) minMax = levelDetails.rewards switch
        {
            Rewards.LOW => (1, 1),
            Rewards.MEDIUM => (1, 3),
            Rewards.HIGH => (2, 4)
        };

        int resourceCount = Random.Range(minMax.Item1, minMax.Item2);
        for(int i = 0; i < resourceCount; i++)
        {
            GameObject resource = Instantiate(resourcePrefab);
            resource.transform.position = room.GetRandomPointInRoom(0.5f);
        }
    }

    void MakeRoomsUnscannable()
    {
        float chance = levelDetails.interference switch
        {
            Interference.LOW => 0.2f,
            Interference.MEDIUM => 0.35f,
            Interference.HIGH => 0.5f
        };

        float randFloat;
        foreach(Room room in allRooms)
        {
            randFloat = Random.Range(0f, 1f);
            room.unscannable = randFloat < chance;
        }
    }

    void OpenDoors()
    {
        Door[] doors = FindObjectsByType<Door>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        float chance = levelDetails.openness switch
        {
            Openness.LOW => 0,
            Openness.MEDIUM => 0.1f,
            Openness.HIGH => 0.2f
        };

        float randFloat;
        foreach(Door door in doors)
        {
            bool portalDoor = false;
            foreach(Room room in door.rooms)
            {
                if (room.portal != null) portalDoor = true;
            }
            if (portalDoor)
            {
                continue;
            }

            randFloat = Random.Range(0f, 1f);
            if(randFloat < chance)
            {
                door.ImmediateOpen();
            }
        }
    }

    void SpawnEnemies()
    {
        float percent = levelDetails.threatLevel switch
        {
            ThreatLevel.LOW => 0.1f,
            ThreatLevel.MEDIUM => 0.2f,
            ThreatLevel.HIGH => 0.3f
        };

        int max = levelDetails.threatLevel switch
        {
            ThreatLevel.LOW => 1,
            ThreatLevel.MEDIUM => 2,
            ThreatLevel.HIGH => 3
        };

        int enemyCount = Mathf.CeilToInt(percent * nonPortalRooms.Length);
        enemyCount = Mathf.Max(enemyCount, max);

        List<Room> remainingRooms = nonPortalRooms.ToList();
        for(int i = 0; i < enemyCount; i++)
        {
            int roomIndex = Random.Range(0, remainingRooms.Count);
            Room room = remainingRooms[roomIndex];
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.transform.position = room.GetRandomPointInRoom();
            remainingRooms.Remove(room);
        }
    }

    void SetAltars()
    {
        List<Altar> altars = FindObjectsByType<Altar>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        foreach(Altar altar in altars)
        {
            altar.gameObject.SetActive(false);
        }
        int count = levelDetails.threatLevel switch
        {
            ThreatLevel.LOW => 0,
            ThreatLevel.MEDIUM => Random.Range(0, 2),
            ThreatLevel.HIGH => Random.Range(0, 2)
        };
        for(int i = 0; i < count; i++)
        {
            int index = Random.Range(0, altars.Count);
            altars[index].gameObject.SetActive(true);
            altars[index].Desecrate();
            altars.Remove(altars[index]);
        }
    }
}
