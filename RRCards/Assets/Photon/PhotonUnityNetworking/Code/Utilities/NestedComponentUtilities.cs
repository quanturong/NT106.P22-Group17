using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Photon.Pun
{

    public static class NestedComponentUtilities
    {

        public static T EnsureRootComponentExists<T, NestedT>(this Transform transform)
            where T : Component
            where NestedT : Component
        {
            var root = GetParentComponent<NestedT>(transform);
            if (root)
            {
                var comp = root.GetComponent<T>();

                if (comp)
                    return comp;

                return root.gameObject.AddComponent<T>();
            }

            return null;
        }

        #region GetComponent Replacements
        private static Queue<Transform> nodesQueue = new Queue<Transform>();
        public static Dictionary<System.Type, ICollection> searchLists = new Dictionary<System.Type, ICollection>();
        private static Stack<Transform> nodeStack = new Stack<Transform>();
        public static T GetParentComponent<T>(this Transform t)
            where T : Component
        {
            T found = t.GetComponent<T>();

            if (found)
                return found;

            var par = t.parent;
            while (par)
            {
                found = par.GetComponent<T>();
                if (found)
                    return found;
                par = par.parent;
            }
            return null;
        }
        public static void GetNestedComponentsInParents<T>(this Transform t, List<T> list)
            where T : Component
        {
            list.Clear();

            while (t != null)
            {
                T obj = t.GetComponent<T>();
                if (obj)
                    list.Add(obj);

                t = t.parent;
            }
        }

        public static T GetNestedComponentInChildren<T, NestedT>(this Transform t, bool includeInactive)
            where T : class
            where NestedT : class
        {
            var found = t.GetComponent<T>();
            if (!ReferenceEquals(found, null))
                return found;
            nodesQueue.Clear();
            nodesQueue.Enqueue(t);

            while (nodesQueue.Count > 0)
            {
                var node = nodesQueue.Dequeue();

                for (int c = 0, ccnt = node.childCount; c < ccnt; ++c)
                {
                    var child = node.GetChild(c);
                    if (!includeInactive && !child.gameObject.activeSelf)
                        continue;
                    if (!ReferenceEquals(child.GetComponent<NestedT>(), null))
                        continue;
                    found = child.GetComponent<T>();
                    if (!ReferenceEquals(found, null))
                        return found;
                    nodesQueue.Enqueue(child);
                }

            }
            return found;
        }
        public static T GetNestedComponentInParent<T, NestedT>(this Transform t)
            where T : class
            where NestedT : class
        {
            T found = null;

            Transform node = t;
            do
            {

                found = node.GetComponent<T>();

                if (!ReferenceEquals(found, null))
                    return found;
                if (!ReferenceEquals(node.GetComponent<NestedT>(), null))
                    return null;

                node = node.parent;
            }
            while (!ReferenceEquals(node, null));

            return null;
        }
        public static T GetNestedComponentInParents<T, NestedT>(this Transform t)
            where T : class
            where NestedT : class
        {
            var found = t.GetComponent<T>();

            if (!ReferenceEquals(found, null))
                return found;
            var par = t.parent;

            while (!ReferenceEquals(par, null))
            {
                found = par.GetComponent<T>();
                if (!ReferenceEquals(found, null))
                    return found;
                if (!ReferenceEquals(par.GetComponent<NestedT>(), null))
                    return null;

                par = par.parent;
            };

            return null;
        }
        public static void GetNestedComponentsInParents<T, NestedT>(this Transform t, List<T> list)
            where T : class
            where NestedT : class
        {
            t.GetComponents(list);
            if (!ReferenceEquals(t.GetComponent<NestedT>(), null))
                return;

            var tnode = t.parent;
            if (ReferenceEquals(tnode, null))
                return;

            nodeStack.Clear();

            while (true)
            {
                nodeStack.Push(tnode);
                if (!ReferenceEquals(tnode.GetComponent<NestedT>(), null))
                    break;
                tnode = tnode.parent;
                if (ReferenceEquals(tnode, null))
                    break;
            }

            if (nodeStack.Count == 0)
                return;

            System.Type type = typeof(T);
            List<T> searchList;
            if (!searchLists.ContainsKey(type))
            {
                searchList = new List<T>();
                searchLists.Add(type, searchList);
            }
            else
            {
                searchList = searchLists[type] as List<T>;
            }
            while (nodeStack.Count > 0)
            {
                var node = nodeStack.Pop();

                node.GetComponents(searchList);
                list.AddRange(searchList);
            }
        }
        public static List<T> GetNestedComponentsInChildren<T, NestedT>(this Transform t, List<T> list, bool includeInactive = true)
            where T : class
            where NestedT : class
        {
            System.Type type = typeof(T);
            List<T> searchList;
            if (!searchLists.ContainsKey(type))
                searchLists.Add(type, searchList = new List<T>());
            else
                searchList = searchLists[type] as List<T>;

            nodesQueue.Clear();

            if (list == null)
                list = new List<T>();
            t.GetComponents(list);
            for (int i = 0, cnt = t.childCount; i < cnt; ++i)
            {
                var child = t.GetChild(i);
                if (!includeInactive && !child.gameObject.activeSelf)
                    continue;
                if (!ReferenceEquals(child.GetComponent<NestedT>(), null))
                    continue;

                nodesQueue.Enqueue(child);
            }
            while (nodesQueue.Count > 0)
            {
                var node = nodesQueue.Dequeue();
                node.GetComponents(searchList);
                list.AddRange(searchList);
                for (int i = 0, cnt = node.childCount; i < cnt; ++i)
                {
                    var child = node.GetChild(i);
                    if (!includeInactive && !child.gameObject.activeSelf)
                        continue;
                    if (!ReferenceEquals(child.GetComponent<NestedT>(), null))
                        continue;

                    nodesQueue.Enqueue(child);
                }
            }

            return list;
        }
        public static List<T> GetNestedComponentsInChildren<T>(this Transform t, List<T> list, bool includeInactive = true, params System.Type[] stopOn)
            where T : class
        {
            System.Type type = typeof(T);
            List<T> searchList;
            if (!searchLists.ContainsKey(type))
                searchLists.Add(type, searchList = new List<T>());
            else
                searchList = searchLists[type] as List<T>;

            nodesQueue.Clear();
            t.GetComponents(list);
            for (int i = 0, cnt = t.childCount; i < cnt; ++i)
            {
                var child = t.GetChild(i);
                if (!includeInactive && !child.gameObject.activeSelf)
                    continue;
                bool stopRecurse = false;
                for (int s = 0, scnt = stopOn.Length; s < scnt; ++s)
                {
                    if (!ReferenceEquals(child.GetComponent(stopOn[s]), null))
                    {
                        stopRecurse = true;
                        break;
                    }
                }
                if (stopRecurse)
                    continue;

                nodesQueue.Enqueue(child);
            }
            while (nodesQueue.Count > 0)
            {
                var node = nodesQueue.Dequeue();
                node.GetComponents(searchList);
                list.AddRange(searchList);
                for (int i = 0, cnt = node.childCount; i < cnt; ++i)
                {
                    var child = node.GetChild(i);
                    if (!includeInactive && !child.gameObject.activeSelf)
                        continue;
                    bool stopRecurse = false;
                    for (int s = 0, scnt = stopOn.Length; s < scnt; ++s)
                    {
                        if (!ReferenceEquals(child.GetComponent(stopOn[s]), null))
                        {
                            stopRecurse = true;
                            break;
                        }
                    }

                    if (stopRecurse)
                        continue;

                    nodesQueue.Enqueue(child);
                }
            }

            return list;
        }
        public static void GetNestedComponentsInChildren<T, SearchT, NestedT>(this Transform t, bool includeInactive, List<T> list)
            where T : class
            where SearchT : class
        {
            list.Clear();
            if (!includeInactive && !t.gameObject.activeSelf)
                return;

            System.Type searchType = typeof(SearchT);
            List<SearchT> searchList;
            if (!searchLists.ContainsKey(searchType))
                searchLists.Add(searchType, searchList = new List<SearchT>());
            else
                searchList = searchLists[searchType] as List<SearchT>;
            nodesQueue.Clear();
            nodesQueue.Enqueue(t);

            while (nodesQueue.Count > 0)
            {
                var node = nodesQueue.Dequeue();
                searchList.Clear();
                node.GetComponents(searchList);
                foreach (var comp in searchList)
                {
                    var casted = comp as T;
                    if (!ReferenceEquals(casted, null))
                        list.Add(casted);
                }
                for (int i = 0, cnt = node.childCount; i < cnt; ++i)
                {
                    var child = node.GetChild(i);
                    if (!includeInactive && !child.gameObject.activeSelf)
                        continue;
                    if (!ReferenceEquals(child.GetComponent<NestedT>(), null))
                        continue;

                    nodesQueue.Enqueue(child);
                }
            }

        }

        #endregion
    }

}