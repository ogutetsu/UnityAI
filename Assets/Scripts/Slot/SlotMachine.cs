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

    private int[] ReelResult = new int[ReelNum];

    private float elapsedTime = 0.0f;



    // Start is called before the first frame update
    void Start()
    {
        
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
        bool Win = true;
        for (int i = 0; i < ReelNum-1; i++)
        {
            if (ReelResult[i] != ReelResult[i + 1])
            {
                Win = false;
            }

        }

        if (Win)
        {
            betResult.text = "YOU WIN!";
            credits += 500 * betAmount;
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
                        ReelResult[i] = randomSpinResult;
                        ReelSpinned[i] = true;
                        elapsedTime = 0;

                        if (i == ReelNum - 1)
                        {
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
