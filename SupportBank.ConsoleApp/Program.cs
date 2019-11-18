
﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using log4net;
using log4net.Core;

 namespace SupportBank.ConsoleApp
 {
   class Program
   {
     private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

     static void Main()
     {
       logger.Info("SupportBank starting up");

       new ConsoleRunner().Run(new Bank());
     }

   }
 }
