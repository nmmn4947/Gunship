using System;
using TMPro;
using UnityEngine;

namespace CardProject
{
    public class DebugPerformance : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _meshProUGUI;
        [SerializeField] private FramePerformance _framePerformance;

        private void Update()
        {
            string s = "";
            s += "PERFORMANCE\n last10seconds\n";
            s += "MeanFPS : " + _framePerformance.realTimeMEAN + "\n";
            s += "MedianFPS : " + _framePerformance.realTimeMEDIAN + "\n";
            if (_framePerformance.realTimeWorst >= float.MaxValue)
            {
                s += "WorstFPS : " + 0 + "\n";
            }
            else
            {
                s += "WorstFPS : " + _framePerformance.realTimeWorst + "\n";
            }
            
            _meshProUGUI.text = s;
        }
    }
}
