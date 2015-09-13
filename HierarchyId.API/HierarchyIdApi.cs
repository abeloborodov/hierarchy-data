using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Hierarchy.Common;

namespace HierarchyId.API
{
    using HierarchyId = System.Data.Entity.Hierarchy.HierarchyId;

    public class HierarchyIdApi : IHierarchyApi, IDisposable
    {
        private HierarchyIdDbContext context;

        public HierarchyIdApi()
        {
            context = new HierarchyIdDbContext();
        }

        public TreeItem ReadTree(string subserviceNs, string versionNumber)
        {
            Block startpoint = context.Blocks.Single(x => x.BlockName == subserviceNs);
            Block version = GetChildren(startpoint).Single(x => x.BlockName == versionNumber);
            return Map(version);
        }

        public IEnumerable<Block> GetChildren(Block parent)
        {
            return context.Blocks.Where(
                x => x.Path.IsDescendantOf(parent.Path) && x.Path.GetLevel() == parent.Path.GetLevel() + 1);
        }

        public void Remove(HierarchyId path)
        {
            var attrsToRemove = context.AttributeMetadatas.Where(x => x.Block.Path.IsDescendantOf(path));
            foreach (var attributeMetadata in attrsToRemove)
            {
                Console.WriteLine(attributeMetadata.AttributeName);
            }
            Console.WriteLine("-------------");
            var l = context.Blocks.Where(x => x.Path.IsDescendantOf(path)).ToList();
            foreach (var block in l)
            {
                Console.WriteLine(block.BlockName);
            }
        }

        /// <summary>
        /// Возвращает путь для вставки в конец
        /// </summary>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        public HierarchyId GetAppendPath(HierarchyId parentPath)
        {
            var index = context.Blocks.Count(x => x.Path.GetLevel() == parentPath.GetLevel() + 1
                                                  && x.Path.IsDescendantOf(parentPath)) + 1;
            HierarchyId newItemPath = HierarchyId.Parse(parentPath + index.ToString() + "/");
            return newItemPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subserviceNs"></param>
        /// <param name="versionNumber"></param>
        /// <param name="tree">отсортированное дерево</param>
        public void WriteTree(string subserviceNs, string versionNumber, TreeItem tree)
        {
            Block subserviceBlock = context.Blocks.SingleOrDefault(x => x.BlockName == subserviceNs) ??
                               AppendSubservice(subserviceNs);

            var versionRoot = new TreeItem
            {
                Name = versionNumber,
                IsChoice = true,
                SubItems = new List<TreeItem> { tree.SubItems[0], tree.SubItems[1] }
            };

            Dive(versionRoot, GetAppendPath(subserviceBlock.Path));
            var watch = Stopwatch.StartNew();
            context.SaveChanges();
            watch.Stop();
            Console.WriteLine("DB Commit: " + watch.ElapsedMilliseconds);
        }

        public Block AppendSubservice(string ns)
        {
            Block systemRoot = context.Blocks.SingleOrDefault(x => x.Path == HierarchyId.GetRoot());
            if (systemRoot == null)
            {
                systemRoot = AddRoot();
            }

            var subservice = new Block { Id = Guid.NewGuid(), BlockName = ns, Path = GetAppendPath(systemRoot.Path) };
            context.Blocks.Add(subservice);
            context.SaveChanges();
            return subservice;
        }

        public Block AddRoot()
        {
            Block systemRoot = new Block() { Id = Guid.NewGuid(), BlockName = "_SystemRoot", Path = HierarchyId.GetRoot() };
            context.Blocks.Add(systemRoot);
            return systemRoot;
        }

        private TreeItem Map(Block blockToMap)
        {
            var newItem = new TreeItem()
            {
                Name = blockToMap.BlockName,
                IsChoice = blockToMap.IsChoice,
                Order = blockToMap.Order,
                SubItems = new List<TreeItem>()
            };
            newItem.SubItems.AddRange(blockToMap.AttributeList.Select(x => new TreeItem
                {
                    Name = x.AttributeName,
                    Order = x.Order,
                    Placeholder = x.Placeholder
                }));
            foreach (Block block in GetChildren(blockToMap))
            {
                newItem.SubItems.Add(Map(block));
            }
            newItem.SubItems.Sort(new TreeNodeComparer());
            return newItem;
        }

        private void Dive(TreeItem currentNode, HierarchyId currentPath)
        {
            var newBlock = new Block
            {
                Id = Guid.NewGuid(),
                Path = currentPath,
                BlockName = currentNode.Name,
                IsChoice = currentNode.IsChoice,
                Order = currentNode.Order,
                AttributeList = new Collection<AttributeMetadata>()
            };
            var subBlocks = currentNode.SubItems.Where(x => x.SubItems != null).ToArray();

            //блок
            for (int i = 1; i <= subBlocks.Length; i++)
            {
                var subItem = currentNode.SubItems[i - 1];
                Dive(subItem, HierarchyId.Parse(currentPath + i.ToString() + "/"));
            }
            /*var subAttributes = currentNode.SubItems.Where(x => x.SubItems == null).Select(x => new AttributeMetadata
                {
                    Id = Guid.NewGuid(),
                    AttributeName = x.Name,
                    BlockId = newBlock.Id,
                    Placeholder = x.Placeholder,
                });*/
            foreach (var node in currentNode.SubItems.Where(x => x.SubItems == null))
            {
                newBlock.AttributeList.Add(new AttributeMetadata
                {
                    Id = Guid.NewGuid(),
                    AttributeName = node.Name,
                    Placeholder = node.Placeholder,
                    Order = node.Order
                });
            }
            context.Blocks.Add(newBlock);
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public void WriteTree(Guid versionId, TreeItem tree)
        {
            var version = context.SubserviceVersions.Single(x => x.Id == versionId);
            WriteTree(version.Subservice.Namespace, version.Version, tree);
        }

        public TreeItem ReadTree(Guid versionId)
        {
            var version = context.SubserviceVersions.Single(x => x.Id == versionId);
            return ReadTree(version.Subservice.Namespace, version.Version);
        }
    }
}