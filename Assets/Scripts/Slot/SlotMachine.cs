using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SlotMachine : MonoBehaviour
{

    public float spinDuration = 2.0f;
    public int numberOfSym = 10;

    private const int ReelNum = 3;

    public Text[] ReelText;

    public Text betResult;

    public Text totalCredits;
    public InputField inputBet;

    private bool startSpin = false;
    private bool[] ReelSpinned = new bool[ReelNum];


    private int betAmount;
    private int credits = 1000;

    public bool[] Weight = new bool[ReelNum];


    private ArrayList weightedReelPoll = new ArrayList();
    private int zeroProbability = 30;


    private int[] ReelResult = new int[ReelNum];

    private float elapsedTime = 0.0f;



    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < zeroProbability; i++)
        {
            weightedReelPoll.Add(0);
        }

        int reminingValuesProb = (100 - zeroProbability) / 9;
        for (int j = 1; j < 10; j++)
        {
            for (int k = 0; k < reminingValuesProb; k++)
            {
                weightedReelPoll.Add(j);
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Spin()
    {
        if (betAmount > 0)
        {
            startSpin = true;
        }
        else
        {
            betResult.text = "Insert a valid bet!";
        }
    }


    void OnGUI()
    {
        try
        {
            betAmount = int.Parse(inputBet.text);
        }
        catch
        {
            betAmount = 0;
        }

        totalCredits.text = credits.ToString();
    }

    void checkBet()
    {
        bool jackpot = true;
        for (int i = 0; i < ReelNum-1; i++)
        {
            if (ReelResult[i] != ReelResult[i + 1])
            {
                jackpot = false;
            }

        }

        if (jackpot)
        {
            betResult.text = "JACKPOT!";
            credits += 50 * betAmount;
        }
        else if (ReelResult[0] == 0 && ReelResult[2] == 0)
        {
            betResult.text = "YOU WIN" + (betAmount/2).ToString();
            credits -= (betAmount / 2);
        }
        else if (ReelResult[0] == ReelResult[1])
        {
            betResult.text = "AWW... ALMOST JACKPOT!";
        }
        else if (ReelResult[0] == ReelResult[2])
        {
            betResult.text = "YOU WIN" + (betAmount * 2).ToString();
            credits -= (betAmount * 2);
        }
        else
        {
            betResult.text = "YOU LOSE!";
            credits -= betAmount;
        }
    }

    void FixedUpdate()
    {
        if (startSpin)
        {
            elapsedTime += Time.deltaTime;
            int randomSpinResult = Random.Range(0, numberOfSym);

            for (int i = 0; i < ReelNum; i++)
            {
                if (!ReelSpinned[i])
                {

                    ReelText[i].text = randomSpinResult.ToString();
                    if (elapsedTime >= spinDuration)
                    {
                        int result = randomSpinResult;
                        if (Weight[i])
                        {
                            int weightedRandom = Random.Range(0, weightedReelPoll.Count);
                            result = (int) weightedReelPoll[weightedRandom];
                        }

                        ReelResult[i] = result;
                        ReelText[i].text = result.ToString();
                        ReelSpinned[i] = true;
                        elapsedTime = 0;

                        //もう少しスマートにするならコールバックを用意する
                        //最後のリール
                        if (i == ReelNum - 1)
                        {
                            if ((ReelResult[0] == ReelResult[1]) &&
                                randomSpinResult != ReelResult[0])
                            {
                                randomSpinResult = ReelResult[0] - 1;
                                if (randomSpinResult < ReelResult[0]) randomSpinResult = ReelResult[0] - 1;
                                if (randomSpinResult > ReelResult[0]) randomSpinResult = ReelResult[0] + 1;
                                if (randomSpinResult < 0) randomSpinResult = 0;
                                if (randomSpinResult > 9) randomSpinResult = 9;

                                ReelText[i].text = randomSpinResult.ToString();
                                ReelResult[i] = randomSpinResult;
                            }
                            
                            startSpin = false;
                            for (int j = 0; j < ReelNum; j++)
                            {
                                ReelSpinned[j] = false;
                            }
                            checkBet();
                        }

                    }
                    break;
                }
            }


        }
    }


}
