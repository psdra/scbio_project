using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject red, green;

    DataImporter importData;
    bool REDPlayer, hasGameFinished, updateCheck, isPlayer, chooseValue, moveDone, wait;
    bool MoveLeft, MoveRight, MoveDown;
    bool inRight, inLeft, inDown, inPause;
    float timeSinceLastToggle;
    int compCol, GameMode;
    float[] weight = new float[7];
    int[] value = new int[7];
    float[] probability = new float[7];
    float sumProbs, cumulativeProbability, randNum;

    [SerializeField]
    Text turnMessage;
    [SerializeField]
    TextMeshProUGUI GameModeMessage;

    const string RED_MESSAGE = "Red's Turn";
    const string GREEN_MESSAGE = "Greens's Turn";

    Color RED_COLOR = new Color(231, 29, 54, 255) / 255;
    Color GREEN_COLOR = new Color(0, 222, 1, 255) / 255;


    Board myBoard;
    GridPosDef gridPos;
    GameObject circle;
    Vector3 spawnPos;

    //INITIALIZATION
    public void Awake()
    {
        //to get the Data from DataImporter script
        importData = GameObject.FindObjectOfType<DataImporter>();

        //invent a Value Vector for the Hard Game Mode
        weight = new float[] { 1f, 1f, 2f, 4f, 4f, 2f, 2f};

        //set initial values
        REDPlayer = true;
        isPlayer = true;
        moveDone = true;
        chooseValue = true;
        hasGameFinished = false;
        turnMessage.text = RED_MESSAGE;
        turnMessage.color = RED_COLOR;
        myBoard = new Board();
        //saves the SpawnPos. It is defined as the middle object in the top row.
        spawnPos = new Vector3(myBoard.data[0,3].x , myBoard.data[0,3].y , -5);
        gridPos = new GridPosDef{row=0, col=3};

        //Spawn first object
        circle = Instantiate(REDPlayer ? red : green); //creates the new disk
        circle.transform.position = spawnPos;   //decides where to spawn it
        circle.GetComponent<Mover>().targetPosition =  spawnPos; //so it doesn't move right away

        //Set GameMode according to OptionsMenu Slider
        if(PlayerPrefs.GetFloat("sliderValue", 0.5f) == 3){
            GameMode = 3;
            GameModeMessage.text = "GameMode: Multiplayer";
        }
        else if(PlayerPrefs.GetFloat("sliderValue", 0.5f) == 2){
            GameMode = 2;
            GameModeMessage.text = "GameMode: Medium";
        }
        else if(PlayerPrefs.GetFloat("sliderValue", 0.5f) == 1){
            GameMode = 1;
            GameModeMessage.text = "GameMode: Easy";
        }
    }

    public void OpenGameMenu ()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    //Every frame
    private void Update()
    {
        //import data
        inLeft = importData.left;
        inRight = importData.right;
        inDown = importData.down;
        inPause = importData.pause;

        if (wait) return;

        if(inPause && timeSinceLastToggle > 0.2f)
        {
           UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }

        //If GameFinsished then stop. Start again if player makes "Down"
        if (hasGameFinished) 
        {
            if(inDown)
            {
                hasGameFinished = false;
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
            }
            return;
        }

        //EASY OPTION: select random column to deploy disk
        if(GameMode==1 && !isPlayer && chooseValue){
            compCol = UnityEngine.Random.Range(0, 7);
            //check if col has free spots. if not, chose other value
            while(myBoard.freeRow(compCol)==0){
                compCol = UnityEngine.Random.Range(0, 7);
            }
            //don't change value again for this turn
            chooseValue = false;
        }

        //HARD OPTION: calculates which column to use
        if(GameMode==2 && !isPlayer && chooseValue){
            sumProbs = 0f;
            //calculate how full every row is
            for (int i = 0; i < 7; i++)
            {
                value[i] = i;
                Debug.Log("Wert: " + value[i]);
            }
            //probability coincides with amounts of disks in the col and in the neighboring cols.
            probability[0] = weight[value[0]] + 0.6f*weight[value[1]];
            probability[6] = weight[value[6]] + 0.6f*weight[value[5]];
            sumProbs = probability[0] + probability[6];
            for (int i = 1; i <= 5; i++)
            {
                probability[i] = weight[value[i]] + 0.3f*weight[value[i-1]] + 0.3f*weight[value[i+1]];
                sumProbs += probability[i];
            }
            //select compCol randomly but with probabilities
            randNum = UnityEngine.Random.Range(0f, sumProbs);
            cumulativeProbability = 0f;
            for (int i = 0; i < 7; i++)
            {
                cumulativeProbability += probability[i];
                if (randNum <= cumulativeProbability)
                {
                    compCol = i;
                    Debug.Log("Wert ausgewählt: " + compCol);
                    break;
                }
            }
            //do it again if it's already full
            while(myBoard.freeRow(compCol) == 0){
                randNum = UnityEngine.Random.Range(0f, sumProbs);
                cumulativeProbability = 0f;
                for (int i = 0; i < 7; i++)
                {
                    cumulativeProbability += probability[i];
                    if (randNum <= cumulativeProbability)
                    {
                        compCol = i;
                        Debug.Log("Wert ausgewählt: " + compCol);
                        break;
                    }
                }
            }

            //don't change value again for this turn
            chooseValue = false;
        }


        //COMPUTER MOVE
        //is position reached?
        if(circle.GetComponent<Mover>().transform.position == circle.GetComponent<Mover>().targetPosition){
            moveDone = true;
        }
        //if not reached, move disk to desired position
        if(compCol>gridPos.col && !isPlayer && moveDone){
            MoveRight = true;
            moveDone = false;
        }
        else if(compCol<gridPos.col && !isPlayer && moveDone){
            MoveLeft = true;
            moveDone = false;
        }
        else if (compCol==gridPos.col && !isPlayer && moveDone){
            MoveDown = true;
            moveDone = false;
        }

        //PLAYER MOVE:
        //if Input = rightInput (in this case right arrow). Should only be possible every 0.5s
        //set MoveRight to true
        //maybe can be deleted, because ImportData script does everything
        timeSinceLastToggle += Time.deltaTime;
    if (inRight && timeSinceLastToggle > 0.2f && isPlayer){
        timeSinceLastToggle = 0.0f;
        MoveRight = true;
    }

            //same for left
    if (inLeft && timeSinceLastToggle > 0.2f && isPlayer){
        timeSinceLastToggle = 0.0f;
        MoveLeft = true;
    }

                //same for Down
    if (inDown && timeSinceLastToggle > 0.2f && isPlayer){
        timeSinceLastToggle = 0.0f;
        MoveDown = true;
    }

    //RIGHT MOVEMENT:
    if (MoveRight == true && gridPos.col < 6){
        //update pos in the grid
        gridPos.col = gridPos.col +1;
        //move the disk to it's desired position
        circle.GetComponent<Mover>().targetPosition = new Vector3(myBoard.data[gridPos.row, gridPos.col].x , myBoard.data[gridPos.row, gridPos.col].y, -5 );
        MoveRight = false;
    }
    //LEFT MOVEMENT:
    if (MoveLeft == true && gridPos.col > 0){
        gridPos.col = gridPos.col -1;
        //move the disk to it's desired position
        circle.GetComponent<Mover>().targetPosition = new Vector3(myBoard.data[gridPos.row, gridPos.col].x , myBoard.data[gridPos.row, gridPos.col].y, -5 );
        MoveLeft = false;
    }
    //DOWN MOVEMENT:
    if (MoveDown == true){ 
        StartCoroutine(MoveDownCoroutine());
    }
    
}

