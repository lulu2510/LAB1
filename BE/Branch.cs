using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Branch
    {
        public int branchNumber { get;  set; } // number of branch
        public string branchName { get; set; }  // name of branch
        public string branchAddress { get; set; }  // address of the branch
        public long branchPhone { get; set; }  // the phone of the branch
        public string branchResponsName { get; set; }  // the name of the branch responsible
        public int branchNumWorkers { get; set; }  // the number of workers in the branch
        public int freeDeliverNum { get; set; }   //the number of the free delevers 
        public kosherLevel hechsher { get; set; } 
        public override string ToString()
        {
            return branchNumber + " " + branchName;
        }
    }
}
