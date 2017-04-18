using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GodSpeak
{
	public interface IFileService
	{
		Task<bool> ExistsAsync(string filename);
		Task<string> ReadTextAsync(string filename);
		Task WriteTextAsync(string filename, string text);
		Task<IEnumerable<string>> GetFilesAsync();
		Task DeleteFileAsync(string filename);	
	}
}
