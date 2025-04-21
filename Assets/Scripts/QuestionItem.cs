using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestionItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI qn;
    [SerializeField] private List<TextMeshProUGUI> allOptions;
    
    [SerializeField] private MCQ mcq;

    public void SetMCQ(MCQ m)
    {
        mcq = m;

        qn.text = mcq.question;

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
}
