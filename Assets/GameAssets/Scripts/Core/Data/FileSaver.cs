using System.IO;
using UnityEngine;

namespace Core.Data
{
	public abstract class FileSaver<T> : IDataSaver<T> where T : IDataStore
	{
		protected readonly string m_Filename;

		/// <summary>
		/// Instantiate a json saver
		/// </summary>
		/// <param name="filename">The filename (including extension) to use.</param>
		/// <remarks><paramref name="filename" /> is relative to <see cref="Application.persistentDataPath" /></remarks>
		protected FileSaver(string filename)
		{
			m_Filename = GetFinalSaveFilename(filename);
		}

		public abstract void Save(T data);

		public abstract bool Load(out T data);

		public void Delete()
		{
			File.Delete(m_Filename);
		}

		public static string GetFinalSaveFilename(string baseFilename)
		{
			return string.Format("{0}/{1}", Application.persistentDataPath, baseFilename);
		}

		protected virtual StreamWriter GetWriteStream()
		{
			return new StreamWriter(new FileStream(m_Filename, FileMode.Create));
		}

		protected virtual StreamReader GetReadStream()
		{
			return new StreamReader(new FileStream(m_Filename, FileMode.Open));
		}
	}
}