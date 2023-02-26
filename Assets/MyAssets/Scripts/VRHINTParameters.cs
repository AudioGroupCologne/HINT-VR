using System.Collections.Generic;
using UnityEngine;
using CustomTypes.VRHINTTypes;

public class VRHINTParameters : MonoBehaviour
{
    // Path to the HINT stimuli
    public string targetAudioPath = "audio/german-hint/";
    // Path to the noise
    public string noisePath = "audio/german-hint/hd600noiseGR_male";
    // Total number of test lists available
    public int numLists = 12;
    // this determines the number of sentences within each list!
    // If there is a mismatch between this and the actual number of files there will be errors during asset loading
    public int numSentences = 20;

    // Number of sentences played from each list during test procedure (must be smaller than numSentences)
    public int numTestSentences = 20;
    // Number of lists used during test procedure (must be smaller than numLists)
    public int numTestLists = 5;
    // List used for pratice mode (should be either 11 or 12 for German HINT)
    public int practiceList = 12;
    // Number of practice rounds (max. 20)
    public int numPracticeRounds = 5;
    // Noise condition for practice mode
    public hintConditions practiceCondition = hintConditions.noiseRight;

    // increased step size (4 dB), no logging of SNR and hitQuotes
    public int calibrationRounds = 4;
    // ratio of correct words required to lower SNR
    public float decisionThreshold = 0.5f;

    // the first 4 sentences are adjusted in 4 dB steps
    public float initSNRStep = 4.0f;
    // the remaining 16 sentences are adjusted in 16 dB steps
    public float adaptiveSNRStep = 2.0f;
    // initial level of Talker channel at the start of each list
    public float targetStartLevel = 0.5f;
    // fixed level of dist channel (should be set according to calibration)
    public float distractorLevel = 0.0f;
    // Noise condition have the same starting level for speech and noise
    // For the quiet condition this won't make sense
    // Instead set starting level to 25 dBA (65 dBA [calib] - 40 dB)
    public float quietStartingOffset = -40.0f;

    // Number of options shown when using the wordSelection feedbackSystems
    public int wordOptions = 5;

}
