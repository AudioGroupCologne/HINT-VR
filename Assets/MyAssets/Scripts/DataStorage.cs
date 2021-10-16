using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataStorage
{
    public static int TrainingGame_Total;
    public static int TrainingGame_Hits;
    public static int TrainingGame_Misses;
    public static int TrainingGame_Unsure;
    public static float[] TrainingGame_SNR = new float[40]; // ToDo: make this dynamic or at least dependent on gameLength...
    // maybe delete this and just hand everything over from TrainingGameManger to Results?
}
