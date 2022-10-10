﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ScriptPlayer.Shared.Subtitles
{
    public static class SubtitleLoaderManager
    {
        private static readonly HashSet<Type> KnownLoaders = new HashSet<Type>();
        private static readonly List<SubtitleLoader> Loaders = new List<SubtitleLoader>();
        private static readonly Dictionary<SubtitleFormat, SubtitleLoader> LoaderByFileFormat = new Dictionary<SubtitleFormat, SubtitleLoader>();

        static SubtitleLoaderManager()
        {
            RegisterLoader<SrtSubtitleLoader>();
            RegisterLoader<SsaSubtitleLoader>();
        }

        public static void RegisterLoader<T>() where T : SubtitleLoader, new()
        {
            if (KnownLoaders.Contains(typeof(T)))
                return;

            SubtitleLoader loader = new T();
            Loaders.Add(loader);
            KnownLoaders.Add(typeof(T));

            List<SubtitleFormat> formats = loader.GetSupportedFormats();

            foreach (SubtitleFormat format in formats)
            {
                LoaderByFileFormat.Add(format, loader);
            }
        }

        public static string[] GetSupportedExtensions()
        {
            return Loaders.SelectMany(l => l.GetSupportedFormats().SelectMany(f => f.Extensions)).Distinct().ToArray();
        }

        public static SubtitleLoader[] GetLoaders(string filename)
        {
            string extension = (Path.GetExtension(filename) ?? "").TrimStart('.').ToLower();
            return Loaders.Where(loader => loader.GetSupportedFormats().Any(f => f.Extensions.Contains(extension)))
                .OrderBy(loader => Loaders.IndexOf(loader))
                .ToArray();
        }

        public static SubtitleLoader[] GetAllLoaders()
        {
            return Loaders.ToArray();
        }
    }

    public class SubtitleFormat
    {
        public string Name { get; set; }
        public string[] Extensions { get; set; }

        public SubtitleFormat()
        { }

        public SubtitleFormat(string name, params string[] extensions)
        {
            Name = name;
            Extensions = extensions;
        }
    }
}
