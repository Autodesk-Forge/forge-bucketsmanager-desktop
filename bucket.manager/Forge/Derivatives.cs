/////////////////////////////////////////////////////////////////////
// Copyright (c) Autodesk, Inc. All rights reserved
// Written by Forge Partner Development
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
/////////////////////////////////////////////////////////////////////

// Forge Extractor
// Based on https://forge.autodesk.com/blog/forge-svf-extractor-nodejs

using Autodesk.Forge;
using Autodesk.Forge.Model;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace bucket.manager.ForgeUtils
{
  public static class Derivatives
  {
    /// <summary>
    /// Prepare a list of downloadables for a given URN
    /// </summary>
    /// <param name="urn">URN of the resource on Autodesk Forge</param>
    /// <param name="accessToken">Valid access token to download the resources</param>
    /// <returns>List of resouces for the given URN</returns>
    public async static Task<List<Resource>> ExtractSVFAsync(string urn, string accessToken)
    {
      DerivativesApi derivativeApi = new DerivativesApi();
      derivativeApi.Configuration.AccessToken = accessToken;

      // get the manifest for the URN
      dynamic manifest = await derivativeApi.GetManifestAsync(urn);

      // list items of the manifest file
      List<ManifestItem> urns = ParseManifest(manifest.derivatives);

      // iterate on what's on the file
      foreach (ManifestItem item in urns)
      {
        switch (item.MIME)
        {
          case "application/autodesk-svf":
            item.Path.Files = await SVFDerivates(item, accessToken);
            break;
          case "application/autodesk-f2d":
            item.Path.Files = await F2DDerivates(item, accessToken);
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
            item.Path.Files = new List<string>()
            {
              item.Path.RootFileName
            };
            break;
        }
      }

      // now organize the list for external usage
      List<Resource> resouces = new List<Resource>();
      foreach (ManifestItem item in urns)
      {
        foreach (string file in item.Path.Files)
        {
          Uri myUri = new Uri(new Uri(item.Path.BasePath), file);
          resouces.Add(new Resource()
          {
            FileName = file,
            RemotePath = DERIVATIVE_PATH + Uri.UnescapeDataString(myUri.AbsoluteUri),
            LocalPath = Path.Combine(item.Path.LocalPath, file)
          });
        }
      }

      return resouces;
    }

    public struct Resource
    {
      /// <summary>
      /// File name (no path)
      /// </summary>
      public string FileName { get; set; }
      /// <summary>
      /// Remove path to download (must add developer.api.autodesk.com prefix)
      /// </summary>
      public string RemotePath { get; set; }
      /// <summary>
      /// Path to save file locally
      /// </summary>
      public string LocalPath { get; set; }
    }

    public const string BASE_URL = "https://developer.api.autodesk.com/";
    public const string DERIVATIVE_PATH = "derivativeservice/v2/derivatives/";

    /// <summary>
    /// Download the manifest and extract it
    /// </summary>
    /// <param name="item"></param>
    /// <param name="accessToken"></param>
    /// <returns></returns>
    private static async Task<JObject> GetDerivativeAsync(string manifest, string accessToken)
    {
      // prepare to download the file
      IRestClient client = new RestClient(BASE_URL);
      RestRequest request = new RestRequest(DERIVATIVE_PATH + "{manifest}", Method.GET);
      request.AddParameter("manifest", manifest, ParameterType.UrlSegment);
      request.AddHeader("Authorization", "Bearer " + accessToken);
      request.AddHeader("Accept-Encoding", "gzip, deflate");
      IRestResponse response = await client.ExecuteTaskAsync(request);

      JObject manifestJson = null;

      // unzip it
      if (manifest.IndexOf(".gz") > -1)
      {
        GZipStream gzip = new GZipStream(new MemoryStream(response.RawBytes), CompressionMode.Decompress);
        using (var fileStream = new StreamReader(gzip))
          manifestJson = JObject.Parse(fileStream.ReadToEnd());
      }
      else
      {
        ZipArchive zip = new ZipArchive(new MemoryStream(response.RawBytes));
        ZipArchiveEntry manifestData = zip.GetEntry("manifest.json");
        using (var stream = manifestData.Open())
        using (var reader = new StreamReader(stream))
          manifestJson = JObject.Parse(reader.ReadToEnd().ToString());
      }

      return manifestJson;
    }

    /// <summary>
    /// Prepare list of resources for SVF files
    /// </summary>
    /// <param name="item"></param>
    /// <param name="accessToken"></param>
    /// <returns></returns>
    private static async Task<List<string>> SVFDerivates(ManifestItem item, string accessToken)
    {
      JObject manifest = await GetDerivativeAsync(item.Path.URN, accessToken);

      List<string> files = new List<string>();
      files.Add(item.Path.URN.Substring(item.Path.BasePath.Length));

      files.AddRange(GetAssets(manifest));

      return files;
    }

    /// <summary>
    /// Prepare list of resources for f2d files
    /// </summary>
    /// <param name="item"></param>
    /// <param name="accessToken"></param>
    /// <returns></returns>
    private static async Task<List<string>> F2DDerivates(ManifestItem item, string accessToken)
    {
      JObject manifest = await GetDerivativeAsync(item.Path.BasePath + "manifest.json.gz", accessToken);

      List<string> files = new List<string>();
      files.Add("manifest.json.gz");

      files.AddRange(GetAssets(manifest));

      return files;
    }

    /// <summary>
    /// Get asset URIs on the manifest file
    /// </summary>
    /// <param name="manifest"></param>
    /// <returns></returns>
    private static List<string> GetAssets(JObject manifest)
    {
      List<string> files = new List<string>();

      // for each "asset" on the manifest, add to the list of files (skip embed)
      foreach (JObject asset in manifest["assets"])
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

    private class ManifestItem
    {
      public string Guid { get; set; }
      public string MIME { get; set; }
      public PathInfo Path { get; set; }
    }

    /// <summary>
    /// Download and parse the SVF file
    /// </summary>
    /// <param name="manifest"></param>
    /// <returns></returns>
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

    private class PathInfo
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
