using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui_test_modul
{
   public class Algorithm
    {

      public  enum STATE_ALGORITHM 
        {
            ALG_IDLE = 1,
            ALG_1,
            ALG_2,
            ALG_3,
            ALG_4,
            ALG_5
        };


       static  public STATE_ALGORITHM _state_algorithm = Algorithm.STATE_ALGORITHM.ALG_IDLE;

        public Algorithm()
        { 
        
        
        
        }

        public void algorithm_process()

        { 
        
        
        
        }
      public STATE_ALGORITHM Current_state_algorithm
        {
            get
            { 
                return _state_algorithm;
            }
            set 
            {
                _state_algorithm = value;
            }
     
        
        }  

    }
}


