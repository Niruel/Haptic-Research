using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class info_Data_Exporter : MonoBehaviour
{
    public GameObject go;
    public HapticPlugin hp;
    public GameManagement timeManager;
    Transform cup_transform;
    
    
    string fileName = "";

    [System.Serializable]
    public class DataItem
    {
        public string name;
        public float x;
        public float y;
        public float z;
        public float time;

       
    }

    [System.Serializable]
    public class DataList
    {
        public DataItem[] dataItems;
    }
    public DataList dataList = new DataList();
    private void Start()
    {
        //dataList.dataItems[3].time=timeManager.timer;
        //hp = go.GetComponent<HapticPlugin>();// grab the haptic actor component
        cup_transform = go.GetComponent<Transform>();
        string dir = Application.dataPath + "\\Data\\";
        string fName= dataList.dataItems[0].name + ".csv";
        fileName = dir + "\\" + fName;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        else
        {
            TextWriter t_Writer = new StreamWriter(fileName, false);
            t_Writer.WriteLine("Name, X, Y, Z, Time");
            t_Writer.Close();
        }
        

    }
    private void Update()
    {
        WriteToCSV();  
    }

    public void WriteToCSV()
    {
        //Vector3 normalized = Vector3.Normalize(hp.CurrentVelocity);
       // if (hp.bIsGrabbing)
        {
            //Debug.Log("X:" + hp.CurrentVelocity.x*Time.deltaTime + " Y:" + hp.CurrentVelocity.y*Time.deltaTime + " Z:" + hp.CurrentVelocity.z*Time.deltaTime);

            if (dataList.dataItems.Length > 0)
            {

                TextWriter t_Writer = new StreamWriter(fileName, true);
                for (int i = 0; i < dataList.dataItems.Length; i++)
                {
                    //float x = dataList.dataItems[i].x + (Mathf.Abs(hp.CurrentVelocity.x));
                    //float y = dataList.dataItems[i].y + (Mathf.Abs(hp.CurrentVelocity.y));
                    //float z = dataList.dataItems[i].z + (Mathf.Abs(hp.CurrentVelocity.z));

                    float x = dataList.dataItems[i].x + cup_transform.position.x;
                    float y = dataList.dataItems[i].y + cup_transform.position.y;
                    float z = dataList.dataItems[i].z + cup_transform.position.z;
                    float time = dataList.dataItems[i].time;    

                    t_Writer.WriteLine(dataList.dataItems[i].name + "," + x + "," + y + "," + z, "," + time);

                }
            t_Writer.Close();


            } 
        }
    }
}
