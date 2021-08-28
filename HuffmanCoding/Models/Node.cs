using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace huffman
{
    public class Node
    {
        public Node Left { get; set; } = null;
        public Node Right { get; set; } = null;
        public float Prob { get; set; } = 0;
        public char Value { get; set; } = default(char);
        public string Code { get; set; } = "";
        public Node Parent { get; set; } = null;
        public bool Visited { get; set; } = false;

    }
}
