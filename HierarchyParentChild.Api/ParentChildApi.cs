using System;
using System.Collections.Generic;
using System.Linq;
using Hierarchy.Common;
using HierarchyParentChild.Api.EF;

namespace HierarchyParentChild.Api
{
    public class ParentChildApi : IDisposable, IHierarchyApi
    {
        public readonly HierarchyParentChildContext HierarchyParentChildContext; 
        public ParentChildApi()
        {
            HierarchyParentChildContext = new HierarchyParentChildContext();
        }
        #region Context

        

//        public static Context GetContext()
//        {
//            return _context = new Context();
//        }

        #endregion

        #region Generate TreeVm

        

        private int OrderInc;

        private List<TreeItem> GenerateTteeItemList(int col, int depth,string version)
        {
            var elements = new List<TreeItem>();
            if (depth-- > 0)
            {
                for (int i = 0; i < col; i++)
                {
                    var el = new TreeItem {/*Id=Guid.NewGuid(),*/Order = OrderInc++};
                    if (i < col/2)
                    {
                        el.Name = "block" + OrderInc + "   " + version;
                        el.SubItems = GenerateTteeItemList(col, depth, version);
                    }
                    else
                    {
                        el.Name = "element" + OrderInc + "   " + version;
                        el.Placeholder = "placeholder";
                    }
                    
                    elements.Add(el);

                }
//                for (int i = 0; i < col; i++)
//                {
//                    var el = new TreeItem()
//                    {
//                        /*Id = Guid.NewGuid(),*/
//                        Name = "element" + OrderInc + "   " + version,
//                        Placeholder = "placeholder",
//                        Order = OrderInc
//                    };
//                    ++OrderInc;
//                    elements.Add(el);
//                }
            }
            return elements;
        }

        public TreeItem GenerateTteeVm(int col, int depth, string version)
        {
            OrderInc = 0;
            var item = new TreeItem() { Name = "root " + version };
            item.SubItems = GenerateTteeItemList(col, depth,version);
            return item;
        }

        #endregion


        #region Save to DB

        public void WriteTree(Guid versionId, TreeItem item)
        {
            OrderInc = 0;
//            using (var context = _context)
            {
                //foreach (var element in item.SubItems.OrderBy(x => x.Order))
                {
                    var root = Dive(item, null, versionId);
                    HierarchyParentChildContext.Elements.Add(root);
                    HierarchyParentChildContext.SaveChanges();
                }
            }
        }

        private Element Dive(TreeItem treeItem, Element parent, Guid versionId)
        {
            //Element root;
            Element element = new Element
            {
                Id = Guid.NewGuid(),
                EnglishName = treeItem.Name,
                RussianName = treeItem.Name,
                IsChoice = treeItem.IsChoice,
                SubserviceVersionId = versionId,
                Order = treeItem.Order
            };
            if (parent != null)
            {
                parent.Children.Add(new TreeElement()
                {
                    Id = Guid.NewGuid(),
                    Child = element
                });
            }

            if (treeItem.SubItems != null && treeItem.SubItems.Count != 0)
            {
                //блок
                foreach (var subItem in treeItem.SubItems.OrderBy(x => x.Order))
                {
                    if (!string.IsNullOrEmpty(subItem.Placeholder))
                    {
                        element.AttributeMetadatas.Add(new AttributeMetadata()
                        {
                            Id = Guid.NewGuid(),
                            Name = subItem.Name,
                            Placeholder = subItem.Placeholder,
                            Order = subItem.Order
                        });
                    }
                    else
                    {
                        Dive(subItem, element, versionId);
                    }
                }
            }
            else
            {
                //атрибут
                if (!string.IsNullOrEmpty(treeItem.Placeholder))
                {
                    element.AttributeMetadatas.Add(new AttributeMetadata()
                    {
                        Id = Guid.NewGuid(),
                        Name = treeItem.Name,
                        Placeholder = treeItem.Placeholder,
                        Order = treeItem.Order
                    });
                }
            }
            return element; //root
        }


        #endregion


        


        #region Get TreeVm from DB
        
        public TreeItem ReadTree(Guid versionId)
        {
            var root = HierarchyParentChildContext.Elements.Single(x => x.Parents.Count == 0 && x.SubserviceVersionId == versionId);
            return GetTreeItem(root);
        }
        private TreeItem GetTreeItem(Element element)
        {
            var treeItem = new TreeItem()
            {
                //                    Id = child.Id,
                Name = element.EnglishName,
                IsChoice = element.IsChoice,
                Order = element.Order,
                SubItems = new List<TreeItem>(),
            };
            if (element.Children != null && element.Children.Count > 0)
            {
                foreach (var child in element.Children.OrderBy(x => x.Child.Order))
                {
                    treeItem.SubItems.Add(GetTreeItem(child.Child));
                }
            }
            if (element.AttributeMetadatas != null && element.AttributeMetadatas.Count > 0)
            {
                foreach (var attr in element.AttributeMetadatas.OrderBy(x => x.Order))
                {
                    treeItem.SubItems.Add(new TreeItem
                    {
                        Name = attr.Name,
                        Order = attr.Order,
                        Placeholder = attr.Placeholder
                    });
                }
            }
            
            
            return treeItem;
        }
       

        #endregion


        #region TreeVm print

        public void TreePrint(TreeItem treeItem)
        {
            foreach (var item in treeItem.SubItems.OrderBy(x => x.Order))
            {
                ChildPrint(item, 0);
            }
        }
        private void ChildPrint(TreeItem element, int i)
        {
            var ii = i;
            var tab = "";
            for (int j = 0; j < i; j++)
            {
                tab = tab + "_";
            }

            if (element.SubItems == null)
            {
                Console.WriteLine(tab + element.Name + " " + element.Placeholder);
            }
            else
            {
                Console.WriteLine(tab + element.Name);
                ++ii;
                foreach (var child in element.SubItems.OrderBy(x => x.Order))
                {
                    ChildPrint(child, ii);
                }
            }
        }

        #endregion

        public void DeleteData()
        {
//            using (var context = GetContext())
            {
                HierarchyParentChildContext.Subservices.RemoveRange(HierarchyParentChildContext.Subservices.Where(x => x.Id != null));
                HierarchyParentChildContext.SubserviceVersions.RemoveRange(HierarchyParentChildContext.SubserviceVersions.Where(x => x.Id != null));
                HierarchyParentChildContext.TreeElements.RemoveRange(HierarchyParentChildContext.TreeElements.Where(x=>x.Id!=null));
                HierarchyParentChildContext.Elements.RemoveRange(HierarchyParentChildContext.Elements.Where(x => x.Id != null));
                HierarchyParentChildContext.SaveChanges();
            }
        }

        public void Dispose()
        {
            HierarchyParentChildContext.Dispose();
        }

        

        
    }
}