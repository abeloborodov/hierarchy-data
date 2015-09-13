using System;
using System.Collections.Generic;
using Hierarchy.Common;

namespace HierarchyId.API
{
    public class Program
    {
        private static void Main(string[] args)
        {
           /* using (var api = new HierarchyIdApi())
            {
                //api.Remove(HierarchyId.Parse("/1/2/1/"));

                TreeItem input;
                TreeItem output;
                api.GenerateCustomDataTree(out input, out output);
                var treeFromEditorView = new TreeItem { SubItems = new List<TreeItem> { input, output } };

                api.WriteTree("http://infos", "1.0.0", treeFromEditorView);


                //var tree = api.ReadTree("http://infos", "1.0.0");

                Console.WriteLine("Готово");
                Console.ReadLine();
            }*/
        }
    }
}
