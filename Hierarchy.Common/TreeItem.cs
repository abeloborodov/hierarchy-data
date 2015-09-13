using System;
using System.Collections.Generic;

namespace Hierarchy.Common
{
    public class TreeItem
    {
        public List<TreeItem> SubItems { get; set; }

        public string Name { get; set; }

        public string Placeholder { get; set; }

        public bool IsChoice { get; set; }

        public int Order { get; set; }
    }

    public interface IHierarchyApi
    {
        void WriteTree(Guid versionId, TreeItem tree);

        TreeItem ReadTree(Guid versionId);
    }
    public class TreeNodeComparer : IComparer<TreeItem>
    {
        public int Compare(TreeItem x, TreeItem y)
        {
            if (x.Order == y.Order) return 0;
            if (x.Order > y.Order) return 1;
            return -1;
        }
    }
}