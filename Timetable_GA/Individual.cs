using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;

namespace Timetable_GA
{
    public class Individual : IComparable<Individual>
    {
        public int[] individual { get; set; }
        GA algorithm;

        public Individual(int individual_size, GA algorithm)
        {
            int[] individual = new int[individual_size];
            this.algorithm = algorithm;
        }

        public Individual(int[] individual, GA algorithm)
        {
            this.individual = individual;
            this.algorithm = algorithm;
        }

        List<List<int>> splitGroups(List<int> individual)
        {
            List<int> new_individual = new List<int>(individual);
            var groups = new List<List<int>>();
            while (new_individual.Any())
            {
                groups.Add(new_individual.Take(algorithm.group_size).ToList());
                new_individual.RemoveRange(0, algorithm.group_size);
            }
            return groups;
        }

        List<List<int>> groupPreprocess(List<int> group)
        {
            var groups = new List<List<int>>();
            for (int i = 0; i < 3; i++)
                groups.Add(new List<int>());
            int sphere = 0;
            for (int i = 0; i < algorithm.group_size; i++)
            {
                groups[sphere].Add(group[i]);

                sphere += 1;
                if (sphere == 3)
                    sphere = 0;
            }

            return groups;
        }

        List<string> distinctUnion(List<List<string>> list)
        {
            List<string> union = new List<string>();
            foreach (var item in list)
            {
                union.AddRange(item);
            }
            return union.Distinct().ToList();
        }

        List<List<int>> zeroTimetable(int rows)
        {
            List<List<int>> zeros = new List<List<int>>();

            for (int i = 0; i < rows; i++)
            {
                zeros.Add(new List<int>(Enumerable.Repeat(0, algorithm.group_size / 3)));
            }

            return zeros;
        }

        public int CompareTo(Individual other)
        {
            return Fitness().CompareTo(other.Fitness());
        }

        public double Fitness()
        {
            var groups = splitGroups(individual.ToList());

            double fitness = 0;
            for (int i = 0; i < algorithm.groups; i++)
                fitness += evaluateGroup(groups[i], i);

            fitness += teacherCrossing(individual.ToList());
            fitness += cabinetCrossing(individual.ToList());

            return fitness;
        }

        public double evaluateGroup(List<int> group, int group_id)
        {
            var groupParts = groupPreprocess(group);
            return evaluateSubject(groupParts[0], group_id) +
                teacherSubject(groupParts[0], groupParts[1], group_id) +
                emptyCabinet(groupParts[2], groupParts[0]);
        }

        public double teacherCrossing(List<int> individual)
        {
            var allTeachers = distinctUnion(algorithm.teacher);
            var timetable = zeroTimetable(allTeachers.Count());
            var groups = splitGroups(individual);

            for (int group_id = 0; group_id < algorithm.groups; group_id++)
            {
                var teachers = groupPreprocess(groups[group_id])[1];
                for (int i = 0; i < teachers.Count(); i++)
                    timetable[allTeachers.IndexOf(algorithm.teacher[group_id][teachers[i]])][i] += 1;
            }

            int fine = 0;
            foreach (var cab in timetable)
                foreach (var hour in cab)
                    if (hour > 1)
                        fine++;

            return fine * algorithm.fines.teacherCrossing;
        }

        public double cabinetCrossing(List<int> individual)
        {
            var allCabinets = distinctUnion(algorithm.cabinets);
            var timetable = zeroTimetable(allCabinets.Count());
            var groups = splitGroups(individual);

            for (int group_id = 0; group_id < algorithm.groups; group_id++)
            {
                var cabinets = groupPreprocess(groups[group_id])[2];
                for (int i = 0; i < cabinets.Count(); i++)
                    timetable[allCabinets.IndexOf(algorithm.cabinets[group_id][cabinets[i]])][i] += 1;
            }

            int fine = 0;
            foreach (var cab in timetable)
                foreach (var hour in cab)
                    if (hour > 1)
                        fine++;
            return fine * algorithm.fines.cabinetCrossing;
        }

        public double teacherSubject(List<int> subjects, List<int> teachers, int group_id)
        {
            int fine = 0;
            for (int i = 0; i < algorithm.group_size / 3; i++)
            {
                if (!algorithm.teacherSubjectNum[group_id][teachers[i]].Contains(subjects[i]))
                    fine++;
            }
            return fine * algorithm.fines.teacherSubject;
        }

        public double emptyCabinet(List<int> cabinets, List<int> subjects)
        {
            int fine = 0;
            for (int i = 0; i < algorithm.group_size / 3; i++)
                if (subjects[i] != 0 && cabinets[i] == 0)
                    fine++;
            return fine * algorithm.fines.emptyCabinet;
        }

        public double evaluateSubject(List<int> subjects, int group_id)
        {
            return
                allHours(subjects, group_id) +
                firstHours(subjects) +
                lastHours(subjects) +
                emptyHours(subjects) +
                fullSubject(subjects);
        }

