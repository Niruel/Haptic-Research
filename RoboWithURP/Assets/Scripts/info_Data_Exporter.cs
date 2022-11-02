using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class info_Data_Exporter : MonoBehaviour
{
    //public GameObject go;
    public HapticPlugin hp;
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
        //hp = go.GetComponent<HapticPlugin>();// grab the haptic actor component
         //cup_transform = go.GetComponent<Transform>();
        
        fileName = Application.dataPath + "/Data.csv";
        TextWriter t_Writer = new StreamWriter(fileName, false);
        t_Writer.WriteLine("Name, X, Y, Z");
        t_Writer.Close();

    }
    private void Update()
    {
            
            
            WriteToCSV();

            Debug.Log(hp.CurrentVelocity.x.ToString());
        //Debug.Log(hp.CurrentPosition);
  
        
        
    }

    public void WriteToCSV()
    {
        if (hp.bIsGrabbing)
        {

        
        if (dataList.dataItems.Length > 0)
        {

            TextWriter t_Writer = new StreamWriter(fileName, true);
            for (int i = 0; i < dataList.dataItems.Length; i++)
            {

                    // you need to remember to set correct types
                    float x = dataList.dataItems[i].x + hp.CurrentVelocity.x;
                    float y = dataList.dataItems[i].y + hp.CurrentVelocity.y;
                    float z = dataList.dataItems[i].z + hp.CurrentVelocity.z;

                    t_Writer.WriteLine(dataList.dataItems[i].name + "," + x + "," + y + "," + z);

                    //t_Writer.WriteLine(dataList.dataItems[i].name + "," +
                    //dataList.dataItems[i].x + hp.CurrentVelocity.x.ToString() + ","
                    //+ dataList.dataItems[i].y + hp.CurrentVelocity.y.ToString() + "," +
                    //dataList.dataItems[i].z + hp.CurrentVelocity.z.ToString());
            }


            t_Writer.Close();


        } 
    }
    }
}
