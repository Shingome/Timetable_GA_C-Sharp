using System.Collections.Generic;
using System.Linq;
using System;
using System.Data;
using System.Globalization;
using System.Net;
using MoreLinq;
using System.Windows.Forms;

namespace Timetable_GA
{
    public class GA
    {
        public int days;
        public int hours;

        public List<List<string>> teacher;
        public List<List<List<string>>> teacherSubject;
        public List<List<List<int>>> teacherSubjectNum;
        public List<List<string>> cabinets;
        public List<List<string>> subjects;
        public List<List<int>> subjectNeeds;

        public List<Individual> population;

        public Individual hof;

        public int groups;
        public int group_size;
        public int individual_size;
        public int population_size;
        public int max_generation;
        public int tournament_size;

        public double p_crossover;
        double p_mutation;

        Random random;

        public Fines fines;

        public GA(Settings settings, Fines fines, Groups groups_list)
        {
            this.days = settings.days;
            this.hours = settings.hours;
            this.groups = settings.groups;
            this.population_size = settings.population_size;
            this.max_generation = settings.max_generation;
            this.tournament_size = settings.tournament_size;
            this.p_crossover = settings.p_crossover;
            this.p_mutation = settings.p_mutation;

            this.fines = fines;

            group_size = hours * days * 3;
            individual_size = group_size * groups;

            teacher = groups_list.teacher;
            teacherSubject = groups_list.teacherSubject;
            teacherSubjectNum = groups_list.teacherSubjectNum;

            cabinets = groups_list.cabinets;

            subjects = groups_list.subjects;
            subjectNeeds = groups_list.subjectNeeds;

            population = new List<Individual>();

            random = new Random();
        }

        public void start(Train output)
        {
            // Инициализируем начальную популяцию
            population = populationCreator();
            // Создаем списки, в которых будет хранится статистика
            List<double> avg = new List<double>();
            List<double> min = new List<double>();
            List<double> max = new List<double>();
            List<double> generations = new List<double>();

            // Запускаем алгоритм на max_generation поколений
            for (int gen = 0; gen < max_generation; gen++)
            {
                // Отбор, кроссинговер, мутаиция
                population = selTournament();
                population = cxTwoPoints();
                population = mutUniformInt();

                // Сохраняем для статистики
                avg.Add(population.Average(ind => ind.Fitness()));
                min.Add(population.Min(ind => ind.Fitness()));
                max.Add(population.Max(ind => ind.Fitness()));
                generations.Add(gen);

                // Сохраняем лучшего индивида
                Hof();

                // Выводим статистику
                output.RefreshListBox(
                    avg[avg.Count() - 1],
                    generations[generations.Count() - 1],
                    min[min.Count() - 1],
                    max[max.Count() - 1]);
                output.Refresh();
                output.DrawGraph(generations, avg, max, min);
            }
        }

        public void Hof()
        {
            var select = new List<Individual>(population);
            select.Add(hof);
            hof = select.Min();
        }

        public int[] individualCreator()
        {
            int[] individual = new int[individual_size];
            int group_id = 0;
            int caret = 0;
            for (int i = 0; i < individual_size; i++)
            {
                if (i % group_size == 0 && i != 0)
                    group_id += 1;

                if (caret == 3)
                    caret = 0;

                if (caret == 0)
                    individual[i] = random.Next(0, subjects[group_id].Count());
                else if (caret == 1)
                    individual[i] = random.Next(0, teacher[group_id].Count());
                else if (caret == 2)
                    individual[i] = random.Next(0, cabinets[group_id].Count());

                caret += 1;
            }
            return individual;
        }

        public List<Individual> populationCreator()
        {
            List<Individual> population = new List<Individual>();
            for (int i = 0; i < population_size; i++)
                population.Add(new Individual(individualCreator(), this));
            return population;
        }

        public List<Individual> selTournament()
        {
            List<Individual> choosen = new List<Individual>();
            List<Individual> new_population = new List<Individual>();
            for (int i = 0; i < population_size; i++)
            {
                choosen.Clear();
                for (int j = 0; j < tournament_size; j++)
                {
                    choosen.Add(population[random.Next(0, population_size)]);
                }
                new_population.Add(choosen.Min());
            }
            return new_population;
        }

        public List<Individual> cxTwoPoints()
        {
            List<Individual> new_population = new List<Individual>();
            int[] ind1, ind2;
            List<int> new_ind1 = new List<int>();
            List<int> new_ind2 = new List<int>();
            while (new_population.Count() <= population_size)
            { 
                new_ind1.Clear();
                new_ind2.Clear();
                ind1 = population[random.Next(0, population_size)].individual;
                ind2 = population[random.Next(0, population_size)].individual;

                if (random.NextDouble() < p_crossover)
                {
                    int cxPoint1 = random.Next(0, individual_size);
                    int cxPoint2 = random.Next(0, individual_size - 1);
                    if (cxPoint2 >= cxPoint1)
                        cxPoint2 += 1;
                    else
                        (cxPoint1, cxPoint2) = (cxPoint2, cxPoint1);


                    new_ind1.AddRange(ind1.ToList().GetRange(0, cxPoint1));
                    new_ind1.AddRange(ind2.ToList().GetRange(cxPoint1, cxPoint2 - cxPoint1));
                    new_ind1.AddRange(ind1.ToList().GetRange(cxPoint2, individual_size - cxPoint2));

                    new_ind2.AddRange(ind2.ToList().GetRange(0, cxPoint1));
                    new_ind2.AddRange(ind1.ToList().GetRange(cxPoint1, cxPoint2 - cxPoint1));
                    new_ind2.AddRange(ind2.ToList().GetRange(cxPoint2, individual_size - cxPoint2));

                    new_population.Add(new Individual(new_ind1.ToArray(), this));
                    new_population.Add(new Individual(new_ind2.ToArray(), this));
                }


            }
            return new_population;
        }

        public List<Individual> mutUniformInt()
        {
            List<Individual> new_population = new List<Individual>();
            Individual new_ind;
            int len = 0;
            for (int i = 0; i < population_size; i++)
            {
                new_ind = population[i];
                if (random.NextDouble() < p_mutation)
                {
                    int group_id = 0;
                    int caret = 0;

                    for (int j = 0; j < individual_size; j++)
                    {
                        if (j % group_size == 0 && j != 0)
                            group_id += 1;

                        if (caret == 3)
                            caret = 0;

                        if (caret == 0)
                            len = subjects[group_id].Count();
                        else if (caret == 1)
                            len = teacher[group_id].Count();
                        else if (caret == 2)
                            len = cabinets[group_id].Count();

                        if (random.NextDouble() < 1.0 / individual_size)
                        {
                            new_ind.individual[j] = random.Next(0, len);
                        }

                        caret += 1;
                    }

                }
                new_population.Add(new_ind);
            }
            return new_population;
        }
    }
}
