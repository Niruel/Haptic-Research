using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;


public class GridManager : MonoBehaviour {

    private int[,] gridmap;//create 2d array
    //private float gridGran = 1.0f;
    public Transform playerposition;
    public enum eDataState
    {
        raw,
        normalized,
        logten 
    };

    private int
        gridSize = 200,
        maxAmount = 0,
        minAmount = 0;

    public eDataState currentState;

    //private Move PlayerTrans;





    // Use this for initialization
    void Start() {

        gridmap = new int[gridSize, gridSize];//initalize 2d array

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                gridmap[i, j] = 0;

            }
        }

       

        InvokeRepeating("OnRecordLocation", 0.1f, 0.1f);

    }
    void OnDestroy()
    {
        SaveJson();
    }
    //record player position
    void OnRecordLocation()
    {
        int newValue = ++gridmap[ToInt(playerposition.position.x - transform.position.x), ToInt(playerposition.position.z - transform.position.z)];
        maxAmount = Mathf.Max(newValue, maxAmount);
        minAmount = Mathf.Min(newValue, minAmount);
      
    }

    public float Normalize(int value)
    {
        return ((float)value) / ((float)(maxAmount - minAmount));
    }
    public float LogTen(int value)
    {
        float fValueTen = Mathf.Log10(value);
        return fValueTen;
    }
    public int ToInt(float value)
    {
        return (int)Mathf.Round(value);
    }

    delegate float Calculation(int value);

    //  draw gizmo wire frame cube
    void OnDrawGizmos()
    {
        if (gridmap != null){
            for (int j = 0; j < gridmap.GetLength(1); j++)
                for (int i = 0; i < gridmap.GetLength(0); i++)
                {
                    Gizmos.DrawWireCube(new Vector3(i, 0, j) + transform.position, new Vector3(1.0f, 0.01f, 1.0f));

                    if (gridmap[i, j] > 0)
                        Gizmos.color = Color.yellow;
                }


            float r = 0, g = 0, b = 0;
            Calculation calc = value => value;
            switch (currentState)
            {
                case eDataState.raw:
                        r = 1.0f; g = 0.7f; b = 1.0f;
                    break;
                case eDataState.normalized:
                        r = 1.0f; g = 0.0f; b = 0.0f;
                        calc = Normalize;
                    break;
                case eDataState.logten:
                        r = 0.0f; g = 1.0f; b = 0.0f;
                        calc = LogTen;
                    break;
            }

            for (int j = 0; j < gridmap.GetLength(1); j++)
                for (int i = gridmap.GetLength(0) - 1; i >= 0; i--)
                    if (gridmap[i, j] > 0)
                    {
                        Gizmos.color = new Color(r, g, b, Vector3.Distance(playerposition.position, new Vector3(1, 0, j) + transform.position) * 0.2f);
                        Gizmos.DrawCube(new Vector3(i, calc((gridmap[i, j]) / 2) + 1.0f, j) + transform.position, new Vector3(0.5f, calc((gridmap[i, j])), 0.5f));
                    }
        }
    }
    string path = "Assets\\JsonFiles\\Nick.json";
    
    

    public void SaveJson()
    {
        List<DataContainer.Data> dataPoints = new List<DataContainer.Data>();
        
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (gridmap[i, j] > 0.0f)
                {
                    dataPoints.Add(new DataContainer.Data(i, j, gridmap[i, j]));

                }
            }
        }

        File.WriteAllText(path, JsonUtility.ToJson(new DataContainer(dataPoints.ToArray()), true));
    }

    public void LoadJson()
    {
        DataContainer dataContainer;
        if (File.Exists(path))
        {
            dataContainer = JsonUtility.FromJson<DataContainer>(File.ReadAllText(path));

            foreach (var item in dataContainer.data)
                gridmap[item.iXPos, item.iYPos] = item.value;
        }
 
    }

    private void OnApplicationQuit()
    {
        SaveJson();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentState = eDataState.raw;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentState = eDataState.normalized;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentState = eDataState.logten;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveJson();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            LoadJson();
        }

    }
}

[Serializable]
public struct DataContainer
{
    [Serializable]
    public struct Data
    {
        public int iXPos;
        public int iYPos;
        public int value;

        public Data(int x, int y, int value)
        {
            iXPos = x;
            iYPos = y;
            this.value = value;
        }
    }

    public Data[] data;

    public DataContainer(Data[] data)
    {
        this.data = data;
    }
 

}
public static class MyExtensions
{
    public static string AppendTimeStamp(this string fileName)
    {
        return string.Concat(
            Path.GetFileNameWithoutExtension(fileName),
            DateTime.Now.ToString("yyyyMMddHHmmssfff"),
            Path.GetExtension(fileName)
            );
    }
}



