using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextualDB.Components
{
    public class TextualDBRow
    {
        public List<string> Data { get; private set; }

        public TextualDBRow()
        {
            Data = new List<string>();
        }
        public TextualDBRow(List<string> data)
        {
            Data = new List<string>();
        }
        public TextualDBRow(params string[] data)
        {
            Data = new List<string>(data);
        }

        public void Add(params string[] data)
        {
            foreach (string str in data)
                Data.Add(str);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Data.Count; i++)
                sb.AppendFormat("{0} | ", Data[i]);
            return sb.ToString();
        }
    }
}
