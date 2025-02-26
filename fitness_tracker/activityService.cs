using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace fitness_tracker
{
    class activityService
    {
        private readonly fitness_treacker_dbTableAdapters.activity_tblTableAdapter activityAdapter;

        public activityService()
        {
            activityAdapter = new fitness_treacker_dbTableAdapters.activity_tblTableAdapter();
        }

        // Add new activity
        public void AddActivity(string name, string metric1, string metric2, string metric3)
        {
            string newActivityId = GenerateNewActivityId();
            activityAdapter.Insert(newActivityId, name, metric1, metric2, metric3);
        }

        // Update activity
        public void UpdateActivity(string id, string name, string metric1, string metric2, string metric3)
        {
            activityAdapter.UpdateActivity(name, metric1, metric2, metric3, id);
        }

        // Delete activity
        public void DeleteActivity(string id)
        {
            activityAdapter.DeleteActivity(id);
        }

        // Fetch all activities
        public DataTable GetAllActivities()
        {
            return activityAdapter.GetData();
        }

        public string GenerateNewActivityId()
        {
            DataTable activityDt = activityAdapter.GetData();
            if (activityDt.Rows.Count > 0)
            {
                int maxId = activityDt.AsEnumerable()
                                   .Select(row => int.Parse(row.Field<string>("activity_id").Replace("activity-", "")))
                                   .Max();
                return $"activity-{(maxId + 1):D3}";
            }
            return "activity-001";
        }

    }
}
