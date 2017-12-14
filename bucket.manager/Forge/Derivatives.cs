using Autodesk.Forge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Autodesk.Forge.Model;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using RestSharp;
using System.IO.Compression;
using System.IO;
using Newtonsoft.Json.Linq;

namespace bucket.manager.ForgeUtils
{
  public static class Derivatives
  {
    public async static Task<List<ManifestItem>> ExtractSVFAsync(string urn, string accessToken)
    {
      DerivativesApi derivativeApi = new DerivativesApi();
      derivativeApi.Configuration.AccessToken = accessToken;

      dynamic manifest = await derivativeApi.GetManifestAsync(urn);
      List<ManifestItem> urns = ParseManifest(manifest.derivatives);
      foreach (ManifestItem item in urns)
      {
        switch (item.MIME)
        {
          case "application/autodesk-svf":
            item.Path.Files = SVFDerivates(item, accessToken);
            break;
          case "application/autodesk-f2d":
            // ToDo
            break;
          case "application/autodesk-db":
            item.Path.Files = new List<string>()
            {
              "objects_attrs.json.gz",
              "objects_vals.json.gz",
              "objects_offs.json.gz",
              "objects_ids.json.gz",
              "objects_avs.json.gz",
              item.Path.RootFileName
            };
            break;
          default:
            item.Path.Files = new List<string>() { item.Path.RootFileName };
            break;
        }
      }

      return urns;
    }

    private static List<string> SVFDerivates(ManifestItem item, string accessToken)
    {
      string svfPath = item.Path.URN.Substring(item.Path.BasePath.Length);
      IRestClient client = new RestClient("https://developer.api.autodesk.com/");
      RestRequest request = new RestRequest("derivativeservice/v2/derivatives/{urn}", Method.GET);
      request.AddParameter("urn", item.Path.URN, ParameterType.UrlSegment);
      request.AddHeader("Authorization", "Bearer " + accessToken);
      request.AddHeader("Accept-Encoding", "gzip, deflate");
      byte[] response = client.DownloadData(request);

      JObject manifestJson = null;

      ZipArchive zip = new ZipArchive(new MemoryStream(response));
      ZipArchiveEntry manifestData = zip.GetEntry("manifest.json");
      using (var stream = manifestData.Open())
      using (var reader = new StreamReader(stream))
        manifestJson = JObject.Parse(reader.ReadToEnd());

      List<string> files = new List<string>();
      files.Add(item.Path.URN.Substring(item.Path.BasePath.Length));

      foreach (JObject asset in manifestJson["assets"])
      {
        System.Diagnostics.Debug.WriteLine(asset["URI"].Value<string>());
        if (asset["URI"].Value<string>().Contains("embed:/")) continue;
        files.Add(asset["URI"].Value<string>());
      }


      return files;
    }

    private static readonly string[] ROLES = {
        "Autodesk.CloudPlatform.DesignDescription",
        "Autodesk.CloudPlatform.PropertyDatabase",
        "Autodesk.CloudPlatform.IndexableContent",
        "leaflet-zip",
        "thumbnail",
        "graphics",
        "preview",
        "raas",
        "pdf",
        "lod",
    };

    public class ManifestItem
    {
      public string Guid { get; set; }
      public string MIME { get; set; }
      public PathInfo Path { get; set; }
    }

    private static List<ManifestItem> ParseManifest(dynamic manifest)
    {
      List<ManifestItem> urns = new List<ManifestItem>();
      foreach (KeyValuePair<string, object> item in manifest.Dictionary)
      {
        DynamicDictionary itemKeys = (DynamicDictionary)item.Value;
        if (itemKeys.Dictionary.ContainsKey("role") && ROLES.Contains(itemKeys.Dictionary["role"]))
        {
          urns.Add(new ManifestItem
          {
            Guid = (string)itemKeys.Dictionary["guid"],
            MIME = (string)itemKeys.Dictionary["mime"],
            Path = DecomposeURN((string)itemKeys.Dictionary["urn"])
          });
        }

        if (itemKeys.Dictionary.ContainsKey("children"))
        {
          urns.AddRange(ParseManifest(itemKeys.Dictionary["children"]));
        }
      }
      return urns;
    }

    public class PathInfo
    {
      public string RootFileName { get; set; }
      public string LocalPath { get; set; }
      public string BasePath { get; set; }
      public string URN { get; set; }
      public List<string> Files { get; set; }
    }

    private static PathInfo DecomposeURN(string encodedUrn)
    {
      string urn = Uri.UnescapeDataString(encodedUrn);

      string rootFileName = urn.Substring(urn.LastIndexOf('/') + 1);
      string basePath = urn.Substring(0, urn.LastIndexOf('/') + 1);
      string localPath = basePath.Substring(basePath.IndexOf('/') + 1);
      localPath = Regex.Replace(localPath, "[/]?output/", string.Empty);

      return new PathInfo()
      {
        RootFileName = rootFileName,
        BasePath = basePath,
        LocalPath = localPath,
        URN = urn
      };
    }
  }
}
