using HackatonFiap.HealthScheduling.Domain.Entities.Bases;

namespace HackatonFiap.HealthScheduling.Domain.Entities.Patients
{
    public sealed class Patient : UserIdentity
    {
        public Patient()
        {
                
        }
        public Patient(string rG)
        {
            RG = rG;
        }
        public string RG { get; private set; }
    }
}
