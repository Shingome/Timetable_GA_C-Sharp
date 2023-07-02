using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable_GA
{
    [Serializable]
    public class Settings
    {
        public int days; 
        public int hours; 
        public int groups;
        public int population_size; 
        public int max_generation; 
        public int tournament_size; 
        public double p_crossover;
        public double p_mutation;

        public Settings() 
        {
            this.days = 5;
            this.hours = 11;
            this.groups = 0;
            this.population_size = 2000;
            this.max_generation = 120;
            this.tournament_size = 3;
            this.p_crossover = 0.9;
            this.p_mutation = 0.1;
        }

        public Settings(int days, int hours, int groups, int population_size, int max_generation, int tournament_size, double p_crossover, double p_mutation) 
        {
            this.days = days;
            this.hours = hours;
            this.groups = groups;
            this.population_size = population_size;
            this.max_generation = max_generation;
            this.tournament_size = tournament_size;
            this.p_crossover = p_crossover;
            this.p_mutation = p_mutation;
        }

        public void SetSettings(int days, int hours, int groups, int population_size, int max_generation, int tournament_size, double p_crossover, double p_mutation)
        {
            this.days = days;
            this.hours = hours;
            this.groups = groups;
            this.population_size = population_size;
            this.max_generation = max_generation;
            this.tournament_size = tournament_size;
            this.p_crossover = p_crossover;
            this.p_mutation = p_mutation;
        }

        public void SetSettings(Settings settings)
        {
            this.days = settings.days;
            this.hours = settings.hours;
            this.groups = settings.groups;
            this.population_size = settings.population_size;
            this.max_generation = settings.max_generation;
            this.tournament_size = settings.tournament_size;
            this.p_crossover = settings.p_crossover;
            this.p_mutation = settings.p_mutation;
        }
    }
}
