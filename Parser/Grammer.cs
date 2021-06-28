using System;
using System.Collections.Generic;

namespace Parser
{
    public static class Grammer
    {
        public static Dictionary<string, List<string[]>> ReadGrammer(string str, Dictionary<string, List<string[]>> result)
        {
            string[] lines = str.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                string[] kvp = line.Split(":=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (kvp.Length == 2)
                {
                    string[] subRights = kvp[1].Split("|".ToCharArray());
                    foreach (string subRight in subRights)
                    {
                        if (string.IsNullOrEmpty(subRight) && line.IndexOf('=') != line.LastIndexOf('='))
                        {
                            if (result.ContainsKey(kvp[0]))
                                result[kvp[0]].Add(SplitToken("="));
                            else
                                result.Add(kvp[0], new List<string[]> { SplitToken("=") });
                        }
                        else
                        {
                            if (result.ContainsKey(kvp[0]))
                                result[kvp[0]].Add(SplitToken(subRight));
                            else
                                result.Add(kvp[0], new List<string[]> { SplitToken(subRight) });
                        }
                    }
                }
            }

            return result;
        }

        public static string GetRootNode(Dictionary<string, List<string[]>> grammers, out string error)
        {
            error = string.Empty;
            HashSet<string> subNodes = new HashSet<string>();
            HashSet<string> maxNodes = new HashSet<string>();

            foreach (KeyValuePair<string, List<string[]>> grammer in grammers)
            {
                subNodes.Clear();
                subNodes.Add(grammer.Key);
                subNodes = GetSubTreeNodes(grammer.Key, grammers, subNodes);
                if (subNodes.Count == grammers.Count)
                {
                    return grammer.Key;
                }
                else
                {
                    if (subNodes.Count > maxNodes.Count)
                        maxNodes = new HashSet<string>(subNodes);
                }
            }

            if (maxNodes.Count > grammers.Count)
                error = GetNoneTerminalNodes(maxNodes, grammers);
            else if (maxNodes.Count < grammers.Count)
                error = GetMissingTerminalNodes(maxNodes, grammers);

            return string.Empty;
        }

        private static string GetNoneTerminalNodes(HashSet<string> maxNodes, Dictionary<string, List<string[]>> grammers)
        {
            string str = string.Empty;
            foreach (string subNode in maxNodes)
            {
                if (!grammers.ContainsKey(subNode))
                    str += subNode + ", ";
            }
            return "Non terminal node(s) found. " + str.TrimEnd(", ".ToCharArray());
        }
        
        private static string GetMissingTerminalNodes(HashSet<string> maxNodes, Dictionary<string, List<string[]>> grammers)
        {
            string str = string.Empty;
            foreach (KeyValuePair<string, List<string[]>> grammer in grammers)
            {
                if (!maxNodes.Contains(grammer.Key))
                    str += grammer.Key + ", ";
            }
            return "Missing terminal node(s) found. " + str.TrimEnd(", ".ToCharArray());
        }
        
        public static HashSet<string> GetSubTreeNodes(string strGrammer, Dictionary<string, List<string[]>> grammers, HashSet<string> distinctValues)
        {
            foreach (KeyValuePair<string, List<string[]>> grammer in grammers)
            {
                if (strGrammer == grammer.Key)
                {
                    foreach (string[] subGrammers in grammer.Value)
                    {
                        foreach (string subGrammer in subGrammers)
                        {
                            if (strGrammer != subGrammer)
                            {
                                HashSet<string> subNode = GetSubTreeNodes(subGrammer, grammers, distinctValues);
                                if (!distinctValues.Contains(subGrammer))
                                {
                                    if (subGrammer.StartsWith("<") && subGrammer.EndsWith(">"))
                                    {
                                        distinctValues.Add(subGrammer);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return distinctValues;
        }

        private static string[] SplitToken(string strs)
        {
            string[] result = new string[0];
            if (strs.StartsWith("<") && strs.EndsWith(">"))
            {
                result = strs.Split(">".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = result[i] + ">";
                }
            }
            else
            {
                result = new string[1] { strs };
            }

            return result;
        }
    }
}
