using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StringInterpreter : MonoBehaviour {
    GameObject turtle;
    TurtleController controller;
    L_System l_system;
    int itt = 3;
    public string sentence;


    public void GenerateString(int weight)
    {
        l_system = new L_System();

        controller = GetComponent<TurtleController>();
        sentence = l_system.setUp(weight);
        for (int i = 0; i < itt; i++)   
        {
            sentence = l_system.nextSentence(sentence);
        }
    }
	
	
	void Update ()
    {
        
	}
    public char NextTask()
    {
        char nextTask;

        nextTask = sentence[0];
        sentence = sentence.Substring(1);
        
        return nextTask;
    }
}
