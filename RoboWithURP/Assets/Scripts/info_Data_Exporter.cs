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

        
        //Debug.Log(hp.CurrentPosition);
  
        
        
    }

    public void WriteToCSV()
    {
        //Vector3 normalized = Vector3.Normalize(hp.CurrentVelocity);
        if (hp.bIsGrabbing)
        {
            Debug.Log("X:" + hp.CurrentVelocity.x*Time.deltaTime + " Y:" + hp.CurrentVelocity.y*Time.deltaTime + " Z:" + hp.CurrentVelocity.z*Time.deltaTime);

            if (dataList.dataItems.Length > 0)
        {

            TextWriter t_Writer = new StreamWriter(fileName, true);
            for (int i = 0; i < dataList.dataItems.Length; i++)
            {

                    //if you are concatanating a nuber write line is a string so 
                    //numbers must be added before put in the string to be written
                    float x = dataList.dataItems[i].x + (hp.CurrentVelocity.x*Time.deltaTime);
                    float y = dataList.dataItems[i].y + (hp.CurrentVelocity.y * Time.deltaTime);
                    float z = dataList.dataItems[i].z + (hp.CurrentVelocity.z * Time.deltaTime);

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
