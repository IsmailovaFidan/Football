﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballApp
{
  static class Extention
    {
        public static bool isNotEmpty(string[]values, string checkInput)
        {
            foreach(string a in values)
            {
                if (a == checkInput)
                {
                    return false;   
                }
            }
            return true;

        }
       
    }
}
