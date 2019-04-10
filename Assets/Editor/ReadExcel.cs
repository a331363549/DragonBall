using UnityEngine;
using System.IO;
using Excel;
using LitJson;
using System.Data;
using System.Collections.Generic;
using UnityEditor;
using Newtonsoft.Json;
using UnityEngine.UI;
using System;

public class ReadExcel
{

    public static void ReadConfig()
    {
        FileStream stream = File.Open(Application.streamingAssetsPath + "/gameData.xlsx", FileMode.Open, FileAccess.Read);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        string filePath = Application.dataPath + @"/Resources/mission.json";
        DataSet result = excelReader.AsDataSet();

        Sheet_Config config = new Sheet_Config();

        for (int i = 0; i < result.Tables.Count; i++)
        {
            switch (i)
            {
                case 0:
                    filePath = Application.dataPath + @"/Resources/mission.json";
                    XLSX(result, filePath, i, config.mission);
                    break;
                case 1:
                    filePath = Application.dataPath + @"/Resources/init_ball.json";
                    XLSX(result, filePath, i, config.init_ball);
                    break;
                case 2:
                    filePath = Application.dataPath + @"/Resources/shoot_plan.json";
                    XLSX(result, filePath, i, config.shoot_plan);
                    break;
                case 3:
                    filePath = Application.dataPath + @"/Resources/pan_plan.json";
                    XLSX(result, filePath, i, config.pan_plan);
                    break;
            }
        }
    }


    public static void XLSX(DataSet result,string filePath,int index,Dictionary<int,string> dic_type)
    {

        int columns = result.Tables[index].Columns.Count;
        int rows = result.Tables[index].Rows.Count;
        for (int i = 2; i < rows; i++)
        {
            string id = result.Tables[index].Rows[i][0].ToString();
            Dictionary<string, string> tmp_dic = new Dictionary<string, string>();
            string msg = "";
            if (id == "")
                break;
            for (int j = 1; j < columns; j++)
            {
                string key = result.Tables[index].Rows[1][j].ToString();
                if (key == "")
                    break;
                string value = result.Tables[index].Rows[i][j].ToString();
                tmp_dic.Add(key, value);
            }
            msg = JsonConvert.SerializeObject(tmp_dic);
            //             Dictionary<string, string> dic_1 = new Dictionary<string, string>();
            //             dic_1 = JsonConvert.DeserializeObject<Dictionary<string,string>>(msg);
            dic_type.Add(int.Parse(id), msg);
        }


        //找到当前路径
        FileInfo file = new FileInfo(filePath);
        //判断有没有文件，有则打开文件，，没有创建后打开文件
        StreamWriter sw = file.CreateText();
        //ToJson接口将你的列表类传进去，，并自动转换为string类型
        string json = JsonConvert.SerializeObject(dic_type);
        sw.WriteLine(json);
        //注意释放资源
        sw.Close();
        sw.Dispose();

        AssetDatabase.Refresh();

    }

}
