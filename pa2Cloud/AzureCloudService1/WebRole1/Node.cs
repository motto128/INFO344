using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1
{
    public class Node
    {
        public Dictionary<char, Node> dictionary { get; set; }

        public bool end { get; set; }

        public Node()
        {
            this.dictionary = null;
            this.end = false;
        }
        
    }
}