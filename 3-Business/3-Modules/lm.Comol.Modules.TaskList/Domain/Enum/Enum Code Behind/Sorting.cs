using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.TaskList.Domain
{
    [CLSCompliant(true)]
    public enum Sorting
    {
        None= 0,
        DeadlineOrder=1,
        AlphabeticalOrder=2
    }
}
