using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;


namespace BluePrintTechnical
{
    class Employee
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string UnitName { get; set; }

    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Blue Print Technical Assesment");
            Console.WriteLine("------------------------------------------------------------------\n");

            int option = 0 ;

            //xml file path
            String URLString = "../../organization.xml";
            //json file path
            String path = "../../updatedOrganization.json";


            //Read xml file
            XmlTextReader reader = new XmlTextReader(URLString);
            //create list to store value from xml file
            List<Employee> ListEmp = new List<Employee>();
            List<Employee> ListEmpUpdated = new List<Employee>();
       
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Employee")
                {
                    string title = (reader.GetAttribute("Title"));

                    Employee emp = new Employee();

                    emp.Name = reader.ReadString();
                    emp.Title = title;
                    emp.UnitName = "";
                    //add object value to list
                    ListEmp.Add(emp);
                }

                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Unit")
                    {
                        //save valut of unit name in variable
                         String UnitName = reader.GetAttribute("Name");

                        //read child element of unit block
                        XmlReader r = reader.ReadSubtree();


                        while (r.Read())
                        {
                            //if child element contain any unit then update unit name
                            if (r.Name == "Unit")
                            {
                                UnitName = reader.GetAttribute("Name");
                            }
                            //search employee tag and save its value in object
                            if (r.NodeType == XmlNodeType.Element && r.Name == "Employee")
                            {
                                string title = (r.GetAttribute("Title"));

                                Employee emp = new Employee();

                                emp.Name = r.ReadString();
                                emp.Title = title;
                                emp.UnitName = UnitName;
                                //add object value to list
                                ListEmp.Add(emp);
                            }
                        }

                    
                }
                   

            }


            //User options

            do
            {
                Console.WriteLine("Choose an option from the following list:");
                Console.WriteLine("\t1- List All Employee");
                Console.WriteLine("\t2- Swap Employees and Generate Json File");
                Console.WriteLine("\t3- Exit");

                Console.Write("Your option? ");
                try
                {
                    switch (Convert.ToInt32(Console.ReadLine()))
                    {
                        case 1:
                            //print all value from xml file
                            Console.WriteLine("Name \t\t\t Title \t\t\t\t Unit Name");
                            Console.WriteLine("===================================================================");
                            foreach (var e in ListEmp)
                            {
                                Console.WriteLine(e.Name + "\t\t " + e.Title + "\t\t\t " + e.UnitName);

                            }
                            break;
                        case 2:
                            //update department from created array and store it in json filie
                            ListEmpUpdated = ListEmp;

                            foreach (var e in ListEmpUpdated)
                            {
                                if (e.UnitName == "Platform Team")
                                {
                                    e.UnitName = "update";
                                }
                                if (e.UnitName == "Maintenance Team")
                                {
                                    e.UnitName = "Platform Team";
                                }
                                if (e.UnitName == "update")
                                {
                                    e.UnitName = "Maintenance Team";
                                }
                            }

                            var output = JsonConvert.SerializeObject(ListEmpUpdated);

                            Console.WriteLine("Output from Json File :");
                            Console.WriteLine("Name \t\t\t Title \t\t\t\t Unit Name");
                            Console.WriteLine("===================================================================");
                            foreach (var e in ListEmpUpdated)
                            {
                                Console.WriteLine(e.Name + "\t\t " + e.Title + "\t\t\t " + e.UnitName);

                            }

                            File.WriteAllText(path, output);
                            break;
                        case 3:
                            option = 3;
                            break;
                        default:
                            Console.WriteLine("Invalid Option Selected");
                            break;
                    }

                }
                catch (FormatException)
                {
                    Console.WriteLine("Please enter valid menu options");
                }
            } while (option != 3);

              

            // Wait for the user to respond before closing.
            Console.Write("Press any key to close the console app...");
            Console.ReadKey();
        }
    }
}