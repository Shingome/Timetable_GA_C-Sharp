using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable_GA
{
    [Serializable]
    public class Fines
    {
        public double fullSubject;
        public double emptyHours;
        public double lastHours;
        public double firstHours;
        public double allHours;
        public double emptyCabinet;
        public double teacherSubject;
        public double teacherCrossing;
        public double cabinetCrossing;

        public Fines(double fullSubject, double emptyHours, double lastHours, double firstHours, double allHours,
            double emptyCabinet, double teacherSubject, double teacherCrossing, double cabinetCrossing)
        {
            this.fullSubject = fullSubject;
            this.emptyHours = emptyHours;
            this.lastHours = lastHours;
            this.firstHours = firstHours;
            this.allHours = allHours;
            this.emptyCabinet = emptyCabinet;
            this.teacherSubject = teacherSubject;
            this.teacherCrossing = teacherCrossing;
            this.cabinetCrossing = cabinetCrossing;
        }

        public Fines() {
            this.fullSubject = 0;
            this.emptyHours = 0;
            this.lastHours = 0;
            this.firstHours = 0;
            this.allHours = 0;
            this.emptyCabinet = 0;
            this.teacherSubject = 0;
            this.teacherCrossing = 0;
            this.cabinetCrossing = 0;
        }

        public void SetFines(double fullSubject, double emptyHours, double lastHours, double firstHours, double allHours,
            double emptyCabinet, double teacherSubject, double teacherCrossing, double cabinetCrossing)
        {
            this.fullSubject = fullSubject;
            this.emptyHours = emptyHours;
            this.lastHours = lastHours;
            this.firstHours = firstHours;
            this.allHours = allHours;
            this.emptyCabinet = emptyCabinet;
            this.teacherSubject = teacherSubject;
            this.teacherCrossing = teacherCrossing;
            this.cabinetCrossing = cabinetCrossing;
        }

        public void SetFines(Fines fines)
        {
            this.fullSubject = fines.fullSubject;
            this.emptyHours = fines.emptyHours;
            this.lastHours = fines.lastHours;
            this.firstHours = fines.firstHours;
            this.allHours = fines.allHours;
            this.emptyCabinet = fines.emptyCabinet;
            this.teacherSubject = fines.teacherSubject;
            this.teacherCrossing = fines.teacherCrossing;
            this.cabinetCrossing = fines.cabinetCrossing;
        }
    }
}