//DOWN MOVEMENT: Is a coroutine, so that it can be paused until the disk has reached it's position
private IEnumerator MoveDownCoroutine() {
        MoveDown = false;
        wait = true;
        //if the space is still free
        updateCheck = myBoard.UpdateBoard(gridPos.col, REDPlayer);
        if(updateCheck == true){
            gridPos.row = myBoard.finalPos.row;
            gridPos.col = myBoard.finalPos.col;
            //move the disk to it's desired position
            circle.GetComponent<Mover>().targetPosition = new Vector3(myBoard.data[gridPos.row, gridPos.col].x , myBoard.data[gridPos.row, gridPos.col].y, -5 );
            // wait until the disk reaches its target position
            while (circle.GetComponent<Mover>().transform.position != circle.GetComponent<Mover>().targetPosition) {
                yield return null;
            }
            //check if he has won. If won, terminate game
            if(myBoard.Result(REDPlayer))
            {
                turnMessage.text = (REDPlayer ? "Red" : "Green") + " Wins!";
                hasGameFinished = true;
            }
            //if not won, toggle player and spawn new disk
            else{
                //toggle player:
                REDPlayer = !REDPlayer;
                if(GameMode == 1 || GameMode == 2 ){
                    isPlayer = !isPlayer;
                    chooseValue = true;
                }
                /////TurnMessage
                turnMessage.text = REDPlayer ? RED_MESSAGE : GREEN_MESSAGE;
                turnMessage.color = REDPlayer ? RED_COLOR : GREEN_COLOR;

                //Spawn new disk
                circle = Instantiate(REDPlayer ? red : green); //creates the new disk
                circle.transform.position = spawnPos;   //decides where to spawn it
                circle.GetComponent<Mover>().targetPosition =  spawnPos; //so it doesn't move right away
                gridPos = new GridPosDef{row=0, col=3};
            }
            wait = false;
        }
        else {
            wait = false;
        }
    }
}

