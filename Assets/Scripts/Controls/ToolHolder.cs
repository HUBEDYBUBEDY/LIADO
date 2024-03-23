using System.Collections.Generic;
using UnityEngine;

public class ToolHolder : MonoBehaviour
{
    [SerializeField] private GameObject toolSprite;
    [SerializeField] private GameObject toolObject;
    [SerializeField] private int MAX;
    [SerializeField] private int current;
    [SerializeField] private List<Transform> spawnLocations;
    [SerializeField] private List<GameObject> storedSprites;

    void Start() {
        current = 0;
        MAX = transform.childCount;

        for(int i=0; i < MAX; i++) {
            spawnLocations.Add(transform.GetChild(i));
            SpawnSprite(transform.GetChild(i));
            current += 1;
        }
    }

    public bool Store(GameObject objectToStore) {
        if(current < MAX && objectToStore.tag == "Pickup") {
            if(objectToStore.GetComponent<ObjectGrabbable>().GetIsTool() == true) {
                SpawnSprite(spawnLocations[current]);
                current += 1;
                return true;
            } else {
                Debug.Log("Can't store a non-tool item here.");
                return false;
            }
        } else {
            Debug.Log("Capacity full.");
            return false;
        }
    }

    public GameObject Spawn() {
        if(current > 0) {
            current -= 1;
            Destroy(storedSprites[current]);
            storedSprites.RemoveAt(current);
            return Instantiate(toolObject, spawnLocations[current].position, Quaternion.identity);
        } else {
            return null;
        }
    }

    private void SpawnSprite(Transform spawnTransform) {
        GameObject tool = Instantiate(toolSprite, spawnTransform.position, Quaternion.identity);
        tool.transform.SetParent(spawnTransform);
        storedSprites.Add(tool);
    }
}
