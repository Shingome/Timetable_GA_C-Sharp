using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable_GA
{
    public class Group
    {
        public string name;
        public List<string> teacher;
        public List<List<string>> teacherSubject;
        public List<List<int>> teacherSubjectNum;
        public List<string> cabinets;
        public List<string> subjects;
        public List<int> subjectNeeds;

        public Group()
        {
            name = string.Empty;
            teacher = new List<string>() { "" };
            teacherSubject = new List<List<string>>() { new List<string>() { "" } };
            teacherSubjectNum = new List<List<int>>() { new List<int>() { 0 } };
            cabinets = new List<string>() { "" };
            subjects = new List<string>() { "" };
            subjectNeeds = new List<int>() { -1 };

        }

        public Group(string name, List<string> teacher, List<List<string>> teacherSubject, List<List<int>> teacherSubjectNum, List<string> cabinets, List<string> subjects, List<int> subjectNeeds)
        {
            this.name = name;
            this.teacher = teacher;
            this.teacherSubject = teacherSubject;
            this.teacherSubjectNum = teacherSubjectNum;
            this.cabinets = cabinets;
            this.subjects = subjects;
            this.subjectNeeds = subjectNeeds;
        }
    }
}
