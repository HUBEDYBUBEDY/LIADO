using System;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    // Set types of gem available
    public enum Type {
        SMOOTH,
        HARD,
        SPIKE,
        NONE
    }

    // Values of each gem
    public enum Value {
        LOW = 1,
        MED = 3,
        HIGH = 5
    }

    // Scale of each gem (percentage)
    public enum Scale {
        LOW = 15,
        MED = 30,
        HIGH = 40
    }

    // Outer radius of light source for each value of gem
    public enum Light {
        LOW = 50,
        MED = 75,
        HIGH = 100
    }

    [SerializeField] private GameObject smoothGem;
    [SerializeField] private GameObject hardGem;
    [SerializeField] private GameObject spikeGem;

    [SerializeField] private Type type;
    [SerializeField] private int totalValue;

    // void Update() {
    //     if(Input.GetButtonDown("Use")) {
    //         Spawn(type, Value.LOW);
    //     }
    //     if(Input.GetButtonDown("Exit")) {
    //         Spawn(type, Value.MED);
    //     }
    //     if(Input.GetButtonDown("Alt Use")) {
    //         Spawn(type, Value.HIGH);
    //     }
    // }

    void Awake() {
        int spawnedValue = 0;
        Array values = Enum.GetValues(typeof(Value));
        System.Random random = new System.Random();

        while(spawnedValue < totalValue) {
            Value value = (Value)values.GetValue(random.Next(values.Length));

            if(spawnedValue + (int)value <= totalValue) {
                spawnedValue += (int)value;
                Spawn(type, value);
            }
        }
    }

    // Spawn an object of specific type and value, returns False if not valid
    public bool Spawn(Type type, Value value) {
        GameObject newObject;

        switch (type) {
            case Type.SMOOTH:
                newObject = Instantiate(smoothGem, transform.position, Quaternion.identity);
                break;
            case Type.HARD:
                newObject = Instantiate(hardGem, transform.position, Quaternion.identity);
                break;
            case Type.SPIKE:
                newObject = Instantiate(spikeGem, transform.position, Quaternion.identity);
                break;
            default:
                Debug.Log("ERROR: Type not recognised!");
                return false;
        }

        return newObject.GetComponent<Gemstone>().SetValue(type, value);
    }
}
