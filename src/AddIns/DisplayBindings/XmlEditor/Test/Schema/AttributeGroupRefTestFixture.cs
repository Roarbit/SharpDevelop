// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Matthew Ward" email="mrward@users.sourceforge.net"/>
//     <version>$Revision$</version>
// </file>

using System;
using ICSharpCode.SharpDevelop.Editor.CodeCompletion;
using ICSharpCode.XmlEditor;
using NUnit.Framework;

namespace XmlEditor.Tests.Schema
{
	/// <summary>
	/// Element that uses an attribute group ref.
	/// </summary>
	[TestFixture]
	public class AttributeGroupRefTestFixture : SchemaTestFixtureBase
	{
		XmlCompletionItemCollection attributeCompletionItems;
		
		public override void FixtureInit()
		{			
			XmlElementPath path = new XmlElementPath();
			path.AddElement(new QualifiedName("note", "http://www.w3schools.com"));
			attributeCompletionItems = SchemaCompletion.GetAttributeCompletion(path);
		}
		
		[Test]
		public void AttributeCount()
		{
			Assert.AreEqual(4, attributeCompletionItems.Count, "Should be 4 attributes.");
		}
		
		[Test]
		public void NameAttribute()
		{
			Assert.IsTrue(attributeCompletionItems.Contains("name"), 
			              "Attribute name does not exist.");
		}		
		
		[Test]
		public void IdAttribute()
		{
			Assert.IsTrue(attributeCompletionItems.Contains("id"), 
			              "Attribute id does not exist.");
		}		
		
		[Test]
		public void StyleAttribute()
		{
			Assert.IsTrue(attributeCompletionItems.Contains("style"), 
			              "Attribute style does not exist.");
		}	
		
		[Test]
		public void TitleAttribute()
		{
			Assert.IsTrue(attributeCompletionItems.Contains("title"), 
			              "Attribute title does not exist.");
		}		
		
		protected override string GetSchema()
		{
			return "<xs:schema xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" targetNamespace=\"http://www.w3schools.com\" xmlns=\"http://www.w3schools.com\" elementFormDefault=\"qualified\">\r\n" +
				"<xs:attributeGroup name=\"coreattrs\">" +
				"\t<xs:attribute name=\"id\" type=\"xs:string\"/>" +
				"\t<xs:attribute name=\"style\" type=\"xs:string\"/>" +
				"\t<xs:attribute name=\"title\" type=\"xs:string\"/>" +
				"</xs:attributeGroup>" +
				"\t<xs:element name=\"note\">\r\n" +
				"\t\t<xs:complexType>\r\n" +
				"\t\t\t<xs:attributeGroup ref=\"coreattrs\"/>" +
				"\t\t\t<xs:attribute name=\"name\" type=\"xs:string\"/>\r\n" +
				"\t\t</xs:complexType>\r\n" +
				"\t</xs:element>\r\n" +
				"</xs:schema>";
		}
	}
}
