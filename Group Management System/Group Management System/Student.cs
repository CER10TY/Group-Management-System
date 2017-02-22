using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMS
{
    // Flow chart explanation: https://drive.google.com/file/d/0BxoG_baSR5dfbzYtalVZNkVvck0/view?usp=sharing (requires draw.io setup with google drive)
    // Alternatively, there is a .png with the flowchart inside the Group Management Folder (together with the .cs files)
    public class Student
    {
        public string StudentName { get; set; }
        public string Grade { get; set; }
        public Class Class { get; set; }
        public List<Group> Groups { get; set; }
        public List<Subject> Subjects { get; set; }
    }
}
