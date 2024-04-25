using AutoMapper;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF_lab3.Dto;
using WPF_lab3.Mapper;
using WPF_lab3.Model;
using WPF_lab3.Persistence;

namespace WPF_lab3.Service
{
    public class TaskService
    {
        private readonly byte[] _encryptionKey = Encoding.UTF8.GetBytes("RadekMistrzUBB69");
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public TaskService(string connectionString)
        {
            _appDbContext  = new AppDbContext(connectionString);
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new NoteMappingProfile());
            });
            _mapper = mapperConfiguration.CreateMapper();
        }

        public IEnumerable<Note> GetNotesForUser(int userId)
        {
            var query = $"SELECT * FROM Notes WHERE UserId = {userId};";
            var notes = _appDbContext.GetFromDatabase<Note>(query);

            foreach (var note in notes)
            {
                note.Name = Decrypt(note.Name);
                note.Description = Decrypt(note.Description);
            }

            return _mapper.Map<IEnumerable<Note>>(notes);
        }

        public void CreateNote(NoteDto note)
        {
            note.Name = Encrypt(note.Name);
            note.Description = Encrypt(note.Description);

            _appDbContext.CreateNote(note);
        }

        public void EditNote(Note note)
        {
            note.Name = Encrypt(note.Name);
            note.Description = Encrypt(note.Description);
            _appDbContext.EditNote(note);
        }
        public void DeleteNote(int noteId)
        {
            _appDbContext.DeleteNote(noteId);
        }

        private string Encrypt(string text)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = _encryptionKey;
                aesAlg.GenerateIV();

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                byte[] encrypted;
                using (var msEncrypt = new System.IO.MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new System.IO.StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }
                    }
                    encrypted = msEncrypt.ToArray();
                }

                return Convert.ToBase64String(aesAlg.IV) + "|" + Convert.ToBase64String(encrypted);
            }
        }

        private string Decrypt(string cipherText)
        {
            var parts = cipherText.Split('|');
            var iv = Convert.FromBase64String(parts[0]);
            var encrypted = Convert.FromBase64String(parts[1]);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = _encryptionKey;
                aesAlg.IV = iv;

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                string text;
                using (var msDecrypt = new System.IO.MemoryStream(encrypted))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new System.IO.StreamReader(csDecrypt))
                        {
                            text = srDecrypt.ReadToEnd();
                        }
                    }
                }

                return text;
            }
        }
    }
}