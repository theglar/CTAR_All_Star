using CTAR_All_Star.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CTAR_All_Star.ViewModels
{
    public class PatientListViewModel : PatientViewModel
    {
        public ObservableCollection<Patient> Patients { get; set; }

        public PatientListViewModel()
        {
            this.Patients = new ObservableCollection<Patient>();
            //Just for tesing
            this.Patients.Add(new Patient
            {
                patientId = 63452574
            });
            this.Patients.Add(new Patient
            {
                patientId = 54554421
            });
            this.Patients.Add(new Patient
            {
                patientId = 46652351
            });
            this.Patients.Add(new Patient
            {
                patientId = 68753444
            });
            this.Patients.Add(new Patient
            {
                patientId = 94345454
            });
        }
    }
}
