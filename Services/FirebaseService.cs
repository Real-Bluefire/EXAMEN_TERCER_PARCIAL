using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using EXAMEN_TERCER_PARCIAL.Models;

namespace EXAMEN_TERCER_PARCIAL.Services
{
    public class FirebaseService
    {
        private readonly FirebaseClient _firebaseClient;

        public FirebaseService()
        {
            _firebaseClient = new FirebaseClient("https://examen3movil-28ce5-default-rtdb.firebaseio.com/");
        }

        public async Task AddNotaAsync(notes nota)
        {
            await _firebaseClient
                .Child("notas")
                .PostAsync(JsonConvert.SerializeObject(nota));
        }

        public async Task<List<notes>> GetNotasAsync()
        {
            var notas = await _firebaseClient
                .Child("notas")
                .OnceAsync<notes>();

            return notas.Select(n => n.Object).ToList();
        }

        public async Task UpdateNotaAsync(notes nota)
        {
            var toUpdateNota = (await _firebaseClient
                .Child("notas")
                .OnceAsync<notes>())
                .FirstOrDefault(a => a.Object.NoteID == nota.NoteID);

            await _firebaseClient
                .Child("notas")
                .Child(toUpdateNota.Key)
                .PutAsync(JsonConvert.SerializeObject(nota));
        }

        public async Task EliminarNotaAsync(int idNota)
        {
            var toDeleteNota = (await _firebaseClient
                .Child("notas")
                .OnceAsync<notes>())
                .FirstOrDefault(a => a.Object.NoteID == idNota);

            if (toDeleteNota != null)
            {
                await _firebaseClient.Child("notas").Child(toDeleteNota.Key).DeleteAsync();
            }
        }

        public async Task<List<notes>> LeerNotasAsync()
        {
            var notas = await _firebaseClient
                .Child("notas")
                .OnceAsync<notes>();

            return notas.Select(n => n.Object).ToList();
        }
    }
}

