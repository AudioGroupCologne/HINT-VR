using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class windowGraph : MonoBehaviour
{

    [SerializeField]  private Sprite circleSprite;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform graphContainer;

    private List<GameObject> gameObjectList;

    void Awake()
    {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        gameObjectList = new List<GameObject>();
    }

    public void SetProgressGraph(List<float> snrValues)
    {
        ShowGraph(snrValues);
    }

    private GameObject createCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);

        return gameObject;
    }

    private void ShowGraph(List <float> valueList)
    {
        float xSize;
        float yMin;
        float yMax;
        float xOffset;
        float graphHeight = graphContainer.sizeDelta.y;

        foreach (GameObject gameObject in gameObjectList)
        {
            Destroy(gameObject);
        }
        gameObjectList.Clear();

        // get bounds of x-axis
        xSize = graphContainer.sizeDelta.x / (valueList.Count + 1);
        xOffset = xSize;

        // get bounds of y-axis
        yMin = valueList[0];
        yMax = valueList[0];

        for (int i = 0; i < valueList.Count; i++)
        {
            if(valueList[i] < yMin)
            {
                yMin = valueList[i];
            }
            if (valueList[i] > yMax)
            {
                yMax = valueList[i];
            }
        }

        // give a bit of overhead
        yMax = yMax + (yMax - yMin) * 0.2f;
        yMin = yMin - (yMax - yMin) * 0.2f;

        GameObject lastCircle = null;

        for(int i = 0; i < valueList.Count; i++)
        {
            float xPosition = (i * xSize) + xOffset;
            float yPosition = Mathf.Abs((valueList[i] - yMin) / ((yMax - yMin)) * graphHeight);

            Debug.Log("yPos " + i + " is " + yPosition + " from " + valueList[i]);

            GameObject currentCircle = createCircle(new Vector2(xPosition, yPosition));
            gameObjectList.Add(currentCircle);
            if(lastCircle != null)
            {
                GameObject connection = CreateDotConnection(lastCircle.gameObject.GetComponent<RectTransform>().anchoredPosition, currentCircle.gameObject.GetComponent<RectTransform>().anchoredPosition);
                gameObjectList.Add(connection);
            }
            lastCircle = currentCircle;


            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -20f);
            labelX.GetComponent<Text>().text = i.ToString();
            gameObjectList.Add(labelX.gameObject);

        }

        int separatorCount = 10;
        for(int i = 0; i <= separatorCount; i++)
        {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            
            float normalizedValue = i * 1f / separatorCount;

            labelY.anchoredPosition = new Vector2(-7f, normalizedValue * graphHeight);
            labelY.GetComponent<Text>().text = Mathf.RoundToInt(yMin + (normalizedValue * (yMax - yMin))).ToString();
            gameObjectList.Add(labelY.gameObject);
        }

    }

    private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

        Vector2 direction = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);

        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + direction * distance * 0.5f;

        float angle = (Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI);
        rectTransform.localEulerAngles = new Vector3(0, 0, angle);

        return gameObject;
    }


}
