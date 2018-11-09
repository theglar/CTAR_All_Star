using System;
using System.Collections.Generic;
using System.Text;

namespace CTAR_All_Star.Model
{
    public class ViewModel  
{
      public List<Person> Data { get; set; }      

      public ViewModel()       
      {
            Data = new List<Person>()
            {
                new Person { Name = "David", Height = 180 },
                new Person { Name = "Michael", Height = 176 },
                new Person { Name = "Steve", Height = 160 },
                new Person { Name = "Jamie", Height = 145 },
                new Person { Name = "Ed", Height = 165 },
                new Person { Name = "Emily", Height = 150 },
                new Person { Name = "Joel", Height = 182 }
            }; 
       }
 }
}
