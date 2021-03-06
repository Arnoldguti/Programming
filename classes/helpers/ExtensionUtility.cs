﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Arnol Gutierrez


namespace Project.Helpers
{
   static class ExtensionUtility
    {

       public static string DecodeFromUtf8(this string utf8String)
       {
           // copy the string as UTF-8 bytes.
           byte[] utf8Bytes = new byte[utf8String.Length];
           for (int i = 0; i < utf8String.Length; ++i)
           {
               //Debug.Assert( 0 <= utf8String[i] && utf8String[i] <= 255, "the char must be in byte's range");
               utf8Bytes[i] = (byte)utf8String[i];
           }

           return Encoding.UTF8.GetString(utf8Bytes, 0, utf8Bytes.Length);
       }

    }
}
