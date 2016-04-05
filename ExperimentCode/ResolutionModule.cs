using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExperimentCode
{
    class NamespaceEntity
    {
        string SrcName;
        string DstName;
        string Values;
        string Actions;
    }

    class Namespace
    {
        string InputFilter;
        List<NamespaceEntity> EntityList = new List<NamespaceEntity>();
        string DefaultActions;
    }
    class ResolutionModule
    {
    }
}
