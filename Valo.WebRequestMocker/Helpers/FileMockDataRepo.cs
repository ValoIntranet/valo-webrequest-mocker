using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valo.WebRequestMocker.Model;

namespace Valo.WebRequestMocker.Helpers
{
    public class FileMockDataRepo : IMockDataRepo
    {
        public string FilePath { get; set; }
        public FileMockDataRepo(string filePath)
        {
            FilePath = filePath;
        }
        public List<MockResponseEntry<T>> LoadMockData<T>()
        {
            string serializedData = File.ReadAllText(FilePath);
            List<MockResponseEntry<T>> result = JsonConvert.DeserializeObject<List<MockResponseEntry<T>>>(serializedData, MockResponseEntry.SerializerSettings);

            return result;
        }

        public void SaveMockData(List<MockResponseEntry> mockedData)
        {
            string serializedData = JsonConvert.SerializeObject(mockedData, MockResponseEntry.SerializerSettings);
            using (StreamWriter outputFile = new StreamWriter(FilePath))
            {
                outputFile.Write(serializedData);
            }
        }
    }
}
