using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using CustomTypes.VRHINTTypes;

public class testUserData
{
    // order of sentence lists of test procedure
    [JsonProperty] List<int> listOrder;
    // order of conditions
    [JsonProperty] List<hintConditions> conditionsOrder;
    // SRT for each test list
    [JsonProperty] List<float> listSRT;
    // hit quote for each sentence
    [JsonProperty] List<float> hitQuote;

    // user constructor
    public testUserData()
    {
        listOrder = new List<int>();
        conditionsOrder = new List<hintConditions>();
        listSRT = new List<float>();
        hitQuote = new List<float>();
    }

    public void addTestResults(List<int> _listOrder, List<hintConditions> _condOrder, List<float> _listSRT, List<float> _hitQuote)
    {

        listOrder = _listOrder;
        conditionsOrder = _condOrder;
        listSRT = _listSRT;
        hitQuote = _hitQuote;

    }

    
    public void getData(out List<int> _listOrder, out List<hintConditions> _condOrder, out List<float> _listSRT, out List<float> _hitQuote)
    {
        _listOrder = listOrder;
        _condOrder = conditionsOrder;
        _listSRT = listSRT;
        _hitQuote = hitQuote;
    }

}
