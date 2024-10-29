using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI instance;
    public Slider healthSlider;
    public TextMeshProUGUI healthText;

    // Use Awake to initialize the instance before other scripts call it
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
