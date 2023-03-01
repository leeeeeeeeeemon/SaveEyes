using SaveEyes.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveEyes
{
    public class DBConnection
    {
        public static SaveEyesEntities connection = new SaveEyesEntities();
    }
}
