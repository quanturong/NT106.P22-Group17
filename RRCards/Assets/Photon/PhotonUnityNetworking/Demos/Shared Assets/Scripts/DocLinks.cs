
using System.Collections.Generic;

namespace Photon.Pun.Demo.Shared
{
	public static class DocLinks {
		public enum DocTypes {Doc,Api};
		public enum Products {OnPremise,Realtime,Pun,Chat,Voice,Bolt,Quantum};
		public enum Versions {Current,V1,V2};
		public enum Languages {English,Japanese,Korean,Chinese};
		public static Versions Version = Versions.Current;
		public static Languages Language = Languages.English;
		public static Products Product = Products.Pun;
		public static string ApiUrlRoot = "https://doc-api.photonengine.com/{0}/{1}/{2}/{3}";
		public static string DocUrlFormat = "https://doc.photonengine.com/{0}/{1}/{2}/{3}";
		static Dictionary<Products,string> ProductsFolders = new Dictionary<Products, string>
		{
			{Products.Bolt, "bolt"},
			{Products.Chat, "chat"},
			{Products.OnPremise, "onpremise"},
			{Products.Pun, "pun"},
			{Products.Quantum, "quantum"},
			{Products.Realtime, "realtime"},
			{Products.Voice, "voice"}
		};
		static Dictionary<Languages,string> ApiLanguagesFolder = new Dictionary<Languages, string>
		{
			{Languages.English, "en"},
			{Languages.Japanese, "ja-jp"},
			{Languages.Korean, "ko-kr"},
			{Languages.Chinese, "zh-tw"}
		};
		static Dictionary<Languages,string> DocLanguagesFolder = new Dictionary<Languages, string>
		{
			{Languages.English, "en-us"},
			{Languages.Japanese, "ja-jp"},
			{Languages.Korean, "ko-kr"},
			{Languages.Chinese, "en"} // fallback to english
		};
		static Dictionary<Versions,string> VersionsFolder = new Dictionary<Versions, string>
		{
			{Versions.Current, "current"},
			{Versions.V1, "v1"},
			{Versions.V2, "v2"}
		};
		public static string GetLink(DocTypes type,string reference)
		{
			if (type == DocTypes.Api) {
				return GetApiLink (reference);
			}

			if (type == DocTypes.Doc) {
				return GetDocLink (reference);
			}

			return "https://doc.photonengine.com";
		}
		public  static string GetApiLink(string reference)
		{
			return string.Format(ApiUrlRoot, ApiLanguagesFolder[Language],ProductsFolders[Product], VersionsFolder[Version], reference);
		}
		public static string GetDocLink(string reference)
		{
			return string.Format(DocUrlFormat, DocLanguagesFolder[Language],ProductsFolders[Product], VersionsFolder[Version], reference);
		}
	}
}