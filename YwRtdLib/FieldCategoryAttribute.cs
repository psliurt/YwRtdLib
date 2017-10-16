using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YwRtdLib
{
    public class FieldCategoryAttribute :Attribute
    {
        public YwFieldGroup Group { get; set; }

        public FieldCategoryAttribute(YwFieldGroup group)
        {
            this.Group = group;
        }        
    }

    [Flags]
    public enum YwFieldGroup
    {
        NotSpecific = 0,
        LastNightFix = 1,
        OpenInitOnce = 2,
        OpenSimulate = 4,
        IntraChange = 8,
        FoundSpec = 16,
        OptionSpec = 32,
        BestFive = 64,
        All = 1024
    }
}
