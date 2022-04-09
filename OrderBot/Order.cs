using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Text;

namespace OrderBot
{
    public class Order
    {
        private string _machinetype;

        public List<string> _foodProducts=new List<string>();
        public List<string> _qualityCriteria=new List<string>();
        private string _appointmentDate;
        private string _appointmentID;

        public string AppointmentID{
            get => _appointmentID;
            set => _appointmentID = value;
        }

        public string AppointmentDate{
            get => _appointmentDate;
            set => _appointmentDate = value;
        }

        public string MachineType{
            get => _machinetype;
            set => _machinetype = value;
        }

        public void Save(){
            StringBuilder food = new StringBuilder();
                                foreach (string s in _foodProducts)
                                {
                                    food.Append(" "+s);
                                }
                StringBuilder quality = new StringBuilder();
                                foreach (string s in _qualityCriteria)
                                {
                                    quality.Append(" "+s);
                                }
           using (var connection = new SqliteConnection(DB.GetConnectionString()))
            {
                connection.Open();
                    var commandInsert = connection.CreateCommand();
                    commandInsert.CommandText =
                    @"
                        INSERT INTO Orders(appointmentId, appointmentDate, machineType, foodProducts, quality)
                        VALUES($appointmentId, $appointmentDate, $machineType, $foodProducts, $quality)
                    ";
                    commandInsert.Parameters.AddWithValue("$appointmentId", AppointmentID);
                    commandInsert.Parameters.AddWithValue("$appointmentDate", AppointmentDate);
                    commandInsert.Parameters.AddWithValue("$machineType", MachineType);
                    commandInsert.Parameters.AddWithValue("$foodProducts", food.ToString());
                    commandInsert.Parameters.AddWithValue("$quality", quality.ToString());
                    int nRowsInserted = commandInsert.ExecuteNonQuery();

                
            }

        }
    }
}
