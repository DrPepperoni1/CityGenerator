using System.Collections;

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class L_System 
{

    public string axiom = "F";
    List<Rule> Rules;
    Rule rule1;
    Rule rule2;
    Rule rule3;
    public string setUp(int weight)
    {
        if (weight == 1)
        {

            rule1 = new Rule('F', "FFFFFFFFFF-FFFFFFFFFFFF+FFFFFFFFFF+FFFFFFFFFFF");
            rule2 = new Rule('F', "FFFFFFFF-FFFFFFFFFFF+FFFFFFFFFFF");
            rule3 = new Rule('F', "FFFFF+FFFF-FFF+FFFFFF");
        }
        else if (weight == 2)
        {
            rule1 = new Rule('F', "FFFFFF-FFFFFFF+FFFFFF-FFFFFF");
            rule2 = new Rule('F', "FFFFF+FFFFF-FFFFF");
            rule3 = new Rule('F', "FFFFFF+FFFFFF-FFFFFF");
        }
        else
        {
            rule1 = new Rule('F', "FFFF+FFFF-FFFF+FFF-FFFFFF");
            rule2 = new Rule('F', "FFF-FFFF+FFF-FFFF");
            rule3 = new Rule('F', "FFFF+FFF-FFF-FFFF+FFFF+FFF");
        }
        Rules = new List<Rule>
        {
            rule1,rule2,rule3
        };

        return axiom;

        
    }
    public string nextSentence(string lastSentence)
    {
        string _nextSentence = "";
        foreach (char c in lastSentence)
        {
            int rnd = Random.Range(0, Rules.Count);


            if (Rules[rnd].Check(c))
            {
                _nextSentence += Rules[rnd].output;
            }
            _nextSentence += c;
            
        }
        return _nextSentence;
    }
}


