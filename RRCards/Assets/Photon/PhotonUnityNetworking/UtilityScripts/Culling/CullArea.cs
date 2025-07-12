
using System.Collections.Generic;
using UnityEngine;

namespace Photon.Pun.UtilityScripts
{
    using System;
    public class CullArea : MonoBehaviour
    {
        private const int MAX_NUMBER_OF_ALLOWED_CELLS = 250;

        public const int MAX_NUMBER_OF_SUBDIVISIONS = 3;
        public readonly byte FIRST_GROUP_ID = 1;
        public readonly int[] SUBDIVISION_FIRST_LEVEL_ORDER = new int[4] { 0, 1, 1, 1 };
        public readonly int[] SUBDIVISION_SECOND_LEVEL_ORDER = new int[8] { 0, 2, 1, 2, 0, 2, 1, 2 };
        public readonly int[] SUBDIVISION_THIRD_LEVEL_ORDER = new int[12] { 0, 3, 2, 3, 1, 3, 2, 3, 1, 3, 2, 3 };

        public Vector2 Center;
        public Vector2 Size = new Vector2(25.0f, 25.0f);

        public Vector2[] Subdivisions = new Vector2[MAX_NUMBER_OF_SUBDIVISIONS];

        public int NumberOfSubdivisions;

        public int CellCount { get; private set; }

        public CellTree CellTree { get; private set; }

        public Dictionary<int, GameObject> Map { get; private set; }

        public bool YIsUpAxis = false;
        public bool RecreateCellHierarchy = false;

