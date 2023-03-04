using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CustomTypes.VRHINTTypes;

public class Counterbalancing : MonoBehaviour
{
    /**
     * Load LatinSquare matrices for list and condition order.
     * Assign correct order based on current userIndex
     */
    public static void ImportLatinSquares(int userIndex, int numTestLists, testOrder order, List<hintConditions> conditions, List<int> listOrder)
    {

        // helper variables
        int overhang = 0;
        int orderedUserIndex = 0;

        // get userIndex
        userIndex = UserManagement.selfReference.getNumTests();

        // create storage
        List<string[]> lqConditions = new List<string[]>();
        List<int[]> lqLists = new List<int[]>();


        // to ensure that all conditions are included at least once (as long as test length is at least the amount of condtition)
        // simply jump a row if it's the second part of the test procedure
        // ToDo: make this optional for a full-VR HINT procedure!
        if (order == testOrder.first)
        {
            orderedUserIndex = userIndex;
        }
        else if (order == testOrder.second)
        {
            orderedUserIndex = userIndex + 1;
        }

        // load lqConditions.csv as TextAsset
        // Convert into array of strings ("quiet,noiseFront,noiseRight,noiseLeft\n" "noiseFront,noiseLeft,quiet,noiseRight\n" ...) 
        TextAsset lqConditionsRaw = Resources.Load("others/lqConditions") as TextAsset;
        string[] lqConditionsSplit = lqConditionsRaw.ToString().Replace("\r", string.Empty).Split('\n');

        // get individual conditions by splitting strings at ',' separator
        for (int i = 0; i < lqConditionsSplit.Length; i++)
        {
            if (lqConditionsSplit[i].Length > 1)
            {
                lqConditions.Add(lqConditionsSplit[i].Split(','));
            }
        }

        for (int i = 0; i < numTestLists; i++)
        {
            // jump into the next row if the length of a row has been exceed (e.g. take conds [0...3] from row 2 and conds [4..5] from row 3
            if (i > 0 && i % lqConditions[0].Length == 0)
            {
                overhang++;
            }

            // assing hintCondition based on the strings from the lq matrix
            switch (lqConditions[(orderedUserIndex + overhang) % lqConditions.Count][i - (overhang * lqConditions[0].Length)])
            {
                case "noiseFront":
                    conditions.Add(hintConditions.noiseFront);
                    break;
                case "noiseLeft":
                    conditions.Add(hintConditions.noiseLeft);
                    break;
                case "noiseRight":
                    conditions.Add(hintConditions.noiseRight);
                    break;
                case "quiet":
                    conditions.Add(hintConditions.quiet);
                    break;
                default:
                    Debug.LogError("Unrecognized condition: " + lqConditions[orderedUserIndex % lqConditions.Count][i]);
                    break;
            }
        }

        Debug.Log("Test Conditions = " + string.Join(" ", new List<hintConditions>(conditions).ConvertAll(i => i.ToString()).ToArray()));

        // load lqLists.csv as TextAsset
        TextAsset lqListsRaw = Resources.Load("others/lqLists") as TextAsset;
        // Convert TextAsset into array of strings ("1,2,10,...6", "2,3,1,...,7" ...)
        string[] lqListsSplit = lqListsRaw.ToString().Replace("\r", string.Empty).Split('\n');

        // Split at separator ',' and convert into array of integers
        for (int i = 0; i < lqListsSplit.Length; i++)
        {
            if (lqListsSplit[i].Length > 1)
            {
                lqLists.Add(System.Array.ConvertAll(lqListsSplit[i].Split(','), int.Parse));
            }
        }

        // temporary storage
        int[] tmp = new int[numTestLists];

        // if more than half of lqLists dim is used, take 'fresh' rows for both tests
        if (numTestLists > lqLists[0].Length / 2)
        {
            for (int i = 0; i < numTestLists; i++)
            {
                if (order == testOrder.first)
                {
                    tmp[i] = lqLists[orderedUserIndex % lqLists.Count][i];
                }
                else if (order == testOrder.second)
                {
                    tmp[i] = lqLists[(orderedUserIndex) % lqLists.Count][i];
                }
            }
        }
        // split row into two parts (ideally 5 + 5 for optimal counter-balancing)
        // example: 1, 2, 10, 3, 9, 4, 8, 5, 7, 6
        // - first: 1, 2, 10, 3, 9
        // - second: 4, 8, 5, 7, 6
        else
        {
            for (int i = 0; i < numTestLists; i++)
            {
                if (order == testOrder.first)
                {
                    tmp[i] = lqLists[userIndex % lqLists.Count][i];
                }
                else if (order == testOrder.second)
                {
                    tmp[i] = lqLists[userIndex % lqLists.Count][i + numTestLists];
                }
            }
        }


        Debug.Log("Test Lists = " + string.Join(" ", new List<int>(tmp).ConvertAll(i => i.ToString()).ToArray()));

        // Move lists from tmp to listOrder variable 
        listOrder.AddRange(tmp);

    }
}
