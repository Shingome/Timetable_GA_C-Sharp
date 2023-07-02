using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Timetable_GA
{
    [Serializable]
    public class Groups
    {
        public List<string> groupsNames;
        public List<List<string>> teacher;
        public List<List<List<string>>> teacherSubject;
        public List<List<List<int>>> teacherSubjectNum;
        public List<List<string>> cabinets;
        public List<List<string>> subjects;
        public List<List<int>> subjectNeeds;

        public Groups()
        {
            groupsNames = new List<string>();
            teacher = new List<List<string>>();
            teacherSubject = new List<List<List<string>>>();
            teacherSubjectNum = new List<List<List<int>>>();
            cabinets = new List<List<string>>();
            subjects = new List<List<string>>();
            subjectNeeds = new List<List<int>>();
        }

        public void AddGroup()
        {
            groupsNames.Add("");
            teacher.Add(new List<string>());
            teacherSubject.Add(new List<List<string>>());
            teacherSubjectNum.Add(new List<List<int>>());
            cabinets.Add(new List<string>());
            subjects.Add(new List<string>());
            subjectNeeds.Add(new List<int>());
        }

        public void AddGroup(Group group)
        {
            groupsNames.Add(group.name);
            teacher.Add(group.teacher);
            teacherSubject.Add(group.teacherSubject);
            teacherSubjectNum.Add(group.teacherSubjectNum);
            cabinets.Add(group.cabinets);
            subjects.Add(group.subjects);
            subjectNeeds.Add(group.subjectNeeds);
        }

        public void SetGroup(int index, Group group)
        {
            groupsNames[index] = group.name;
            teacher[index] = group.teacher;
            teacherSubject[index] = group.teacherSubject;
            teacherSubjectNum[index] = group.teacherSubjectNum;
            cabinets[index] = group.cabinets;
            subjects[index] = group.subjects;
            subjectNeeds[index] = group.subjectNeeds;
        }

        public Group GetGroup(int index) => new Group(
            groupsNames[index],
            teacher[index],
            teacherSubject[index],
            teacherSubjectNum[index],
            cabinets[index],
            subjects[index],
            subjectNeeds[index]);

        public void RemoveAt(int index)
        {
            groupsNames.RemoveAt(index);
            teacher.RemoveAt(index);
            teacherSubject.RemoveAt(index);
            teacherSubjectNum.RemoveAt(index);
            cabinets.RemoveAt(index);
            subjects.RemoveAt(index);
            subjectNeeds.RemoveAt(index);
        }
    }
}
