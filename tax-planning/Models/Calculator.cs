using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tax_planning.Models
{
    public class Calculator
    {
        public static Dictionary<string, Table> GetTablesFor(FormModel model)
        {
            return new Dictionary<string, Table>()
            {
                { "desired", model.Assets.GetDesiredScheduleFor(model) },
                { "optimal", model.Assets.GetOptimalScheduleFor(model) }
            };
        }
    }
}
