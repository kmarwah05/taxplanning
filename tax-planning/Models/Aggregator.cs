using System.Collections.Generic;
using tax_planning.Extensions;

namespace tax_planning.Models
{
    public class Aggregator
    {
        public static Dictionary<string, Table> GetTablesFor(Data data)
        {
            Dictionary<string, Table> tables = new Dictionary<string, Table>()
            {
                { "Overall Desired", data.Assets.GetDesiredScheduleFor(data) },
                { "Overall Optimal", data.Assets.GetOptimalScheduleFor(data) }
            };

            foreach (var asset in data.Assets)
            {
                tables.Add(asset.Name + " Desired", asset.GetDesiredScheduleFor(data));
                tables.Add(asset.Name + " Optimal", asset.GetOptimalScheduleFor(data));
            }

            return tables;
        }
    }
}
