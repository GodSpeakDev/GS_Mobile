using System;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace GodSpeak.Droid
{
	public class FileService : IFileService
	{
		public FileService()
		{
		}

		public Task<bool> ExistsAsync(string filename)
		{
			bool exists = File.Exists(GetFilePath(filename));
			return Task<bool>.FromResult(exists);
		}

		public async Task<string> ReadTextAsync(string filename)
		{
			var filePath = GetFilePath(filename);
			using (StreamReader reader = File.OpenText(filePath))
			{
				return await reader.ReadToEndAsync();
			}
		}

		public async Task WriteTextAsync(string filename, string text)
		{
			var filePath = GetFilePath(filename);
			using (StreamWriter writer = File.CreateText(filePath))
			{
				await writer.WriteAsync(text);
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
			File.Delete(GetFilePath(filename));
			return Task.FromResult(true);
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
