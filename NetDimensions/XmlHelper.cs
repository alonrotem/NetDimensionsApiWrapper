using System;
using System.Linq;
using System.Xml;

namespace NetDimensionsWrapper.NetDimensions
{
    /// <summary>
    /// A helper class to help generate XML nodes quickly and easily
    /// </summary>
    public static class XmlHelper
    {
        /// <summary>
        /// Creates and appends a node to a parent node.
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="nodeName">Name of the node to create.</param>
        /// <param name="nodeTextContent">Content of the node's inner text.</param>
        /// <returns>The created node</returns>
        public static XmlNode AppendNode(this XmlNode parentNode, string nodeName, string nodeTextContent = "")
        {
            if ((!string.IsNullOrWhiteSpace(nodeName)) && (parentNode != null))
            {
                XmlNode xmlnode;
                if (parentNode is XmlDocument)
                {
                    xmlnode = ((XmlDocument)parentNode).CreateElement(nodeName);
                }
                else
                {
                    xmlnode = parentNode.OwnerDocument.CreateElement(nodeName);
                }

                if (!string.IsNullOrWhiteSpace(nodeTextContent))
                {
                    xmlnode.AppendChild(parentNode.OwnerDocument.CreateTextNode(nodeTextContent));
                }

                parentNode.AppendChild(xmlnode);
                return xmlnode;
            }
            return null;
        }

        /// <summary>
        /// Appends an attribute to a node.
        /// </summary>
        /// <param name="node">The node to append the attribute to.</param>
        /// <param name="attributeName">The attribute's name</param>
        /// <param name="attributeValue">The attribute's value.</param>
        public static void AppendAttribute(this XmlNode node, string attributeName, string attributeValue)
        {
            if ((node != null) && (!string.IsNullOrWhiteSpace(attributeName)))
            {
                XmlAttribute attr = node.OwnerDocument.CreateAttribute(attributeName);
                attr.Value = attributeValue;
                node.Attributes.Append(attr);
            }
        }
    }
}