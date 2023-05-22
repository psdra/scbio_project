using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    DataImporter importData;
    public Slider GameMode;
    public GameObject[] Menus;
    private bool inRight, inLeft, inDown;
    public float toggleTimer;
        [SerializeField]
    TextMeshProUGUI GameModeExplain;
    public void OnSliderValueChanged()
    {
        // Save the value of the slider in the PlayerPrefs
        PlayerPrefs.SetFloat("sliderValue", GameMode.value);
    }

    void Start()
    {
        importData = GameObject.FindObjectOfType<DataImporter>();
        GameMode.value = PlayerPrefs.GetFloat("sliderValue", 0.5f);
        toggleTimer = 0.0f;
    }

    void Update()
    {
        switch(GameMode.value)
        {
            case 1:
            GameModeExplain.text = "The computer chooses randomly which column to pick.";
            break;
            case 2:
            GameModeExplain.text = "The computer is going to choose columns more likely, if there are already disks in the columns nearby.";
            break;
            case 3:
            GameModeExplain.text = "Two players play against each other and have to switch.";
            break;
            default:
            GameModeExplain.text = "";
            break;
        }

                //runs timer
        toggleTimer += Time.deltaTime;
        inRight = importData.right;
        inLeft = importData.left;
        inDown = importData.down;

        //usually should import the data from Matlab. Right now uses arrow keys
        if(inRight && toggleTimer > 0.2f && GameMode.value <3)
        {
            toggleTimer = 0.0f;
            GameMode.value += 1;
        }
        if(inLeft && toggleTimer > 0.2f && GameMode.value > 1)
        {
            toggleTimer = 0.0f;
            GameMode.value -= 1;
        }
        if(inDown && toggleTimer > 0.2f)
        {
            toggleTimer = 0.0f;
            Menus[0].SetActive(false);
            Menus[1].SetActive(true);
        }
    }
}
