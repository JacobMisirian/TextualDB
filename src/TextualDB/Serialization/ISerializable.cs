using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextualDB.Serialization
{
    public interface ISerializable
    {
        void Serialize(StringBuilder sb);
    }
}