        public double allHours(List<int> subjects, int group_id)
        {
            var needs = algorithm.subjectNeeds[group_id];
            int[] predict = Enumerable.Repeat(0, needs.Count).ToArray();
            for (int i = 0; i < algorithm.group_size / 3; i++)
            {
                predict[subjects[i]] += 1;
            }

            int fine = 0;

            for (int i = 1; i < predict.Length; i++)
                if (predict[i] != needs[i])
                    fine++;

            return fine * algorithm.fines.allHours;
        }

        public double firstHours(List<int> subjects)
        {
            int fine = 0;
            for (int i = 0; i < algorithm.group_size / 3; i += algorithm.hours)
                if (subjects[i] != 0)
                    fine++;

            return fine * algorithm.fines.firstHours;
        }

        public double lastHours(List<int> subjects)
        {
            int fine = 0;
            for (int i = 9; i < algorithm.group_size / 3; i += algorithm.hours)
            {

                if (subjects[i] != 0)
                    fine++;
                if (subjects[i + 1] != 0)
                    fine++;
            }

            return fine * algorithm.fines.lastHours;
        }

        public double emptyHours(List<int> subjects)
        {
            int fine = 0;
            for (int i = 1; i < algorithm.group_size / 3; i += algorithm.hours)
            {

                for (int j = 0; j < 6; j++)
                    if (subjects[i + j] == 0)
                        fine++;
            }
            return fine * algorithm.fines.emptyHours;
        }

        public double fullSubject(List<int> subjects)
        {
            int fine = 0;
            for (int i = 1; i < algorithm.group_size / 3; i += 2)
            {
                if (subjects[i] != subjects[i + 1])
                    fine++;
            }
            return fine * algorithm.fines.fullSubject;
        }

        public List<List<string>> SplitToDays(List<string> group)
        {
            List<string> group_copy = new List<string>(group);
            var days = new List<List<string>>();
            while (group_copy.Any())
            {
                days.Add(group_copy.Take(algorithm.group_size / algorithm.days).ToList());
                group_copy.RemoveRange(0, algorithm.group_size / algorithm.days);
            }
            return days;
        }

        public List<List<string>> GroupsToString(List<List<int>> groups)
        {
            var groups_str = new List<List<string>>();
            for (int group_id = 0; group_id < groups.Count; group_id++)
            {
                var group = groups[group_id];
                groups_str.Add(new List<string>());
                int sphere = 0;
                for (int i = 0; i < algorithm.group_size; i++)
                {
                    if (sphere == 0)
                        groups_str[group_id].Add(algorithm.subjects[group_id][group[i]]);

                    if (sphere == 1)
                        groups_str[group_id].Add(algorithm.teacher[group_id][group[i]]);

                    if (sphere == 2)
                        groups_str[group_id].Add(algorithm.cabinets[group_id][group[i]]);

                    sphere += 1;
                    if (sphere == 3)
                        sphere = 0;
                }
            }
            return groups_str;
        }

        public List<List<List<string>>> ToTable()
        {
            List<List<string>> groups = GroupsToString(splitGroups(individual.ToList()));
            List<List<List<string>>> table = new List<List<List<string>>>();
            foreach (var group in groups)
                table.Add(SplitToDays(group));
            return table;
        }

        public string EvaluateIndivid()
        {
            List<string> parameters = new List<string>();

            var groups = splitGroups(individual.ToList());

            double teacherSubjectFine = 0;
            double emptyCabinetFine = 0;
            double allHoursFine = 0;
            double firstHoursFine = 0;
            double lastHoursFine = 0;
            double emptyHoursFine = 0;
            double fullSubjectFine = 0;

            for(int group_id = 0; group_id < groups.Count; group_id++)
            {
                var groupParts = groupPreprocess(groups[group_id]);

                teacherSubjectFine += teacherSubject(groupParts[0], groupParts[1], group_id);
                emptyCabinetFine += emptyCabinet(groupParts[2], groupParts[0]);
                allHoursFine += allHours(groupParts[0], group_id);
                firstHoursFine += firstHours(groupParts[0]);
                lastHoursFine += lastHours(groupParts[0]);
                emptyHoursFine += emptyHours(groupParts[0]);
                fullSubjectFine += fullSubject(groupParts[0]);
            }

            parameters.Add("Fitness: " + Fitness());
            parameters.Add("teacherCrossing: " + teacherCrossing(individual.ToList()));
            parameters.Add("cabinetCrossing: " + cabinetCrossing(individual.ToList()));
            parameters.Add("teacherSubject: " + teacherSubjectFine);
            parameters.Add("emptyCabinet: " + emptyCabinetFine);
            parameters.Add("allHours: " + allHoursFine);
            parameters.Add("firstHours: " + firstHoursFine);
            parameters.Add("lastHours: " + lastHoursFine);
            parameters.Add("emptyHours: " + emptyHoursFine);
            parameters.Add("fullSubject: " + fullSubjectFine);

            return string.Join("\n", parameters);
        }
    }
}