        private byte idCounter;
        private void Awake()
        {
            this.idCounter = this.FIRST_GROUP_ID;

            this.CreateCellHierarchy();
        }
        public void OnDrawGizmos()
        {
            this.idCounter = this.FIRST_GROUP_ID;

            if (this.RecreateCellHierarchy)
            {
                this.CreateCellHierarchy();
            }

            this.DrawCells();
        }
        private void CreateCellHierarchy()
        {
            if (!this.IsCellCountAllowed())
            {
                if (Debug.isDebugBuild)
                {
                    Debug.LogError("There are too many cells created by your subdivision options. Maximum allowed number of cells is " + (MAX_NUMBER_OF_ALLOWED_CELLS - this.FIRST_GROUP_ID) +
                                   ". Current number of cells is " + this.CellCount + ".");
                    return;
                }
                else
                {
                    Application.Quit();
                }
            }

            CellTreeNode rootNode = new CellTreeNode(this.idCounter++, CellTreeNode.ENodeType.Root, null);

            if (this.YIsUpAxis)
            {
                this.Center = new Vector2(transform.position.x, transform.position.y);
                this.Size = new Vector2(transform.localScale.x, transform.localScale.y);

                rootNode.Center = new Vector3(this.Center.x, this.Center.y, 0.0f);
                rootNode.Size = new Vector3(this.Size.x, this.Size.y, 0.0f);
                rootNode.TopLeft = new Vector3((this.Center.x - (this.Size.x / 2.0f)), (this.Center.y - (this.Size.y / 2.0f)), 0.0f);
                rootNode.BottomRight = new Vector3((this.Center.x + (this.Size.x / 2.0f)), (this.Center.y + (this.Size.y / 2.0f)), 0.0f);
            }
            else
            {
                this.Center = new Vector2(transform.position.x, transform.position.z);
                this.Size = new Vector2(transform.localScale.x, transform.localScale.z);

                rootNode.Center = new Vector3(this.Center.x, 0.0f, this.Center.y);
                rootNode.Size = new Vector3(this.Size.x, 0.0f, this.Size.y);
                rootNode.TopLeft = new Vector3((this.Center.x - (this.Size.x / 2.0f)), 0.0f, (this.Center.y - (this.Size.y / 2.0f)));
                rootNode.BottomRight = new Vector3((this.Center.x + (this.Size.x / 2.0f)), 0.0f, (this.Center.y + (this.Size.y / 2.0f)));
            }

            this.CreateChildCells(rootNode, 1);

            this.CellTree = new CellTree(rootNode);

            this.RecreateCellHierarchy = false;
        }
        private void CreateChildCells(CellTreeNode parent, int cellLevelInHierarchy)
        {
            if (cellLevelInHierarchy > this.NumberOfSubdivisions)
            {
                return;
            }

            int rowCount = (int)this.Subdivisions[(cellLevelInHierarchy - 1)].x;
            int columnCount = (int)this.Subdivisions[(cellLevelInHierarchy - 1)].y;

            float startX = parent.Center.x - (parent.Size.x / 2.0f);
            float width = parent.Size.x / rowCount;

            for (int row = 0; row < rowCount; ++row)
            {
                for (int column = 0; column < columnCount; ++column)
                {
                    float xPos = startX + (row * width) + (width / 2.0f);

                    CellTreeNode node = new CellTreeNode(this.idCounter++, (this.NumberOfSubdivisions == cellLevelInHierarchy) ? CellTreeNode.ENodeType.Leaf : CellTreeNode.ENodeType.Node, parent);

                    if (this.YIsUpAxis)
                    {
                        float startY = parent.Center.y - (parent.Size.y / 2.0f);
                        float height = parent.Size.y / columnCount;
                        float yPos = startY + (column * height) + (height / 2.0f);

                        node.Center = new Vector3(xPos, yPos, 0.0f);
                        node.Size = new Vector3(width, height, 0.0f);
                        node.TopLeft = new Vector3(xPos - (width / 2.0f), yPos - (height / 2.0f), 0.0f);
                        node.BottomRight = new Vector3(xPos + (width / 2.0f), yPos + (height / 2.0f), 0.0f);
                    }
                    else
                    {
                        float startZ = parent.Center.z - (parent.Size.z / 2.0f);
                        float depth = parent.Size.z / columnCount;
                        float zPos = startZ + (column * depth) + (depth / 2.0f);

                        node.Center = new Vector3(xPos, 0.0f, zPos);
                        node.Size = new Vector3(width, 0.0f, depth);
                        node.TopLeft = new Vector3(xPos - (width / 2.0f), 0.0f, zPos - (depth / 2.0f));
                        node.BottomRight = new Vector3(xPos + (width / 2.0f), 0.0f, zPos + (depth / 2.0f));
                    }

                    parent.AddChild(node);

                    this.CreateChildCells(node, (cellLevelInHierarchy + 1));
                }
            }
        }
        private void DrawCells()
        {
            if ((this.CellTree != null) && (this.CellTree.RootNode != null))
            {
                this.CellTree.RootNode.Draw();
            }
            else
            {
                this.RecreateCellHierarchy = true;
            }
        }
        private bool IsCellCountAllowed()
        {
            int horizontalCells = 1;
            int verticalCells = 1;

            foreach (Vector2 v in this.Subdivisions)
            {
                horizontalCells *= (int)v.x;
                verticalCells *= (int)v.y;
            }

            this.CellCount = horizontalCells * verticalCells;

            return (this.CellCount <= (MAX_NUMBER_OF_ALLOWED_CELLS - this.FIRST_GROUP_ID));
        }
        public List<byte> GetActiveCells(Vector3 position)
        {
            List<byte> activeCells = new List<byte>(0);
            this.CellTree.RootNode.GetActiveCells(activeCells, this.YIsUpAxis, position);
            int cellsActive = this.NumberOfSubdivisions + 1;
            int cellsNearby = activeCells.Count - cellsActive;
            if (cellsNearby > 0)
            {
                activeCells.Sort(cellsActive, cellsNearby, new ByteComparer());
            }
            return activeCells;
        }
    }
    public class CellTree
    {
        public CellTreeNode RootNode { get; private set; }
        public CellTree()
        {
        }
        public CellTree(CellTreeNode root)
        {
            this.RootNode = root;
        }
    }
    public class CellTreeNode
    {
        public enum ENodeType : byte
        {
            Root = 0,
            Node = 1,
            Leaf = 2
        }
        public byte Id;
        public Vector3 Center, Size, TopLeft, BottomRight;
        public ENodeType NodeType;
        public CellTreeNode Parent;
        public List<CellTreeNode> Childs;
        private float maxDistance;
        public CellTreeNode()
        {
        }
        public CellTreeNode(byte id, ENodeType nodeType, CellTreeNode parent)
        {
            this.Id = id;

            this.NodeType = nodeType;

            this.Parent = parent;
        }
        public void AddChild(CellTreeNode child)
        {
            if (this.Childs == null)
            {
                this.Childs = new List<CellTreeNode>(1);
            }

            this.Childs.Add(child);
        }
        public void Draw()
        {
#if UNITY_EDITOR
        if (this.Childs != null)
        {
            foreach (CellTreeNode node in this.Childs)
            {
                node.Draw();
            }
        }

        Gizmos.color = new Color((this.NodeType == ENodeType.Root) ? 1 : 0, (this.NodeType == ENodeType.Node) ? 1 : 0, (this.NodeType == ENodeType.Leaf) ? 1 : 0);
        Gizmos.DrawWireCube(this.Center, this.Size);

        byte offset = (byte)this.NodeType;
        GUIStyle gs = new GUIStyle() { fontStyle = FontStyle.Bold };
        gs.normal.textColor = Gizmos.color;
        UnityEditor.Handles.Label(this.Center+(Vector3.forward*offset*1f), this.Id.ToString(), gs);
#endif
        }
        public void GetActiveCells(List<byte> activeCells, bool yIsUpAxis, Vector3 position)
        {
            if (this.NodeType != ENodeType.Leaf)
            {
                foreach (CellTreeNode node in this.Childs)
                {
                    node.GetActiveCells(activeCells, yIsUpAxis, position);
                }
            }
            else
            {
                if (this.IsPointNearCell(yIsUpAxis, position))
                {
                    if (this.IsPointInsideCell(yIsUpAxis, position))
                    {
                        activeCells.Insert(0, this.Id);

                        CellTreeNode p = this.Parent;
                        while (p != null)
                        {
                            activeCells.Insert(0, p.Id);

                            p = p.Parent;
                        }
                    }
                    else
                    {
                        activeCells.Add(this.Id);
                    }
                }
            }
        }
        public bool IsPointInsideCell(bool yIsUpAxis, Vector3 point)
        {
            if ((point.x < this.TopLeft.x) || (point.x > this.BottomRight.x))
            {
                return false;
            }

            if (yIsUpAxis)
            {
                if ((point.y >= this.TopLeft.y) && (point.y <= this.BottomRight.y))
                {
                    return true;
                }
            }
            else
            {
                if ((point.z >= this.TopLeft.z) && (point.z <= this.BottomRight.z))
                {
                    return true;
                }
            }

            return false;
        }
        public bool IsPointNearCell(bool yIsUpAxis, Vector3 point)
        {
            if (this.maxDistance == 0.0f)
            {
                this.maxDistance = (this.Size.x + this.Size.y + this.Size.z) / 2.0f;
            }

            return ((point - this.Center).sqrMagnitude <= (this.maxDistance * this.maxDistance));
        }
    }


    public class ByteComparer : IComparer<byte>
    {
        public int Compare(byte x, byte y)
        {
            return x == y ? 0 : x < y ? -1 : 1;
        }
    }
}