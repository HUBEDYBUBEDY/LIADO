using System;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineManager : MonoBehaviour
{
    [SerializeField] private GameManger gameManger;
    [SerializeField] private SubmarineMovement2D submarineMovement;
    [SerializeField] private Transform water;
    [SerializeField] private GameObject leakObject;

    [SerializeField] public float activationDepth = 30f;
    [SerializeField] private bool inAir = false;
    [SerializeField] private float currentAir;
    [SerializeField] private float maxAir = 30f;

    [SerializeField] private float currentWater;
    [SerializeField] private float collisionThreshold = 0.5f;
    [SerializeField] private float sinkMultiplier = 1f;
    [SerializeField] private float leakRate = 0.01f;
    [SerializeField] private float drainRate = 0f;
    [SerializeField] private List<GameObject> leaks;

    [SerializeField] private List<Container> containers;

    private Rigidbody2D rb;
    private float waterOriginalY;
    private float waterTargetY = -0.34f;

    void Awake()
    {
        gameManger = FindObjectOfType<GameManger>();
        gameManger.SetMaxAir(maxAir);
        submarineMovement = GetComponent<SubmarineMovement2D>();
        currentAir = maxAir;

        rb = GetComponent<Rigidbody2D>();
        currentWater = 0f;
        drainRate = 0f;
        waterOriginalY = water.localPosition.y;

        // Add all containers currently on the ship
        foreach(Container container in FindObjectsOfType<Container>()) {
            containers.Add(container);
        }
    }

    void Update() {
        if (currentAir > 0 && !inAir) {
            currentAir -= Time.deltaTime;
            gameManger.SetAir(currentAir);
        } else if(!inAir) {
            OnSubmarineDrown();
        }
    }

    // Use for rigidbody calculations
    void FixedUpdate() {
        // Calculate current water by adding leakrate from all leaks and subtracting drainrate
        currentWater = Mathf.Clamp(currentWater + (GetLeaks()*leakRate - drainRate)*Time.deltaTime, 0f, 1f);
        submarineMovement.SetWater(currentWater);
        // Move water sprite from original position by currentWater
        water.localPosition = new Vector3(water.localPosition.x, waterOriginalY + Mathf.Abs(waterOriginalY-waterTargetY)*currentWater, water.localPosition.z);
        // Sink submarine based on current water
        rb.AddForce(transform.up * -currentWater * submarineMovement.GetYSpeedMax() * sinkMultiplier);
    }

    private void OnSubmarineDrown() {
        enabled = false;
        gameManger.OnSubmarineDrown();
    }

    // Returns true if object was successfully stored, false otherwise
    public bool Store(GameObject objectToStore) {
        bool stored = false;

        // Attempt to store the object in each container, in order
        foreach(Container container in containers) {
            if(!stored) {
                stored = container.Store(objectToStore);
                if(stored) return true;
            }
        }

        return false;
    }

    public int GetTotalValue() {
        int value = 0;
        foreach(Container container in containers) {
            value += container.GetValue();
        }
        return value;
    }

    public void EnterAir() {
        if(!inAir) {
            inAir = true;
            currentAir = maxAir;
            submarineMovement.EnterAir();

            gameManger.SetAir(currentAir);
            gameManger.OnResurface(GetTotalValue());
        }
    }

    public void ExitAir() {
        if(inAir) {
            inAir = false;
            submarineMovement.ExitAir();
            gameManger.OnStart();
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Ground")) {
            // Calculate magnitude of collision impact
            float magnitude = collision.relativeVelocity.magnitude;
            // Debug.Log("Collided with ground at: " + magnitude);

            if(magnitude >= collisionThreshold) {
                // e.g spawn 3 leaks if magnitude is 3.5*collisionThreshold
                int strength = (int)Math.Truncate(magnitude/collisionThreshold);

                ContactPoint2D point = collision.GetContact(0);

                // Spawn leak, rotated to face inwards from point of contact
                float angle = Vector2.SignedAngle(Vector2.up, point.normal);
                GameObject newLeak = Instantiate(leakObject, point.point, Quaternion.Euler(0f, 0f, angle), transform);

                // Set the strength depending on force of impact
                newLeak.GetComponent<Leak>().SetStrength(strength);
                leaks.Add(newLeak);
            }
        }
    }

    // Returns the effective number of leaks (i.e total strength of all leaks)
    public int GetLeaks() {
        int totalLeaks = 0;

        for(int i=0; i<leaks.Count; i++) {
            if(leaks[i] == null) {
                // If leak has been repaired, remove it from list
                leaks.Remove(leaks[i--]);
            } else {
                // Otherwise add its strength to total leak strength
                totalLeaks += leaks[i].GetComponent<Leak>().GetStrength();
            }
        }

        return totalLeaks;
    }

    // Add to the current drain rate
    public void Drain(float drainRate) {
        this.drainRate = Mathf.Clamp(this.drainRate+drainRate, 0f, float.MaxValue);
    }
}
