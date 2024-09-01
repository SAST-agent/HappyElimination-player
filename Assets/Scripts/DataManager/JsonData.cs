using System;
using System.Collections.Generic;

namespace DataManager
{
    public struct JsonData
    {
        public int Round { get; set; }
        public int Player { get; set; }
        public int Steps { get; set; }
        public Operation Operation { get; set; }
        public List<StateChange> StateChanges { get; set; }
    }
}