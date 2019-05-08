using System.Collections.ObjectModel;
using CTAR_All_Star.Models;

namespace CTAR_All_Star.ViewModels
{
    public class TutorialViewModel
    {
        private Tutorial _oldTutorial;

        public ObservableCollection<Tutorial> Tutorials { get; set; }

        public TutorialViewModel()
        {
            Tutorials = new ObservableCollection<Tutorial>
            {
                new Tutorial { Topic = "Hardware",
                    isVisible = false,
                    Description = "To turn the device on, flip the switch located at the bottom of the box. If the device does not turn on, " +
                        "it may need a new 9 volt battery. To use the device, ensure it is connected with the application. When ready to start an exercise, " +
                        "place the ball between the chin and chest and apply pressure when prompted\n",
                    ImageName = "Hardware.jpeg"
                     },

                new Tutorial { Topic = "Bluetooth",
                    isVisible = false,
                    Description = "For a bluetooth connection, the CTAR device needs to be on and in range. " +
                        "Make sure that bluetooth is enabled on the device. Choose \"Bluetooth\" from the main menu and press the \"Tap to scan for devices\" button. " +
                        "From the list  of devices select \"CTAR All-Star\" and it will connect automatically." +
                        "\n\nIf a connection fails please ensure that no other devices are connected to the CTAR All-Star device.\n",
                    ImageName = "Bluetooth.png"},

                new Tutorial { Topic = "Creating an Excercise",
                    isVisible = false,
                    Description = "To create an exercise, navigate to the \"Create Exercise\" option from the main menu. Select an existing patient or create a new patient from the drop down. " +
                        "If creating a new patient, you will have to enter the new patients EMR number in the box that appears to the right. " +
                        "Next, tap the \"Type\" option and select between an isometric or isotonic exercise. Once selected, many fields will be populated automatically. " +
                        "Adjust them appropriately for the patient. The threshold option is the percentage of the patients one-rep-max pressure reading. " +
                        "Once all the options are populated, select \"Save Workout\" at the bottom of the screen and this exercise will become available in the patients " +
                        "\"Choose Exercise\" section.\n",
                    ImageName = "CreateWorkout.jpg"
                    },

                new Tutorial { Topic = "History",
                    isVisible = false,
                    Description = "The History page shows the data stored from each workout completed on the device. " +
                    	"To view the history, select \"History\" from the main menu." +
                    	"\nThe data can be filtered by tapping Date, Patient and/or Session at the top of the page. " +
                    	"After selecting the filter(s) the data can be viewed on a graph by selecting the \"View Graph\" " +
                    	"button at the bottom of the page.",
                    ImageName = "History.jpg"},

                new Tutorial {Topic = "Managing Patients",
                    isVisible = false,
                    Description = "To Manage Patients, tap the \"Manage Patients\" option from the main menu. All current patients will be listed in the white box. " +
                    	"\nIf the box is empty, then patients need to be added.\nTo add a new patient tap the \"Add\" button. Enter the patients EMR number and then click \"Add Patient.\"" +
                        "\nTo remove a patient, Select the patient you want to remove and tap the \"Remove\" button. Click yes when prompted about deleting the patient." +
                        "\nTo create an exercise for a patient, you can tap the \"Create Exercise\" option at the bottom of the screen. This will take you to the create",
                    ImageName = "ManagePatients.jpg"},

                new Tutorial { Topic = "Performing Excercise",
                    isVisible = false,
                    Description = "Before performing an exercise, an exercise must be chosen and the device must be connected via bluetooth. " +
                        "Once ready, go to the \"Exercise\" option from the main menu and press begin. Ensure the ball is placed between the chin and upper chest. " +
                        "Apply pressure to the ball when prompted to \"Apply Pressure\" and relax when prompted to \"Rest.\" " +
                        "The horizontal red bar is the threshold. When applying pressure the timer will only countdown when the pressure is above the threshold." +
                        "When resting, the timer will only count down when the pressure is below the threshold. " +
                        "When the workout is complete, press \"Done\" to save the workout data to the database.\n",
                    ImageName = "Workout.png"
                    },

                new Tutorial { Topic = "Choosing an Excercise",
                    isVisible = false,
                    Description = "To choose an excercise First select \"Choose Excercise\" from the main menu. " +
                        "If an exercise has already been created then it will be listed in this page. " +
                        "Tap the exercise you want and you can begin the exercise or tap details to see its specifications. " +
                        "If there are not any exercises to choose from then an exercise has not yet been created. " +
                        "To assign an exercise tap \"Assign an exercise\" at the bottom of the page. This will take you to the manage exercises page.\n",
                    ImageName="SquareLogo.png"
                        },

                new Tutorial { Topic = "Setting the One-Rep-Max",
                    isVisible = false,
                    Description = "How to set the One-Rep-Max",
                    ImageName = "SquareLogo.png"
                    },

                new Tutorial { Topic = "Project Website",
                    isVisible = false,
                    Description = "To Navigate to the Project Website, tap the link below.",
                    URL = "CTAR All-Star Website"
                    }
            };
        }

        public void HideorShowTutorial(Tutorial tutorial)
        {
            if (_oldTutorial == tutorial)
            {
                //hides
                tutorial.isVisible = !tutorial.isVisible;
                UpdateTutorial(tutorial);
            }

            else
            {
                if (_oldTutorial != null)
                {
                    //hide old
                    _oldTutorial.isVisible = false;
                    UpdateTutorial(_oldTutorial);
                }

                //show new
                tutorial.isVisible = true;
                UpdateTutorial(tutorial);
            }

            _oldTutorial = tutorial;
     
        }

        private void UpdateTutorial(Tutorial tutorial)
        {
            var index = Tutorials.IndexOf(tutorial);
            Tutorials.Remove(tutorial);
            Tutorials.Insert(index, tutorial);
        }
    }
}
