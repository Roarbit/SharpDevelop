using System;
using System.ComponentModel.Design;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using ICSharpCode.SharpDevelop;
using ICSharpCode.Core;
using ICSharpCode.Reports.Core;

namespace ICSharpCode.Reports.Addin
{
	internal class ReportDefinitionDeserializer : ReportDefinitionParser
	{
		private IDesignerHost host;
		private ReportSettings reportSettings;
		private Stream stream;
		
		#region Constructor
		
		public ReportDefinitionDeserializer(IDesignerHost host,Stream stream)
		{
			if (host == null) {
				throw new ArgumentNullException("host");
			}
			if (stream == null) {
				throw new ArgumentNullException("stream");
			}
			this.host = host;
			this.stream = stream;
		}
		
		#endregion
		
		public ReportModel LoadObjectFromFileDefinition()
		{
			
			XmlDocument doc = new XmlDocument();
			doc.Load(this.stream);
			if (doc.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
			{
				XmlDeclaration xmlDeclaration = (XmlDeclaration)doc.FirstChild;
				xmlDeclaration.Encoding = "utf-8";
			}
			return LoadObjectFromXmlDocument(doc.DocumentElement);
		}
		
		
		private ReportModel LoadObjectFromXmlDocument(XmlElement elem)
		{
			//ReportSettings
			OpenedFile file =(OpenedFile) host.GetService(typeof(OpenedFile));
			BaseItemLoader baseItemLoader = new BaseItemLoader();
			XmlNodeList n =  elem.FirstChild.ChildNodes;
			XmlElement rse = (XmlElement) n[0];
			ReportModel model = ReportModel.Create();
			
			// manipulate reportSettings if Filename differs
			this.reportSettings = baseItemLoader.Load(rse) as ReportSettings;
			if (this.reportSettings.FileName != file.FileName) {
				System.Diagnostics.Trace.WriteLine("LoadObjectFromXmlDocument - filename changed" );
				this.reportSettings.FileName = file.FileName;
			}
			
			model.ReportSettings = this.reportSettings;
			
			host.Container.Add(this.reportSettings);
			
			//Move to SectionCollection
			XmlNodeList sectionList =  elem.LastChild.ChildNodes;
			
			foreach (XmlNode sectionNode in sectionList) {
				try {
					object o = this.Load(sectionNode as XmlElement,null);
					BaseSection section = o as BaseSection;
					Absolut2RelativePath(section,this.reportSettings.FileName);
					host.Container.Add(section);
				} catch (Exception e) {
					MessageService.ShowError(e);
				}
			}
			return model;
		}
		
		
		private static void Absolut2RelativePath (BaseSection section, string fileName)
		{
			foreach (Control item in section.Controls) {
				BaseImageItem baseImageItem = item as BaseImageItem;
				if (baseImageItem != null) {
					baseImageItem.ReportFileName = fileName;
					
					if (Path.IsPathRooted(baseImageItem.ImageFileName)) {
						string d = ICSharpCode.Reports.Core.FileUtility.GetRelativePath(
							Path.GetDirectoryName(fileName),
							Path.GetDirectoryName(baseImageItem.ImageFileName));

						baseImageItem.RelativeFileName = d + Path.DirectorySeparatorChar + Path.GetFileName(baseImageItem.ImageFileName);
					}
				}
			}
		}
		
		private static void old_Absolut2RelativePath (BaseSection section, string fileName)
		{
			System.Diagnostics.Trace.WriteLine("Absolut2RelativePath");
			foreach (Control item in section.Controls) {
				BaseImageItem baseImageItem = item as BaseImageItem;
				if (baseImageItem != null) {
					baseImageItem.ReportFileName = fileName;
					
					if (Path.IsPathRooted(baseImageItem.ImageFileName)) {
						
						string relPath = ICSharpCode.Reports.Core.FileUtility.GetRelativePath(
							Path.GetDirectoryName(fileName),
							Path.GetDirectoryName(baseImageItem.ImageFileName));
						
						System.Diagnostics.Trace.WriteLine(String.Format("relativ image {0}",relPath));
						
						string relFile = relPath + Path.DirectorySeparatorChar + Path.GetFileName(baseImageItem.ImageFileName);
						System.Diagnostics.Trace.WriteLine(String.Format("ggg image {0}",relFile));

						baseImageItem.RelativeFileName = relPath + Path.DirectorySeparatorChar + Path.GetFileName(baseImageItem.ImageFileName);
						
						string ddd = ICSharpCode.Reports.Core.FileUtility.GetAbsolutePath(Path.GetDirectoryName(fileName),relFile);
						System.Diagnostics.Trace.WriteLine(String.Format("baseImageItem.RelativeFileName {0}",relFile));
						if (File.Exists(ddd)){
							Console.WriteLine("found");
						}
					}
				}
			}
		}
		
		
		protected override Type GetTypeByName(string ns, string name)
		{
			Type t = typeof(BaseSection).Assembly.GetType(typeof(BaseSection).Namespace + "." + name);
			return t;
		}
		
		
		#region Properties
		
		public string ReportName {
			get { return this.reportSettings.ReportName; }
		}
		
		#endregion
		
	}
}
