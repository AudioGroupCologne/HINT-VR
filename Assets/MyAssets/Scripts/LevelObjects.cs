using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObjects : MonoBehaviour
{

    [SerializeField] List<levelObject> levelObjects;
    [SerializeField] List<GameObject> objects;
    [SerializeField] List<Vector3> positions;

    public class levelObject
    {
        public GameObject obj;
        public Vector3 pos;
        public string tag;

        public levelObject(GameObject _obj, Vector3 _pos, string _tag)
        {
            obj = _obj;
            pos = _pos;
            tag = _tag;
        }
    }

    public levelObject getLevelObject(string tag)
    {
        for (int i = 0; i < levelObjects.Count; i++)
        {
            if (levelObjects[i].tag == tag)
                return levelObjects[i];
        }

        return null;
    }

    public GameObject getGameObject(int index)
    {
        if (index >= levelObjects.Count)
        {
            Debug.LogWarning("Object does not exists!");
            return null;
        }
            

        return levelObjects[index].obj;
    }

    public Vector3 getPosition(int index)
    {
        Vector3 empty = new Vector3(0, 0, 0);

        if (index >= levelObjects.Count)
        {
            Debug.LogWarning("Position does not exists!");
            return empty;
        }
            
        return levelObjects[index].pos;
    }

}
