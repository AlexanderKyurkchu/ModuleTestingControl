using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui_test_modul
{
  public  class StatusByte
    {
        
      private byte status;

        public StatusByte(byte Status) { status = Status; }

        public bool IsSpectrum => (status & 0b1) != 0;
        public bool IsBeta => (status & 0b10) != 0;
        public bool IsAlfa => (status & 0b100) != 0;
        public bool IsAlfaOn => (status & 0b1000) != 0;
        public bool IsGammaOn => (status & 0b1_0000) != 0;
        public bool IsBetaOn => (status & 0b10_0000) != 0;
        public bool IsSetting => (status & 0b100_0000) != 0;
        public bool IsHighBit => (status & 0b1000_0000) != 0;
    }

}
