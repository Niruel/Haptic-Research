using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class info_Data_Exporter : MonoBehaviour
{
    public GameObject gO;
    HapticPlugin hp;
    Transform cup_transform;
    
    
    string fileName = "";

    [System.Serializable]
    public class DataItem
    {
        public string name;
        
        public float x;
        public float y;
        public float z;

        
    }

    [System.Serializable]
    public class DataList
    {
        public DataItem[] dataItems;
    }
    public DataList dataList = new DataList();
    private void Start()
    {
        //hp = gO.GetComponent<HapticPlugin>();// grab the haptic actor component
         cup_transform = gO.GetComponent<Transform>();
        
        fileName = Application.dataPath + "/Data.csv";
        TextWriter t_Writer = new StreamWriter(fileName, false);
        t_Writer.WriteLine("Name, X, Y, Z");
        t_Writer.Close();

    }
    private void Update()
    {
            
       
            WriteToCSV();
            //Debug.Log(hp.CurrentVelocity);
        //Debug.Log(hp.CurrentPosition);
  
        
        
    }

    public void WriteToCSV()
    {
        if (dataList.dataItems.Length>0)
        {
            
            TextWriter t_Writer = new StreamWriter(fileName, true);
            for (int i = 0; i < dataList.dataItems.Length; i++)
            {
                t_Writer.WriteLine(dataList.dataItems[i].name + "," +
                    dataList.dataItems[i].x + cup_transform.position.x.ToString() + ","
                    + dataList.dataItems[i].y + cup_transform.position.y.ToString() + "," +
                    dataList.dataItems[i].z + cup_transform.position.z.ToString());
            }

          
               t_Writer.Close();
          
            
        }
    }
}
