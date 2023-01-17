using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Events;
using UnityEngine.UI;

//extend the MonoBehaviour class
public class NumberPad : MonoBehaviour
    {
    public string sequence;

    public KeycardSpawner cardSpawner;

    public TextMeshProUGUI inputDisplayText;
    
    private string m_CurrentEnteredCode = "";

//set displayed code to empty as default
    private void Awake()
    {
        inputDisplayText.text = "";
    }

    public void ButtonPressed(int valuePressed)
    {
        
        //appends current number pressed to m_CurrentEnteredCode string
        m_CurrentEnteredCode += valuePressed;

        
        //Display entered code as number of inputs hidden by '*'. Resets text and colour in case of incorrect previous code input
        if (m_CurrentEnteredCode.Length == 1)
        {
            inputDisplayText.text = "*";
            inputDisplayText.color = Color.black;
        }
        else 
        {
            inputDisplayText.text += "*";
        }

        //Correct code check once entered code is sufficiently long 
        if (m_CurrentEnteredCode.Length == sequence.Length)
        {
            //If entered code matches correct sequence, spawn key card. Display green success message 
            if (m_CurrentEnteredCode == sequence)
            {
                cardSpawner.SpawnKeyCard();
                inputDisplayText.text = "Access Granted!";
                inputDisplayText.color = Color.green;

            }
            else //Notify player of incorrect code in red text
            {
                Debug.Log("Wrong sequenced entered");
                inputDisplayText.text = "Invalid Code!";
                inputDisplayText.color = Color.red;
                
            }
            
            //Reset m_CurrentEnteredCode var only(hence use of false) so that success/failure message remains until a button is pressed again
            ResetSequence(false);
        }
    }
	
    //Clears current entered code so new code may be attempted
	public void ResetSequence(bool clearText)
    {
        m_CurrentEnteredCode = "";
		
		if(clearText)
		{
			inputDisplayText.text = "";
			inputDisplayText.color = Color.black;
		}
	}
    
}