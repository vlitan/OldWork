using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spec1
{
    class BytePack
    {
        private byte val;
        public byte Value 
        { 
            get
            {
                return val;
            }
            set
            {
                val=value;
            }
        }
        
        public bool this[byte i]
        {
            get
            { 
                return GetValueAt(i); 
            }
            set
            {
                PutIn(i, value);
            }
        }

        private bool GetValueAt(byte index)
        {
            byte aux = 1;
            if (index == 0)
            {
                index = 7;
            }
            else
            {
                index = (byte)(index - 1);
            }


            aux = (byte)(aux << (index));
            return ((val & aux) > 0) ? true : false;
        }

        private void PutIn(int index, bool k)
        {
            if (index == 0)
            {
                index = 7;
            }
            else
            {
                index = (byte)(index - 1);
            }


            if (k)
                val|=(byte)(1 << (index));
            else
                val&=(byte)((1 <<(index)) ^ val);
        }

        public BytePack()
        {
            val = 0;
        }
    }
}
