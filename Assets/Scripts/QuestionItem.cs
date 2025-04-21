using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI qn;
    [SerializeField] private List<TextMeshProUGUI> allOptions;
    [SerializeField] private List<Toggle> allToggles;
    
    [SerializeField] private MCQ mcq;

    public void SetMCQ(MCQ m)
    {
        mcq = m;

        qn.text = $"{mcq.question} [-/5]";

        for (int i = 0; i < allOptions.Count; i++)
        {
            if (i == mcq.options.Count)
            {
                allOptions[i].text = "";
                continue;
            }
            
            allOptions[i].text = mcq.options[i];
        }
    }

    public bool IsCorrect()
    {
        for (int i = 0; i < allToggles.Count; i++)
        {
            if (allToggles[i].isOn)
            {
                if (i+1 == mcq.answer)
                {
                    qn.text = $"{mcq.question} [5/5]";
                    return true;
                }
                
                qn.text = $"{mcq.question} [0/5]";
                return false;
            }
        }

        qn.text = $"{mcq.question} [0/5]";
        return false;
    }
}
