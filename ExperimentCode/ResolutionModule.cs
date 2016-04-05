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

    public class Namespace
    {
        string NSName;
        string InputFilter;
        List<NamespaceEntity> EntityList = new List<NamespaceEntity>();
        string DefaultActions;
    }

    public static class Namespaces
    {
        public static List<Namespace> NamespaceList = new List<Namespace>();
    }

    class ResolutionModule
    {
        public static void CreatNS()
        {
            ;
        }
    }
}
