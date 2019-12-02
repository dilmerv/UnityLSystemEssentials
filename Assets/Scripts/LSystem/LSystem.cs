using System.Linq;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LSystem
{
    [SerializeField]
    private string sentence;

    private string originalSentence;

    [SerializeField]
    private List<Rule> rules = new List<Rule>();

    private int generations;

    public int GenerationCount
    {
        get { return generations; }
    }

    public int RuleCount
    {
        get { return rules.Count; }
    }

    public string GeneratedSentence 
    {
        get { return sentence; }
    }
    
    public void SaveOriginalSentence()
    {
        if(string.IsNullOrEmpty(originalSentence))
        {
            originalSentence = sentence;
        }
    }

    public void RestoreToOriginalSentence()
    {
        sentence = originalSentence;
    }

    public void Generate()
    {
        StringBuilder nextGen = new StringBuilder();

        for(int i = 0; i < sentence.Length; i++)
        {
            // example axiom :  'F--F--F'
            // example rule 1:  'F -> F--F--F--G'
            // example results: 'F--F--F--G--F--F--F--G--F--F--F--G'
            
            char curr = sentence[i];
            string replace = $"{curr}";

            for(int j = 0; j < rules.Count(); j++)
            {
                char ruleCharacter = rules[j].ruleCharacter;
                if(ruleCharacter == curr)
                {
                    replace = rules[j].ruleReplacement;
                    break;
                }
            }
            nextGen.Append(replace);
        }

        sentence = nextGen.ToString();

        generations++;
    }
}
