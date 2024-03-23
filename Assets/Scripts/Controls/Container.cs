using System;
using TMPro;
using UnityEngine;

public class Container : MonoBehaviour
{
    [SerializeField] private SubmarineManager manager;
    [SerializeField] private int value = 0;
    [SerializeField] private int maxValue = 15;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private LootSpawner.Type type = LootSpawner.Type.NONE;

    void Start() {
        value = 0;
        scoreText.text = "0\n/" + maxValue.ToString();
    }

    public bool Store(GameObject objectToStore) {
        // Only store pickup objects with a positive value if there is space
        if(value < maxValue && objectToStore.tag == "Pickup") {
            int valueToStore = objectToStore.GetComponent<ObjectGrabbable>().GetValue();

            // If object has value, it must be of type 'Gemstone'
            if(valueToStore > 0) {
                Gemstone gemstone = objectToStore.GetComponent<Gemstone>();
                if(type == LootSpawner.Type.NONE) {
                    // If loot spawner has no type, set to type of gemstone and store
                    type = gemstone.type;
                    scoreText.color = gemstone.GetColor();
                } else if(type != gemstone.type) {
                    // If loot spawner already has type which does not match, do not store
                    return false;
                }

                // Add value to store, or clamp value at max if overflowing
                value = Math.Min(value + valueToStore, maxValue);
                scoreText.text = value.ToString() + "\n/" + maxValue.ToString();
                return true;
            }
        }
        return false;
    }

    public int GetValue() {
        return value;
    }

    public int GetMaxValue() {
        return maxValue;
    }

    public LootSpawner.Type GetLootType() {
        return type;
    }

    public SubmarineManager GetManager() {
        return manager;
    }
}
