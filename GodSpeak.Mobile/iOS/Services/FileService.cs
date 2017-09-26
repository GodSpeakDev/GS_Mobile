using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GodSpeak.iOS
{
	public class FileService : IFileService
	{
		private static object locker = new object();

		public FileService()
		{
		}

		public Task<bool> ExistsAsync(string filename)
		{
			lock (locker)
			{
				bool exists = File.Exists(GetFilePath(filename));
				return Task<bool>.FromResult(exists);
			}
		}

		public Task<string> ReadTextAsync(string filename)
		{
			lock (locker)
			{
				var filePath = GetFilePath(filename);
				return Task.FromResult(File.ReadAllText(filePath));
			}
		}

		public async Task WriteTextAsync(string filename, string text)
		{
			lock (locker)
			{
				var filePath = GetFilePath(filename);
				File.WriteAllText(filePath, text);
			}
		}

		public Task<IEnumerable<string>> GetFilesAsync()
		{
			IEnumerable<string> filenames =
				Directory.EnumerateFiles(GetDocsFolder()).Select(x => Path.GetFileName(x));

			return Task<IEnumerable<string>>.FromResult(filenames);
		}

		public Task DeleteFileAsync(string filename)
		{
			lock (locker)
			{
				File.Delete(GetFilePath(filename));
				return Task.FromResult(true);
			}
		}

		string GetDocsFolder()
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		}

		public string GetFilePath(string filename)
		{
			string docsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			return Path.Combine(docsPath, filename);
		}	
	}
}
