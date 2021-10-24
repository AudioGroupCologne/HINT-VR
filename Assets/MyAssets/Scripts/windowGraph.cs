using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class windowGraph : MonoBehaviour
{

    [SerializeField]  private Sprite circleSprite;
    private RectTransform graphContainer;

    void Awake()
    {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();

        //createCircle(new Vector2(200, 200));
        List<int> valueList = new List<int>() { 5, 98, 56, 45, 30, 22, 17, 15, 13, 17, 25};
        ShowGraph(valueList);
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

    private void ShowGraph(List <int> valueList)
    {
        float xSize = 50f;
        float yMax = 100f;
        float xOffset = xSize;
        float graphHeight = graphContainer.sizeDelta.y;

        GameObject lastCircle = null;

        for(int i= 0; i < valueList.Count; i++)
        {
            float xPosition = (i * xSize) + xOffset;
            float yPosition = (valueList[i] / yMax) * graphHeight;
            GameObject currentCircle = createCircle(new Vector2(xPosition, yPosition));
            if(lastCircle != null)
            {
                CreateDotConnection(lastCircle.gameObject.GetComponent<RectTransform>().anchoredPosition, currentCircle.gameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircle = currentCircle;
        }
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
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

        float angle = (Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI);//Angle(direction);
        rectTransform.localEulerAngles = new Vector3(0, 0, angle);
    }

    private static float Angle(Vector2 p_vector2)
    {
        if (p_vector2.x < 0)
        {
            return 360 - (Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg;
        }
    }
}
