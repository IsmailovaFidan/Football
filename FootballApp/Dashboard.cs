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
    public partial class Dashboard : Form
    {
        FootballEntities db = new FootballEntities();
        Pitch selecetedPitch;
        Room selectedRoom;
        public Dashboard()
        {
            InitializeComponent();
        }
        //Reservation Form
        private void BtnReservation_Click(object sender, EventArgs e)
        {
            ToggleInput("Reservation");
            panelReservation.Visible = true;
            panelPitch.Visible = false;
            PanelRoom.Visible = false;
            panelClient.Visible = false;
            FilldtgViewReservation();
        }
        private void BtnAddReservation_Click(object sender, EventArgs e)
        {
            Reservation_Demo rvDemo = new Reservation_Demo();
            rvDemo.ShowDialog();
        }
        //Method for Reservation
        private void FilldtgViewReservation()
        {
            dtgViewReservation.DataSource=db.Reservations.Select(a=>new
            {
                FirstName=a.Client.First_Name,
                LastName=a.Client.Last_Name,
                Phone=a.Client.Phone,
                PitchName=a.Pitch.Pitch_Name,
                PitchNumber=a.Pitch.Pitch_Number,
                RoomsNumber=a.Room.Rooms_Number,
                Date=a.Reservation_Date,
                Price=a.Price
            }).ToList();
        }



        //Pitchs Form
        private void BtnPitchs_Click(object sender, EventArgs e)
        {      
            FilldtgViewPitches();
            ToggleInput("Pitchs");
            panelPitch.Visible = true;
            panelReservation.Visible = false;
            PanelRoom.Visible = false;
            panelClient.Visible = false;
        }
        //Method for Pitches
        private void FilldtgViewPitches()
        {           
            dtgViewPitchs.DataSource = db.Pitches.Where(st=>st.Status==1).Select(a => new
            {
                a.Id,
                Name = a.Pitch_Name,
                Number = a.Pitch_Number,
                Price = a.Price,
                Mode = a.Status == 0 ? "Empty" : "Full"

            }).ToList();
            dtgViewPitchs.Columns[0].Visible = false;
        }
        private void ChangeMode(string mode)
        {
            if (mode == "edit")
            {
                btnAddPitch.Visible = false;
                btnPitchEdit.Visible = true;
                btnPitchDelete.Visible = true;
            }
            else
            {
                btnAddPitch.Visible = true;
                btnPitchEdit.Visible = false;
                btnPitchDelete.Visible = false;
            }
        }
        private void BtnAddPitch_Click_1(object sender, EventArgs e)
        {
            string PitchName = txtPitchName.Text;
            string PitchNumber = txtPitchNumber.Text;
            decimal PitchPrice = numPitchPrice.Value;
            if (PitchName != string.Empty && PitchNumber != string.Empty && PitchPrice != 0)
            {
                Pitch pt = null;
                pt = new Pitch
                {                    
                    Pitch_Name = PitchName,
                    Pitch_Number = PitchNumber,
                    Price = PitchPrice,
                    Status = 0
                };
                db.Pitches.Add(pt);
                db.SaveChanges();
                MessageBox.Show("Pitch was added successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FilldtgViewPitches();
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "Please filled all input";
            }
            
        }
        private void DtgViewPitchs_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int pitchId = (int)dtgViewPitchs.Rows[e.RowIndex].Cells[0].Value;
            selecetedPitch = db.Pitches.FirstOrDefault(pt => pt.Id == pitchId);
            txtPitchName.Text = selecetedPitch.Pitch_Name;
            txtPitchNumber.Text = selecetedPitch.Pitch_Number;
            numPitchPrice.Value = selecetedPitch.Price;
            ChangeMode("edit");
        }
        private void BtnPitchEdit_Click(object sender, EventArgs e)
        {
            string PitchName = txtPitchName.Text;
            string PitchNumber = txtPitchNumber.Text;
            decimal Price = numPitchPrice.Value;
            if (selecetedPitch != null)
            {
                if (PitchName != string.Empty && PitchNumber != string.Empty && Price != 0)
                {
                    selecetedPitch.Pitch_Name = txtPitchName.Text;
                    selecetedPitch.Pitch_Number = txtPitchNumber.Text;
                    selecetedPitch.Price = numPitchPrice.Value;
                    db.SaveChanges();
                    FilldtgViewPitches();
                    ChangeMode("add");
                }
            }

        }
        private void BtnPitchDelete_Click(object sender, EventArgs e)
        {
            string PitchName = txtPitchName.Text;
            string PitchNumber = txtPitchNumber.Text;
            decimal Price = numPitchPrice.Value;
            if (selecetedPitch != null)
            {
                if(PitchName!=string.Empty && PitchNumber!=string.Empty && Price != 0)
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to delete " + PitchName + "?", "Infomation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {
                       
                      
                        selecetedPitch.Status = 0;
                        db.SaveChanges(); 
                        ChangeMode("add");
                        FilldtgViewPitches();
                        txtPitchName.Text = " ";
                        txtPitchNumber.Text = " ";
                        numPitchPrice.Value = 0;
                    }
                }
            }
        }






        //Room Form
        private void BtnRooms_Click(object sender, EventArgs e)
        {
            ToggleInput("Rooms");
            FilldtgViewRooms();
            PanelRoom.Visible = true;
            panelPitch.Visible = false;
            panelReservation.Visible = false;
            panelClient.Visible = false;

        }
        private void BtnAddRoom_Click(object sender, EventArgs e)
        {
            string RoomNumber = txtRoomNumber.Text;
            decimal PersonCount = numPersonCount.Value;
            decimal RoomsPrise = numRoomPrice.Value;
            if(RoomNumber!=string.Empty && PersonCount != 0 && RoomsPrise!=0)
            {
                Room rm = null;
                rm = new Room
                {
                    Rooms_Number = txtRoomNumber.Text,
                    Percon_Count = (int)numPersonCount.Value,
                    Rooms_Price=(int)numRoomPrice.Value,
                    Status=0
                };
                db.Rooms.Add(rm);
                db.SaveChanges();
                MessageBox.Show("Room was added successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FilldtgViewRooms();
            }
        }
        //Method for Rooms
        private void FilldtgViewRooms()
        {
            dtgViewRoom.DataSource = db.Rooms.Where(st=>st.Status==1).Select(r => new
            {
                r.Id,
                r.Rooms_Number,
                r.Percon_Count,
                r.Rooms_Price,
                Mode = r.Status == 0 ? "Empty" : "Full"
            }).ToList();
            dtgViewRoom.Columns[0].Visible = false;

        }
        private void ChangeModeRoom(string mode)
        {
            if (mode == "edit")
            {
                btnAddRoom.Visible = false;
                btnEditRoom.Visible = true;
                btnDeleteRoom.Visible = true;
            }
            else
            {
                btnAddRoom.Visible = true;
                btnEditRoom.Visible = false;
                btnDeleteRoom.Visible = false;
            }
        }
        private void DtgViewRoom_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int roomid = (int)dtgViewRoom.Rows[e.RowIndex].Cells[0].Value;
            selectedRoom = db.Rooms.FirstOrDefault(rm => rm.Id == roomid);
            txtRoomNumber.Text = selectedRoom.Rooms_Number;
            numPersonCount.Value = selectedRoom.Percon_Count;
            ChangeModeRoom("edit");

        }
        private void BtnEditRoom_Click(object sender, EventArgs e)
        {
            string roomNumber = txtRoomNumber.Text;
            decimal count = numPersonCount.Value;
            decimal rmprice = numRoomPrice.Value;
            if (selectedRoom != null)
            {
                if(roomNumber!=string.Empty && count != 0 && rmprice!=0)
                {
                    selectedRoom.Rooms_Number = roomNumber;
                    selectedRoom.Percon_Count = (int)count;
                    selectedRoom.Rooms_Price = (int)rmprice;
                    db.SaveChanges();
                    FilldtgViewRooms();
                    ChangeModeRoom("add");
                }
            }
        }
        private void BtnDeleteRoom_Click(object sender, EventArgs e)
        {
            string roomNum = txtRoomNumber.Text;
            decimal count = numPersonCount.Value;
            decimal rmprice = numRoomPrice.Value;
            if (selectedRoom != null)
            {
                if(roomNum!=null && count != 0 && rmprice != 0)
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to delete " + roomNum + "?", "Infomation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {
                        selectedRoom.Status = 0;
                        db.SaveChanges();
                        ChangeModeRoom("add");
                        FilldtgViewRooms();
                        txtRoomNumber.Text = "";
                        numPersonCount.Value = 0;
                        numRoomPrice.Value = 0;


                    }
                }
            }
        }







        //Clients Form
        private void BtnClients_Click(object sender, EventArgs e)
        {
            ToggleInput("Clients");
            panelClient.Visible = true;
            panelPitch.Visible = false;
            panelReservation.Visible = false;
            PanelRoom.Visible = false;
        }

        //Metodlar
        private void ToggleInput(string mode) //Buttons' designs
        {
            if (mode=="Reservation")
            {
                panelReservationDesign.Visible = true;
                panelPitchDesign.Visible = false;
                panelRoomDesign.Visible = false;
                panelClientDesign.Visible = false;
            }
            else if(mode == "Pitchs")
            {
                panelPitchDesign.Visible = true;
                panelRoomDesign.Visible = false;
                panelClientDesign.Visible = false;
                panelReservationDesign.Visible = false;
            }
            else if (mode == "Rooms")
            {
                panelRoomDesign.Visible = true;
                panelPitchDesign.Visible = false;
                panelClientDesign.Visible = false;
                panelReservationDesign.Visible = false;
            }
            else if (mode == "Clients")
            {
                panelClientDesign.Visible = true;
                panelReservationDesign.Visible = false;
                panelPitchDesign.Visible = false;
                panelRoomDesign.Visible = false;
            }
        }
        

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            panelPitch.Visible = false;
            FilldtgViewReservation();

        }

       
    }
}
