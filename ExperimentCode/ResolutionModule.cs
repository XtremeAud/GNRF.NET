using System;
using System.Collections.Generic;

namespace ExperimentCode
{
    public class NamespaceEntity
    {
        public string SrcName;
        public string DstName;
        public string Values;
        public string Actions;

        public NamespaceEntity(string SrcName, string DstName, string Values, string Actions)
        {
            this.SrcName = SrcName;
            this.DstName = DstName;
            this.Values = Values;
            this.Actions = Actions;
        }
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

        public static void InsertNamespaceEntity(string NSName, string SrcName, string DstName, string Values, string Actions)
        {
            Namespaces.NamespaceList[FindNS(NSName)].EntityList.Add(new NamespaceEntity(SrcName, DstName, Values, Actions));
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

        public static void ListNamespace()
        {
            string Line = "";
            Console.WriteLine("Listing All the Namespaces:Index | Name | Input Filter | Default Actions");
            for (int n = 0; n < Namespaces.NamespaceList.Count; n++)
            {
                Console.WriteLine("_____________________________________________________________");
                Line = n.ToString() + " | " + Namespaces.NamespaceList[n].NSName + " | " + Namespaces.NamespaceList[n].InputFilter +
                    " | " + Namespaces.NamespaceList[n].DefaultActions + " |";
                Console.WriteLine(Line);
            }
        }

        public static void ListNamespaceEntities(int IndexofNamespace)
        {
            string Line = "";
            Console.WriteLine("Listing All the Entities:Index | SrcName | DstName | Values | Actions");
            for (int n = 0; n < Namespaces.NamespaceList[IndexofNamespace].EntityList.Count; n++)
            {
                Console.WriteLine("_____________________________________________________________");
                Line = n.ToString() + " | " + Namespaces.NamespaceList[IndexofNamespace].EntityList[n].SrcName + " | " + Namespaces.NamespaceList[IndexofNamespace].EntityList[n].DstName +
                    " | " + Namespaces.NamespaceList[IndexofNamespace].EntityList[n].Values + " | " + Namespaces.NamespaceList[IndexofNamespace].EntityList[n].Actions + " |";
                Console.WriteLine(Line);
            }
        }

        public static void DeleteNamespace(int IndexofNamespace)
        {
            Namespaces.NamespaceList.RemoveAt(IndexofNamespace);
        }

        public static void DeleteEntity(int IndexofNamespace, int IndexofEntity)
        {
            Namespaces.NamespaceList[IndexofNamespace].EntityList.RemoveAt(IndexofEntity);
        }
    }
}
