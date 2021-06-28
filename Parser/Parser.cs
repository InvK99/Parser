using System;
using System.Collections.Generic;
using System.IO;

namespace Parser
{
    public class Parser
    {
        private char[] tokens = new char[0];

        private Dictionary<string, List<string[]>> nodes = new Dictionary<string, List<string[]>>();

        public string Error = string.Empty;

        public int MaximumPoint = 0;

        public bool Verbose = false;

        public Branch Branch = new Branch();

        public Parser()
        {
        }
        
        /// <summary>
        /// Add a grammer text
        /// </summary>
        /// <param name="text"></param>
        public void AddGrammer(string text)
        {
            nodes = Grammer.ReadGrammer(text, nodes);
        }

        public void ClearGrammer()
        {
            nodes.Clear();
        }

        private int ParseNode(string strNode, int currPointer, int depth, out Branch branch)
        {
            branch = new Branch(strNode);

            if (depth < 0)
            {
                return currPointer;
            }

            foreach (KeyValuePair<string, List<string[]>> node in nodes)
            {
                if (tokens.Length > currPointer)
                {
                    if (node.Key == strNode)
                    {
                        foreach (string[] subNodes in node.Value)
                        {
                            int initPointer = currPointer;

                            string nextToken = GetNextToken(currPointer);

                            branch.Children = new Branch[subNodes.Length];

                            if (subNodes.Length > 0)
                            {
                                if (subNodes[0].StartsWith("<") && subNodes[0].EndsWith(">"))
                                {
                                    int numberfound = 0;

                                    foreach (string subNode in subNodes)
                                    {
                                        if (subNode.StartsWith("<") && subNode.EndsWith(">"))
                                        {
                                            int pointer = ParseNode(subNode, currPointer, depth - 1, out branch.Children[numberfound]);
                                            if (pointer > currPointer)
                                            {
                                                currPointer = pointer;
                                                numberfound++;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }

                                    if (numberfound == subNodes.Length)
                                    {
                                        return currPointer;
                                    }
                                }
                                else
                                {
                                    if (subNodes.Length == 1)
                                    {
                                        branch.Children[0] = new Branch(subNodes[0]);
                                        if (IsCorrctTokens(subNodes[0], currPointer))
                                        {
                                            return currPointer + subNodes[0].Length;
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("Impossible exception!!!"); //this should never happen
                                    }
                                }
                            }

                            currPointer = initPointer;
                        }
                    }
                }
            }

            if (MaximumPoint < currPointer)
                MaximumPoint = currPointer;

            return currPointer;
        }

        /// <summary>
        /// Parses the specified text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public bool Parse(string text)
        {
            int tokenPointer = 0;
            try
            {
                tokens = text.ToCharArray();
                string root = Grammer.GetRootNode(nodes, out Error);
                if (nodes.Count == 0)
                    Error = "No grammer file found." + Error;
                else if (string.IsNullOrWhiteSpace(root))
                    Error = "Grammer file is incorrect. " + Error;
                else
                    tokenPointer = ParseNode(root, 0, (nodes.Count > text.Length ? nodes.Count : text.Length + 1), out Branch);
            }
            catch (Exception ex)
            {
                Error = ex.ToString();
            }
            return (tokenPointer >= tokens.Length);
        }

        public string GetNextToken(int tokenPointer)
        {
            char[] t = new char[1];
            Array.Copy(tokens, tokenPointer, t, 0, 1);
            return new string(t);
        }
        
        public bool IsCorrctTokens(string grammer, int tokenPointer)
        {
            char[] t = new char[grammer.Length];
            Array.Copy(tokens, tokenPointer, t, 0, grammer.Length);
            return (new string(t) == grammer);
        }
    }
}
