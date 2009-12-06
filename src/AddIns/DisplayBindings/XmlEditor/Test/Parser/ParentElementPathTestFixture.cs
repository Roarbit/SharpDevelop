// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Matthew Ward" email="mrward@users.sourceforge.net"/>
//     <version>$Revision$</version>
// </file>

using ICSharpCode.XmlEditor;
using NUnit.Framework;
using System;
using System.Xml;

namespace XmlEditor.Tests.Parser
{
	[TestFixture]
	public class ParentElementPathTestFixture 
	{		
		XmlElementPath elementPath;
		XmlElementPath expectedElementPath;
		string namespaceURI = "http://foo/foo.xsd";

		[Test]
		public void GetParentElementPathForRootElement()
		{
			string text = "<foo xmlns='" + namespaceURI + "' ><";
			elementPath = XmlParser.GetParentElementPath(text);
			
			expectedElementPath = new XmlElementPath();
			expectedElementPath.AddElement(new QualifiedName("foo", namespaceURI));
			Assert.IsTrue(elementPath.Equals(expectedElementPath), 
			              "Incorrect active element path.");
		}
		
		[Test]
		public void GetParentElementPathForRootElementAfterChildElementEnd()
		{
			string text = "<foo xmlns='" + namespaceURI + "' ><bar></bar><";			
			elementPath = XmlParser.GetParentElementPath(text);
			
			expectedElementPath = new XmlElementPath();
			expectedElementPath.AddElement(new QualifiedName("foo", namespaceURI));
			Assert.IsTrue(elementPath.Equals(expectedElementPath), 
			              "Incorrect active element path.");
		}
		
		[Test]
		public void GetParentElementPathForRootElementAfterEmptyChildElementEnd()
		{
			string text = "<foo xmlns='" + namespaceURI + "' ><bar/><";
			elementPath = XmlParser.GetParentElementPath(text);
			
			expectedElementPath = new XmlElementPath();
			expectedElementPath.AddElement(new QualifiedName("foo", namespaceURI));
			Assert.IsTrue(elementPath.Equals(expectedElementPath), 
			              "Incorrect active element path.");
		}
		
		[Test]
		public void GetParentElementPathForTwoElementsInDifferentNamespaces()
		{
			string text = "<bar xmlns='http://test.com'><foo xmlns='" + namespaceURI + "' ><";
			elementPath = XmlParser.GetParentElementPath(text);
			
			expectedElementPath = new XmlElementPath();
			expectedElementPath.AddElement(new QualifiedName("bar", "http://test.com"));
			expectedElementPath.AddElement(new QualifiedName("foo", namespaceURI));
			Assert.IsTrue(elementPath.Equals(expectedElementPath), 
			              "Incorrect active element path.");
		}
	}
}
