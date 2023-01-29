using System.Collections.Generic;
using System.IO;

namespace SpecFlowSampleProject
{
    public class Transformer
    {
        public List<SampleJE> TransformFile(string filename)
        {
            using (var stream = new FileStream(filename, FileMode.Open))
            using (var reader = new StreamReader(stream))
            {
                List<SampleJE> lines = new List<SampleJE>();
                string line;
                while((line = reader.ReadLine()) != null)
                {
                    var columns = line.Split(",");
                    var journalEntry = new SampleJE(columns[0], columns[1], int.Parse(columns[2]), decimal.Parse(columns[3]), decimal.Parse(columns[4]));
                    lines.Add(journalEntry);
                }

                return lines;
            }
        }
    }
}