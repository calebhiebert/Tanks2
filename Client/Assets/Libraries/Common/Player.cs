using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    public class Player
    {
        public NetConnection Connection { get; set; }
        public string Name { get; set; }
        public Tank Tank { get; set; }
        public PlayerInput Input { get; set; }
    }
}
