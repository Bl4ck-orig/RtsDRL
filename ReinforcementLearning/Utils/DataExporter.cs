using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;

namespace ReinforcementLearning.Utils
{
    public class DataExporter
    {
        public static void ExportData(double[] data, string filePath)
        {
            var jsonData = JsonConvert.SerializeObject(data.ToList());
            File.WriteAllText(filePath, jsonData);
        }

        public static void ExportData(List<double> data, string filePath)
        {
            var jsonData = JsonConvert.SerializeObject(data);
            File.WriteAllText(filePath, jsonData);
        }
    }
}
