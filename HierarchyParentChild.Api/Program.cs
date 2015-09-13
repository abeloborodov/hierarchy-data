using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HierarchyParentChild.Api.EF;

namespace HierarchyParentChild.Api
{
    internal class Program
    {
        private static IEnumerable<Subservice> CreateSubservices(int subsNum,int versNum)
        {
            var subs = new List<Subservice>();
            using (var context = new HierarchyParentChildContext())
            {
                for (int i = 0; i < subsNum; i++)
                {
                    var id = Guid.NewGuid();
                    var subservice = new Subservice()
                    {
                        Id = id,
                        Namespace = "namespace " + id,
                        SubserviceVersions = new Collection<SubserviceVersion>()
                    };
                    for (int j = 0; j < versNum; j++)
                    {
                        subservice.SubserviceVersions.Add(new SubserviceVersion
                        {
                            Id = Guid.NewGuid(),
                            Version = j+1 + ".0.0"
                        });
                    }

                    context.Subservices.Add(subservice);
                    subs.Add(subservice);

                }
                context.SaveChanges();
            }
            return subs;
        }

        private static void Main(string[] args)
        {
            var subserviceList = CreateSubservices(1, 2);
            foreach (var subservice in subserviceList)
            {
                foreach (var version in subservice.SubserviceVersions)
                {
                    Console.WriteLine("Generate Tree");
                    using (var editor = new ParentChildApi())
                    {
                        var tree0 = editor.GenerateTteeVm(4, 5, version.Version);
                        editor.TreePrint(tree0);
                        editor.WriteTree(version.Id, tree0);
                    }
                    using (var operatr = new ParentChildApi())
                    {
                        Console.WriteLine();
                        Console.WriteLine("Read Tree");
                        var tree1 = operatr.ReadTree(version.Id);
                        operatr.TreePrint(tree1);
                    }
                }
            }
            Console.ReadKey();
            new ParentChildApi().DeleteData();


//            var versions = new Context().SubserviceVersions.Where(x => x.Version != null);
//
//            foreach (var version in versions)
//            {
//                Console.WriteLine("Generate Tree");
//                using (var editor = new ApiParentChild())
//                {
//                    var tree0 = editor.GenerateTteeVm(4, 5, version.Version);
//                    editor.TreePrint(tree0);
//                    editor.SaveTree(tree0, version.Id);
//                }
//                using (var operatr = new ApiParentChild())
//                {
//                    Console.WriteLine();
//                    Console.WriteLine("Read Tree");
//                    var tree1 = operatr.GetTreeVm(version.Id);
//                    operatr.TreePrint(tree1);
//                }
//                break;
//            }
//
//
//            Console.ReadKey();
//            new ApiParentChild().DeleteData();
        }

        //var tree0 = Api.TestTreeVm();
//            var versions = ApiParentChild.GetContext().SubserviceVersions.Where(x => x.Version!=null);
//            foreach (var version in versions)
//            {
//                Console.WriteLine("Generate Tree");
//                var tree0 = ApiParentChild.GenerateTteeVm(4, 5, version.Version);
//                ApiParentChild.TreePrint(tree0);
//                ApiParentChild.SaveTree(tree0, version.Id);
//
//                Console.WriteLine();
//                Console.WriteLine("Read Tree");
//                var tree1 = ApiParentChild.GetTreeVm(version.Id);
//                ApiParentChild.TreePrint(tree1);
//                break;
//            }
//            
//            
//            Console.ReadKey();
//            ApiParentChild.DeleteData();
    }
}

