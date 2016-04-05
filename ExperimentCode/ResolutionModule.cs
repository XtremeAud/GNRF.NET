using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExperimentCode
{
    public class NamespaceEntity
    {
        public string SrcName;
        public string DstName;
        public string Values;
        public string Actions;
    }

    public class Namespace
    {
        public string NSName;
        public string InputFilter;
        public List<NamespaceEntity> EntityList = new List<NamespaceEntity>();
        public string DefaultActions;

        public Namespace(string NSName, string InputFilter, string DefaultActions)
        {
            this.NSName = NSName;
            this.InputFilter = InputFilter;
            this.DefaultActions = DefaultActions;
        }

    }

    public static class Namespaces
    {
        public static List<Namespace> NamespaceList = new List<Namespace>();
    }

    class ResolutionModule
    {
        public static void CreatNS(string NSName, string InputFilter, string DefaultAcitons)
        {
            if (FindNS(NSName) != -1)
            {
                Namespaces.NamespaceList.Add(new Namespace(NSName, InputFilter, DefaultAcitons));
            }
        }

        public static int FindNS(string NSName)
        {
            int Index = -1;
            for (int index = 0; index < Namespaces.NamespaceList.Count; index++)
            {
                if (Namespaces.NamespaceList[index].NSName == NSName)
                {
                    return index;
                }
            }
            return Index;
        }
    }
}
