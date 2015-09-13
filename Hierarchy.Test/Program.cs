using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Hierarchy.Common;
using HierarchyId.API.BreadthFirst;
using HierarchyParentChild.Api;
using HierarchyParentChild.Api.EF;
//using HierarchyIdSubservice = HierarchyId.API.Subservice;
//using HierarchyIdSubserviceVersion = HierarchyId.API.SubserviceVersion;
using HierarchyIdSubservice = HierarchyId.API.BreadthFirst.Subservice;
using HierarchyIdSubserviceVersion = HierarchyId.API.BreadthFirst.SubserviceVersion;
using SubservicePc = HierarchyParentChild.Api.EF.Subservice;
using SubserviceVersionPc = HierarchyParentChild.Api.EF.SubserviceVersion;


namespace Hierarchy.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Guid subserviceId = Guid.NewGuid();
            Guid versionId1 = Guid.Parse("9F6C1435-C758-449B-865E-E2B304D172E6");
            Guid versionId2 = Guid.Parse("9E17B518-A14D-40B8-B13A-34B199338627");
            string versionNumber1 = "1.0.0", versionNumber2 = "1.1.1";
            string ns = "Http://asdf";

            #region GenDatas
            #region HierarchyId
            //            using (var context = new HierarchyIdDbContext())
            //            {
            //                context.Subservices.Add(new HierarchyIdSubservice()
            //                {
            //                    Id = subserviceId,
            //                    Namespace = ns,
            //                    SubserviceVersions =
            //                        new Collection<HierarchyIdSubserviceVersion>()
            //                        {
            //                            new HierarchyIdSubserviceVersion()
            //                            {
            //                                Id = versionId1,
            //                                SubserviceId = subserviceId,
            //                                Version = versionNumber1
            //                            },
            //                            new HierarchyIdSubserviceVersion()
            //                            {
            //                                Id = versionId2,
            //                                SubserviceId = subserviceId,
            //                                Version = versionNumber2
            //                            }
            //                        }
            //                });
            //                context.SaveChanges();
            //            }
            //            return;
            #endregion

            #region HierarchyParentChild
            //            using (var context = new HierarchyParentChildContext())
            //            {
            //                
            //
            //                context.Subservices.Add(new SubservicePc()
            //                {
            //                    Id = subserviceId,
            //                    Namespace = ns,
            //                    SubserviceVersions =
            //                        new Collection<SubserviceVersionPc>()
            //                        {
            //                            new SubserviceVersionPc()
            //                            {
            //                                Id = versionId1,
            //                                SubserviceId = subserviceId,
            //                                Version = versionNumber1
            //                            },
            //                            new SubserviceVersionPc()
            //                            {
            //                                Id = versionId2,
            //                                SubserviceId = subserviceId,
            //                                Version = versionNumber2
            //                            }
            //                        }
            //                });
            //                context.SaveChanges();
            //            }
            //            return;
            #endregion
            #endregion

            _maxLevel = 4;
            _subAttributesCount = 10;
            _subBlocksCount = 7;
            var expectedTree1 = GenerateTestTree(string.Empty, 0, string.Empty);
            Console.WriteLine("кол атрибутов = " + _attrCount);
            expectedTree1.Name = versionNumber1;
            /* _maxLevel = 3;
             _subAttributesCount = 3;
             _subBlocksCount = 3;
             var expectedTree2 = GenerateTestTree(string.Empty, 0, string.Empty);
             expectedTree2.Name = versionNumber2;*/


            using (var api = new HierarchyIdApi())
            //using (var api = new ParentChildApi())
            {
                var watchGlobal = Stopwatch.StartNew();
                var watch = Stopwatch.StartNew();
                api.WriteTree(versionId1, expectedTree1);
                watch.Stop();
                Console.WriteLine(watch.ElapsedMilliseconds);

                /*watch = Stopwatch.StartNew();
                api.WriteTree(versionId2, expectedTree2);
                watch.Stop();
                Console.WriteLine(watch.ElapsedMilliseconds);*/

                TreeItem retreived1 = null;
                watch = Stopwatch.StartNew();
                retreived1 = api.ReadTree(versionId1);
                watch.Stop();
                watchGlobal.Stop();
                Console.WriteLine("read: " + watch.ElapsedMilliseconds);

                Console.WriteLine("Global = " + watchGlobal.ElapsedMilliseconds);

                /*watch = Stopwatch.StartNew();
                var retreived2 = api.ReadTree(versionId2);
                watch.Stop();
                Console.WriteLine(watch.ElapsedMilliseconds);*/

                var r1 = CompareTrees(expectedTree1, retreived1);
                /*var r2 = CompareTrees(expectedTree2, retreived2);*/

                Console.WriteLine(r1.Equals);
                /*Console.WriteLine(r2.Equals);*/
            }


            Console.WriteLine("Готово");
            Console.ReadLine();
        }



        private static int _maxLevel = 3;
        private static int _subAttributesCount = 2;
        private static int _subBlocksCount = 1;
        private static int _attrCount = 0;

        private static TreeItem GenerateTestTree(string parentName, int level, string index)
        {
            var current = new TreeItem { Name = parentName + index + "/" };
            current.Order = string.IsNullOrEmpty(index) ? 0 : int.Parse(index);
            if (level == _maxLevel)
            {
                //атрибут
                current.Placeholder = current.Name + "Placeholder";
                _attrCount++;
            }
            else
            {
                current.SubItems = new List<TreeItem>();

                int childNumber;
                if (level == 0)
                {
                    //уровень узлов input output
                    current.IsChoice = true;
                    childNumber = 2;
                }
                else
                {
                    childNumber = _subBlocksCount;

                    for (int i = childNumber + 1; i <= childNumber + _subAttributesCount; i++)
                    {
                        var path = current.Name + i + "/";
                        current.SubItems.Add(new TreeItem
                        {
                            Name = path,
                            Order = i,
                            Placeholder = path + "Placeholder"
                        });
                        _attrCount++;
                    }
                }
                for (int i = 1; i <= childNumber; i++)
                {
                    current.SubItems.Add(GenerateTestTree(current.Name, level + 1, i.ToString()));
                }
                current.SubItems.Sort(new TreeNodeComparer());
            }
            return current;
        }

        private static Result CompareTrees(TreeItem tree1, TreeItem tree2)
        {
            if (tree1.Name != tree2.Name) return new Result(false, tree1, tree2);
            if (tree1.Order != tree2.Order) return new Result(false, tree1, tree2);
            if (tree1.SubItems == null)
            {
                if (tree2.SubItems != null) return new Result(false, tree1, tree2);
                if (tree1.Placeholder != tree2.Placeholder) return new Result(false, tree1, tree2);
            }
            else
            {
                if (tree2.SubItems == null) return new Result(false, tree1, tree2);
                if (tree1.SubItems.Count != tree2.SubItems.Count) return new Result(false, tree1, tree2);
                for (int i = 0; i < tree1.SubItems.Count; i++)
                {
                    var r = CompareTrees(tree1.SubItems[i], tree2.SubItems[i]);
                    if (!r.Equals) return r;
                }
            }
            return new Result(true, null, null);
        }
    }

    public class Result
    {
        public Result(bool @equals, TreeItem left, TreeItem right)
        {
            Equals = @equals;
            Left = left;
            Right = right;
        }

        public bool Equals { get; set; }
        public TreeItem Left { get; set; }
        public TreeItem Right { get; set; }
    }
}
