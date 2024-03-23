using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Gemstone : ObjectGrabbable
{
    [SerializeField] private Sprite lowSprite;
    [SerializeField] private Sprite medSprite;
    [SerializeField] private Sprite highSprite;
    [SerializeField] private Color color;

    private SpriteRenderer spriteRenderer;
    private Light2D light2D;

    public LootSpawner.Type type;

    void Awake() {
        objectRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        light2D = GetComponent<Light2D>();
    }

    // Set the value, update the sprite and scale of the gem correspondingly
    public bool SetValue(LootSpawner.Type newType, LootSpawner.Value newValue) {
        type = newType;
        SetValue((int)newValue);

        float newScale;
        float newRadius;

        switch (newValue) {
            case LootSpawner.Value.LOW:
                spriteRenderer.sprite = lowSprite;
                newScale = (float)LootSpawner.Scale.LOW / 100f;
                newRadius = (float)LootSpawner.Light.LOW / 100f;
                break;
            case LootSpawner.Value.MED:
                spriteRenderer.sprite = medSprite;
                newScale = (float)LootSpawner.Scale.MED / 100f;
                newRadius = (float)LootSpawner.Light.MED / 100f;
                break;
            case LootSpawner.Value.HIGH:
                spriteRenderer.sprite = highSprite;
                newScale = (float)LootSpawner.Scale.HIGH / 100f;
                newRadius = (float)LootSpawner.Light.HIGH / 100f;
                break;
            default:
                Debug.Log("ERROR: value not recognised.");
                return false;
        }

        transform.localScale = new Vector3(newScale, newScale, newScale);
        light2D.pointLightOuterRadius = newRadius;
        return true;
    }

    public Color GetColor() {
        return color;
    }
}
