using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace IndoorMappingApp
{
    public static class LocalStorageService
    {
        private static string GetFilePath(string filename)
        {
            // Caminho para salvar no diretório de dados do aplicativo
            string directory = FileSystem.AppDataDirectory;
            return Path.Combine(directory, filename);
        }

        public static async Task SaveDataAsync(string filename, string data)
        {
            string filePath = GetFilePath(filename);
            await File.WriteAllTextAsync(filePath, data); // Salva o dado no arquivo
        }

        public static async Task<string> GetDataAsync(string filename)
        {
            string filePath = GetFilePath(filename);
            if (File.Exists(filePath))
            {
                return await File.ReadAllTextAsync(filePath); // Retorna o conteúdo do arquivo
            }
            return string.Empty; // Retorna vazio se o arquivo não existir
        }
        public static async Task DeleteDataAsync(string filename)
        {
            string filePath = GetFilePath(filename);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);  // Apaga o arquivo
                await Task.CompletedTask;  // Garantir que a operação seja assíncrona
            }
        }
    }
}
