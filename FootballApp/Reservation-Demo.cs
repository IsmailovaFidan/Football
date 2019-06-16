using FootballApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballApp
{
    public partial class Reservation_Demo : Form
    {
        FootballEntities db = new FootballEntities();
        decimal total;
        public Reservation_Demo()
        {
            InitializeComponent();
        }

      

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            string FirstName = txtFirstName.Text;
            string LastName = txtLastName.Text;
            string Phone = txtPhone.Text;
            string PitchName = cmbPitchName.Text;
            string PitchNumber = cmbPitchNumber.Text;
            string RoomNumber = cmbRoomNumber.Text;
            DateTime DateFrom = dateFrom.Value;
            DateTime DateTo = dateTo.Value;
           
        
            int PhoneNumber;

            if (Extention.isNotEmpty(new string[]{
                FirstName,LastName,Phone,PitchName,PitchNumber,RoomNumber
            }, string.Empty)) 
            {
                
                    if (db.Rooms.Any(m => m.Percon_Count <= 12)){
                    if (int.TryParse(Phone, out PhoneNumber))
                    {
                        Room selectedRoom =db.Rooms.First(a => a.Rooms_Number == RoomNumber);
                        Pitch selectedPitch = db.Pitches.First(b => b.Pitch_Name == PitchName);
                        total = selectedRoom.Rooms_Price + selectedPitch.Price;
                      
                        Client selectClient = null;
                        int clientId = 0;

                        Task ClientTask = Task.Factory.StartNew(() =>  
                        {

                            selectClient = db.Clients.Add(new Client
                            {
                                First_Name = FirstName,
                                Last_Name = LastName,
                                Phone = PhoneNumber
                            });
                            db.SaveChanges();

                        });


                        ClientTask.Wait();
                        if (ClientTask.IsCompleted){
                            clientId = selectClient.Id;
                        }
                       
                        Reservation rv = db.Reservations.Add(new Reservation
                        {
                            Reservation_Date = DateFrom,
                            Reservation_Deadline = DateTo,
                            Clients_Id =clientId,
                            Pitch_Id=selectedPitch.Id,
                            Rooms_Id=selectedRoom.Id,
                            Price=(int)total

                        });
                        db.SaveChanges();
                        
                        
                        
                        

                        
                        MessageBox.Show("Reservation was added", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "Please enter correct phone number";
                    }
                }
                    else
                    {
                    lblError.Visible = true;
                    lblError.Text = "This room if full";
                    
                        


                    }
                }
               
            
            else
            {
                lblError.Visible = true;
                lblError.Text = "Please field all input";
            }

        }
        private void Reservation_Demo_Load(object sender, EventArgs e)
        {
            FillcmbPitchName();
            FillcmbPitchNumber();
            FillcmbPitchNumber();
            FillcmbRoomName();
        }
        private void FillcmbRoomName()
        {
            cmbRoomNumber.Items.AddRange(db.Rooms.Select(rm => rm.Rooms_Number).ToArray());
        }
        private void FillcmbPitchName()
        {
            cmbPitchName.Items.AddRange(db.Pitches.Select(pn => pn.Pitch_Name).ToArray());
        }
        private void FillcmbPitchNumber()
        {
            cmbPitchNumber.Items.AddRange(db.Pitches.Select(a => a.Pitch_Number).ToArray());
        }
        private void FillcmbRoomNumber()
        {
            cmbRoomNumber.Items.AddRange(db.Rooms.Select(b => b.Rooms_Number).ToArray());
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CmbRoomNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblPrice.Text = total.ToString();
        }
    }
}
