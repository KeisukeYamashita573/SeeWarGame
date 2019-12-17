using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;


namespace Server
{
    [DataContract]
    class PlayerData
    {
        [DataMember]
        public Dictionary<string, string> PlLists { get; set; }
    }
}
