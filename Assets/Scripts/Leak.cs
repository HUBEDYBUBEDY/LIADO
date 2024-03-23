using UnityEngine;

public class Leak : MonoBehaviour
{
    [SerializeField] private float baseAngle = 10f;
    [SerializeField] private float baseEmission = 50;
    [SerializeField] private float baseHealth = 1.5f;
    
    [SerializeField] private int strength = 0;
    [SerializeField] private float currentHealth = 0f;

    private ParticleSystem particle;

    void Awake() {
        particle = GetComponentInChildren<ParticleSystem>();
    }

    void Update() {
        // Adjust angle depending on health (health is already scaled by strength)
        var newShape = particle.shape;
        newShape.angle = baseAngle * (currentHealth/baseHealth);

        // Adjust emission rate depending on health
        var newEmission = particle.emission;
        newEmission.rateOverTime = baseEmission * (currentHealth/baseHealth);
    }

    public void SetStrength(int strength) {
        this.strength = strength;
        currentHealth = baseHealth * strength;
    }

    public int GetStrength() {
        return strength;
    }

    public void Fix(float fixAmount) {
        currentHealth -= fixAmount;
        if(currentHealth <= 0f) {
            Destroy(gameObject);
        }
    }
}
