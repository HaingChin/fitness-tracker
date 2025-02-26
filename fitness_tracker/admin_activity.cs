using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fitness_tracker
{
    public partial class admin_activity : UserControl
    {
        private readonly activityService activityService;
        public admin_activity()
        {
            InitializeComponent();
            activityService = new activityService();
        }

        private void LoadActivities()
        {
           

            dgv_activities.DataSource = activityService.GetAllActivities();
        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        // Add new activity
        private void btn_add_Click(object sender, EventArgs e)
        {
            string name = txt_activity_name.Text.Trim();
            string metric1 = txt_metric1.Text.Trim();
            string metric2 = txt_metric2.Text.Trim();
            string metric3 = txt_metric3.Text.Trim();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(metric1) ||
                string.IsNullOrWhiteSpace(metric2) || string.IsNullOrWhiteSpace(metric3))
            {
                MessageBox.Show("Please fill all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            activityService.AddActivity(name, metric1, metric2, metric3);
            MessageBox.Show("Activity added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearFields();
            LoadActivities();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if (dgv_activities.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select an activity to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string id = dgv_activities.SelectedRows[0].Cells["activity_id"].Value.ToString();
            string name = txt_activity_name.Text.Trim();
            string metric1 = txt_metric1.Text.Trim();
            string metric2 = txt_metric2.Text.Trim();
            string metric3 = txt_metric3.Text.Trim();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(metric1) ||
                string.IsNullOrWhiteSpace(metric2) || string.IsNullOrWhiteSpace(metric3))
            {
                MessageBox.Show("Please fill all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            activityService.UpdateActivity(id, name, metric1, metric2, metric3);
            MessageBox.Show("Activity updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearFields();
            LoadActivities();
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
        
            if (dgv_activities.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select an activity to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string id = dgv_activities.SelectedRows[0].Cells["activity_id"].Value.ToString();

            DialogResult confirm = MessageBox.Show("Are you sure you want to delete this activity?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                activityService.DeleteActivity(id);
                MessageBox.Show("Activity deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadActivities();
            }
        }


        // Load activities into DataGridView on form load
        private void admin_activity_Load(object sender, EventArgs e)
        {
            LoadActivities();
            txt_activity_id.Text = activityService.GenerateNewActivityId();
            

        }

        // Clear input fields
        private void ClearFields()
        {
            txt_activity_name.Clear();
            txt_metric1.Clear();
            txt_metric2.Clear();
            txt_metric3.Clear();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
           ClearFields();
        }

        
    }
}
